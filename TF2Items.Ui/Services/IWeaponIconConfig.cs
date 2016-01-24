using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TF2Items.Ui.Services
{
    internal interface IWeaponIconConfig
    {
        string CacheDirectory { get; }
    }

    class WeaponIconConfig : IWeaponIconConfig
    {
        private readonly Dictionary<string, string> _section;

        public WeaponIconConfig()
        {
            _section = (ConfigurationManager.GetSection("Tf2Items/WeaponIcon") as System.Collections.Hashtable)
                 .Cast<System.Collections.DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());
        }

        public string CacheDirectory
        {
            get
            {
                if (!_section.ContainsKey("CacheDirectory"))
                    _section["CacheDirectory"] = "Cache\\WeaponIcons";
                return _section["CacheDirectory"];
            }
        }
    }
}