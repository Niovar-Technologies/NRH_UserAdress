namespace NRH_UserAdress.Models
{
    public class Province
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string country { get; set; }
        public int paysID { get; set; }
        public Pays Pays { get; set; }

    }
}
