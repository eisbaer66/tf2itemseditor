using System.Collections.Generic;
using System.Linq;
using log4net;
using TF2Items.Core;
using ValveFormat;

namespace TF2Items.Parsers
{
    public interface IStatsParser
    {
        void AddStats(IStatContainer weapon, List<DataNode> nodes);
    }

    public class StatsParser : IStatsParser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemsGameWeaponsParser));

        public void AddStats(IStatContainer weapon, List<DataNode> nodes)
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