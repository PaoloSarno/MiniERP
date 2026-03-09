namespace MiniERP.Domain.ValueObjects
{
    public readonly record struct Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email is required");

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format");

            Value = value;
        }

        public override string ToString() => Value;
    }
}
