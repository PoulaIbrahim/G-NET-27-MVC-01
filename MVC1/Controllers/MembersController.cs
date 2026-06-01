using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }

    }
}
