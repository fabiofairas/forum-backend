using Project.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Model.Repository.Interfaces
{
    public interface IQuestionRepositoryRead
    {
        Task<List<Question>> GetAllQuestionAsync();
        Task<Question> GetQuestionByIdAsync(string id);
    }
}