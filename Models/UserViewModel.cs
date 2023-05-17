using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace VKR_Poom_Reserving.Models
{
    public class UserViewModel
    {
        [Required]
        [Remote("VerifyName", "Home", ErrorMessage = "Name is not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required]
        [Remote("VerifySurname", "Home", ErrorMessage = "Surname is not valid.")]
        [DataType(DataType.Text)]
        [Display(Name = "Surname")]
        public string UserSurname { get; set; }
    }
}