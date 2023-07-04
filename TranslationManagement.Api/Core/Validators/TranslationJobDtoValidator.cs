using FluentValidation;
using TranslationManagement.Api.Core.Models.Dto;

namespace TranslationManagement.Api.Core.Validators
{
    public class TranslationJobDtoValidator : AbstractValidator<TranslatorDto>
    {
        public TranslationJobDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
