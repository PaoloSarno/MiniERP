using MiniERP.Domain.Enums;
using MiniERP.Domain.ValueObjects;

namespace MiniERP.Domain.Entities
{
    public class Client
    {
        private readonly List<Address> _addresses = new();

        public Guid Id { get; private set; }

        public string FullName { get; private set; }

        public Email Email { get; private set; }

        public PhoneNumber Phone { get; private set; }

        public ClientStatus Status { get; private set; }

        public ClientType Type { get; private set; }

        public IReadOnlyCollection<Address> Addresses => _addresses;

        public Client(
            string fullName,
            Email email,
            PhoneNumber phone,
            ClientType type)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Email = email;
            Phone = phone;
            Type = type;
            Status = ClientStatus.Active;
        }

        public void AddAddress(Address address)
        {
            _addresses.Add(address);
        }
    }
}
