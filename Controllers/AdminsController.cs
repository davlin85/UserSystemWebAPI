using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Filters;
using WebAPI.Models.Entities;
using WebAPI.Models.Input;
using WebAPI.Models.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AdminsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public AdminsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("GetAdmins")]
        [UseAdminApiKey]
        public async Task<ActionResult<IEnumerable<AdminModel>>> GetAdmins()
        {
            var admins = new List<AdminModel>();

            foreach (var admin in await _appDbContext.Admins.ToListAsync())

                admins.Add(new AdminModel(
                    admin.Id,
                    admin.FirstName,
                    admin.LastName,
                    admin.Email));

            return admins;
        }

        [HttpGet("GetAdmin/{id}")]
        [UseAdminApiKey]
        public async Task<ActionResult<AdminModel>> GetAdmin(int id)
        {
            var adminEntity = await _appDbContext.Admins.FirstOrDefaultAsync(x => x.Id == id);

            if (adminEntity == null)
            {
                return NotFound("No Admin found!");
            }

            return new AdminModel(
                adminEntity.Id,
                adminEntity.FirstName,
                adminEntity.LastName,
                adminEntity.Email);

        }

        [HttpPost("CreateAdmin")]
        [UseAdminApiKey]
        public async Task<ActionResult<AdminModel>> CreateAdmin(AdminInput model)
        {
            if (await _appDbContext.Admins.AnyAsync(x => x.Email == model.Email))
                    return BadRequest("Email already Exist!");

            var adminEntity = new AdminEntity(
                model.FirstName,
                model.LastName,
                model.Email);

            adminEntity.CreatePassword(model.Password);

            _appDbContext.Admins.Add(adminEntity);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = adminEntity.Id },
                new AdminModel(
                    adminEntity.Id,
                    adminEntity.FirstName,
                    adminEntity.LastName,
                    adminEntity.Email));
        }

        [HttpPut("UpdateAdmin/{id}")]
        public async Task<ActionResult<AdminUpdateModel>> UpdateAdmin(int id, AdminUpdateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("No admin with that Id!");
            }

            var adminEntity = await _appDbContext.Admins.FindAsync(model.Id);

            adminEntity.FirstName = model.FirstName;
            adminEntity.LastName = model.LastName;
            adminEntity.Email = model.Email;

            _appDbContext.Entry(adminEntity).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmingEntityExist(id))
                {
                    return NotFound("Something went wrong!");
                }

                else
                {
                    throw;
                }
            }

            return Ok("Admin is updated!");

        }

        [HttpDelete("DeleteAdmin/{id}")]
        [UseAdminApiKey]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var adminEntity = await _appDbContext.Admins.FindAsync(id);

            if (adminEntity == null)
            {
                return NotFound("No Admin with that Id");
            }

            _appDbContext.Admins.Remove(adminEntity);

            await _appDbContext.SaveChangesAsync();

            return Ok("Admin is deleted!");
        }

        private bool AdmingEntityExist(int id)
        {
            return _appDbContext.Admins.Any(a => a.Id == id);
        }

    }
}
