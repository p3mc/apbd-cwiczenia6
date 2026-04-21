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
        
        public static int NextReservationId()
            => reservations.Count == 0 ? 1 : reservations.Max(r => r.Id) + 1;
        
        // GET api/reservations
        [HttpGet]
        public IActionResult Get([FromQuery] int? minId = 1)
        {
            return Ok(reservations.Where(r => r.Id >= minId));
        }
        
        // GET api/reservatinos/{id}
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute] int id)
        {
            var room = reservations.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            
            return Ok(room);
        }
        
        // GET /api/reservations?date=2026-05-10&status=confirmed&roomId=2
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetAll(
            [FromQuery] DateOnly? date,
            [FromQuery] string? status,
            [FromQuery] int? roomId)
        {
            IEnumerable<Reservation> query = reservations;

            if (date.HasValue)
                query = query.Where(r => r.Date == date.Value);
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);

            return Ok(query.ToList());
        }
        
        private static bool HasTimeConflict(Reservation candidate, int? ignoreId)
        {
            return reservations.Any(r =>
                r.RoomId == candidate.RoomId &&
                r.Date == candidate.Date &&
                (ignoreId is null || r.Id != ignoreId) &&
                r.StartTime < candidate.EndTime &&
                candidate.StartTime < r.EndTime);
        }
        
        // POST /api/reservations
        [HttpPost]
        public ActionResult<Reservation> Create([FromBody] Reservation reservation)
        {
            var room = RoomsController.rooms.FirstOrDefault(r => r.Id == reservation.RoomId); 
            
            if (room is null)
                return NotFound(new { message = $"Room {reservation.RoomId} does not exist." });
            if (!room.IsActive)
                return Conflict(new { message = "Room is not active." });

            if (HasTimeConflict(reservation, ignoreId: null))
                return Conflict(new { message = "Collides with another reservation." });

            reservation.Id = NextReservationId();
            reservations.Add(reservation);

            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        
        // PUT /api/reservations/{id} 
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Reservation updated)
        {
            var existing = reservations.FirstOrDefault(r => r.Id == id);
            if (existing is null) return NotFound();

            var room = RoomsController.rooms.FirstOrDefault(r => r.Id == updated.RoomId);
            
            if (room is null)
                return NotFound(new { message = $"Room {updated.RoomId} does ot exist." });
            if (!room.IsActive)
                return Conflict(new { message = "Room is not active." });

            var candidate = new Reservation
            {
                Id = id,
                RoomId = updated.RoomId,
                Date = updated.Date,
                StartTime = updated.StartTime,
                EndTime = updated.EndTime
            };
            if (HasTimeConflict(candidate, ignoreId: id))
                return Conflict(new { message = "Collides with another reservation." });

            existing.RoomId = updated.RoomId;
            existing.OrganizerName = updated.OrganizerName;
            existing.Topic = updated.Topic;
            existing.Date = updated.Date;
            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.Status = updated.Status;

            return Ok(existing);
        }
        
        // DELETE /api/reservations/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var res = reservations.FirstOrDefault(r => r.Id == id);
            if (res is null) return NotFound();

            reservations.Remove(res);
            return NoContent();
        }
        
    }
}