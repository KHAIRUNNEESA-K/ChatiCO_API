using ChatiCO.Domain.Common;
using ChatiCO.Domain.Entities;

public class Archive: BaseEntity
{
    public int ArchiveId { get; set; }
    public int UserId { get; set; }        
    public int ContactId { get; set; }
    public bool IsArchived { get; set; } = true;


    public User User { get; set; }
    public User Contact { get; set; }
}
