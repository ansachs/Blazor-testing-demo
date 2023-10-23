using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DemoTesting.Client.Common.Client;
using DemoTesting.Shared;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ClientTest.Common.Client;


public class WeatherClientTest: IDisposable
{
    public WireMockServer Server { get; private set; } = WireMockServer.Start(9876);

    public void Dispose()
    {
        Server.Stop();
    }
    
    /*
     * as a user
     * when I click Fetch Data
     * it should display the weather data
     *
     */

    [Fact]
    public async Task GetWeatherData_ReturnsWeatherData()
    {
        CreateWeatherInfoStub();

        var mockHttpClient = new HttpClient {BaseAddress = new Uri("http://localhost:" + Server.Port)};
        var weatherClient = new WeatherClient(mockHttpClient);

        var weatherData = await weatherClient.GetWeatherData();

        Assert.Contains(weatherData, w => w.Summary == "too hot");
    }

    private void CreateWeatherInfoStub()
    {
        var weatherForecast = new List<WeatherForecast>()
        {
            new()
            {
                Summary = "too hot"
            }
        }.ToArray();
        
        Server.Given(
                Request.Create().WithPath("/WeatherForecast").UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "text/plain")
                    .WithBody(JsonConvert.SerializeObject(weatherForecast))
            );
    }
}