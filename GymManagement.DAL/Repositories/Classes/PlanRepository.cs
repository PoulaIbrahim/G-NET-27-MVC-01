using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.DbContexts;
using GymMangement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {

        public PlanRepository(GymDbContext context) : base(context)
        {

        }

        public Task<Plan> GetAllWithMemberAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
