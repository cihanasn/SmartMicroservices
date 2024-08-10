using BuildingBlocks.CQRS;
using FluentValidation;
using Test.API.Models;

namespace Test.API.Products.StoreProduct;

public class StoreProductHandler
{
    public record StoreProductCommand(Product Product) : ICommand<StoreProductResult>;
    public record StoreProductResult(Guid Id);

    public class StoreProductCommandValidator : AbstractValidator<StoreProductCommand>
    {
        public StoreProductCommandValidator() 
        {
            RuleFor(x => x.Product).NotNull().WithMessage("Product can not be null");
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    public class StoreProductCommandHandler() : ICommandHandler<StoreProductCommand, StoreProductResult>
    {
        public async Task<StoreProductResult> Handle(StoreProductCommand command, CancellationToken cancellationToken)
        { 
            command.Product.Id = Guid.NewGuid();

            return new StoreProductResult(command.Product.Id);
        }
    }
}
