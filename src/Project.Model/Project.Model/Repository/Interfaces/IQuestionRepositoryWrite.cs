using Project.Model.Entities;
using Project.Model.Enums;
using System.Threading.Tasks;

namespace Project.Model.Repository.Interfaces
{
    public interface IQuestionRepositoryWrite
    {
        Task LikeQuestionAsync(string questionId, ETypePost typePost);
        Task AddQuestionAsync(Question question);
        Task AddAnswerAsync(Answer answer);
        Task InitializeAsync();
    }
}