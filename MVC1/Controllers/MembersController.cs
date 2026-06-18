using GymManagement.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymMangement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        //member/index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);

            ////To Send Extra Info. To View
            ////ViewData
            //ViewData["Data01"] = "Hello to viewdata";

            ////ViewBag
            //ViewBag.Data02 = "Hello to viewbag";

            ////TempData
            //TempData["Data03"] = "Hello to tempdata";

            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid) //server side validation
            {
                var result = await _memberService.CreateMemberAsync(model, ct);

                if (result)
                {
                    TempData["SuccessMessage"] = "Member Created Successfully! :)";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed To Create Member :(";
                }
             
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            //Get Member By Id
            var result = await _memberService.GetMemberDetailsAsync(id, ct);
            if(result is null)
            {
                TempData["ErrorMessage"] = "Member Not Found !!";
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var result = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Health Member Not Found !!";
                return RedirectToAction("Index"); 
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            //Get Data By Id
            var result = await _memberService.GetMemberToUpdatedAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Member Not Found :(";
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id,MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid) //server side validation
            {
                var result = await _memberService.UpdateMemberAsync(id, model, ct);

                if (result)
                {
                    TempData["SuccessMessage"] = "Member Update Successfully! :)";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed To Update Member :(";
                }

                return RedirectToAction("Index");
            }
            //Get Data By Id 
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result =await _memberService.GetMemberDetailsAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Member Not Found :(";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Member Delete Successfully! :)";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Member :(";
            }
            return RedirectToAction ("Index");
        }
    }
}
