using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        // Get all members
        // Get member by ID
        // Get member's health record
        // Add new member
        // Update member
        // Delete member
        // Get member to update

        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct);
        Task<MemberToUpdateViewModel?> GetMemberToUpdatedAsync(int memberId, CancellationToken ct);
        Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct);
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct);


    }
}
