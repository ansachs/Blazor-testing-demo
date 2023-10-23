## setup 
#### Instructions for running
* install dotnet 7
* clone project
* open project and run dotnet restore, make sure no errors
* to run Server
  * ```cd Server; dotnet run```
* to run Tests
  * run from IDE
  * run from terminal: ```dotnet test```

#### How this project was built
```
dotnet new blazorwasm --hosted -o DemoTesting (will create a client served by a server in one)

dotnet new xunit -o ClientTest
dotnet new xunit -o ServerTest 
dotnet sln add ClientTest ServerTest

cd ClientTest
dotnet add package moq
dotnet add package Newtonsoft.Json
dotnet add package bunit
dotnet add package FluentAssertions
dotnet add package WireMock.Net
cd ..

cd Server
dotnet add package Microsoft.AspNetCore.Mvc.Testing
cd ..

(right click on ClientTest/ServerTest and add reference to the project being tested)
```
## Testing libraries
#### moq - https://github.com/devlooped/moq
* used to create proxy around interfaces and classes inorder to assign our own behavior

#### Newtonsoft.Json
* json parsing library, use here for converting object to json

#### bunit - https://bunit.dev/
* testing utilities for razor/blazor pages, allowing us to render in a virtual dom and assert on the html.

#### FluentAssertions
* ability to write inline assertions, ie taking any value and adding ```.Should()``` 
* optional, consider using this if people like it

#### WireMock.net
* create a mock local server and mock out api response

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

* to test the controller layer, use the dotnet WebApplicationFactory to create an integration test HttpClient. Needs to reference the program.cs in the
project under test
```
public class WeatherForecastControllerTest: IClassFixture<WebApplicationFactory<Program>>
{
    
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
```


## Application Structure
* separate view and backing class through use of ```partial class``` and same naming

## Additional References
[microsoft guide to integration testing](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0)

some useful projects for reference:
- https://github.com/DamianEdwards/MinimalApiPlayground/tree/main