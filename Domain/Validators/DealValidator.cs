using DealManagementSystem.Domain.Models;
using FluentValidation;

namespace DealManagementSystem.Domain.Validators;

public class DealValidator : AbstractValidator<Deal>
{
    public DealValidator()
    {
        RuleFor(d => d.Slug)
            .NotEmpty().WithMessage("Deal slug is required.")
            .NotNull().WithMessage("Deal slug is required.");
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Deal name is required.")
            .NotNull().WithMessage("Deal name is required.");
        RuleFor(d => d.Hotels)
                .NotEmpty().WithMessage("Deal must have at least one hotel.")
                .NotNull().WithMessage("Deal hotels cannot be null.")
                .Must(hotels => hotels.Count > 0).WithMessage("Deal must have at least one hotel.");
        RuleForEach(d => d.Hotels).SetValidator(new HotelValidator());
    }
}