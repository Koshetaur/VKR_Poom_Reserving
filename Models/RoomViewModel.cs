using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace VKR_Poom_Reserving.Models
{
    public class RoomViewModel
    {
        [Required]
        [Remote("VerifyRoomName", "Home", ErrorMessage = "Переговорная с таким названием уже существует.")]
        [DataType(DataType.Text)]
        [Display(Name = "Переговорная")]
        public string RoomName { get; set; }
    }
}