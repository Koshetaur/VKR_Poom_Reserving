using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace VKR_Poom_Reserving.Models
{
    public class UserViewModel
    {
        [Required]
        [Remote("VerifyName", "Home", ErrorMessage = "Имя не поддерживается.")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string UserName { get; set; }

        [Required]
        [Remote("VerifySurname", "Home", ErrorMessage = "Фамилия не поддерживается.")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string UserSurname { get; set; }
    }
}