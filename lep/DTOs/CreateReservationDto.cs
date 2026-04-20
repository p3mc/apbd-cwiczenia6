using System.ComponentModel.DataAnnotations;

namespace lep.DTOs;

public class CreateReservationDto
{
    [StringLength(100)] 
    [MinLength(3)] 
    public string OrganizerName { get; set; } = string.Empty;
}