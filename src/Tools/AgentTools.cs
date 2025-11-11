using System.ComponentModel;

namespace DevUIDemo.Tools;

public class AgentTools
{
    private readonly IHttpClientFactory _httpClientFactory;
    public AgentTools(IHttpClientFactory httpClientClientFactory)
    {
        _httpClientFactory = httpClientClientFactory;
    }

    [Description("Gets a weather forecast based on the city in JSON format.")]
    public async Task<string> GetWeatherForecast(
    [Description("Name of the city to get the weather forecast")] string city)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var url = $"https://wttr.in/{Uri.EscapeDataString(city)}?format=3";
        try
        {
            var response = await httpClient.GetStringAsync(url);
            return response;
        }
        catch (Exception ex)
        {
            return $"Error fetching weather data: {ex.Message}";
        }
    }
}