
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
    public class PaysController : Controller
    {
        private readonly UserAdressContext _context;

        public PaysController(UserAdressContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pays>>> GetPays()
        {
            // return await _context.Pays.ToListAsync();
            return await _context.Pays.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pays>> GetPays(long id)
        {
            var Pays = await _context.Pays.FindAsync(id);

            if (Pays == null)
            {
                return NotFound();
            }

            return Pays;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPays(long id, Pays Pays)
        {
            if (id != Pays.ID)
            {
                return BadRequest();
            }

            _context.Entry(Pays).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaysExists(id))
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
        public async Task<ActionResult<Pays>> PostPays(Pays Pays)
        {
            _context.Pays.Add(Pays);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPays", new { id = Pays.Id }, Pays);
            return CreatedAtAction(nameof(GetPays), new { id = Pays.ID }, Pays);
        }

        // DELETE: NiovarRH/PaysMicroservices/Pays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePays(int id)
        {
            var Pays = await _context.Pays.FindAsync(id);
            if (Pays == null)
            {
                return NotFound();
            }

            _context.Pays.Remove(Pays);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaysExists(long id)
        {
            return _context.Pays.Any(e => e.ID == id);
        }


        // les provinces d'un pays
        [HttpGet("ProvincesPays/{paysid}")]
        public async Task<ActionResult<IEnumerable<Province>>> ProvincesPays(int paysid)
        {
            var Provinces = _context.Province
                .Where(b => b.paysID == paysid);

            if (Provinces == null)
                return NotFound();

            return await Provinces.ToListAsync();
        }
    }
}

