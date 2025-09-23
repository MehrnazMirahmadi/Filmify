using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.Controllers;

public class AuthUIController : Controller
{
    private readonly IdentityApiClient _identityApi;

    public AuthUIController(IdentityApiClient identityApi)
    {
        _identityApi = identityApi;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "ایمیل و پسورد الزامی است.";
            return View();
        }

        try
        {
            var result = await _identityApi.LoginAsync(email, password);

            // ذخیره JWT در سشن
            HttpContext.Session.SetString("AuthToken", result.Token);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string fullName, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "ایمیل و پسورد الزامی است.";
            return View();
        }

        try
        {
            await _identityApi.RegisterAsync(fullName, email, password, new[] { "User" });

            TempData["Message"] = "ثبت‌نام با موفقیت انجام شد، لطفاً وارد شوید.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }
    }
}
