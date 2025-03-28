using Microsoft.AspNetCore.Mvc;
using MvcOAuthEmpleados.Filters;
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

        [AuthorizeEmpleados]
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.service.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int id)
        {
            Empleado emp = await this.service.FindEmpleadoAsync(id);
            return View(emp);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Perfil()
        {

            Empleado emp = await this.service.GetPerfilAsync();
            return View(emp);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Compis()
        {

            List<Empleado> emps = await this.service.GetCompisAsync();
            return View(emps);
        }
    }
}
