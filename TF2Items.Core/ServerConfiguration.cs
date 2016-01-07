using System.Collections.Generic;

namespace TF2Items.Core
{
    public class ServerConfiguration
    {
        public IList<WeaponCollection> WeaponCollections { get; set; }

        public ServerConfiguration()
        {
            WeaponCollections = new List<WeaponCollection>();
        }
    }
}
