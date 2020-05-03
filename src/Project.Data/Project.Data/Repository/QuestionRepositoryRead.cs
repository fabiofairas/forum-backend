using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Project.Model.Entities;
using Project.Model.Repository.Interfaces;
using Project.Model.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Data.Repository
{
    public class QuestionRepositoryRead : IQuestionRepositoryRead
    {
        private readonly Context _context;
        public QuestionRepositoryRead(IOptions<CosmosDBSettings> settings)
        {
            _context = new Context(settings);
        }

        public async Task<List<Question>> GetAllQuestionAsync()
        {
            await _context.initializeAsync();

            var sqlQueryText = $"SELECT c.id, c.Text, c.User, c.CreationDate, c.QuantityAnswers, c.QuantityLikes FROM c";
            
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Question> queryResultSetIterator = _context.container.GetItemQueryIterator<Question>(queryDefinition);

            List<Question> questions = new List<Question>();
           
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Question> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach (Question question in currentResultSet)
                {
                    questions.Add(question);
                }
            }           

            return questions;
        }

        public async Task<Question> GetQuestionByIdAsync(string id)
        {
            await _context.initializeAsync();

            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Question> queryResultSetIterator = _context.container.GetItemQueryIterator<Question>(queryDefinition);

            Question question = new Question();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Question> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach (Question questionResult in currentResultSet)
                {
                    question = questionResult;                    
                }
            }

            return question;
        }
    }
}
