using NRH_UserAdress.Models;
namespace NRH_UserAdress.Models
{
    public class MyJsonType
    {

        public string MyStringProperty01 { get; set; }

        public int MyIntegerProperty { get; set; }

        public MyJsonSubDocumentType MySubDocument { get; set; }

        public List<String> MyListProperty { get; set; }
    }
}
