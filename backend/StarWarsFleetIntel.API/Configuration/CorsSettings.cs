﻿namespace StarWarsFleetIntel.API.Configuration
{
    public class CorsSettings
    {
        public const string SectionName = "CorsSettings";

        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    }
}
