using FundAdministration.Api.DTOs;
using FluentValidation;

namespace FundAdministration.Api.Validators;

public class TransactionCreateDtoValidator : AbstractValidator<TransactionCreateDto>
{
    public TransactionCreateDtoValidator()
    {
        RuleFor(x => x.InvestorId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Transaction amount must be positive");
        RuleFor(x => x.Type).IsInEnum();
    }
}
