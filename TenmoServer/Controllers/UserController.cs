using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDao userDao;
        private readonly IAccountDao accountDao;
        private readonly ITransferDao transferDao;

        public UserController(IUserDao _userDao, IAccountDao _accountDao, ITransferDao _transferDao)
        {
            userDao = _userDao;
            accountDao = _accountDao;
            transferDao = _transferDao;
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            List<User> users = userDao.GetUsers();
            return users;
        }


        [HttpPut("{username}")]
        public ActionResult<User> GetUserAction(string username)
        {
            User user = userDao.GetUser(username);
            return user;
        }
        [HttpGet("account")]
        public ActionResult<decimal> GetBalance(int id)
        {
            string name = User.Identity.Name;
            decimal balance = accountDao.GetBalance(id);
            if (balance > 0)
            {
                return Ok(balance);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("allusers")]
        public List<User> GetAllUsers()
        {
            List<User> allUsers = userDao.GetUsers();
            return allUsers;
        }
    }
}
