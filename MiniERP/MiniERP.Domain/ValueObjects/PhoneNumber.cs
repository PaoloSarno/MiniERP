namespace MiniERP.Domain.ValueObjects
{ 
    public readonly record struct PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone number is required");

            Value = value;
        }

        public override string ToString() => Value;
    }
}

