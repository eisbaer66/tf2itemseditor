using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using TF2Items.Core;
using ValveFormat;

namespace TF2Items.Parsers
{
    public class Tf2AttributesParser 
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Tf2AttributesParser));
        
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

        public Tf2AttributesParser()
        {
            WeaponsFilter = DefaultWeaponsFilter;
        }

        public IDictionary<int, Tf2Attribute> ParseAsDictionary(string filePath)
        {
            return ParseSingle(filePath)
                        .Where(f => f.Id.HasValue)
                        .ToDictionary(f => f.Id.Value);
        }
        public IList<Tf2Attribute> Parse(string filePath)
        {
            return ParseSingle(filePath).ToList();
        }
        public IEnumerable<Tf2Attribute> ParseSingle(string filePath)
        {
            using (NDC.Push("parse"))
            {
                ValveFormatParser parser = new ValveFormatParser(filePath);
                parser.LoadFile();

                return CreateWeaponAttributes(parser.RootNode.SubNodes);
            }
        }

        private IEnumerable<Tf2Attribute> CreateWeaponAttributes(List<DataNode> nodes)
        {
            DataNode itemsNode = nodes.Find(n => n.Key == "attributes");
            if (itemsNode == null)
            {
                Log.Error("could not find node 'attributes'");
                yield break;
            }

            using (NDC.Push("Attribute"))
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

                        Tf2Attribute attribute = CreateWeaponAttribute(node);
                        if (attribute == null)
                            continue;


                        yield return attribute;
                    }
                }
            }
        }

        private Tf2Attribute CreateWeaponAttribute(DataNode node)
        {
            string name = null;
            string format = String.Empty;
            string attributeClass = String.Empty;
            string effectType = String.Empty;

            foreach (DataNode subNode in node.SubNodes)
            {
                switch (subNode.Key)
                {
                    case "attribute_class":
                        attributeClass = subNode.Value;
                        break;
                    case "name":
                        name = subNode.Value;
                        break;
                    case "description_format":
                        format = subNode.Value;
                        break;
                    case "effect_type":
                        effectType = subNode.Value;
                        break;
                }

            }
            if (string.IsNullOrEmpty(name))
            {
                Log.Warn("could not find 'name'");
                return null;
            }

            int id;
            if (!Primitives.TryParse(node.Key, "Key", out id))
                return null;

            return new Tf2Attribute(id, attributeClass, name, format, effectType);
        }
    }
}