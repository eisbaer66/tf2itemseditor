using System.Globalization;
using log4net;
using TF2Items.Core;

namespace TF2Items.Parsers
{
    public class Primitives
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Primitives));

        public static bool ParseWeaponIdentifier(string idRaw, out WeaponIdentifier weaponId)
        {
            if (idRaw == "*")
                weaponId = WeaponIdentifier.Any();
            else
            {
                int id;
                if (!TryParse(idRaw, "Key", out id))
                {
                    weaponId = WeaponIdentifier.Any();
                    return false;
                }
                weaponId = WeaponIdentifier.FromId(id);
            }
            return true;
        }

        public static bool TryParse(string idRaw, string label, out int id)
        {
            if (!int.TryParse(idRaw, NumberStyles.Any, new CultureInfo("en-US"), out id))
            {
                Log.Warn(string.Format("Could not parse {1} {0} as Integer", idRaw, label));
                return false;
            }
            return true;
        }

        public static bool TryParse(string idRaw, string label, out float id)
        {
            if (!float.TryParse(idRaw, NumberStyles.Any, new CultureInfo("en-US"), out id))
            {
                Log.Warn(string.Format("Could not parse {1} {0} as Float", idRaw, label));
                return false;
            }
            return true;
        }
    }
}