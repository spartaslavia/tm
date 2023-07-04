using FluentValidation;
using TranslationManagement.Api.Core.Models.Dto;

namespace TranslationManagement.Api.Core.Validators
{
    public class TranslatorDtoValidator : AbstractValidator<TranslatorDto>
    {
        public TranslatorDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
