using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

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
    public async  Task<IActionResult> CurrencyList()
    {
        // return await _context.CurrencyItems.ToListAsync();
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

    // PUT: api/Currency/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<CurrencyItem>> CurrencyEdit(int id, CurrencyItem currencyItem)
    {
        if (id != currencyItem.ID)
        {
            return BadRequest();
        }

        _context.Entry(currencyItem).State = EntityState.Modified;

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
    public async Task<ActionResult<CurrencyItem>> CurrencyCreate(CurrencyItem currencyItem)
    {
        _context.CurrencyItems.Add(currencyItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction("CurrencyDetails", new { id = currencyItem.ID }, currencyItem);
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