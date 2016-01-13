using System;
using System.Collections.Generic;
using log4net;
using TF2Items.Core;
using ValveFormat;

namespace TF2Items.Parsers
{
    public class Tf2ItemsWeaponsParser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Tf2ItemsWeaponsParser));
        
        public ServerConfiguration Parse(string filePath)
        {
            using (NDC.Push("parse"))
            {
                ValveFormatParser parser = new ValveFormatParser(filePath);
                parser.LoadFile();

                ServerConfiguration configuration = new ServerConfiguration();

                AddWeaponCollections(configuration, parser.RootNode.SubNodes);

                return configuration;
            }
        }

        private void AddWeaponCollections(ServerConfiguration config, List<DataNode> nodes)
        {
            using (NDC.Push("WeaponCollection"))
            {
                foreach (DataNode node in nodes)
                {
                    using (NDC.Push(node.Key))
                    {
                        WeaponCollection collection = CreateWeaponCollection(node);
                        if (collection == null)
                            continue;

                        AddWeapons(collection, node.SubNodes);

                        config.WeaponCollections.Add(collection);
                    }
                }
            }
        }

        private void AddWeapons(WeaponCollection collection, List<DataNode> nodes)
        {
            using (NDC.Push("Weapon"))
            {
                foreach (DataNode node in nodes)
                {
                    string idRaw = node.Key;
                    using (NDC.Push(idRaw))
                    {
                        ConfigWeapon weapon = CreateWeapon(idRaw);
                        if (weapon == null)
                            continue;

                        AddWeaponAttributes(weapon, node.SubNodes);

                        collection.Weapons.Add(weapon);
                    }
                }
            }
        }

        private void AddWeaponAttributes(ConfigWeapon weapon, List<DataNode> nodes)
        {
            using (NDC.Push("Attribute"))
            {
                foreach (DataNode node in nodes)
                {
                    using (NDC.Push(node.Key))
                    {
                        ConfigWeaponAttribute attribute = CreateWeaponAttribute(weapon, node);
                        if (attribute == null)
                            continue;

                        weapon.Attributes.Add(attribute);
                    }
                }
            }
        }

        private static WeaponCollection CreateWeaponCollection(DataNode node)
        {
            UserIdentifier users;
            if (node.Key == "*")
                users = UserIdentifier.Any();
            else
            {
                string[] streamIds = node.Key.Split(new[] { " ; " }, StringSplitOptions.RemoveEmptyEntries);
                if (streamIds.Length == 0)
                {
                    Log.Warn("Could not detect Users in " + node.Key);
                    return null;
                }

                users = UserIdentifier.FromStreamIds(streamIds);
            }

            WeaponCollection collection = new WeaponCollection(users);
            return collection;
        }

        private static ConfigWeapon CreateWeapon(string idRaw)
        {
            WeaponIdentifier weaponId;
            if (!Primitives.ParseWeaponIdentifier(idRaw, out weaponId))
                return null;

            ConfigWeapon weapon = new ConfigWeapon(weaponId);
            return weapon;
        }

        private ConfigWeaponAttribute CreateWeaponAttribute(ConfigWeapon weapon, DataNode node)
        {
            if (node.Key == "quality")
            {
                int quality;
                if (!Primitives.TryParse(node.Value, "quality", out quality))
                    return null;
                weapon.Quality = quality;
                return null;
            }
            if (node.Key == "level")
            {
                int level;
                if (!Primitives.TryParse(node.Value, "level", out level))
                    return null;
                weapon.Level = level;
                return null;
            }
            if (node.Key == "admin-flags")
            {
                weapon.AdminFlags = node.Value;
                return null;
            }

            int index;
            if (!Primitives.TryParse(node.Key, "attribute-index", out index))
                return null;

            string[] pair = node.Value.Split(new[] {" ; "}, StringSplitOptions.RemoveEmptyEntries);
            if (pair.Length != 2)
            {
                Log.Warn("could not detect attribute in " + node.Value);
                return null;
            }

            int id;
            if (!Primitives.TryParse(pair[0], "attribute-id", out id))
                return null;

            ConfigWeaponAttribute attribute = new ConfigWeaponAttribute(id, pair[1]);
            return attribute;
        }
    }
}