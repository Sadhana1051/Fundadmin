using FundAdministration.Api.DTOs;
using FluentValidation;

namespace FundAdministration.Api.Validators;

public class InvestorUpdateDtoValidator : AbstractValidator<InvestorUpdateDto>
{
    public InvestorUpdateDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
    }
}
