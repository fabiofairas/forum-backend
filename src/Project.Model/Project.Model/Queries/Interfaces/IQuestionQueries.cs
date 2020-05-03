using Project.Model.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Model.Queries.Interfaces
{
    public interface IQuestionQueries
    {
        Task<List<QuestionDTO>> GetAllQuestionAsync();
        Task<QuestionDTO> GetQuestionByIdAsync(string id);
    }
}