﻿using System.Net.Http.Json;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.HttpClients;

public class UsersMicroserviceClient
{
    private readonly HttpClient _httpClient;

    public UsersMicroserviceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDTO?> GetUserByUserID(Guid id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/users/{id}");

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
        
        UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
        if (user == null)
            throw new ArgumentException("Invalid User ID");
        
        return user;
    }
}