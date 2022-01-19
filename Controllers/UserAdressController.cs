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
    public class UserAdressController : Controller
    {
        private readonly UserAdressContext _context;

        public UserAdressController(UserAdressContext context)
        {
            _context = context;
        }

        [HttpGet("health")]
        public ActionResult health()
        {
            return
                Ok("Statut du microservice NRH_UserAdress: OK");
        }

        [HttpGet("PaysFromJson")]
        public async Task<ActionResult<UserAdressContext>> PaysFromJson()
        {
            var myJsonString = System.IO.File.ReadAllText("C:/inetpub/wwwroot/rh.niovar.ca/docs/countries00.json");
            var myJObject = JObject.Parse(myJsonString);

            // Get Pays JSON
            
            foreach (var x in myJObject)
            {
                var pays = new Pays();

                // string name = x.Key;
                JToken value = x.Value;

Console.WriteLine("value: " + value);
                pays.name = value.SelectToken("name").Value<string>();
                pays.iso3 = value.SelectToken("iso3").Value<string>();
                pays.iso2 = value.SelectToken("iso2").Value<string>();
                pays.phone_code = value.SelectToken("phone_code").Value<string>();
                pays.capital = value.SelectToken("capital").Value<string>();
                pays.currency = value.SelectToken("currency").Value<string>();
                pays.native = value.SelectToken("native").Value<string>();
                pays.region = value.SelectToken("region").Value<string>();
                pays.subregion = value.SelectToken("subregion").Value<string>();
                pays.emoji = value.SelectToken("emoji").Value<string>();
                pays.emojiU = value.SelectToken("emojiU").Value<string>();

                _context.Pays.Add(pays);
                    await _context.SaveChangesAsync();

            }
            

            return
               Ok("Pays ajoutés dans la base de donnée.");
        }

        [HttpGet("ProvinceFromJson")]
        public async Task<ActionResult<UserAdressContext>> ProvinceFromJson()
        {
            var myJsonString = System.IO.File.ReadAllText("C:/inetpub/wwwroot/rh.niovar.ca/states00.json"); // states01.json
            var myJObject = JObject.Parse(myJsonString);

            // Get Province JSON

            foreach (var x in myJObject)
            {
                var province = new Province();

                // string name = x.Key;
                JToken value = x.Value;

                Console.WriteLine("value: " + value);
                province.name = value.SelectToken("name").Value<string>();
                province.code = value.SelectToken("state_code").Value<string>();
                province.country = value.SelectToken("iso2").Value<string>();
                
                var paysId = GetPaysId(province.country);
Console.WriteLine("paysId" + paysId);
                province.paysID = paysId;

                _context.Province.Add(province);
                await _context.SaveChangesAsync();

            }

            return
               Ok("Provinces ajoutés dans la base de donnée.");
        }

        [HttpGet("VilleFromJson")]
        public async Task<ActionResult<UserAdressContext>> VilleFromJson()
        {
            var myJsonString = System.IO.File.ReadAllText("C:/inetpub/wwwroot/rh.niovar.ca/cities00.json"); // cities01.json
            var myJObject = JObject.Parse(myJsonString);

            // Get Province JSON

            foreach (var x in myJObject)
            {
                var ville = new Ville();

                // string name = x.Key;
                JToken value = x.Value;

Console.WriteLine("value: " + value);
                ville.name = value.SelectToken("name").Value<string>();
                ville.latitude = value.SelectToken("latitude").Value<string>();
                ville.longitude = value.SelectToken("longitude").Value<string>();
                ville.state_code = value.SelectToken("state_code").Value<string>();

                var provinceId = GetProvinceId(ville.state_code);
Console.WriteLine("provinceId" + provinceId);
                ville.ProvinceID = provinceId;

                _context.Ville.Add(ville);
                await _context.SaveChangesAsync();

            }

            return
               Ok("Villes ajoutés dans la base de donnée.");
        }

        [HttpGet("GetPaysId")]
        public int GetPaysId(String iso2)
        {

            var Pays = _context.Pays
                .Where(b => b.iso2 == iso2)
                .FirstOrDefault();
            if( Pays == null)
                return 0;

            return Pays.ID;
        }

        [HttpGet("GetProvinceId")]
        public int GetProvinceId(String state_code)
        {
            var Province = _context.Province
                .Where(b => b.code == state_code)
                .FirstOrDefault();
            if (Province == null)
                return 0;

            return Province.ID;
        }

        // les users d'un pays
        [HttpGet("PaysUsers/{paysid}")]
        public async Task<ActionResult<IEnumerable<Account>>> PaysUsers(int paysid)
        {
            var Accounts = _context.Account
                .Where(b => b.PaysId == paysid);

            if (Accounts == null)
                return NotFound();
            return await _context.Account.ToListAsync();
        }

        // les users d'une province
        [HttpGet("ProvinceUsers/{provinceid}")]
        public async Task<ActionResult<IEnumerable<Account>>> ProvinceUsers(int provinceid)
        {
            var Accounts = _context.Account
                .Where(b => b.ProvinceId == provinceid);

            if (Accounts == null)
                return NotFound();
            return await _context.Account.ToListAsync();
        }

        // les users d'une ville
        [HttpGet("VilleUsers/{villeid}")]
        public async Task<ActionResult<IEnumerable<Account>>> VilleUsers(int villeid)
        {
            var Accounts = _context.Account
                .Where(b => b.VilleId == villeid);

            if (Accounts == null)
                return NotFound();
            return await _context.Account.ToListAsync();
        }
    }
}
