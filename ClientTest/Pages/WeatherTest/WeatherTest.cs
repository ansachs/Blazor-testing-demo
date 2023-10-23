using System;
using System.Collections.Generic;
using Bunit;
using DemoTesting.Client.Common.Client;
using DemoTesting.Client.Pages.Weather;
using DemoTesting.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ClientTest.Pages.WeatherTest;

public class WeatherTest: TestContext
{
    
    private ITestOutputHelper _testOutputHelper;
    private Action _action;

    public WeatherTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    /*
     * as a user
     * when I click Fetch Data
     * it should display the weather data
     */
    
    /*
     * note mocking http response is one way also can setup a mock server and integration test through http client
     */

    [Fact]
    public void ClickOnFetchData_DisplayWeatherData()
    {
        var mockClient = new Mock<IWeatherClient>();
        var mockWeatherData = new List<WeatherForecast>(){ new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = "Tornadoes"
            }
        }.ToArray();
        
        mockClient.Setup(weatherClient => weatherClient.GetWeatherData())
            .ReturnsAsync(mockWeatherData);
        
        Services.AddSingleton(mockClient.Object);
        
        var cut = RenderComponent<FetchData>();
        var forecastTable = cut.Find(".table");
        
        _testOutputHelper.WriteLine(forecastTable.InnerHtml);
        forecastTable.InnerHtml.Should().ContainAll("Date", "Temp", "Summary", "Tornadoes");
    }
    
}