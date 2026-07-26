using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
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
        private readonly IMapper _mapper;

       public SessionService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.EndDate <= model.StartDate) return false;
            if (model.StartDate <= DateTime.Now) return false;
            if (model.Capacity < 1 || model.Capacity > 25) return false;

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null) return false;

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category is null) return false;

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return false;

            var session = _mapper.Map<Session>(model);
            _unitOfWork.GetRepository<Session>().Add(session);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0;
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
