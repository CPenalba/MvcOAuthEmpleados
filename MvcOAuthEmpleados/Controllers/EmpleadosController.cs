using Microsoft.AspNetCore.Mvc;
using MvcOAuthEmpleados.Models;
using MvcOAuthEmpleados.Services;

namespace MvcOAuthEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private ServiceEmpleados service;
        public EmpleadosController(ServiceEmpleados service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.service.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int id)
        {
            //TENDREMOS EL TOKEN EN SESSION
            string token = HttpContext.Session.GetString("TOKEN");
            if (token == null)
            {
                ViewData["MENSAJE"] = "Debe validarse en Login";
                return View();
            }
            else
            {
                Empleado emp = await this.service.FindEmpleadoAsync(id, token);
                return View(emp);
            }
        }
    }
}
