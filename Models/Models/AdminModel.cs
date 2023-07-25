using WebAPI.Models.Interfaces;

namespace WebAPI.Models.Models
{
    public class AdminModel : IPersonInterface
    {
        public AdminModel(
            int id, 
            string firstName, 
            string lastName, 
            string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public RolesPolicy RolesPolicy { get; set; }
    }

    public class AdminUpdateModel : IPersonInterface
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public RolesPolicy RolesPolicy { get; set; }
    }
}
