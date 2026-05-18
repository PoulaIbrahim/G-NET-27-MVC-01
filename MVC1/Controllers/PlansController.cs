using GymMangement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymMangement.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext _context = new GymDbContext();
        //Index
        public async Task<IActionResult> Index()
        {
            var Plans = await _context.Plans.ToListAsync();
            return View(Plans);
        }

        //Details
        public async Task<IActionResult> Details (int id)
        {
            var plan =await _context.Plans.FirstOrDefaultAsync(p =>  p.Id == id);
            if (plan is null) return RedirectToAction(nameof(Index));

            return View(plan);
        }
    }
}
