using System.Collections.Generic;
using System.Threading.Tasks;
using TF2Items.Core;
using TF2Items.Parsers;

namespace TF2Items.Ui.Services
{
    public interface ITf2WeaponService
    {
        Task<IEnumerable<Tf2Weapon>> Get();
    }

    public class Tf2WeaponService : ITf2WeaponService
    {
        private readonly IItemsGameWeaponsParser _itemsGameParser;
        private readonly ISettingsService _settingsService;

        public Tf2WeaponService(IItemsGameWeaponsParser itemsGameParser, ISettingsService settingsService)
        {
            _itemsGameParser = itemsGameParser;
            _settingsService = settingsService;
        }

        public async Task<IEnumerable<Tf2Weapon>> Get()
        {
            return await _itemsGameParser.ParseSingle(_settingsService.ItemsGameTxt);
        }
    }
}