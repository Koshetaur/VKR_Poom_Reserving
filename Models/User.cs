using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VKR_Poom_Reserving.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surame { get; set; }
    }
}
