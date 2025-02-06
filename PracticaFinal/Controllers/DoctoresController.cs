using Microsoft.AspNetCore.Mvc;
using PracticaFinal.Models;
using PracticaFinal.Repositories;

namespace PracticaFinal.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryDoctores repo;

        public DoctoresController()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.repo = new RepositoryDoctores(connectionString);
        }

        public IActionResult Index()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }

        public IActionResult Buscador()
        {
            List<string> especialidades = this.repo.GetEspecialidades();
            ViewData["Especialidades"] = especialidades;
            return View();
        }

        [HttpPost]
        public IActionResult Buscador(string especialidad)
        {
            List<string> especialidades = this.repo.GetEspecialidades();
            ViewData["Especialidades"] = especialidades;

            List<Doctor> doctores = this.repo.GetDoctoresByEspecialidad(especialidad);
            ViewData["EspecialidadSeleccionada"] = especialidad;
            return View(doctores);
        }
    }
}
