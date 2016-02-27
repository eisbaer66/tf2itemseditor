using System.Linq;
using TF2Items.Core;

namespace TF2Items.Ui.ViewModel
{
    public interface IConfigWeaponService
    {
        ConfigWeapon GetConfigWeaponFor(Tf2Weapon weapon);
    }

    class ConfigWeaponService : IConfigWeaponService
    {
        private readonly WeaponCollection _serverConfig = new WeaponCollection();


        public ConfigWeapon GetConfigWeaponFor(Tf2Weapon weapon)
        {
            ConfigWeapon configWeapon = _serverConfig.Weapons.FirstOrDefault(w => Equals(w.Id, weapon.Id));
            if (configWeapon == null)
            {
                configWeapon = new ConfigWeapon(weapon.Id);
                _serverConfig.Weapons.Add(configWeapon);
            }
            return configWeapon;
        }
    }
}