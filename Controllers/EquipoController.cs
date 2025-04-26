using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using prog_parcial.Data;
using prog_parcial.Models;

namespace prog_parcial.Controllers
{
    public class EquipoController : Controller
    {
        private readonly ILogger<EquipoController> _logger;
        private readonly ApplicationDbContext _context;

        public EquipoController(ILogger<EquipoController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Mostrar formulario para asociar jugador a equipo
        [HttpGet]
        public IActionResult AsociarJugador()
        {
            ViewBag.Jugadores = _context.DbSetPlayer.ToList();
            ViewBag.Equipos = _context.DbSetTeam.ToList();
            return View();
        }

        // Procesar asociación
        [HttpPost]
        public IActionResult AsociarJugador(int jugadorId, int equipoId)
        {
            var yaAsignado = _context.DbSetAssignment.Any(a => a.PlayerId == jugadorId && a.TeamId == equipoId);

            if (yaAsignado)
            {
                ModelState.AddModelError("", "El jugador ya está asignado a este equipo.");
            }
            else
            {
                var nuevo = new Assignment
                {
                    PlayerId = jugadorId,
                    TeamId = equipoId
                };

                _context.DbSetAssignment.Add(nuevo);
                _context.SaveChanges();

                TempData["Success"] = "Jugador asignado correctamente.";
                return RedirectToAction("ListarJugadores");
            }

            ViewBag.Jugadores = _context.DbSetPlayer.ToList();
            ViewBag.Equipos = _context.DbSetTeam.ToList();
            return View();
        }

        // Listar jugadores y su equipo
        public IActionResult ListarJugadores()
        {
            var jugadoresConEquipos = from a in _context.DbSetAssignment
                                      join j in _context.DbSetPlayer on a.PlayerId equals j.Id
                                      join e in _context.DbSetTeam on a.TeamId equals e.Id
                                      select new JugadorConEquipoViewModel
                                      {
                                          NombreJugador = j.Nombre,
                                          Edad = j.Edad,
                                          Posicion = j.Posicion,
                                          NombreEquipo = e.Nombre
                                      };

            return View(jugadoresConEquipos.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // ViewModel para mostrar jugador con su equipo
    public class JugadorConEquipoViewModel
    {
        public string? NombreJugador { get; set; }
        public int Edad { get; set; }
        public string? Posicion { get; set; }
        public string? NombreEquipo { get; set; }
    }
}
