namespace Playground.Models.Interfaces
{
    public interface IHasAuditProperties
    {
        DateOnly? CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateOnly? ModifiedDate { get; set; }
        string ModifiedBy { get; set; }


    }
}
