using System.Net.Http.Json;
using DemoTesting.Shared;
using RestSharp;

namespace DemoTesting.Client.Common.Client;

/*
 * if interface only represents one class, seems ok to put in same file
 */
public interface IWeatherClient
{
    Task<WeatherForecast[]?> GetWeatherData();
}

public class WeatherClient : IWeatherClient
{
    
    readonly HttpClient _client;

    public WeatherClient(HttpClient httpClient)
    {
        _client = httpClient;
    }
    
    public async Task<WeatherForecast[]?> GetWeatherData()
    {
        var response = await _client.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast") 
               ?? Enumerable.Empty<WeatherForecast>().ToArray();
        return response;
    }
    
}