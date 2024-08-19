using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using audBackEnd.Models;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.Identity.Client;


namespace audBackEnd.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class FetcherController : Controller
    {
        private readonly CurrenciesDBContext _context;
    
        public FetcherController(CurrenciesDBContext context)
        {
            _context = context;
        }

        // GET: api/Fetcher
        [HttpGet]
        // public async Task<ActionResult<CurrencyRates>> FetchRateContent()
        public async Task<IActionResult> FetchRateContent()
        {
            // Fetch json from public api
            string url = "http://api.currencyapi.com/v3/latest"; 
            string apikey = "fca_live_fcxICI1hMR8xzFktbwu0P9mDaJlCwwgHpcHhiUsY";
            string content = "";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string tar = url+"?apikey="+apikey+"&currencies=&base_currency=AUD";
                    Console.WriteLine("target url:{0}", tar);
                    HttpResponseMessage response = await client.GetAsync(tar);
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
            // var jsonData = parsedJson.RootElement.GetRawText();
            // var jsonHeader = parsedJson.RootElement.GetProperty("meta").GetRawText();    

           CurrencyRates currency = JsonConvert.DeserializeObject<CurrencyRates>(content); 
           Console.Write(content); 
           var maxId = _context.CurrencyItems.Count();
           var id = maxId;
           if (currency != null)
           {
            foreach (var pair in currency.Data.Values)
            {    
                    var newItem = new CurrencyItem(){
                        ID = id + 1,
                        Timestamp=DateTimeOffset.UtcNow,
                        Name= pair.Code,
                        moneyCode = pair.Code,
                        baseValue = 1,
                        value = pair.Value,
                    };
                    _context.CurrencyItems.Add(newItem);
                    await _context.SaveChangesAsync();
            }
           }
          
           return Json(currency);


            // CurrencyRates currencyRates = JsonSerializer.Deserialize<CurrencyRates>(jsonData);
            
            // Console.WriteLine($"Last Updated At: {currency.Meta.LastUpdatedAt}");
            // return View(currency);
        }
    }
}