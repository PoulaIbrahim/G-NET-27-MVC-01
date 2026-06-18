using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;

        public MemberService(
            IGenericRepository<Member> memberRepository,
            IGenericRepository<Membership> membershipRepository,
            IGenericRepository<HealthRecord> healthRecordRepository,
            IGenericRepository<Booking> bookingRepository
            )
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _healthRecordRepository = healthRecordRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            //Check Email
            //Check Phone
            var EmailExists = await _memberRepository.AnyAsync(M => M.Email == model.Email, ct);
            var PhoneExists = await _memberRepository.AnyAsync(M => M.Phone == model.Phone, ct);

            if (EmailExists || PhoneExists) return false;
            //Casting [mapping] From CreateMemberViewModel to member

            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street,
                },
                HealthRecord = new HealthRecord()
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note,
                }
            };
            //Add to database

          var count = await  _memberRepository.AddAsync(member, ct);
            return count > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);

            // Mapping --> Casting From Member to MemberViewModel
            //var membersViewModel = new List<MemberViewModel>();

            //foreach (var member in members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Photo = member.Photo,
            //        Phone = member.Phone,
            //        Email = member.Email,
            //        Gender = member.Gender.ToString()
            //    };
            //    membersViewModel.Add(memberViewModel);
            //}

            var membersViewModel = members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Photo = member.Photo,
                Phone = member.Phone,
                Email = member.Email,
                Gender = member.Gender.ToString()
            });


            return membersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member is null) return null;

            var model = new MemberViewModel()
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
            var activeMembership = await _membershipRepository.FirstOrDefaultAsync(M => M.MemberId == memberId && M.EndDate > DateTime.UtcNow, ct);

            if(activeMembership is not null)
            {
                model.Phone = activeMembership.Plan.Name;
                model.MembershipStartDate = activeMembership.CreatedAt.ToString();
                model.MembershipEndDate = activeMembership.EndDate.ToString();
            }
            return model;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
            
            var healthRecord = await _healthRecordRepository.FirstOrDefaultAsync(H => H.MemberId == memberId, ct);
            if (healthRecord is null) return null;

            var model = new HealthRecordViewModel()
            {
                Height = healthRecord.Height,
                Weight = healthRecord.Weight,
                BloodType = healthRecord.BloodType,
                Note = healthRecord.Note,
            };

            return model;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdatedAsync(int memberId, CancellationToken ct)
        {
           var member = await _memberRepository.GetByIdAsync(memberId, ct);
           if (member is null) return null;

            var model = new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Phone = member.Phone,
                Photo = member.Photo,
                City = member.Address.City,
                BuildingNumber = member.Address.BuildingNumber,
                Email = member.Email,
                Street = member.Address.Street,
            };

            return model;
        }

        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member is null) return false;

            var EmailExists = await _memberRepository.AnyAsync(M => M.Email == model.Email, ct);
            var PhoneExists = await _memberRepository.AnyAsync(M => M.Phone == model.Phone, ct);

            if (EmailExists || PhoneExists) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;

            var count = await _memberRepository.UpdateAsync(member);
            return count > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member is null) return false;

            var hasFutureSessions = await _bookingRepository.AnyAsync(B => B.MemberId  == memberId && B.Session.StartDate > DateTime.Now, ct);
            if (hasFutureSessions) return false;

            var count = await _memberRepository.DeleteAsync(member, ct);
            return count > 0;
        }
    }
}
