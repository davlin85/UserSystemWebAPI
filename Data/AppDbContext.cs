using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Entities;

namespace WebAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }


        public virtual DbSet<AdminEntity> Admins { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<AddressEntity> Addresses { get; set; }
    }
}
