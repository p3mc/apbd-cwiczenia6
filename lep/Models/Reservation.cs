namespace lep.Models;


public class Reservation
{
    public enum ReservationStatus
    {
        planned,
        cancelled,
        ongoing
    }

    public int Id { get; set; }
    public int RoomId { get; set; }
    public string OrganizerName { get; set; }
    public string Topic { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; }
}