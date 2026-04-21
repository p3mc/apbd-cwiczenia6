using lep.DTOs;
using lep.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace lep.Controllers
{
    // api/rooms
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        public static List<Room> rooms = new List<Room>()
        {
            new Room() { Id = 1, Name = "Room1", Capacity = 5},
            new Room() { Id = 2, Name = "Room2", Capacity = 5},
            new Room() { Id = 3, Name = "Room3", Capacity = 5},
            new Room() { Id = 4, Name = "Room4", Capacity = 6},
            new Room() { Id = 5, Name = "Room5", Capacity = 20}
        };
        
        public static int NextRoomId()
            => rooms.Count == 0 ? 1 : rooms.Max(r => r.Id) + 1;
        
        // GET api/rooms
        [HttpGet]
        public IActionResult Get([FromQuery] int? minCapacity = 0)
        {
            return Ok(rooms.Where(r => r.Capacity >= minCapacity));
        }
        
        // GET api/rooms/{id}
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute] int id)
        {
            var room = rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            
            return Ok(room);
        }
        
        // GET /api/rooms/building/{buildingCode}
        [HttpGet("building/{buildingCode}")]
        public IActionResult GetByBuildingCode([FromRoute] string buildingCode)
        {
            var room = rooms.FirstOrDefault(r => r.BuildingCode == buildingCode);
            if  (room == null) return NotFound();

            return Ok(room);
        }
        
        // GET /api/rooms?minCapacity=20&hasProjector=true&activeOnly=true
        [HttpGet]
        public ActionResult<IEnumerable<Room>> GetAll(
            [FromQuery] int? minCapacity,
            [FromQuery] bool? hasProjector,
            [FromQuery] bool? activeOnly)
        {
            IEnumerable<Room> query = rooms;

            if (minCapacity.HasValue)
                query = query.Where(r => r.Capacity >= minCapacity.Value);

            if (hasProjector.HasValue)
                query = query.Where(r => r.HasProjector == hasProjector.Value);

            if (activeOnly == true)
                query = query.Where(r => r.IsActive);

            return Ok(query.ToList());
        }
        
        // POST /api/rooms {}
        [HttpPost]
        public IActionResult Post([FromBody] CreateRoomDto createRoomDto)
        {
            var room = new Room()
            {
                Id = rooms.Count + 1,
                Name = createRoomDto.Name,
                Capacity = createRoomDto.Capacity
            };
            
            rooms.Add(room);
            
            //201 created
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }
        
        // PUT /api/rooms/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Room updated)
        {
            var existing = rooms.FirstOrDefault(r => r.Id == id);
            if (existing is null) return NotFound();

            existing.Name = updated.Name;
            existing.BuildingCode = updated.BuildingCode;
            existing.Floor = updated.Floor;
            existing.Capacity = updated.Capacity;
            existing.HasProjector = updated.HasProjector;
            existing.IsActive = updated.IsActive;

            return Ok(existing);
        }
        
        // DELETE /api/rooms/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var room = rooms.FirstOrDefault(r => r.Id == id);
            if (room is null) return NotFound();

            bool hasReservations = ReservationsController.reservations.Any(r => r.RoomId == id);
            if (hasReservations)
                return Conflict(new { message = "Unable to delete reserved room." });

            rooms.Remove(room);
            return NoContent();
        }
    }
}