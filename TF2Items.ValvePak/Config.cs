using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TF2Items.ValvePak
{
    public interface IConfig
    {
        string HlExtractLocation { get; }
        string VtfCmdLocation { get; }
    }

    public class Config : IConfig
    {
        private readonly Dictionary<string, string> _section;

        public Config()
        {
            _section = (ConfigurationManager.GetSection("Tf2Items/ValvePak") as System.Collections.Hashtable)
                 .Cast<System.Collections.DictionaryEntry>()
                 .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());
        }

        public string HlExtractLocation
        {
            get
            {
                if (!_section.ContainsKey("HlExtractLocation"))
                    _section["HlExtractLocation"] = "tools\\HLExtract.exe";
                return _section["HlExtractLocation"];
            }
        }

        public string VtfCmdLocation
        {
            get
            {
                if (!_section.ContainsKey("VtfCmdLocation"))
                    _section["VtfCmdLocation"] = "tools\\VTFCmd.exe";
                return _section["VtfCmdLocation"];
            }
        }
    }
}
