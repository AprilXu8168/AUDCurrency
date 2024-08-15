using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using audBackEnd.Models;
using System.Text.Json;


namespace audBackEnd.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class FetcherController : Controller
    {
        // GET: api/Fetcher
        [HttpGet]
        public async Task<ActionResult<CurrencyRates>> FetchRateContent()
        {
            string url = "http://api.freecurrencyapi.com/v1/latest"; 
            string apikey = "fca_live_fcxICI1hMR8xzFktbwu0P9mDaJlCwwgHpcHhiUsY";
            string content = "";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url+"?apikey="+apikey+"&currencies=&base_currency=AUD");
                    response.EnsureSuccessStatusCode();
                    content = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    // Handle errors, log them, etc.
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                    content = "Error fetching website content.";
                }
            }

            // Pass the content to a view or return as a JSON response
            ViewBag.Content = content;

            var parsedJson = JsonDocument.Parse(content);
            var jsonData = parsedJson.RootElement.GetProperty("data").GetRawText();
            Console.WriteLine(content);
            var currencyRates = JsonSerializer.Deserialize<CurrencyRates>(jsonData);
            
            Console.WriteLine($"AUD: {currencyRates.AUD}");
            Console.WriteLine($"USD: {currencyRates.USD}");
            return currencyRates;
        }
    }
}