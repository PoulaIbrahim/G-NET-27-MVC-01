using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.DbContexts;
using GymMangement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymMangement.Controllers
{
    //CLR
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _planService.GetAllPlansAsync(ct));

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanByIdAsync(id, ct);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan cannot be edited (not found, inactive, or has active membership.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _planService.UpdatePlanAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Plan updated successfully.";
            else
                TempData["ErrorMessage"] = "Plan failed to update.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await _planService.ToggleActivationAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Plan status changed";
            else
                TempData["ErrorMessage"] = "Failed to Toggle plan status";
            return RedirectToAction(nameof(Index));
        }

    }
}
