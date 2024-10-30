using Common;

namespace Customer.Core.Entities
{
    public class Customer : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
    }
}