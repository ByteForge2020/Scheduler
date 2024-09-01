using Common;

namespace General.Core.Entities
{
    public class Account : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Specialist> Specialists { get; set; }
    }
}