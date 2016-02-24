using System;
using System.Collections.Generic;
using TF2Items.Core;

namespace TF2Items.Ui.ViewModel
{
    public interface IWeaponDetailsAttributeViewModelFactory
    {
        WeaponDetailsAttributeViewModel Get(Tf2WeaponAttribute tf2WeaponAttribute, Tf2Weapon model, AttributeClass attribute);
        void Revoke(Tf2Weapon model, WeaponDetailsAttributeViewModel vm);
    }

    class WeaponDetailsAttributeViewModelFactory : IWeaponDetailsAttributeViewModelFactory
    {
        private readonly IDictionary<string, WeaponDetailsAttributeViewModel> _cache = new Dictionary<string, WeaponDetailsAttributeViewModel>();
        private readonly Func<WeaponDetailsAttributeViewModel> _getVm;

        public WeaponDetailsAttributeViewModelFactory(Func<WeaponDetailsAttributeViewModel> getVm)
        {
            _getVm = getVm;
        }

        public WeaponDetailsAttributeViewModel Get(Tf2WeaponAttribute tf2WeaponAttribute, Tf2Weapon model, AttributeClass attribute)
        {
            string key = model.Id.Id + "~" + attribute.Name;
            if (!_cache.ContainsKey(key))
            {
                WeaponDetailsAttributeViewModel vm = _getVm();
                vm.Model = attribute;
                vm.Value = tf2WeaponAttribute.Value;

                _cache.Add(key, vm);
            }

            return _cache[key];
        }

        public void Revoke(Tf2Weapon model, WeaponDetailsAttributeViewModel vm)
        {
            string key = model.Id.Id + "~" + vm.Model.Name;
            if (!_cache.ContainsKey(key))
                return;

            _cache.Remove(key);
        }
    }
}