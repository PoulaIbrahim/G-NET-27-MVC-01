using GymManagement.DAL.Models;

namespace GymMangement.Models
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Membership> Members { get; set; }
    }
}
