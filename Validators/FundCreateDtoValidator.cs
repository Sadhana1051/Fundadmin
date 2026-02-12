using FundAdministration.Api.DTOs;
using FluentValidation;

namespace FundAdministration.Api.Validators;

public class FundCreateDtoValidator : AbstractValidator<FundCreateDto>
{
    public FundCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}
