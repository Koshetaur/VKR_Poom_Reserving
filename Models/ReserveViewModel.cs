using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VKR_Poom_Reserving.Models
{
    public class ReserveViewModel
    {
        [Display(Name = "Сотрудник")]
        public string? SelectedUserId { get; set; }
        public SelectList? Users { get; set; }

        [Remote(action: "VerifyReserve", controller: "Home", AdditionalFields = nameof(StartTime) + "," + nameof(EndTime) + "," + nameof(Id))]
        [Display(Name = "Переговорная")]
        public int SelectedRoomId { get; set; }
        public SelectList? Rooms { get; set; }

        [Remote(action: "VerifyReserve", controller: "Home", AdditionalFields = nameof(SelectedRoomId) + "," + nameof(EndTime) + "," + nameof(Id), ErrorMessage = "Эта переговорная уже занята на данное время.")]
        [Display(Name = "Время начала")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Время окончания")]
        [Remote(action: "VerifyTime", controller: "Home", AdditionalFields = nameof(StartTime), ErrorMessage = "Время окончания резерва не должно превышать время начала.")]
        public DateTime EndTime { get; set; }

        [HiddenInput]
        [Remote(action: "VerifyReserve", controller: "Home", AdditionalFields = nameof(StartTime) + "," + nameof(SelectedRoomId) + "," + nameof(EndTime))]
        public int Id { get; set; }
    }
}