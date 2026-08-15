using System.Collections.Generic;

namespace StreamViewer.Models
{
    /// <summary>
    /// Repräsentiert eine Streaming-Plattform mit ihren Einbettungs-URL-Informationen.
    /// </summary>
    public sealed class StreamPlatform
    {
        /// <summary>
        /// Ruft den internen Namen der Plattform ab oder legt diesen fest.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ruft den Anzeigenamen der Plattform ab oder legt diesen fest.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Ruft die Einbettungs-URL mit Platzhalter {0} für den Kanalnamen ab oder legt diese fest.
        /// </summary>
        public string EmbedUrl { get; set; }

        /// <summary>
        /// Ruft die Markenfarbe der Plattform als Hex-Zeichenfolge ab oder legt diese fest.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Ruft das Emoji-Symbol der Plattform ab oder legt dieses fest.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gibt die Liste aller unterstützten Streaming-Plattformen zurück.
        /// </summary>
        /// <returns>Eine Liste von StreamPlatform-Objekten.</returns>
        public static List<StreamPlatform> GetPlatforms()
        {
            return new List<StreamPlatform>
            {
                new StreamPlatform
                {
                    Name = "twitch",
                    DisplayName = "Twitch",
                    EmbedUrl = "https://player.twitch.tv/?channel={0}&parent=localhost",
                    Color = "#9146FF",
                    Icon = "🟣"
                },
                new StreamPlatform
                {
                    Name = "kick",
                    DisplayName = "Kick",
                    EmbedUrl = "https://player.kick.com/{0}",
                    Color = "#53FC18",
                    Icon = "🟢"
                },
                new StreamPlatform
                {
                    Name = "youtube",
                    DisplayName = "YouTube Live",
                    EmbedUrl = "https://www.youtube.com/embed/{0}?autoplay=1",
                    Color = "#FF0000",
                    Icon = "🔴"
                },
                new StreamPlatform
                {
                    Name = "trovo",
                    DisplayName = "Trovo",
                    EmbedUrl = "https://trovo.live/s/player/{0}",
                    Color = "#21BC4F",
                    Icon = "🟩"
                },
                new StreamPlatform
                {
                    Name = "wasd",
                    DisplayName = "WASD.TV",
                    EmbedUrl = "https://wasd.tv/embed/{0}",
                    Color = "#FF6600",
                    Icon = "🟠"
                }
            };
        }

        /// <summary>
        /// Formatiert die Einbettungs-URL mit dem angegebenen Kanalnamen.
        /// </summary>
        /// <param name="channelName">Der Name des Kanals.</param>
        /// <returns>Die formatierte Einbettungs-URL.</returns>
        public string GetEmbedUrl(string channelName)
        {
            return string.Format(EmbedUrl, channelName);
        }

        /// <summary>
        /// Gibt die Such-URL für die Plattform mit der angegebenen Suchanfrage zurück.
        /// </summary>
        /// <param name="query">Die Suchanfrage.</param>
        /// <returns>Die Such-URL der Plattform.</returns>
        public string GetSearchUrl(string query)
        {
            switch (Name)
            {
                case "twitch":
                    return "https://www.twitch.tv/search?term=" + query;
                case "kick":
                    return "https://kick.com/search?search=" + query;
                case "youtube":
                    return "https://www.youtube.com/results?search_query=" + query;
                case "trovo":
                    return "https://trovo.live/search?keyword=" + query;
                case "wasd":
                    return "https://wasd.tv/search?query=" + query;
                default:
                    return string.Empty;
            }
        }
    }
}
