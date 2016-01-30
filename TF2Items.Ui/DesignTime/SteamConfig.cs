using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TF2Items.Core;
using TF2Items.Ui.Services;
using TF2Items.ValvePak;

namespace TF2Items.Ui.DesignTime
{
    class SteamConfig : ISteamConfig
    {
        public string TeamFortress2Directory { get { return @"D:\Games\Steam\SteamApps\common\Team Fortress 2"; } }
    }

    public class Config : IConfig
    {
        public string HlExtractLocation
        {
            get { return @"C:\Users\Jan\Documents\GitHub\tf2itemseditor_vso\tools\HLExtract.exe"; }
        }

        public string VtfCmdLocation
        {
            get { return @"C:\Users\Jan\Documents\GitHub\tf2itemseditor_vso\tools\VTFCmd.exe"; }
        }
    }

    class WeaponIconConfig : IWeaponIconConfig
    {
        public string CacheDirectory
        {
            get { return @"C:\Users\Jan\Documents\GitHub\tf2itemseditor_vso\TF2Items.Ui\bin\Debug\Cache"; }
        }
    }
}