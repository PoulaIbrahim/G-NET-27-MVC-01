using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymMangement.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbContext context) : base(context)
        {

        }
    }
}
