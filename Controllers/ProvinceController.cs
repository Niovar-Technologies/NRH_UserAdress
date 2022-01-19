
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
    public class ProvinceController : Controller
    {
        private readonly UserAdressContext _context;

        public ProvinceController(UserAdressContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Province>>> GetProvince()
        {
            // return await _context.Province.ToListAsync();
            return await _context.Province.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Province>> GetProvince(long id)
        {
            var Province = await _context.Province.FindAsync(id);

            if (Province == null)
            {
                return NotFound();
            }

            return Province;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvince(long id, Province Province)
        {
            if (id != Province.ID)
            {
                return BadRequest();
            }

            _context.Entry(Province).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinceExists(id))
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
        public async Task<ActionResult<Province>> PostProvince(Province Province)
        {
            _context.Province.Add(Province);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetProvince", new { id = Province.Id }, Province);
            return CreatedAtAction(nameof(GetProvince), new { id = Province.ID }, Province);
        }

        // DELETE: NiovarRH/ProvinceMicroservices/Province/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvince(int id)
        {
            var Province = await _context.Province.FindAsync(id);
            if (Province == null)
            {
                return NotFound();
            }

            _context.Province.Remove(Province);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProvinceExists(long id)
        {
            return _context.Province.Any(e => e.ID == id);
        }

        // les villes d'une province
        [HttpGet("VillesProvince/{provinceid}")]
        public async Task<ActionResult<IEnumerable<Ville>>> PaysProvinces(int provinceid)
        {
            var Villes = _context.Ville
                .Where(b => b.ProvinceID == provinceid);

            if (Villes == null)
                return NotFound();

            return await Villes.ToListAsync();
        }
    }
}

