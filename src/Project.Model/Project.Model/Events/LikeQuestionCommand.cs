using FluentValidation;
using Project.Model.Commands;
using Project.Model.Enums;

namespace Project.Model.Events
{
    public class LikeQuestionCommand : Command
    {
        public string QuestionId { get; private set; }
        public ETypePost TypePost { get; private set; }

        public LikeQuestionCommand(string questionId, ETypePost typePost)
        {
            QuestionId = questionId;
            TypePost = typePost;
        }

        public override bool IsValid()
        {
            ValidationResult = new LikeQuestionCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class LikeQuestionCommandValidation : AbstractValidator<LikeQuestionCommand>
    {
        public LikeQuestionCommandValidation()
        {
            RuleFor(c => c.QuestionId)
               .NotEmpty()
               .WithMessage("Campo QuestionId inválido");
        }
    }
}
