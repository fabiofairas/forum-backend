using MediatR;
using Project.Model.Entities;
using Project.Model.Events;
using Project.Model.Notifications;
using Project.Model.Repository.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Model.Commands
{
    public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IQuestionRepositoryWrite _questionRepositoryWrite;        

        public AddQuestionCommandHandler(IMediator mediator, IQuestionRepositoryWrite questionRepositoryWrite)
        {
            _mediator = mediator;
            _questionRepositoryWrite = questionRepositoryWrite;
        }

        public async Task<bool> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            if (!ValidadeCommand(request)) return false;

            var question = new Question() 
            { 
                Id = request.Id,
                User = request.User,
                Text = request.Text
            };

            await _questionRepositoryWrite.AddQuestionAsync(question);

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