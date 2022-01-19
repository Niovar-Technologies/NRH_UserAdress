using  System.ComponentModel.DataAnnotations.Schema;

namespace NRH_UserAdress.Models
{
    public class Account
    {
        public int ID { get; set; }
        public int PaysId { get; set; }
        public int ProvinceId { get; set; }
        public int VilleId { get; set; }
    }
}
