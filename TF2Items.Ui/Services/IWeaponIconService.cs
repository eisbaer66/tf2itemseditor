using System.IO;
using System.Threading.Tasks;
using TF2Items.Core;
using TF2Items.ValvePak;

namespace TF2Items.Ui.Services
{
    public interface IWeaponIconService
    {
        Task<string> Get(Tf2Weapon weapon);
    }

    class WeaponIconService : IWeaponIconService
    {
        private readonly IValvePakService _vpkService;
        private readonly IWeaponIconConfig _config;

        public WeaponIconService(IValvePakService vpkService, IWeaponIconConfig config)
        {
            _vpkService = vpkService;
            _config = config;
        }

        public async Task<string> Get(Tf2Weapon weapon)
        {
            if (weapon == null)
                return null;
            if (string.IsNullOrEmpty(weapon.ImageDirectory))
                return null;


            string cacheSubDirectory = weapon.ImageDirectory.Replace('/', '\\');
            string filenameWithoutExtension = Path.GetFileName(weapon.ImageDirectory);
            string filename = filenameWithoutExtension + ".png";

            string completeCacheDirectory = Path.Combine(_config.CacheDirectory, cacheSubDirectory);
            string cachePath = Path.Combine(completeCacheDirectory, filename);
            if (File.Exists(cachePath))
                return Path.Combine(Directory.GetCurrentDirectory(), cachePath);

            AssureExistence(completeCacheDirectory);
            string iconPath = await _vpkService.ExtractWeaponIcon(weapon.ImageDirectory, completeCacheDirectory);

            if (string.IsNullOrEmpty(iconPath))
                return "/TF2Items.Ui;component/assets/icons/error.png";
            return Path.Combine(Directory.GetCurrentDirectory(), iconPath);
        }

        private void AssureExistence(string dir)
        {
            if (Directory.Exists(dir))
                return;

            Directory.CreateDirectory(dir);
        }
    }
}