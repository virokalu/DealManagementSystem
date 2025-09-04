using DealManagementSystem.Domain.Models;
using FluentValidation;

namespace DealManagementSystem.Domain.Validators;

public class HotelValidator : AbstractValidator<Hotel>
{
    public HotelValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Hotel name is required.")
            .NotNull().WithMessage("Hotel name is required.");
    }
}