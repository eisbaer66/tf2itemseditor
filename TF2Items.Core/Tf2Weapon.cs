using System.Collections.Generic;
using System.Diagnostics;

namespace TF2Items.Core
{
    [DebuggerDisplay("{Id}: {Name}")]
    public class Tf2Weapon
    {
        public Tf2Weapon(WeaponIdentifier id)
        {
            Id = id;
            Attributes = new List<Tf2WeaponAttribute>();
        }

        public WeaponIdentifier Id { get; set; }
        public string Name { get; set; }
        public IList<Tf2WeaponAttribute> Attributes { get; set; }
    }
}