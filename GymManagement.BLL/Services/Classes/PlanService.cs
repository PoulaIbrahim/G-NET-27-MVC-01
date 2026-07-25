using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            var plansViewModel = plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = p.IsActive,
                Price = p.Price,
                Description = p.Description,
                DurationDays = p.DurationDays,
            });
            return plansViewModel;
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);
            if (plan is null)
                return null;
            else
                return new PlanViewModel()
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    IsActive = plan.IsActive,
                    Price = plan.Price,
                    Description = plan.Description,
                    DurationDays = plan.DurationDays,
                };
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int PlanId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);
            if (plan is null || !plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(PlanId, ct))
                return null;
            else
                return new UpdatePlanViewModel()
                {
                    PlanName = plan.Name,
                    Price = plan.Price,
                    DurationDays = plan.DurationDays,
                    Description = plan.Description
                    
                };
        }

        public async Task<bool> ToggleActivationAsync(int PlanId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(PlanId, ct);
            if (plan is null) return false;

            if (plan.IsActive && await HasActiveMembershipsAsync(PlanId, ct))
                return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return false;
            if (await HasActiveMembershipsAsync(id, ct)) 
                return false;
            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        #region Helper Methods
        private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<Membership>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }
        #endregion

    }
}
