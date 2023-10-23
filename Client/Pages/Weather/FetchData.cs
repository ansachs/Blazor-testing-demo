using DemoTesting.Shared;

namespace DemoTesting.Client.Pages.Weather;

public partial class FetchData
{
    private WeatherForecast[]? _forecasts;
    

    protected override async Task OnInitializedAsync()
    {
        _forecasts = await WeatherClient.GetWeatherData();
    }
    
}