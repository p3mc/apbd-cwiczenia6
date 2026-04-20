using lep.Models;
using lep.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace lep.Controllers
{
    // api/reservations
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public static List<Reservation> reservations = new List<Reservation>()
        {
            new Reservation() { Id = 1, RoomId = 1, OrganizerName = "Jan Nowak", Topic = "NAI", Date = new DateOnly(2026, 4, 30), StartTime = new TimeOnly(10, 15), EndTime = new TimeOnly(12, 30) },
            new Reservation() { Id = 2, RoomId = 1, OrganizerName = "Adam Kowalski", Topic = "APBD", Date = new DateOnly(2026, 4, 30), StartTime = new TimeOnly(12, 45), EndTime = new TimeOnly(14, 00) },
            new Reservation() { Id = 3, RoomId = 2, OrganizerName = "Jan Nowak", Topic = "PSM", Date = new DateOnly(2026, 4, 24), StartTime = new TimeOnly(10, 15), EndTime = new TimeOnly(12, 30) },
            new Reservation() { Id = 4, RoomId = 3, OrganizerName = "Adam Nowak", Topic = "APBD", Date = new DateOnly(2026, 4, 30), StartTime = new TimeOnly(10, 15), EndTime = new TimeOnly(12, 30) },
            new Reservation() { Id = 5, RoomId = 5, OrganizerName = "Jan Kowalski", Topic = "PRI", Date = new DateOnly(2026, 5, 1), StartTime = new TimeOnly(10, 15), EndTime = new TimeOnly(12, 30) }
        };
        
        // GET api/reservations
        [HttpGet]
        public IActionResult Get([FromQuery] int? minId = 1)
        {
            return Ok(reservations.Where(r => r.Id >= minId));
        }
    }
}