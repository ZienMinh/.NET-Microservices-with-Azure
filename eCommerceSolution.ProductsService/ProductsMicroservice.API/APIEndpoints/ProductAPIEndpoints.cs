using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;

namespace ProductsMicroservice.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MappProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //GET /api/products
        app.MapGet("/api/products", async (
            IProductsService productsService) =>
        {
            List<ProductResponse?> products = await productsService.GetProducts();

            return Results.Ok(products);
        });

        //GET /api/products/search/product-id
        app.MapGet("/api/products/search/product-id/{id:guid}", async (
            IProductsService productsService,
            Guid id) =>
        {
            await Task.Delay(100);
            throw new NotImplementedException();

            ProductResponse? product = await productsService.GetProductByCondition(
                temp => temp.ProductID == id);

            if (product == null)
                return Results.NotFound();

            return Results.Ok(product);
        });

        //GET /api/products/search
        app.MapGet("/api/products/search/{SearchString}", async (
            IProductsService productsService,
            string SearchString) =>
        {
            List<ProductResponse?> productsByProductName = await
                productsService.GetProductsByCondition(
                    temp => temp.ProductName != null && temp.ProductName.Contains
                        (SearchString, StringComparison.OrdinalIgnoreCase));

            List<ProductResponse?> productsByCategory = await
                productsService.GetProductsByCondition(
                    temp => temp.Category != null &&
                            temp.Category.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            var product = productsByProductName.Union(productsByCategory);

            return Results.Ok(product);
        });

        //POST /api/products
        app.MapPost("/api/products", async (
            IProductsService productsService,
            IValidator<ProductAddRequest> productAddRequestValidator,
            ProductAddRequest productAddRequest) =>
        {
            //Validate the ProductAddRequest object using FluentValidation
            ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

            //Check the validation result
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                    .ToDictionary(grp => grp.Key, grp => grp
                        .Select(err => err.ErrorMessage)
                        .ToArray());

                return Results.ValidationProblem(errors);
            }

            var addedProductResponse = await productsService.AddProduct(productAddRequest);
            if (addedProductResponse != null)
                return Results.Created($"/api/products/search/product-id/" +
                                       $"{addedProductResponse.ProductID}",
                    addedProductResponse);
            else
                return Results.Problem("Error in adding product");
        });

        //PUT /api/products
        app.MapPut("/api/products", async (
            IProductsService productsService,
            IValidator<ProductUpdateRequest> productUpdateRequestValidator,
            ProductUpdateRequest productUpdateRequest) =>
        {
            //Validate the ProductAddRequest object using FluentValidation
            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            //Check the validation result
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                    .ToDictionary(grp => grp.Key, grp => grp
                        .Select(err => err.ErrorMessage)
                        .ToArray());

                return Results.ValidationProblem(errors);
            }

            var updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);
            if (updatedProductResponse != null)
                return Results.Ok(updatedProductResponse);
            else
                return Results.Problem("Error in updating product");
        });

        //DELETE /api/products
        app.MapDelete("/api/products/{id:guid}", async (
            IProductsService productsService,
            Guid id) =>
        {
            bool isDeleted = await productsService.DeleteProduct(id);
            if (isDeleted)
                return Results.Ok(true);
            else
                return Results.Problem("Error in deleting product");
        });

        return app;
    }
}