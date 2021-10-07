using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleJoining.Models;

namespace MultipleJoining
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthsController : ControllerBase
    {
        private readonly ModelContext _context;

        public MonthsController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Months
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Month>>> GetMonths()
        {
            return await _context.Months.ToListAsync();
        }

        // GET: api/Months/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Month>> GetMonth(decimal? id)
        {
            var month = await _context.Months.FindAsync(id);

            if (month == null)
            {
                return NotFound();
            }

            return month;
        }

        // PUT: api/Months/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonth(decimal? id, Month month)
        {
            if (id != month.Mid)
            {
                return BadRequest();
            }

            _context.Entry(month).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonthExists(id))
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

        // POST: api/Months
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Month>> PostMonth(Month month)
        {
            _context.Months.Add(month);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MonthExists(month.Mid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMonth", new { id = month.Mid }, month);
        }

        // DELETE: api/Months/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonth(decimal? id)
        {
            var month = await _context.Months.FindAsync(id);
            if (month == null)
            {
                return NotFound();
            }

            _context.Months.Remove(month);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonthExists(decimal? id)
        {
            return _context.Months.Any(e => e.Mid == id);
        }
    }
}
