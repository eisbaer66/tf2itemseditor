using System.Collections.Generic;

namespace TF2Items.Core
{
    public class Weapon
    {
        public Weapon(WeaponIdentifier id)
        {
            Id = id;
            Attributes = new List<WeaponAttribute>();
        }

        public WeaponIdentifier Id { get; set; }
        public string Name { get; set; }
        public int? Quality { get; set; }
        public int? Level { get; set; }
        public string AdminFlags { get; set; }
        public IList<WeaponAttribute> Attributes { get; set; }
    }
}