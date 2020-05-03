using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Project.Data.Repository.Base;
using Project.Model.Entities;
using Project.Model.Enums;
using Project.Model.Repository.Interfaces;
using Project.Model.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Data.Repository
{
    public class QuestionRepositoryWrite : BaseRepositoryWrite, IQuestionRepositoryWrite
    {   
        public QuestionRepositoryWrite(IOptions<CosmosDBSettings> settings) : base(settings)
        {   
        }

        public async Task LikeQuestionAsync(string questionId, ETypePost typePost)
        {
            await _context.initializeAsync();

            ItemResponse<Question> wakefieldFamilyResponse = await _context.container.ReadItemAsync<Question>(questionId, new PartitionKey(_context.partitionKey));

            var itemBody = wakefieldFamilyResponse.Resource;

            itemBody.QuantityLikes = typePost == ETypePost.Like ? itemBody.QuantityLikes + 1 : itemBody.QuantityLikes - 1;

            await _context.container.ReplaceItemAsync(itemBody, itemBody.Id, new PartitionKey(itemBody.PartitionName));
        }

        public async Task AddQuestionAsync(Question question)
        {           
            await _context.initializeAsync();

            question.PartitionName = _context.partitionKey;

            await _context.container.CreateItemAsync(question, new PartitionKey(question.PartitionName));
        }

        public async Task AddAnswerAsync(Answer answer)
        {
            await _context.initializeAsync();

            ItemResponse<Question> wakefieldFamilyResponse = await _context.container.ReadItemAsync<Question>(answer.QuestionId, new PartitionKey(_context.partitionKey));

            var itemBody = wakefieldFamilyResponse.Resource;
            itemBody.QuantityAnswers += 1;

            if (itemBody.Answers == null)
                itemBody.Answers = new List<Answer>();

            itemBody.Answers.Add(new Answer
            {
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                User = answer.User,
                Text = answer.Text                
            });

            await _context.container.ReplaceItemAsync(itemBody, itemBody.Id, new PartitionKey(itemBody.PartitionName));
        }
    }
}