## setup 
#### all projects target dotnet 7

```
dotnet new blazorwasm --hosted -o DemoTesting (will create a client served by a server in one)

dotnet new xunit -o ClientTest
 dotnet sln add ClientTest
 
dotnet cd Client
dotnet add package restsharp
cd ..

dotnet cd ClientTest
dotnet add package moq
dotnet add package Newtonsoft.Json
dotnet add package bunit
dotnet add package FluentAssertions
dotnet add package WireMock.Net
cd ..

dotnet new xunit -o ClientTest
dotnet new nunit -o ServerTest (trying out different test lib)
dotnet sln add ClientTest ServerTest

(right click on ClientTest/ServerTest and reference to the project being tested)
```
## Testing conventions
* separate projects for testing
  * code separation, easier to run based on comments


## Testing examples
* bunit has a limited query language as compared to react testing library
  * complex ui tests maybe better served with an e2e framework
```
var cut = RenderComponent<Counter>();
        
        countDisplay = cut.Find("[role='status']");
        countDisplay.InnerHtml.Should().Contain("Current count: 0");
```
* mocking with moq works best when injecting interfaces, that way you dont need to explicitly add the  
dependencies for the ```WeatherClient``` in the below example
``` 
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
```
* used wiremock to test the restclient calls from the client application. Given the pain in setting these up, we may prefer  
to use an e2e library like playwright
```
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
```


## Application Structure
* separate view and backing class through use of ```partial class``` and same naming

## Additional References
[microsoft guide to integration testing](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0)

some useful projects for reference:
- https://github.com/DamianEdwards/MinimalApiPlayground/tree/main