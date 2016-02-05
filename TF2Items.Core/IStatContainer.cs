using System.Collections.Generic;

namespace TF2Items.Core
{
    public interface IStatContainer
    {
        string Name { get; set; }
        string ImageDirectory { get; set; }
        IList<Tf2WeaponAttribute> Attributes { get; set; }
        string PrefabName { get; set; }
    }
}