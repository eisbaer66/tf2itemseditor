using System.Collections.Generic;
using TF2Items.Core;
using ValveFormat;

namespace TF2Items.Parsers
{
    public interface ITf2ItemsWeaponsWriter
    {
        void Write(ServerConfiguration config, string filename);
    }

    public class Tf2ItemsWeaponsWriter : ITf2ItemsWeaponsWriter
    {
        public void Write(ServerConfiguration config, string filename)
        {
            ValveFormatParser parser = new ValveFormatParser
                                       {
                                           RootNode = new DataNode
                                                      {
                                                          Key = "custom_weapons_v3",
                                                      }
                                       };
            AddWeaponCollections(parser.RootNode, config.WeaponCollections);

            parser.SaveFile(filename);
        }

        private void AddWeaponCollections(DataNode parent, IList<WeaponCollection> weaponCollections)
        {
            foreach (WeaponCollection weaponCollection in weaponCollections)
            {
                AddWeaponCollection(parent, weaponCollection);
            }
        }

        private void AddWeaponCollection(DataNode parent, WeaponCollection weaponCollection)
        {
            if (weaponCollection.Weapons.Count == 0)
                return;

            DataNode node = new DataNode
                            {
                                Key = weaponCollection.Users.ToString(),
                            };
            AddWeapons(node, weaponCollection.Weapons);
            AddChildNode(parent, node);
        }

        private void AddWeapons(DataNode collectionNode, IList<ConfigWeapon> weapons)
        {
            foreach (ConfigWeapon weapon in weapons)
            {
                AddWeapon(collectionNode, weapon);
            }
        }

        private void AddWeapon(DataNode collectionNode, ConfigWeapon weapon)
        {
            if (weapon.Attributes.Count == 0 &&
                string.IsNullOrEmpty(weapon.AdminFlags) &&
                weapon.Level == null &&
                weapon.Quality == null)
                return;

            DataNode node = new DataNode
                            {
                                Key = weapon.Id.ToString(),
                            };
            AddAttribute<int?, int>(node, "quality", weapon.Quality);
            AddAttribute<int?, int>(node, "level", weapon.Level);
            AddAttribute(node, "admin-flags", weapon.AdminFlags);
            AddAttributes(node, weapon.Attributes);
            AddChildNode(collectionNode, node);
        }

        private void AddAttributes(DataNode node, IList<ConfigWeaponAttribute> attributes)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                ConfigWeaponAttribute attribute = attributes[i];
                string attributeString = string.Format("{0} ; {1}", attribute.Id, attribute.Value);

                string index = (i+1).ToString();
                AddAttribute(node, index, attributeString);
            }
        }

        private void AddAttribute<T, TUnit>(DataNode node, string key, T item) where TUnit : struct 
        {
            TUnit? nullable = item as TUnit?;
            if (nullable == null)
                return;

            string value = nullable.Value.ToString();
            AddAttribute(node, key, value);
        }

        private void AddAttribute(DataNode node, string key, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            DataNode qualityNode = new DataNode
                                   {
                                       Key = key,
                                       Value = value
                                   };
            AddChildNode(node, qualityNode);
        }

        private void AddChildNode(DataNode parent, DataNode child)
        {
            child.Parent = parent;
            parent.SubNodes.Add(child);
        }
    }
}