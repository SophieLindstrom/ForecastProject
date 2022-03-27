using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_01.Models;

namespace Assignment_A1_01.Services
{
    public class OpenWeatherService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "9516011d36968c6458853eec14a51b3f"; // Your API Key
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
                //https://openweathermap.org/current
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?" +
                $"lat={latitude}" +
                $"&lon={longitude}" +
                $"&units=metric" +
                $"&lang={language}" +
                $"&appid={apiKey}";

                //Read the response from the WebApi
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

                //Your Code to convert WeatherApiData to Forecast using Linq.

                Forecast forecast = new Forecast();


                forecast.Items = new List<ForecastItem>();

                wd.list.ForEach(wdListItem => { forecast.Items.Add(GetForecastItem(wdListItem)); });

               

                return forecast;
       
        }

        private ForecastItem GetForecastItem(List wdListItem) {

            ForecastItem item = new ForecastItem();
            item.DateTime = UnixTimeStampToDateTime(wdListItem.dt);

            item.Temperature = wdListItem.main.temp;
            item.Description = wdListItem.weather.Count > 0 ? wdListItem.weather.First().description : "No info";
            item.WindSpeed = wdListItem.wind.speed;

            return item;
        }
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
