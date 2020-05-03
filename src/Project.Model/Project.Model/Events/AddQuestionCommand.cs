using FluentValidation;
using Project.Model.Commands;

namespace Project.Model.Events
{
    public class AddQuestionCommand : Command
    {
        public string Id { get; private set; }
        public string User { get; private set; }
        public string Text { get; private set; }

        public AddQuestionCommand(string id, string user, string text)
        {
            User = user;
            Text = text;
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddQuestionCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddQuestionCommandValidation : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidation()
        {
            RuleFor(c => c.Text)               
               .Length(20, 200)
               .WithMessage("Campo text inválido, o campo deve possuir entre 20 e 200 caracteres");

            RuleFor(c => c.User)               
               .Length(5, 20)
               .WithMessage("Campo user inválido, o campo deve possuir entre 10 e 20 caracteres");
        }
    }
}
