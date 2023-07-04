using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Interfaces;

namespace WebAPI.Models.Entities
{
    public class AddressEntity : IAddressInterface
    {
        public AddressEntity()
        {
            Users = new HashSet<UserEntity>();    
        }

        public AddressEntity(
            string streetName, 
            string postalCode, 
            string city)
        {
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
        }

        [Key]
        public int Id { get; set; }

        [Required, Column(TypeName = "nvarchar(50)")]
        public string StreetName { get; set; } = null!;

        [Required, Column(TypeName = "char(5)")]
        public string PostalCode { get; set; } = null!;

        [Required, Column(TypeName = "nvarchar(50)")]
        public string City { get; set; } = null!;


        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
