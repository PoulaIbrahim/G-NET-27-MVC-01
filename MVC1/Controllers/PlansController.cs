using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymMangement.Controllers
{
    public class PlansController : Controller
    {
        //private readonly GymDbContext _context = new GymDbContext();
        private readonly IPlanRepository _planRepository;

        public PlansController(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        //Index
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            //var Plans = await _context.Plans.ToListAsync();
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }

        //Details
        public async Task<IActionResult> Details (int id, CancellationToken ct = default)
        {
            //var plan =await _context.Plans.FirstOrDefaultAsync(p =>  p.Id == id);
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null) return RedirectToAction(nameof(Index));

            return View(plan);
        }
    }
}
