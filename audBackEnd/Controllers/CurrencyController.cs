using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

using audBackEnd.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace audBackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : Controller
{
    private readonly CurrenciesDBContext _context;

    public CurrencyController(CurrenciesDBContext context)
    {
        _context = context;
    }

    // GET: api/Currency
    [HttpGet]
    public async Task<IActionResult> CurrencyList()
    {
        var currencyItems = await _context.CurrencyItems.ToListAsync();
        return View(currencyItems);
    }

    // GET: api/Currency/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CurrencyItem>> CurrencyDetails(int id)
    {
        var currencyItem = await _context.CurrencyItems.FindAsync(id);

        if (currencyItem == null)
        {
            return NotFound();
        }

        return View(currencyItem);
    }

    // GET: api/fetchratecontent
     [HttpGet("fetchratecontent")]
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

        // var parsedJson = System.Text.Json.JsonDocument.Parse(content);

        // var jsonData = parsedJson.RootElement.GetRawText();
        // var jsonHeader = parsedJson.RootElement.GetProperty("meta").GetRawText();    

        CurrencyRates currency = JsonConvert.DeserializeObject<CurrencyRates>(content); 
        Console.Write(content); 
        var maxId = _context.CurrencyItems.Max(ci => (int?)ci.ID) ?? 0;
        var newId = maxId;
        if (currency != null)
        {
            foreach (var pair in currency.Data.Values)
            {    
                newId += 1;
                var newItem = new CurrencyItem(){
                    ID = newId,
                    Timestamp=DateTimeOffset.UtcNow,
                    Name= pair.Code,
                    moneyCode = pair.Code,
                    baseValue = 1,
                    value = pair.Value,
                };

                var existingItem = await _context.CurrencyItems.FindAsync(newItem.ID);
                if (existingItem == null)
                {
                    _context.CurrencyItems.Add(newItem);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Insert currency of: {0}: {1}--{2}", newItem.ID, newItem.Name, newItem.value);
                }
                else{
                    Console.WriteLine("id dulicated, insert failed, current maxium id: {0}", maxId);
                }
            }
        }
        
        return Json(currency);


        // CurrencyRates currencyRates = JsonSerializer.Deserialize<CurrencyRates>(jsonData);
        
        // Console.WriteLine($"Last Updated At: {currency.Meta.LastUpdatedAt}");
        // return View(currency);
    }
 

    // PUT: api/Currency/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<CurrencyItem>> CurrencyEdit(int id, CurrencyItem currencyitem)
    {
        if (id != currencyitem.ID)
        {
            return BadRequest();
        }

        _context.Entry(currencyitem).State = EntityState.Modified;

        try
        {
            var item = await _context.SaveChangesAsync();
            var display = await _context.CurrencyItems.FindAsync(id);

            if (display == null)
            {
                return NotFound();
            }

            return View(display);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CurrencyItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        
    }

    // POST: api/Currency
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CurrencyItem>> CurrencyCreate(CurrencyItem currencyitem)
    {
        _context.CurrencyItems.Add(currencyitem);
        await _context.SaveChangesAsync();

        return CreatedAtAction("CurrencyDetails", new { id = currencyitem.ID }, currencyitem);
    }

    // DELETE: api/Currency/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCurrencyItem(int id)
    {
        var currencyItem = await _context.CurrencyItems.FindAsync(id);
        if (currencyItem == null)
        {
            return NotFound();
        }

        _context.CurrencyItems.Remove(currencyItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CurrencyItemExists(int id)
    {
        return _context.CurrencyItems.Any(e => e.ID == id);
    }
}