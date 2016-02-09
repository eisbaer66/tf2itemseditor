using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Tf2Attribute>> GetAttributes()
        {
            return await _attributeParser.ParseSingle(_settingsService.ItemsGameTxt);
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