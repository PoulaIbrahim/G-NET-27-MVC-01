using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.sessionRepository.GetAllSessionWithTrainerAndCategoryAsync(ct);

            if (sessions is null || !sessions.Any()) return null;
            // Trainer
            // Category
            // Mapping

            var mappedSessions = sessions.Select(S => new SessionViewModel()
            {
                Id = S.Id,
                Capacity = S.Capacity,
                CategoryName = S.Category.CategoryName,
                TrainerName = S.Trainer.Name,
                Description = S.Description,
                EndDate = S.EndDate,
                StartDate = S.StartDate,
            });

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.sessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return mappedSessions;
        }

        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
