using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static WebAPI.Models.Interfaces.IPersonInterface;

namespace WebAPI.Models.Entities
{
    public class RolesEntity
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Required, Column(Order = 1)]
        public Role Role { get; set; }

        [Required, Column(Order = 2)]
        public string RoleName { get; set; } = null!;

        [Required, Column(TypeName = "nvarchar(100)", Order = 3)]
        public string Email { get; set; } = null!;


        public void CreateRole()
        {
            var roleAdmin = Role.Admin;

            if (roleAdmin == 0)
            {
                RoleName = "Admin";
            }
            else
            {
                RoleName = "User";
            }
        }

    }
}
