using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using TF2Items.Core;
using ValveFormat;

namespace TF2Items.Parsers
{
    public interface ITf2AttributesParser
    {
        Func<DataNode, bool> Filter { get; set; }
        Task<IDictionary<int, Tf2Attribute>> ParseAsDictionary(string filePath);
        Task<IList<Tf2Attribute>> Parse(string filePath);
        Task<IEnumerable<Tf2Attribute>> ParseSingle(string filePath);
    }

    public class Tf2AttributesParser : ITf2AttributesParser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Tf2AttributesParser));
        
        public Func<DataNode, bool> Filter { get; set; }

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
            Filter = DefaultWeaponsFilter;
        }

        public async Task<IDictionary<int, Tf2Attribute>> ParseAsDictionary(string filePath)
        {
            return (await ParseSingle(filePath))
                        .Where(f => f.Id.HasValue)
                        .ToDictionary(f => f.Id.Value);
        }
        public async Task<IList<Tf2Attribute>> Parse(string filePath)
        {
            return (await ParseSingle(filePath))
                            .ToList();
        }
        public async Task<IEnumerable<Tf2Attribute>> ParseSingle(string filePath)
        {
            using (NDC.Push("parse"))
            {
                ValveFormatParser parser = new ValveFormatParser(filePath);

                await Task.Run(() => parser.LoadFile());

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
                        if (!Filter(node))
                        {
                            Log.Info("ignored due to Filter");
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