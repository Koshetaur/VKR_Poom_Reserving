using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace VKR_Poom_Reserving.Models
{
    public class RoomViewModel
    {
        [Required]
        [Remote("VerifyRoomName", "Home", ErrorMessage = "Office name already exists.")]
        [DataType(DataType.Text)]
        [Display(Name = "Office")]
        public string RoomName { get; set; }
    }
}