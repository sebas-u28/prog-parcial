using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using prog_parcial.Data;
using prog_parcial.Models;

namespace prog_parcial.Controllers;

public class JugadorController : Controller
{
    private readonly ILogger<JugadorController> _logger;
    private readonly ApplicationDbContext _context;


    public JugadorController(ILogger<JugadorController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    // Mostrar formulario
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Equipos = _context.DbSetTeam.ToList();
        ViewBag.Posiciones = new List<string> { "Portero", "Defensa", "Mediocampista", "Delantero" };
        return View();
    }

    // Procesar formulario
    [HttpPost]
    public IActionResult Create(Player jugador, int EquipoId)
    {
        if (ModelState.IsValid)
        {
            // Verificar si ya existe un jugador con ese nombre en ese equipo
            var jugadorExistente = (from a in _context.DbSetAssignment
                                    join p in _context.DbSetPlayer on a.PlayerId equals p.Id
                                    where p.Nombre == jugador.Nombre && a.TeamId == EquipoId
                                    select a).FirstOrDefault();

            if (jugadorExistente != null)
            {
                ModelState.AddModelError("", "Este jugador ya está registrado en este equipo.");
            }
            else
            {
                // Guardar nuevo jugador
                _context.DbSetPlayer.Add(jugador);
                _context.SaveChanges();

                // Crear la relación en Assignment
                var assignment = new Assignment
                {
                    PlayerId = jugador.Id,
                    TeamId = EquipoId
                };

                _context.DbSetAssignment.Add(assignment);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }

        // Si hay errores, recargar selects
        ViewBag.Equipos = _context.DbSetTeam.ToList();
        ViewBag.Posiciones = new List<string> { "Portero", "Defensa", "Mediocampista", "Delantero" };
        return View(jugador);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
