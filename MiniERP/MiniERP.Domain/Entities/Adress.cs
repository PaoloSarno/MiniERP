using MiniERP.Domain.Enums;

namespace MiniERP.Domain.Entities
{
    public class Address
    {
        public Guid AddressId { get; private set; }
        public Guid ClientId { get; private set; }
        public AddressType AddressType { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public string? Province { get; private set; }
        public string Country { get; private set; }

        public Address(Guid id, Guid clientId, AddressType type, string street, string city, string postalCode, string country)
        {
            AddressId = id;
            ClientId = clientId;
            AddressType = type;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }
    }
}
