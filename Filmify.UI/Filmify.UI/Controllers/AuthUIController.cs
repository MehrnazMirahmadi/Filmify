using Filmify.UI.Models;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.Controllers;

public class AuthUIController(IdentityApiClient identityApi) : Controller
{

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var result = await identityApi.LoginAsync(model.Email, model.Password);

            HttpContext.Session.SetString("AuthToken", result.Token);


            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await identityApi.RegisterAsync(model.FullName, model.Email, model.Password, new[] { "User" });

            TempData["Message"] = "User registered successfully";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

}
