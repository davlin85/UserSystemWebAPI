using WebAPI.Models.Interfaces;

namespace WebAPI.Models.Models
{
    public class AddressModel : IAddressInterface
    {
        public AddressModel(
            int id,
            string streetName, 
            string postalCode, 
            string city)
        {
            Id = id;
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
