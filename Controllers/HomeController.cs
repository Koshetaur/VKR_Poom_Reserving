using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VKR_Poom_Reserving.Models;
using DomainLayer;
using AutoMapper;

namespace ReserveWebApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<ReserveDto, ReservesViewModel>()
                .ForMember(dst => dst.User, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"))
                .ForMember(dst => dst.Room, opt => opt.MapFrom(src => src.Room.Name))
                .ForMember(dst => dst.TimeStart, opt => opt.MapFrom(src => $"{src.TimeStart}"))
                .ForMember(dst => dst.TimeEnd, opt => opt.MapFrom(src => $"{src.TimeEnd}"));
                x.CreateMap<UserViewModel, AddUserCommand>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.UserSurname));
                x.CreateMap<RoomViewModel, AddRoomCommand>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.RoomName));
                x.CreateMap<ReserveViewModel, AddReserveCommand>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.SelectedUserId))
                .ForMember(dst => dst.RoomId, opt => opt.MapFrom(src => src.SelectedRoomId))
                .ForMember(dst => dst.TimeStart, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dst => dst.TimeEnd, opt => opt.MapFrom(src => src.EndTime));
                x.CreateMap<ReserveViewModel, EditReserveCommand>()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.SelectedUserId))
                .ForMember(dst => dst.RoomId, opt => opt.MapFrom(src => src.SelectedRoomId))
                .ForMember(dst => dst.TimeStart, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dst => dst.TimeEnd, opt => opt.MapFrom(src => src.EndTime));
            }).CreateMapper();
        }

        #region Validators

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyRoomName(string roomName)
        {
            var result = await _mediator.Send(new VerifyRoomQuery
            {
                RoomName = roomName
            });
            return Json(result);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyReserve(int SelectedRoomId, DateTime StartTime, DateTime EndTime, int Id)
        {
            var result = await _mediator.Send(new VerifyReserveQuery
            {
                Id = Id,
                RoomId = SelectedRoomId,
                TimeStart = StartTime,
                TimeEnd = EndTime
            });
            return Json(result);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifySurname([RegularExpression(@"^[^\-][\p{L}\-]*[^\-]$")] string userSurname)
        {
            return Json(ModelState.IsValid);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName([RegularExpression(@"^[\p{L}]*$")] string userName)
        {
            return Json(ModelState.IsValid);
        }

        #endregion


        public async Task<IActionResult> Index()
        {
            var reserves = await _mediator.Send(new GetReserveListQuery
            {
                MinTime = DateTime.Today,
                MaxTime = DateTime.Today.AddDays(7)
            });
            var result = _mapper.Map<List<ReservesViewModel>>(reserves.OrderBy(res => res.TimeStart).ToList());

            return View(result);
        }

        public IActionResult AddUser()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(new UserViewModel());
            await _mediator.Send(_mapper.Map<AddUserCommand>(model));

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddRoom()
        {
            return View(new RoomViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
                return View(new RoomViewModel());
            await _mediator.Send(_mapper.Map<AddRoomCommand>(model));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddReserve()
        {
            return View(await GetReserveViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReserve(ReserveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await GetReserveViewModel());

            await _mediator.Send(_mapper.Map<AddReserveCommand>(model));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EditReserve(int id)
        {
            return View(await GetReserveViewModel(id));
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReserve(int id, ReserveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await GetReserveViewModel(id));

            await _mediator.Send(_mapper.Map<EditReserveCommand>(model));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteReserve(int id)
        {
            await _mediator.Send(new DeleteReserveCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<ReserveViewModel> GetReserveViewModel(int? id = null)
        {
            var model = new ReserveViewModel();


            var row = id.HasValue ? await _mediator.Send(new GetReserveQuery { Id = id.Value }) : null;

            var userList = await _mediator.Send(new GetUserListQuery());
            var users = userList.Select(x => new
            {
                x.Id,
                Name = $"{x.Surname} {x.Name}"
            })
            .OrderBy(x => x.Name)
            .ToList();
            model.SelectedUserId = row?.User.Id ?? users.Select(x => x.Id).FirstOrDefault();
            model.Users = new SelectList(users, "Id", "Name");

            var roomList = await _mediator.Send(new GetRoomListQuery());
            var rooms = roomList.OrderBy(x => x.Name).ToList();
            model.SelectedRoomId = row?.Room.Id ?? rooms.Select(x => x.Id).FirstOrDefault();
            model.Rooms = new SelectList(rooms, "Id", "Name");

            var now = DateTime.Now;
            model.StartTime = row?.TimeStart ?? RoundUp(now, TimeSpan.FromMinutes(1));
            model.EndTime = row?.TimeEnd ?? model.StartTime.AddHours(1);

            return model;
        }

        private static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
    }
}