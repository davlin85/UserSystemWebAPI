using WebAPI.Models.Interfaces;

namespace WebAPI.Models.Models
{
    public class UserModel : IPersonInterface
    {
        public UserModel(
            int id, 
            string firstName, 
            string lastName, 
            string email, 
            AddressModel address)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AddressModel Address { get; set; }
    }

    public class UserUpdateModel : IPersonInterface, IAddressInterface
    {
        public UserUpdateModel()
        {
            
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}