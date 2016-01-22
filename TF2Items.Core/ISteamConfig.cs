using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TF2Items.Core
{
    public interface ISteamConfig
    {
        string TeamFortress2Directory { get; }
    }

    public class SteamConfig : ISteamConfig
    {
        private readonly Dictionary<string, string> _section;

        public SteamConfig()
        {
            _section = (ConfigurationManager.GetSection("Tf2Items/Steam") as System.Collections.Hashtable)
                 .Cast<System.Collections.DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());
        }

        public string TeamFortress2Directory
        {
            get
            {
                if (!_section.ContainsKey("TeamFortress2Directory"))
                    return string.Empty;
                return _section["TeamFortress2Directory"];
            }
        }
    }
}