using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using TF2Items.Core;
using ValveFormat;

namespace TF2Items.Parsers
{
    public interface IItemsGameWeaponsParser
    {
        Func<DataNode, bool> WeaponsFilter { get; set; }
        Task<IDictionary<WeaponIdentifier, Tf2Weapon>> ParseAsDictionary(string filePath);
        Task<IList<Tf2Weapon>> Parse(string filePath);
        Task<IEnumerable<Tf2Weapon>> ParseSingle(string filePath);
    }

    public class ItemsGameWeaponsParser : IItemsGameWeaponsParser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemsGameWeaponsParser));
        
        public Func<DataNode, bool> WeaponsFilter { get; set; }

        public Func<DataNode, bool> DefaultWeaponsFilter = node =>
                                                           {
                                                               foreach (DataNode subNode in node.SubNodes)
                                                               {
                                                                   if (subNode.Key == "item_type_name" && (subNode.Value == "#TF_T" || subNode.Value == "#TF_LockedCrate"))
                                                                       return false;
                                                                   if (subNode.Key == "item_name" && (subNode.Value == "#TF_SupplyCrate"))
                                                                       return false;
                                                                   if (subNode.Key == "prefab" && (subNode.Value == "valve paint_can"))
                                                                       return false;
                                                               }

                                                               return true;
                                                           };

        public ItemsGameWeaponsParser()
        {
            WeaponsFilter = DefaultWeaponsFilter;
        }

        public async Task<IDictionary<WeaponIdentifier, Tf2Weapon>> ParseAsDictionary(string filePath)
        {
            return (await ParseSingle(filePath))
                        .ToDictionary(f => f.Id);
        }
        public async Task<IList<Tf2Weapon>> Parse(string filePath)
        {
            return (await ParseSingle(filePath)).ToList();
        }
        public async Task<IEnumerable<Tf2Weapon>> ParseSingle(string filePath)
        {
            using (NDC.Push("parse"))
            {
                ValveFormatParser parser = new ValveFormatParser(filePath);

                await  Task.Run(() => parser.LoadFile());

                return AddWeapons(parser.RootNode.SubNodes);
            }
        }

        private IEnumerable<Tf2Weapon> AddWeapons(List<DataNode> nodes)
        {
            DataNode itemsNode = nodes.Find(n => n.Key == "items");
            if (itemsNode == null)
            {
                Log.Error("could not find node 'items'");
                yield break;
            }

            using (NDC.Push("Weapon"))
            {
                foreach (DataNode node in itemsNode.SubNodes)
                {
                    using (NDC.Push(node.Key))
                    {
                        if (!WeaponsFilter(node))
                        {
                            Log.Info("ignored due to WeaponsFilter");
                            continue;
                        }

                        Tf2Weapon weapon = CreateWeapon(node);
                        if (weapon == null)
                            continue;

                        AddStats(weapon, node.SubNodes);

                        yield return weapon;
                    }
                }
            }
        }

        private Tf2Weapon CreateWeapon(DataNode node)
        {
            WeaponIdentifier weaponIdentifier;
            if (!Primitives.ParseWeaponIdentifier(node.Key, out weaponIdentifier))
                return null;

            return new Tf2Weapon(weaponIdentifier);
        }

        private void AddStats(Tf2Weapon weapon, List<DataNode> nodes)
        {
            using (NDC.Push("Property"))
            {
                foreach (DataNode node in nodes)
                {
                    using (NDC.Push(node.Key))
                    {
                        switch (node.Key)
                        {
                            case "name":
                                weapon.Name = node.Value;
                                break;
                            case "image_inventory":
                                weapon.ImageDirectory = node.Value;
                                break;
                            case "attributes":
                                weapon.Attributes = CreateAttributes(node.SubNodes).ToList();
                                break;

                        }
                    }
                }
            }
        }

        private IEnumerable<Tf2WeaponAttribute> CreateAttributes(List<DataNode> nodes)
        {
            foreach (DataNode node in nodes)
            {
                string value = null;
                string attributeClass = null;

                foreach (DataNode subNode in node.SubNodes)
                {
                    switch (subNode.Key)
                    {
                        case "attribute_class":
                            attributeClass = subNode.Value;
                            break;
                        case "value":
                            value = subNode.Value;
                            break;
                    }

                }
                if (string.IsNullOrEmpty(attributeClass))
                {
                    Log.Warn("could not find attribute class");
                    continue;
                }
                if (string.IsNullOrEmpty(value))
                {
                    Log.Warn("could not find value");
                    continue;
                }

                yield return new Tf2WeaponAttribute(attributeClass, node.Key, value);
            }
        }
    }
}