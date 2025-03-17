using System.Diagnostics;
using System.Security.Claims;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PizzaShop.Controllers;


public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    public IActionResult Profile()
    {
        var email = Request.Cookies["email"];


        if (string.IsNullOrEmpty(email))
        {
            // If email is not found in cookies, get it from session
            email = HttpContext.Session.GetString("UserEmail");
        }

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required.");
        }

        var profileVM = _userService.GetUserProfileByEmail(email);
        if (profileVM == null)
        {
            return NotFound();
        }

        PopulateDropdowns(profileVM.CountryId, profileVM.StateId);
        return View(profileVM);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Profile(ProfileVM model, IFormFile ProfilePhoto)
    {

        PopulateDropdowns(model.CountryId, model.StateId);
        var email = Request.Cookies["email"];
        model.Email = email;

        _userService.UpdateUserProfile(model, ProfilePhoto);

        TempData["Message"] = "User updated successfully!";
        TempData["MessageType"] = "info";

        return RedirectToAction("Profile", "User");
    }

    [Authorize(Policy = "UserEditPolicy")]
    public IActionResult Adduser()
    {
        var email = Request.Cookies["email"];
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required.");
        }

        var user = _userService.GetUserByEmail(email);

        if (user == null)
        {
            return NotFound();
        }
        PopulateDropdowns(user.CountryId, user.StateId);
        return View();
    }

    [HttpPost]
    public IActionResult Adduser(AddUserVM model, IFormFile profileimage)
    {
        PopulateDropdowns(model.CountryId, model.StateId);
        _userService.AddUser(model, profileimage);
        TempData["Message"] = "User added successfully!";
        TempData["MessageType"] = "success";

        return RedirectToAction("Userpage", "Home");
    }

    [Authorize(Policy = "UserEditPolicy")]
    [HttpGet]
    public IActionResult Edituser(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        PopulateDropdowns(user.CountryId, user.StateId);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edituser(AddUserVM model, IFormFile ProfileImage)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns(model.CountryId, model.StateId);
            var result = _userService.UpdateUser(model, ProfileImage);

            if (!result)
            {
                return NotFound();
            }

            TempData["Message"] = "User updated successfully!";
            TempData["MessageType"] = "info";
            return RedirectToAction("Edituser", "User");
        }
        return RedirectToAction("Userpage", "Home");

    }

    [Authorize]
    [HttpGet]
    public JsonResult GetStatesByCountry(int countryId)
    {
        var states = _userService.GetStatesByCountry(countryId);
        var stateSelectList = states.Select(s => new SelectListItem
        {
            Value = s.StateId.ToString(),
            Text = s.StateName
        }).ToList();

        return Json(stateSelectList);
    }

    [Authorize]
    [HttpGet]
    public JsonResult GetCitiesByState(int stateId)
    {
        var cities = _userService.GetCitiesByState(stateId);
        var citySelectList = cities.Select(c => new SelectListItem
        {
            Value = c.CityId.ToString(),
            Text = c.CityName
        }).ToList();

        return Json(citySelectList);
    }

    [Authorize(Policy = "UserDeletePolicy")]
    [HttpPost]
    public IActionResult Delete(int id)
    {
        AddUserVM user = _userService.GetUserById(id);
        _userService.DeleteUser(user);
        TempData["Message"] = "User deleted successfully!";
        TempData["MessageType"] = "success";
        return RedirectToAction("Userpage", "Home");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Changepassword()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var email = Request.Cookies["email"];
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required.");
        }

        var user = _userService.GetUserByEmail(email);
        if (user == null)
        {
            return NotFound();
        }

        if (!_userService.CheckPassword(user, model.CurrentPassword))
        {
            ModelState.AddModelError(string.Empty, "The current password is incorrect.");
            TempData["Message"] = "The current password is incorrect.";
            TempData["MessageType"] = "error";
            return View("Changepassword", model);
        }

        _userService.ChangePassword(user, model.NewPassword);

        TempData["Message"] = "Password changed successfully!";
        TempData["MessageType"] = "success";

        return RedirectToAction("ChangePassword");
    }

    private void PopulateDropdowns(int? countryId = null, int? stateId = null)
    {
        ViewBag.Roles = _userService.GetAllRoles().Select(r => new SelectListItem
        {
            Value = r.RoleId.ToString(),
            Text = r.RoleName
        }).ToList();

        ViewBag.Countries = _userService.GetAllCountries().Select(c => new SelectListItem
        {
            Value = c.CountryId.ToString(),
            Text = c.CountryName
        }).ToList();

        ViewBag.States = countryId.HasValue ?
            _userService.GetStatesByCountry(countryId.Value).Select(s => new SelectListItem
            {
                Value = s.StateId.ToString(),
                Text = s.StateName
            }).ToList()
            : new List<SelectListItem>();

        ViewBag.Cities = stateId.HasValue ?
            _userService.GetCitiesByState(stateId.Value).Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.CityName
            }).ToList()
            : new List<SelectListItem>();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
