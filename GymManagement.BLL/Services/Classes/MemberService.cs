using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
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

        public Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
