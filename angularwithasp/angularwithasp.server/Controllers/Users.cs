using angularwithasp.server.Data;
using angularwithasp.server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace angularwithasp.server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class Users : ControllerBase
    {
        StockDbContext dbx;
        public Users(StockDbContext dbContext)
        {
            dbx = dbContext;
        }

        [HttpGet("get")]
        public UserDTO Get([FromQuery] int skip, [FromQuery] int take)
        {
            UserDTO userDTO = new UserDTO();

            userDTO.Total = (from User item in dbx.Users
                             select item).Count();

            if (userDTO.Total > skip)
            {
                userDTO.Skip = skip;
            }

            userDTO.Users = (from User item in dbx.Users
                             select item).Skip(userDTO.Skip).Take(take).ToArray();

            return userDTO;
        }

        [HttpGet("add")]
        public string Add([FromQuery] string user)
        {
            User newUser = JsonSerializer.Deserialize<User>(user);
            if (newUser.FirstName.Length < 2 || newUser.LastName.Length < 2)
            {
                throw new Exception("Invalid user name");
            }

            if (!Helper.IsValidEmail(newUser.Email))
            {
                throw new Exception("Invalid email address");
            }

            dbx.Users.Add(newUser);
            dbx.SaveChanges();

            return newUser.Id.ToString();
        }

        [HttpGet("update")]
        public string Update([FromQuery] string user)
        {
            User newUser = JsonSerializer.Deserialize<User>(user);
            if (newUser.FirstName.Length < 2 || newUser.LastName.Length < 2)
            {
                throw new Exception("Invalid user name");
            }

            if (!Helper.IsValidEmail(newUser.Email))
            {
                throw new Exception("Invalid email address");
            }

            dbx.Update(newUser);
            dbx.SaveChanges();

            return string.Empty;
        }

        [HttpGet("delete")]
        public string Delete(long id)
        {
            dbx.Users.Where(u => u.Id == id).ExecuteDelete();
            dbx.SaveChanges();

            return string.Empty;
        }
    }
}