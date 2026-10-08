using LancerWebAPI.Database;
using LancerWebAPI.DTOs;
using LancerWebAPI.Models;
using LancerWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LancerWebAPI.Controllers
{
    public class UsersController : Controller
    {
        private UserService _userService;
        private UserRepository _userRepo;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET: UsersController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestUserDto request)
        {
            // 1. Check if a user with this email already exists
            if (await _userService.CheckIfUserExists(request.Email))
            {
                return BadRequest(new { message = "A user with this email already exists." });
            }

            // 2. Controller delegates the heavy lifting to the Service
            await _userService.RegisterUserAsync(request);

            return Ok(new { message = "User registered successfully!" });
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
