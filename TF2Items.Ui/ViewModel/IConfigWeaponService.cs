using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TF2Items.Core;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public interface IConfigWeaponService
    {
        Task<ConfigWeapon> GetConfigWeaponFor(Tf2Weapon weapon);
    }

    class ConfigWeaponService : IConfigWeaponService
    {
        private readonly WeaponCollection _serverConfig = new WeaponCollection();
        private readonly ITf2WeaponService _tf2WeaponService;
        private IDictionary<string, AttributeClass> _attributeClasses;
        private IDictionary<int, AttributeClass> _attributeClassesByAttributeId;

        public ConfigWeaponService(ITf2WeaponService tf2WeaponService)
        {
            _tf2WeaponService = tf2WeaponService;
        }


        public async Task<ConfigWeapon> GetConfigWeaponFor(Tf2Weapon weapon)
        {
            await Init();
            ConfigWeapon configWeapon = _serverConfig.Weapons.FirstOrDefault(w => Equals(w.Id, weapon.Id));
            if (configWeapon == null)
            {
                configWeapon = CreateConfigWeapon(weapon);

                _serverConfig.Weapons.Add(configWeapon);
            }
            return configWeapon;
        }

        private async Task Init()
        {
            if (_attributeClasses == null)
                _attributeClasses = await _tf2WeaponService.GetAttributeClassesAsDictionary();
            if (_attributeClassesByAttributeId == null)
                _attributeClassesByAttributeId = await _tf2WeaponService.GetAttributeClassesAsAttributeIdDictionary();
        }

        private ConfigWeapon CreateConfigWeapon(Tf2Weapon weapon)
        {
            ConfigWeapon configWeapon = new ConfigWeapon(weapon.Id);
            configWeapon.Attributes = weapon.Attributes.Select(a =>
                                                               {
                                                                   ConfigWeaponAttribute attribute = GetConfigAttributeFor(a, configWeapon);
                                                                   attribute.IsPredefined = true;
                                                                   return attribute;
                                                               }).ToList();
            return configWeapon;
        }

        private ConfigWeaponAttribute GetConfigAttributeFor(Tf2WeaponAttribute attribute, ConfigWeapon weapon)
        {
            ConfigWeaponAttribute configAttribute = weapon.Attributes.FirstOrDefault(a => _attributeClassesByAttributeId[a.Id.Value].Name == attribute.Class);
            if (configAttribute != null)
                return configAttribute;


            AttributeClass attributeClass = _attributeClasses[attribute.Class];
            float value;
            if (!float.TryParse(attribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                throw new UnexpectedAttributeValue(attribute);

            Tf2Attribute tf2Attribute = attributeClass.Get(value, attribute.Name);
            configAttribute = new ConfigWeaponAttribute(tf2Attribute.Id.Value, value);

            weapon.Attributes.Add(configAttribute);
            return configAttribute;
        }
    }

    internal class UnexpectedAttributeValue : Exception
    {
        public Tf2WeaponAttribute Attribute { get; set; }

        public UnexpectedAttributeValue(Tf2WeaponAttribute attribute)
            :base(string.Format("Value '{0}' of attribute '{1}' could not be recognized as float.", attribute.Value, attribute.Class))
        {
            Attribute = attribute;
        }
    }
}