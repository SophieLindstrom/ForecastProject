using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_01.Models;
using Assignment_A1_01.Services;

namespace Assignment_A1_01
{
    class Program
    {
        static void Main(string[] args)
        {
            double latitude = 59.5086798659495;
            double longitude = 18.2654625932976;

            OpenWeatherService service = new OpenWeatherService();

            Task<Forecast> t1 = service.GetForecastAsync(latitude, longitude);

            Task.WaitAll(t1);

            Console.WriteLine("-----------------");
            if (t1?.Status == TaskStatus.RanToCompletion)
            {
                Forecast forecast = t1.Result;
                Console.WriteLine($"Weather forecast for {forecast.City}");
                var GroupedList = forecast.Items.GroupBy(item => item.DateTime.Date);

                foreach (IGrouping<DateTime, ForecastItem> group in GroupedList)
                {
                    Console.WriteLine(group.Key.Date.ToShortDateString());
                    foreach (ForecastItem item in group)
                    {
                        Console.WriteLine($"   - {item.DateTime.ToShortTimeString()}: {item.Description}, temperature: {item.Temperature} degC, wind: {item.WindSpeed} m/s");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Geolocation weather service error.");
            }

        }
    }
}
