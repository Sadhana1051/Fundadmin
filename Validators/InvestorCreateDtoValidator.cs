using FundAdministration.Api.DTOs;
using FluentValidation;

namespace FundAdministration.Api.Validators;

public class InvestorCreateDtoValidator : AbstractValidator<InvestorCreateDto>
{
    public InvestorCreateDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.FundId).NotEmpty();
    }
}
