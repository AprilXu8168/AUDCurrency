using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using audBackEnd.Models;

namespace audBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrenciesDBContext _context;

        public CurrencyController(CurrenciesDBContext context)
        {
            _context = context;
        }

        // GET: api/Currency
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyItem>>> GetCurrencyItems()
        {
            return await _context.CurrencyItems.ToListAsync();
        }

        // GET: api/Currency/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyItem>> GetCurrencyItem(int id)
        {
            var currencyItem = await _context.CurrencyItems.FindAsync(id);

            if (currencyItem == null)
            {
                return NotFound();
            }

            return currencyItem;
        }

        // PUT: api/Currency/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrencyItem(int id, CurrencyItem currencyItem)
        {
            if (id != currencyItem.Timestamp)
            {
                return BadRequest();
            }

            _context.Entry(currencyItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Currency
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CurrencyItem>> PostCurrencyItem(CurrencyItem currencyItem)
        {
            _context.CurrencyItems.Add(currencyItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurrencyItem", new { id = currencyItem.Timestamp }, currencyItem);
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
            return _context.CurrencyItems.Any(e => e.Timestamp == id);
        }
    }
}
