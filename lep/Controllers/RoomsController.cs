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

        public static List<Reservation> reservations = new List<Reservation>()
        {
            new Reservation() { Id = 1, RoomId = 1, OrganizerName = "Jan Nowak", Topic = "NAI", Date = new DateOnly(2026,4,30), StartTime = new TimeOnly(10,15), EndTime = new TimeOnly(12,30)},
            new Reservation() { Id = 2, RoomId = 1, OrganizerName = "Adam Kowalski", Topic = "APBD", Date = new DateOnly(2026,4,30), StartTime = new TimeOnly(12,45), EndTime = new TimeOnly(14,00)},
            new Reservation() { Id = 3, RoomId = 2, OrganizerName = "Jan Nowak", Topic = "PSM", Date = new DateOnly(2026,4,24), StartTime = new TimeOnly(10,15), EndTime = new TimeOnly(12,30)},
            new Reservation() { Id = 4, RoomId = 3, OrganizerName = "Adam Nowak", Topic = "APBD", Date = new DateOnly(2026,4,30), StartTime = new TimeOnly(10,15), EndTime = new TimeOnly(12,30)},
            new Reservation() { Id = 5, RoomId = 5, OrganizerName = "Jan Kowalski", Topic = "PRI", Date = new DateOnly(2026,5,1), StartTime = new TimeOnly(10,15), EndTime = new TimeOnly(12,30)}
        };
        
        // GET api/rooms
        [HttpGet]
        public IActionResult Get([FromQuery] int? minCapacity = 0)
        {
            return Ok(rooms.Where(r => r.Capacity >= minCapacity));
        }
        
        // GET api/rooms/3
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute] int id)
        {
            var room = rooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();
            
            return Ok(room);
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
    }
}