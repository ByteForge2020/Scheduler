using Common;

namespace General.Core.Entities
{
    public class Specialist  : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Account Account{ get; set; }
        public Guid AccountId { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}