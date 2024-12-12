﻿using System.Net.Http.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;

namespace BusinessLogicLayer.HttpClients;

public class ProductsMicroserviceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductsMicroserviceClient> _logger;

    public ProductsMicroserviceClient(HttpClient httpClient, ILogger<ProductsMicroserviceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProductDTO?> GetProductByProductID(Guid id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/products/search/product-id/{id}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException(
                        "Bad request",
                        null,
                        System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    throw new HttpRequestException(
                        $"Http request failed with status code {response.StatusCode}"
                    );
                }
            }

            ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>();
            if (product == null)
                throw new ArgumentException("Invalid Product ID");

            return product;
        }
        catch (BulkheadRejectedException ex)
        {
            _logger.LogError(ex, "Bulkhead isolation blocks the request since " +
                                 "the request queue is full");

            return new ProductDTO(
                ProductID: Guid.Empty,
                ProductName: "Temporarily Unavailable (Bulkhead)",
                Category: "Temporarily Unavailable (Bulkhead)",
                UnitPrice: 0,
                QuantityInStock: 0);
        }
    }
}