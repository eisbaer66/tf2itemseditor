using System.Collections.Generic;
using System.Diagnostics;

namespace TF2Items.Core
{
    [DebuggerDisplay("{Id}: {Name}")]
    public class Tf2Weapon : IStatContainer
    {
        public Tf2Weapon(WeaponIdentifier id)
        {
            Id = id;
            Attributes = new List<Tf2WeaponAttribute>();
        }

        public WeaponIdentifier Id { get; set; }
        public string Name { get; set; }
        public string ImageDirectory { get; set; }
        public string PrefabName { get; set; }
        public IList<Tf2WeaponAttribute> Attributes { get; set; }
    }
}