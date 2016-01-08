using System;
using System.Diagnostics;
using System.Globalization;

namespace TF2Items.Core
{
    [DebuggerDisplay("{Id} {Name}: {Value}")]
    public class WeaponAttribute
    {
        private WeaponAttribute()
        {
            
        }
        public static WeaponAttribute FromItemsGameAttributes(int id, string attributeClass, string name, string format, string effectType)
        {
            return new WeaponAttribute
                   {
                       Id = id,
                       Class = attributeClass,
                       Name = name,
                       Format = format,
                       EffectType = effectType,
                   };
        }
        public static WeaponAttribute FromItemsGameWeapon(string attributeClass, string name, string value)
        {
            return new WeaponAttribute
                   {
                       Class = attributeClass,
                       Name = name,
                       Value = value
                   };
        }

        public static WeaponAttribute FromTf2ItemsWeapons(int id, float value)
        {
            return FromTf2ItemsWeapons(id, value.ToString(new CultureInfo("en-US")));
        }
        public static WeaponAttribute FromTf2ItemsWeapons(int id, string value)
        {
            return new WeaponAttribute
                   {
                       Id = id,
                       Value = value
                   };
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Format { get; set; }
        public string EffectType { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Value))
                return String.Empty;
            return string.Format("{0} ; {1}", Id, Value);
        }
    }
}