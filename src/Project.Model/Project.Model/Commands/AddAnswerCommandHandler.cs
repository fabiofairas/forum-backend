using MediatR;
using Project.Model.Entities;
using Project.Model.Events;
using Project.Model.Notifications;
using Project.Model.Repository.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Model.Commands
{
    public class AddAnswerCommandHandler : IRequestHandler<AddAnswerCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IQuestionRepositoryWrite _questionRepositoryWrite;

        public AddAnswerCommandHandler(IMediator mediator, IQuestionRepositoryWrite questionRepositoryWrite)
        {
            _mediator = mediator;
            _questionRepositoryWrite = questionRepositoryWrite;
        }

        public async Task<bool> Handle(AddAnswerCommand request, CancellationToken cancellationToken)
        {
            if (!ValidadeCommand(request)) return false;

            var answer = new Answer()
            { 
                Id = request.Id,
                QuestionId = request.QuestionId,
                User = request.User,
                Text = request.Text
            };

            await _questionRepositoryWrite.AddAnswerAsync(answer);

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
