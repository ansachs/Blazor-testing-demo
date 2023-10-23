using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ServerTest.Controllers;

public class WeatherForecastControllerTest: IClassFixture<WebApplicationFactory<Program>>
{
    
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    /*
     * as a user
     * when I click Fetch Data
     * it should display the weather data
     */
    
    /*
     * as is this test requires the program reference in the server project to be public, ie the final line in the program.cs file.
     * we should investigate if there are alternatives to this
     */
    
    [Fact]
    public async void Get_WeatherForecast_Success()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }
    
}