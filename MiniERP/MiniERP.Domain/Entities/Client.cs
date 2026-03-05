using MiniERP.Domain.Enums;

namespace MiniERP.Domain.Entities
{
    public class Client
    {
        public Guid ClientId { get; private set; }
        public string FullName { get; private set; }
        public string? CompanyName { get; private set; }
        public string? ContactFullName { get; private set; }
        public string EmailAddress { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? CellNumber { get; private set; }
        public ClientStatus ClientStatus { get; private set; }
        public ClientType ClientType { get; private set; }

        public List<Address> Addresses { get; private set; } = new();
        public List<Order> Orders { get; private set; } = new();

        public Client(Guid id, string fullName, string email, ClientType type)
        {
            ClientId = id;
            FullName = fullName;
            EmailAddress = email;
            ClientType = type;
            ClientStatus = ClientStatus.Active;
        }

        // Méthodes métier
        public void Deactivate() => ClientStatus = ClientStatus.Inactive;
    }
}
