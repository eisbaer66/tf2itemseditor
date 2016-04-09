using System;
using System.Collections;
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

    public class Tf2AttributesParserCache : ITf2AttributesParser
    {
        private readonly ITf2AttributesParser _parser;
        private IDictionary<int, Tf2Attribute> _dict;
        private IList<Tf2Attribute> _list;
        private CachingEnumarable<Tf2Attribute> _enum;

        public Func<DataNode, bool> Filter
        {
            get { return _parser.Filter; }
            set { _parser.Filter = value; }
        }

        public async Task<IDictionary<int, Tf2Attribute>> ParseAsDictionary(string filePath)
        {
            if (_dict != null)
                return _dict;

            IDictionary<int, Tf2Attribute> dictionary = await _parser.ParseAsDictionary(filePath);
            _dict = dictionary;
            return _dict;
        }

        public async Task<IList<Tf2Attribute>> Parse(string filePath)
        {
            if (_list != null)
                return _list;

            IList<Tf2Attribute> attributes = await _parser.Parse(filePath);
            _list = attributes;
            return _list;
        }

        public async Task<IEnumerable<Tf2Attribute>> ParseSingle(string filePath)
        {
            if (_enum != null)
                return _enum;

            IEnumerable<Tf2Attribute> attributes = await _parser.ParseSingle(filePath);
            _enum = new CachingEnumarable<Tf2Attribute>(attributes);
            return _enum;
        }

        public Tf2AttributesParserCache(ITf2AttributesParser parser)
        {
            _parser = parser;
        }
    }

    public class CachingEnumarable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _enumerable;
        private IList<T> _cache;

        public CachingEnumarable(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_cache != null)
                return _cache.GetEnumerator();

            return new CachingEnumerator<T>(_enumerable.GetEnumerator(), c => _cache = c);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_cache != null)
                return _cache.GetEnumerator();

            return new CachingEnumerator(((IEnumerable)_enumerable).GetEnumerator(), c => _cache = c.Cast<T>().ToList());
        }
    }

    public class CachingEnumerator : IEnumerator
    {
        private readonly IEnumerator _enumerator;
        private readonly Action<IList> _finished;
        private readonly IList _cache;

        public CachingEnumerator(IEnumerator enumerator, Action<IList> finished)
        {
            _enumerator = enumerator;
            _finished = finished;
            _cache = new ArrayList();
        }

        public bool MoveNext()
        {
            bool moveNext = _enumerator.MoveNext();
            if (moveNext)
                _cache.Add(Current);
            else
                _finished(_cache);

            return moveNext;
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public object Current
        {
            get { return _enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }
    }
    public class CachingEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly Action<IList<T>> _finished;
        private readonly IList<T> _cache;

        public CachingEnumerator(IEnumerator<T> enumerator, Action<IList<T>>  finished)
        {
            _enumerator = enumerator;
            _finished = finished;
            _cache = new List<T>();
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            bool moveNext = _enumerator.MoveNext();
            if (moveNext)
                _cache.Add(Current);
            else
                _finished(_cache);

            return moveNext;
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public T Current
        {
            get { return _enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }
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
                        .ToDictionary(f => f.Id);
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