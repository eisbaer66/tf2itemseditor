using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ValveFormat;

namespace TF2Items.Parsers
{
    public interface IItemsGamePrefabsParser
    {
        Task<IDictionary<string, Tf2Prefab>> ParseAsDictionary(string filePath);
        Task<IList<Tf2Prefab>> Parse(string filePath);
        Task<IEnumerable<Tf2Prefab>> ParseSingle(string filePath);
    }

    public class ItemsGamePrefabsParser : IItemsGamePrefabsParser
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ItemsGamePrefabsParser));

        private readonly IStatsParser _statsParser;

        public ItemsGamePrefabsParser(IStatsParser statsParser)
        {
            _statsParser = statsParser;
        }

        public async Task<IDictionary<string, Tf2Prefab>> ParseAsDictionary(string filePath)
        {
            return (await ParseSingle(filePath))
                .ToDictionary(f => f.Id);
        }
        public async Task<IList<Tf2Prefab>> Parse(string filePath)
        {
            return (await ParseSingle(filePath)).ToList();
        }
        public async Task<IEnumerable<Tf2Prefab>> ParseSingle(string filePath)
        {
            using (NDC.Push("parse"))
            {
                ValveFormatParser parser = new ValveFormatParser(filePath);

                await Task.Run(() => parser.LoadFile());

                return GetPrefabs(parser.RootNode.SubNodes);
            }
        }

        private IEnumerable<Tf2Prefab> GetPrefabs(List<DataNode> nodes)
        {
            DataNode itemsNode = nodes.Find(n => n.Key == "prefabs");
            if (itemsNode == null)
            {
                Log.Error("could not find node 'prefabs'");
                yield break;
            }

            using (NDC.Push("Weapon"))
            {
                foreach (DataNode node in itemsNode.SubNodes)
                {
                    using (NDC.Push(node.Key))
                    {
                        Tf2Prefab prefab = CreateWeapon(node);
                        if (prefab == null)
                            continue;

                        _statsParser.AddStats(prefab, node.SubNodes);

                        yield return prefab;
                    }
                }
            }
        }

        private Tf2Prefab CreateWeapon(DataNode node)
        {
            return new Tf2Prefab{Id = node.Key};
        }
    }
}