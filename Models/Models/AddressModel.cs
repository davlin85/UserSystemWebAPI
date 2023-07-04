using WebAPI.Models.Interfaces;

namespace WebAPI.Models.Models
{
    public class AddressModel : IAddressInterface
    {
        public AddressModel(
            string streetName, 
            string postalCode, 
            string city)
        {
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
        }

        public int Id { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
