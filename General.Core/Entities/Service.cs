using Common;

namespace General.Core.Entities
{
    public class Service  : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Account Account{ get; set; }
        public Guid AccountId { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}