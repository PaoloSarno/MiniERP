using MiniERP.Domain.Enums;

namespace MiniERP.Domain.ValueObjects
{
    public sealed class Address
    {
        public AddressType Type { get; }

        public string Street { get; }

        public string City { get; }

        public string PostalCode { get; }

        public string Country { get; }

        public Address(
            AddressType type,
            string street,
            string city,
            string postalCode,
            string country)
        {
            Type = type;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }
    }

}
