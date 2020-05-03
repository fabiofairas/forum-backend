using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Project.Data.Repository;
using Project.Model.Commands;
using Project.Model.DTOs;
using Project.Model.Enums;
using Project.Model.Events;
using Project.Model.Notifications;
using Project.Model.Queries;
using Project.Model.Queries.Interfaces;
using Project.Model.Repository.Interfaces;
using Project.Model.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Test
{
    public class ForumTest
    {
        private ServiceProvider serviceProvider { get; set; }

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.Configure<CosmosDBSettings>(options =>
            {
                options.EndpointUri = "https://forum.documents.azure.com:443/";
                options.PrimaryKey = "DGj5ZAYZc2tlfHVxTt44KZYAPrnjDvhUsZiZ04GMNaGKzGBsieZdLihiRW6m2ZHfdhSex7NwkwvXRnuOqHtThA==";
                options.DatabaseId = "forum";
                options.ContainerId = "items";
                options.PartitionKey = "Questions";
                options.ApplicationName = "CosmosDBDotnetForum";
            });

            services.AddTransient<IQuestionRepositoryWrite, QuestionRepositoryWrite>();
            services.AddTransient<IQuestionRepositoryRead, QuestionRepositoryRead>();

            services.AddTransient<IQuestionQueries, QuestionQueries>();

            services.AddMediatR(typeof(ForumTest));
            services.AddScoped<IRequestHandler<AddQuestionCommand, bool>, AddQuestionCommandHandler>();
            services.AddScoped<IRequestHandler<AddAnswerCommand, bool>, AddAnswerCommandHandler>();
            services.AddScoped<IRequestHandler<LikeQuestionCommand, bool>, LikeQuestionCommandHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            serviceProvider = services.BuildServiceProvider();
        }

        [Test]
        public async Task GetAllAsync()
        {
            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "fabiofarias",
                Text = "Qualquer pergunta pode ser feita?"
            };

            var _mediator = serviceProvider.GetService<IMediator>();
            var result = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsTrue(result);

            var _questionQueries = serviceProvider.GetService<IQuestionRepositoryRead>();
            var questions = await _questionQueries.GetAllQuestionAsync();

            Assert.IsTrue(questions.Any());
        }

        [Test]
        public async Task GetByIdAync()
        {

            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "fabiofarias",
                Text = "Qualquer pergunta pode ser feita?"
            };

            var _mediator = serviceProvider.GetService<IMediator>();
            var result = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsTrue(result);

            var _questionQueries = serviceProvider.GetService<IQuestionRepositoryRead>();

            var resultQuestion = await _questionQueries.GetQuestionByIdAsync(question.Id);

            Assert.AreEqual(resultQuestion.Id, question.Id);
        }

        [Test]
        public async Task AddQuestionAsync()
        {

            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "fabiofarias",
                Text = "Qualquer pergunta pode ser feita?"
            };

            var _mediator = serviceProvider.GetService<IMediator>();
            var result = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAnswerAsync()
        {

            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "fabiofarias",
                Text = "Qualquer pergunta pode ser feita?"
            };

            var _mediator = serviceProvider.GetService<IMediator>();
            var resultQuestion = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsTrue(resultQuestion);

            var answer = new AnswerDTO()
            {
                Id = Guid.NewGuid().ToString(),
                QuestionId = question.Id,
                User = "fabiofarias",
                Text = "Qualquer resposta pode ser dada"
            };

            var resultAnswer = await _mediator.Send(new AddAnswerCommand(answer.Id, answer.User, answer.Text, answer.QuestionId));

            Assert.IsTrue(resultAnswer);

            var _questionQueries = serviceProvider.GetService<IQuestionRepositoryRead>();
            var resultQuestionById = await _questionQueries.GetQuestionByIdAsync(question.Id);

            Assert.AreEqual(resultQuestionById.Id, question.Id);
            Assert.IsTrue(resultQuestionById.Answers.Any(a => a.Id == answer.Id));
        }

        [Test]
        public async Task LikeQuestionAsync()
        {

            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "fabiofarias",
                Text = "Qualquer pergunta pode ser feita?",                
            };

            var _mediator = serviceProvider.GetService<IMediator>();
            var result = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsTrue(result);

            var resultLike =  await _mediator.Send(new LikeQuestionCommand(question.Id, ETypePost.Like));

            Assert.IsTrue(resultLike);

            var _questionQueries = serviceProvider.GetService<IQuestionRepositoryRead>();

            var resultQuestion = await _questionQueries.GetQuestionByIdAsync(question.Id);

            Assert.IsTrue(resultQuestion.QuantityLikes > 0);
        }

        [Test]
        public async Task DisLikeQuestionAsync()
        {

            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "fabiofarias",
                Text = "Qualquer pergunta pode ser feita?"                
            };

            var _mediator = serviceProvider.GetService<IMediator>();
            var result = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsTrue(result);

            var resultLike = await _mediator.Send(new LikeQuestionCommand(question.Id, ETypePost.Like));

            Assert.IsTrue(resultLike);

            var _questionQueries = serviceProvider.GetService<IQuestionRepositoryRead>();

            var resultQuestion = await _questionQueries.GetQuestionByIdAsync(question.Id);

            Assert.IsTrue(resultQuestion.QuantityLikes > 0);

            var resultDisLike = await _mediator.Send(new LikeQuestionCommand(question.Id, ETypePost.DisLike));

            Assert.IsTrue(resultDisLike);

            resultQuestion = await _questionQueries.GetQuestionByIdAsync(question.Id);

            Assert.IsTrue(resultQuestion.QuantityLikes == 0);
        }

        [Test]
        public async Task ValidatorUserAsync()
        {
            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "",
                Text = "Qualquer pergunta pode ser feita?"
            };

            var notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var _notifications = (DomainNotificationHandler)notifications;

            var _mediator = serviceProvider.GetService<IMediator>();
            var result = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsFalse(result);
            Assert.IsTrue(_notifications.HasNotification());
            Assert.IsTrue(_notifications.GetNotifications().Any(x => x.Value == "Campo user inválido, o campo deve possuir entre 10 e 20 caracteres"));
        }

        [Test]
        public async Task ValidatorTextAsync()
        {

            var _questionRepositoryWrite = serviceProvider.GetService<IQuestionRepositoryWrite>();
            await _questionRepositoryWrite.InitializeAsync();

            var question = new QuestionDTO()
            {
                Id = Guid.NewGuid().ToString(),
                User = "fabiofarias",
                Text = ""
            };

            var notifications = serviceProvider.GetService<INotificationHandler<DomainNotification>>();
            var _notifications = (DomainNotificationHandler)notifications;

            var _mediator = serviceProvider.GetService<IMediator>();
            var result = await _mediator.Send(new AddQuestionCommand(question.Id, question.User, question.Text));

            Assert.IsFalse(result);
            Assert.IsTrue(_notifications.HasNotification());
            Assert.IsTrue(_notifications.GetNotifications().Any(x => x.Value == "Campo text inválido, o campo deve possuir entre 20 e 200 caracteres"));
        }
    }
}