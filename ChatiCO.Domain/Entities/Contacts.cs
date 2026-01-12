using ChatiCO.Domain.Common;

namespace ChatiCO.Domain.Entities
{
    public class Contacts : BaseEntity
    {
        public int ContactId { get; set; }
        public int UserId { get; set; }
        public int ContactUserId { get; set; }
        public bool IsBlocked { get; set; } = false;
        public User User { get; set; }
        public User ContactUser { get; set; }
    }
}
