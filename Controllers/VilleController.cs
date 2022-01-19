
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NRH_UserAdress.Models;
using NRH_UserAdress.Rabbit;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore.SqlServer;
using Newtonsoft.Json.Linq;

namespace NRH_UserAdress.Controllers
{
    [Route("NiovarRH/UserAdressMicroservices/[controller]")]
    [ApiController]
    public class VilleController : Controller
    {
        private readonly UserAdressContext _context;

        public VilleController(UserAdressContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ville>>> GetVille()
        {
            // return await _context.Ville.ToListAsync();
            return await _context.Ville.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ville>> GetVille(long id)
        {
            var Ville = await _context.Ville.FindAsync(id);

            if (Ville == null)
            {
                return NotFound();
            }

            return Ville;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVille(long id, Ville Ville)
        {
            if (id != Ville.ID)
            {
                return BadRequest();
            }

            _context.Entry(Ville).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VilleExists(id))
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

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ville>> PostVille(Ville Ville)
        {
            _context.Ville.Add(Ville);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetVille", new { id = Ville.Id }, Ville);
            return CreatedAtAction(nameof(GetVille), new { id = Ville.ID }, Ville);
        }

        // DELETE: NiovarRH/VilleMicroservices/Ville/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVille(int id)
        {
            var Ville = await _context.Ville.FindAsync(id);
            if (Ville == null)
            {
                return NotFound();
            }

            _context.Ville.Remove(Ville);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VilleExists(long id)
        {
            return _context.Ville.Any(e => e.ID == id);
        }
    }
}

