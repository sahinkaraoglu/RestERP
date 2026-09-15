namespace RestERP.Application.Features.Users
{
    public record UserCommandResult(bool Succeeded, IReadOnlyList<string> Errors)
    {
        public static UserCommandResult Success() => new(true, Array.Empty<string>());
        public static UserCommandResult Fail(params string[] errors) => new(false, errors);
        public static UserCommandResult Fail(IEnumerable<string> errors) => new(false, errors.ToList());
    }
}
