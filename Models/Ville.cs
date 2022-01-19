namespace NRH_UserAdress.Models
{
    public class Ville
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string state_code { get; set; }

        public int ProvinceID { get; set; }
        public Province Province { get; set; }

    }
}
