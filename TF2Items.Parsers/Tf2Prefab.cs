using System.Collections.Generic;
using System.Diagnostics;
using TF2Items.Core;

namespace TF2Items.Parsers
{
    [DebuggerDisplay("{Id}")]
    public class Tf2Prefab : IStatContainer
    {
        public string Id { get; set; }
        public string PrefabName { get; set; }
        public string Name { get; set; }
        public string ImageDirectory { get; set; }
        public IList<Tf2WeaponAttribute> Attributes { get; set; }

        public Tf2Prefab()
        {
            Attributes = new List<Tf2WeaponAttribute>();
        }
    }
}