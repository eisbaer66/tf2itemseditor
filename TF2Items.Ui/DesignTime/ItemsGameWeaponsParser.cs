using System;
using System.Collections.Generic;
using System.Linq;
using TF2Items.Core;
using TF2Items.Parsers;
using ValveFormat;

namespace TF2Items.Ui.DesignTime
{
    class ItemsGameWeaponsParser : IItemsGameWeaponsParser
    {
        public Func<DataNode, bool> WeaponsFilter { get; set; }
        public IDictionary<WeaponIdentifier, Tf2Weapon> ParseAsDictionary(string filePath)
        {
            return ParseSingle(filePath)
                .ToDictionary(f => f.Id);
        }

        public IList<Tf2Weapon> Parse(string filePath)
        {
            return ParseSingle(filePath).ToList();
        }

        public IEnumerable<Tf2Weapon> ParseSingle(string filePath)
        {
            return new List<Tf2Weapon>
                   {
                       new Tf2Weapon(WeaponIdentifier.FromId(66))
                       {
                           Name = "icebear2000",
                           Attributes = new []
                                        {
                                            new Tf2WeaponAttribute("mult_dmg_vs_players", "dmg penalty vs players", "10"),
                                        }
                       }
                   };
        }
    }
}