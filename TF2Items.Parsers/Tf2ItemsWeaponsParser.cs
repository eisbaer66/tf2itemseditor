using System;
using System.Collections.Generic;
using System.Globalization;
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
                        UserIdentifier users;
                        if (node.Key == "*")
                            users = UserIdentifier.Any();
                        else
                        {
                            string[] streamIds = node.Key.Split(new []{" ; "}, StringSplitOptions.RemoveEmptyEntries);
                            if (streamIds.Length == 0)
                            {
                                Log.Warn("Could not detect Users in " + node.Key);
                                continue;
                            }

                            users = UserIdentifier.FromStreamIds(streamIds);
                        }

                        WeaponCollection collection = new WeaponCollection(users);

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
                        WeaponIdentifier weaponId;
                        if (!ParseWeaponIdentifier(idRaw, out weaponId))
                            continue;

                        Weapon weapon = new Weapon(weaponId);

                        AddStats(weapon, node.SubNodes);

                        collection.Weapons.Add(weapon);
                    }
                }
            }
        }

        private static bool ParseWeaponIdentifier(string idRaw, out WeaponIdentifier weaponId)
        {
            if (idRaw == "*")
                weaponId = WeaponIdentifier.Any();
            else
            {
                int id;
                if (!TryParse(idRaw, "Key", out id))
                {
                    weaponId = WeaponIdentifier.Any();
                    return false;
                }
                weaponId = WeaponIdentifier.FromId(id);
            }
            return true;
        }

        private static void AddStats(Weapon weapon, List<DataNode> nodes)
        {
            using (NDC.Push("Attribute"))
            {
                foreach (DataNode node in nodes)
                {
                    using (NDC.Push(node.Key))
                    {
                        if (node.Key == "quality")
                        {
                            int quality;
                            if (!TryParse(node.Value, "quality", out quality))
                                continue;
                            weapon.Quality = quality;
                            continue;
                        }
                        if (node.Key == "level")
                        {
                            int level;
                            if (!TryParse(node.Value, "level", out level))
                                continue;
                            weapon.Level = level;
                            continue;
                        }
                        if (node.Key == "admin-flags")
                        {
                            weapon.AdminFlags = node.Value;
                            continue;
                        }

                        int index;
                        if (!TryParse(node.Key, "attribute-index", out index))
                            continue;

                        string[] pair = node.Value.Split(new[] {" ; "}, StringSplitOptions.RemoveEmptyEntries);
                        if (pair.Length != 2)
                        {
                            Log.Warn("could not detect attribute in " + node.Value);
                            continue;
                        }

                        int id;
                        if (!TryParse(pair[0], "attribute-id", out id))
                        {
                            continue;
                        }

                        float value;
                        if (!TryParse(pair[1], "attribute-value", out value))
                        {
                            continue;
                        }

                        WeaponAttribute attribute = new WeaponAttribute(id, value);
                        weapon.Attributes.Add(attribute);
                    }
                }
            }
        }

        private static bool TryParse(string idRaw, string label, out int id)
        {
            if (!int.TryParse(idRaw, NumberStyles.Any, new CultureInfo("en-US"), out id))
            {
                Log.Warn(string.Format("Could not parse {1} {0} as Integer", idRaw, label));
                return false;
            }
            return true;
        }

        private static bool TryParse(string idRaw, string label, out float id)
        {
            if (!float.TryParse(idRaw, NumberStyles.Any, new CultureInfo("en-US"), out id))
            {
                Log.Warn(string.Format("Could not parse {1} {0} as Float", idRaw, label));
                return false;
            }
            return true;
        }
    }
}