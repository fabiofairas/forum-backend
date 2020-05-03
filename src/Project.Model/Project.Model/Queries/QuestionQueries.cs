using Project.Model.DTOs;
using Project.Model.Entities;
using Project.Model.Queries.Interfaces;
using Project.Model.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Model.Queries
{
    public class QuestionQueries : IQuestionQueries
    {
        private readonly IQuestionRepositoryRead _questionRepositoryRead;

        public QuestionQueries(IQuestionRepositoryRead questionRepositoryRead)
        {
            _questionRepositoryRead = questionRepositoryRead;
        }

        public Task<List<QuestionDTO>> GetAllQuestionAsync()
        {
            var questions = new List<QuestionDTO>();

            var result = _questionRepositoryRead.GetAllQuestionAsync();

            if (result.Result != null)
            {
                foreach (var question in result.Result)
                {
                    questions.Add(MapperQuestion(question));
                }
            }

            return Task.FromResult(questions);
        }

        public Task<QuestionDTO> GetQuestionByIdAsync(string id)
        {
            var question = new QuestionDTO();

            var result = _questionRepositoryRead.GetQuestionByIdAsync(id);

            if (result.Result != null)
            {
                question = MapperQuestion(result.Result);
            }

            return Task.FromResult(question);
        }

        private QuestionDTO MapperQuestion(Question question)
        {
            return new QuestionDTO
            {
                Id = question.Id,
                CreationDate = question.CreationDate,
                User = question.User,
                QuantityAnswers = question.QuantityAnswers,
                QuantityLikes = question.QuantityLikes,
                Text = question.Text,
                Answers = MapperAnswers(question.Answers)
            };
        }

        private IList<AnswerDTO> MapperAnswers(IList<Answer> Answers)
        {
            var result = new List<AnswerDTO>();

            foreach (var answer in Answers)
            {
                result.Add(new AnswerDTO
                {
                    Id = answer.Id,
                    CreationDate = answer.CreationDate,
                    User = answer.User,
                    Text = answer.Text
                });
            }

            return result;
        }
    }
}