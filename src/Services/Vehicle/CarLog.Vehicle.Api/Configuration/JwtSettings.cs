namespace CarLog.Vehicle.Api.Configuration
{
    public sealed class JwtSettings
    {
        public const string SectionName = "Jwt";

        public required string Issuer { get; set; }

        public required string Audience { get; set; }
        
        public required string SecretKey { get; set; }
    }
}
