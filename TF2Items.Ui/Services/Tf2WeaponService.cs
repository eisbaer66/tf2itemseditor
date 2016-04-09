using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using TF2Items.Core;
using TF2Items.Parsers;

namespace TF2Items.Ui.Services
{
    public interface ITf2WeaponService
    {
        Task<IEnumerable<Tf2Weapon>> Get();
        Task<IEnumerable<Tf2Attribute>> GetAttributes();
        Task<IDictionary<string, Tf2Attribute>> GetAttributesAsClassDictionary();
        Task<IDictionary<string, AttributeClass>> GetAttributeClassesAsDictionary();
        Task<IDictionary<int, AttributeClass>> GetAttributeClassesAsAttributeIdDictionary();
        Task<IEnumerable<AttributeClass>> GetAttributeClasses();
    }

    public class Tf2WeaponService : ITf2WeaponService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Tf2WeaponService));

        private readonly IItemsGameWeaponsParser _weaponsParser;
        private readonly ITf2AttributesParser _attributeParser;
        private readonly IItemsGamePrefabsParser _prefabsParser;
        private readonly ISettingsService _settingsService;

        public Tf2WeaponService(IItemsGameWeaponsParser weaponsParser, ITf2AttributesParser attributeParser, IItemsGamePrefabsParser prefabsParser, ISettingsService settingsService)
        {
            _weaponsParser = weaponsParser;
            _attributeParser = attributeParser;
            _settingsService = settingsService;
            _prefabsParser = prefabsParser;
        }

        public async Task<IEnumerable<Tf2Weapon>> Get()
        {
            IEnumerable<Tf2Weapon> weapons = await _weaponsParser.ParseSingle(_settingsService.ItemsGameTxt);
            IDictionary<string, Tf2Prefab> prefabs = await _prefabsParser.ParseAsDictionary(_settingsService.ItemsGameTxt);
            prefabs = Hydrate(prefabs);


            return weapons.Select(w => Hydrate(w, prefabs));
        }

        public async Task<IDictionary<string, Tf2Attribute>> GetAttributesAsClassDictionary()
        {
            return (await _attributeParser.ParseSingle(_settingsService.ItemsGameTxt))
                .ToLookup(a => a.Class)
                .ToDictionary(p => p.Key, p => p.FirstOrDefault());
        }

        public async Task<IEnumerable<Tf2Attribute>> GetAttributes()
        {
            return await _attributeParser.ParseSingle(_settingsService.ItemsGameTxt);
        }

        public async Task<IDictionary<string, AttributeClass>> GetAttributeClassesAsDictionary()
        {
            return (await GetAttributeClasses())
                .ToLookup(a => a.Name)
                .ToDictionary(p => p.Key, p => p.FirstOrDefault());
        }

        public async Task<IDictionary<int, AttributeClass>> GetAttributeClassesAsAttributeIdDictionary()
        {
            return (await GetAttributeClasses())
                .SelectMany(c =>
                            {
                                return c.Attributes.Select(a => new
                                                                {
                                                                    Class = c,
                                                                    Attribute = a,
                                                                });
                            })
                .ToLookup(p => p.Attribute.Id, p => p.Class)
                .ToDictionary(p => p.Key, p => p.FirstOrDefault());
        }

        public async Task<IEnumerable<AttributeClass>> GetAttributeClasses()
        {
            return (await GetAttributes())
                .GroupBy(a => new {a.Class})
                .Select(g =>
                        {
                            string @class = g.Key.Class;

                            List<Tf2Attribute> attributes = g.ToList();
                            Tf2Attribute attribute = attributes[0];

                            AttributeClass attributeClass = CreateAttributeClass(attribute.Format, @class);
                            attributeClass.Name = @class;
                            attributeClass.Attributes = attributes;
                            return attributeClass;
                        });
        }

        private static AttributeClass CreateAttributeClass(string format, string @class)
        {
            if (@class.StartsWith("set_"))
                return new AttributeClassSet();

            switch (format)
            {
                case "value_is_percentage":
                case "value_is_inverted_percentage":
                    return new AttributeClassPercentage();
                case "value_is_additive":
                case "value_is_additive_percentage":
                    return new AttributeClassAdditive();
                case "value_is_or":
                case "value_is_particle_index":
                    return new AttributeClass();
                default:
                    Debug.WriteLine(format);
                    return new AttributeClass();
            }
        }

        private IDictionary<string, Tf2Prefab> Hydrate(IDictionary<string, Tf2Prefab> prefabs)
        {
            IDictionary<string, Tf2Prefab> processedPrefabs = new Dictionary<string, Tf2Prefab>();
            Queue<Tf2Prefab> queue = new Queue<Tf2Prefab>(prefabs.Values);

            while (queue.Count > 0)
            {
                Tf2Prefab current = queue.Dequeue();

                if (string.IsNullOrEmpty(current.PrefabName))
                {
                    processedPrefabs.Add(current.Id, current);
                    continue;
                }

                string[] definedPrefabs = current.PrefabName.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (!definedPrefabs.All(p => processedPrefabs.ContainsKey(p)))
                {
                    queue.Enqueue(current);
                    continue;
                }

                foreach (string definedPrefab in definedPrefabs)
                {
                    Tf2Prefab prefab = prefabs[definedPrefab];
                    Hydrate(current, prefab);
                }
                processedPrefabs.Add(current.Id, current);
            }

            return processedPrefabs;
        }

        private Tf2Weapon Hydrate(Tf2Weapon weapon, IDictionary<string, Tf2Prefab> prefabs)
        {
            if (string.IsNullOrEmpty(weapon.PrefabName))
                return weapon;

            if (!prefabs.ContainsKey(weapon.PrefabName))
            {
                Log.ErrorFormat("could not find prefab '{0}' specified in weapon '{1} ({2})'", weapon.PrefabName, weapon.Id, weapon.Name);
                return weapon;
            }

            Tf2Prefab prefab = prefabs[weapon.PrefabName];

            Hydrate(weapon, prefab);

            return weapon;
        }

        private static void Hydrate(IStatContainer weapon, Tf2Prefab prefab)
        {
            if (string.IsNullOrEmpty(weapon.ImageDirectory))
                weapon.ImageDirectory = prefab.ImageDirectory;

            IEnumerable<Tf2WeaponAttribute> missingAttributes = prefab.Attributes.Where(pa => weapon.Attributes.All(wa => wa.Class != pa.Class));
            foreach (Tf2WeaponAttribute attribute in missingAttributes)
                weapon.Attributes.Add(attribute.Duplicate());
        }
    }
}