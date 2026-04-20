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