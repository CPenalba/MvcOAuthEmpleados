using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcOAuthEmpleados.Models;
using MvcOAuthEmpleados.Services;
using System.Security.Claims;

namespace MvcOAuthEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceEmpleados service;
        public ManagedController(ServiceEmpleados service)
        {
            this.service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string token = await this.service.GetTokenAsync(model.UserName, model.Password);
            if (token == null)
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
            else
            {
                ViewData["MENSAJE"] = "Ya tienes tu Token!!!!";
                HttpContext.Session.SetString("TOKEN", token);
                return View();
            }

        }
    }
}
