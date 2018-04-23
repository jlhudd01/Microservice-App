using FluentValidation;

namespace OrderWebAPI.Commands.Validators
{
    public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<RemoveOrderItemFromOrderCommand, bool>>
    {
        public IdentifiedCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}