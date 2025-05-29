namespace ProductsApp.Infrastructure;

internal record JwtOptions
{
    public required string Secret { get; init; }

    public required string Issuer { get; init; }
}