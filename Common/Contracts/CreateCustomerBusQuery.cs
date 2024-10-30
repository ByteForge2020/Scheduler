namespace Common.Contracts;
public class CreateCustomerBusQuery
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Surname { get; set; }
}