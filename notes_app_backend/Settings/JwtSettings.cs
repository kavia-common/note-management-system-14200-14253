namespace NotesAppBackend.Settings
{
    /// <summary>
    /// JWT configuration settings bound from configuration.
    /// </summary>
    public class JwtSettings
    {
        public string Secret { get; set; } = "PLEASE_CHANGE_ME_MIN_32_CHARS";
        public string Issuer { get; set; } = "notes-app";
        public string Audience { get; set; } = "notes-app";
        public int ExpirationHours { get; set; } = 24;
    }
}
