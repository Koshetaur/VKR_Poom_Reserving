using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VKR_Poom_Reserving.Models
{
    [Index("Name", IsUnique = true, Name = "RoomName_Index")]
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}