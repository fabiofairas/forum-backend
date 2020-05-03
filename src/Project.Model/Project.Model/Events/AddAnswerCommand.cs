using FluentValidation;
using Project.Model.Commands;

namespace Project.Model.Events
{

    public class AddAnswerCommand : Command
    {
        public string Id { get; private set; }
        public string QuestionId { get; private set; }
        public string User { get; private set; }
        public string Text { get; private set; }

        public AddAnswerCommand(string id, string user, string text, string questionId)
        {
            Id = id;
            QuestionId = questionId;
            User = user;
            Text = text;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddAnswerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddAnswerCommandValidation : AbstractValidator<AddAnswerCommand>
    {
        public AddAnswerCommandValidation()
        {
            RuleFor(c => c.QuestionId)
               .NotEmpty()
               .WithMessage("Campo QuestionId inválido");

            RuleFor(c => c.Text)               
               .Length(20, 200)
               .WithMessage("Campo text inválido, o campo deve possuir entre 20 e 200 caracteres");

            RuleFor(c => c.User)               
               .Length(5, 20)
               .WithMessage("Campo user inválido, o campo deve possuir entre 10 e 20 caracteres");
        }
    }
}
