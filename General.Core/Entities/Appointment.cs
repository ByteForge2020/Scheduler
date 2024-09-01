using Common;

namespace General.Core.Entities
{
    public class Appointment  : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid SpecialistId { get; set; }
        public Account Account{ get; set; }
        public Customer Customer { get; set; }
        public Service Service { get; set; }
        public Specialist Specialist { get; set; }
        public DateTime Start { get; set; }
    }
}