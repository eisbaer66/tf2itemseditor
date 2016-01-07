using System.Collections.Generic;

namespace TF2Items.Core
{
    public class WeaponCollection
    {
        public UserIdentifier Users { set; get; }
        public IList<Weapon> Weapons { set; get; }

        public WeaponCollection()
            :this(UserIdentifier.Any())
        {
        }

        public WeaponCollection(UserIdentifier users)
        {
            Users = users;
            Weapons = new List<Weapon>();
        }
    }
}