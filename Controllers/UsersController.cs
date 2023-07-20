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
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public UsersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("GetUsers")]
        [UseAdminApiKey]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = new List<UserModel>();

            foreach (var user in await _appDbContext.Users.Include(x => x.Addresses).ToListAsync())

                users.Add(new UserModel(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                        new AddressModel(
                            user.Addresses.StreetName,
                            user.Addresses.PostalCode,
                            user.Addresses.City)));

            return users;
              
        }

        [HttpGet("GetUser/{id}")]
        [UseUserApiKey]
        [UseAdminApiKey]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var userEntity = await _appDbContext.Users.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Id == id);

            if (userEntity == null)
            {
                return NotFound("No user found!");
            }

            return new UserModel(
                userEntity.Id,
                userEntity.FirstName,
                userEntity.LastName,
                userEntity.Email,
                    new AddressModel(
                        userEntity.Addresses.StreetName,
                        userEntity.Addresses.PostalCode,
                        userEntity.Addresses.City));
                
        }

        [HttpPost("CreateUser")]
        [UseAdminApiKey]
        public async Task<ActionResult<UserModel>> CreateUser(UserInput model)
        {
            if (await _appDbContext.Users.AnyAsync(x => x.Email == model.Email))
                return BadRequest("Email already exist!");

            var userEntity = new UserEntity(
                model.FirstName,
                model.LastName,
                model.Email);

            userEntity.CreatePassword(model.Password);

            var addresses = await _appDbContext.Addresses
                .FirstOrDefaultAsync(x => x.StreetName == model.StreetName &&  x.PostalCode == model.PostalCode && x.City == model.City);

            if (addresses != null)
            {
                userEntity.AddressesId = addresses.Id;
            }
            else
            {
                userEntity.Addresses = new AddressEntity(
                    model.StreetName,
                    model.PostalCode,
                    model.City);
            }

            _appDbContext.Add(userEntity);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = userEntity.Id },
                new UserModel(
                   userEntity.Id,
                   userEntity.FirstName,
                   userEntity.LastName,
                   userEntity.Email,
                        new AddressModel(
                            userEntity.Addresses.StreetName,
                            userEntity.Addresses.PostalCode,
                            userEntity.Addresses.City)));


        }

        
        [HttpPut("UpdateUser/{id}")]
        [UseUserApiKey]
        [UseAdminApiKey]
        public async Task <ActionResult<UserUpdateModel>> UpdateUser(int id,  UserUpdateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("No user with that id!");
            }

            var userEntity = await _appDbContext.Users.FindAsync(model.Id);

            userEntity.FirstName = model.FirstName;
            userEntity.LastName = model.LastName;
            userEntity.Email = model.Email;

            var addresses = await _appDbContext.Addresses.FirstOrDefaultAsync(
                x => x.StreetName == model.StreetName && x.PostalCode == model.PostalCode && x.City == model.City);

            if (addresses != null)
            {
                userEntity.AddressesId = addresses.Id;
            }
            else
            {
                userEntity.Addresses = new AddressEntity(
                    model.StreetName,
                    model.PostalCode,
                    model.City);
            }

            _appDbContext.Entry(userEntity).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserEntityExists(id))
                {
                    return NotFound("Something went wrong!");
                }
                else
                {
                    throw;
                }
            }

            return Ok("User is updated!");
        }

        [HttpDelete("DeleteUser/{id}")]
        [UseAdminApiKey]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userEntity = await _appDbContext.Users.FindAsync(id);

            if (userEntity == null)
            {
                return NotFound("No user with that id!");
            }

            _appDbContext.Users.Remove(userEntity);

            await _appDbContext.SaveChangesAsync();

            return Ok("User is deleted!");

        }

        private bool UserEntityExists(int id)
        {
            return _appDbContext.Users.Any(u => u.Id == id);
        }

    }
}
