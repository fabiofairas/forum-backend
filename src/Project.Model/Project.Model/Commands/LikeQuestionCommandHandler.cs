using MediatR;
using Project.Model.Events;
using Project.Model.Notifications;
using Project.Model.Repository.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Model.Commands
{
    public class LikeQuestionCommandHandler : IRequestHandler<LikeQuestionCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IQuestionRepositoryWrite _questionRepositoryWrite;

        public LikeQuestionCommandHandler(IMediator mediator, IQuestionRepositoryWrite questionRepositoryWrite)
        {
            _mediator = mediator;
            _questionRepositoryWrite = questionRepositoryWrite;
        }

        public async Task<bool> Handle(LikeQuestionCommand request, CancellationToken cancellationToken)
        {
            if (!ValidadeCommand(request)) return false;

            await _questionRepositoryWrite.LikeQuestionAsync(request.QuestionId, request.TypePost);

            return true;
        }

        private bool ValidadeCommand(Command request)
        {
            if (request.IsValid()) return true;

            foreach (var error in request.ValidationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(error.ErrorMessage));
            }

            return false;
        }
    }
}
