using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.Contracts.Categories;
using FluentValidation;
using FluentValidation.Results;

namespace BulletinBoard.AppServices.Validators;

public class CreateCategoryValidator(ICategoryService categoryService) : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage("Невозможное имя категории.");
        RuleFor(x => x.ParentCategoryId).MustAsync(async id => await ParentCategoryIdIsValid(id)).WithMessage("Родительская категория должна существовать");
    }

    private async Task<bool> ParentCategoryIdIsValid(Guid? parentCategoryId)
    {
        var parentCategory = await categoryService.GetCategoryByIdAsync(parentCategoryId, CancellationToken.None);
    }
}