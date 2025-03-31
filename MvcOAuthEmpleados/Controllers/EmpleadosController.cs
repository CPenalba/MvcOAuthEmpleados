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

        public async Task<IActionResult> EmpleadosOficio()
        {
            List<string> oficios = await this.service.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmpleadosOficio(int? incremento, List<string> oficio, string accion)
        {
            List<string> oficios = await this.service.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;

            if (accion.ToLower() == "update")
            {
                await this.service.UpdateEmpleadosOficioAsync(incremento.Value, oficio);
            }
            List<Empleado> empleados = await this.service.GetEmpleadosOficioAsync(oficio);

            return View(empleados);
        }
    }
}
