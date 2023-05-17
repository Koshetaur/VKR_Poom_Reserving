using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VKR_Poom_Reserving.Models
{
    public class ReserveViewModel
    {
        [Required]
        [Display(Name = "Employee")]
        public string SelectedUserId { get; set; }
        public SelectList? Users { get; set; }

        [Remote(action: "VerifyReserve", controller: "Home", AdditionalFields = nameof(StartTime) + "," + nameof(EndTime) + "," + nameof(Id), ErrorMessage = "Room already reserved on this time.")]
        [Display(Name = "Office")]
        public int SelectedRoomId { get; set; }
        public SelectList? Rooms { get; set; }

        [Remote(action: "VerifyReserve", controller: "Home", AdditionalFields = nameof(SelectedRoomId) + "," + nameof(EndTime) + "," + nameof(Id), ErrorMessage = "Room already reserved on this time.")]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        [Remote(action: "VerifyReserve", controller: "Home", AdditionalFields = nameof(StartTime) + "," + nameof(SelectedRoomId) + "," + nameof(Id), ErrorMessage = "Room already reserved on this time.")]
        public DateTime EndTime { get; set; }

        [HiddenInput]
        [Remote(action: "VerifyReserve", controller: "Home", AdditionalFields = nameof(StartTime) + "," + nameof(SelectedRoomId) + "," + nameof(EndTime), ErrorMessage = "Room already reserved on this time.")]
        public int Id { get; set; }
    }
}