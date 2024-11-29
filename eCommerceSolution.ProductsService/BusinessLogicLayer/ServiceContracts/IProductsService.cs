﻿using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
    /// <summary>
    /// Retrieves the list of products from the products repository
    /// </summary>
    /// <returns>Returns list of ProductResponse objects</returns>
    Task<List<ProductResponse?>> GetProducts();

    /// <summary>
    /// Retrieves list of products matching with given condition
    /// </summary>
    /// <param name="conditionExpression">Expression that represents condition to check</param>
    /// <returns>Returns matching product</returns>
    Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Returns a single product that matches with given condition
    /// </summary>
    /// <param name="conditionExpression">Express that represents the condition to check</param>
    /// <returns>Returns matching product or null</returns>
    Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Add (inserts) product into the table using products repository
    /// </summary>
    /// <param name="productAddRequest">Product to insert</param>
    /// <returns>Product after inserting or null if unsuccessful</returns>
    Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);

    /// <summary>
    /// Updates the existing product based on the ProductID
    /// </summary>
    /// <param name="productAddRequest">Product data to update</param>
    /// <returns>Returns product object after successful updation; otherwise null</returns>
    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);

    /// <summary>
    /// Delete an existing product based on given ProductID
    /// </summary>
    /// <param name="productID">ProductID to search and delete</param>
    /// <returns>Returns true if the deletion is successful; otherwise false</returns>
    Task<bool> DeleteProduct(Guid productID);
}