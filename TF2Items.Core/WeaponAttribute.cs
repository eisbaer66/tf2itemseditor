using System;

namespace TF2Items.Core
{
    public class WeaponAttribute
    {
        public WeaponAttribute(int id, string name, string format, string effectType)
        {
            Id = id;
            Name = name;
            Format = format;
            EffectType = effectType;
        }

        public WeaponAttribute(int id, float value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public string EffectType { get; set; }
        public float? Value { get; set; }

        public override string ToString()
        {
            if (Value == null)
                return String.Empty;
            return string.Format("{0} ; {1}", Id, Value);
        }
    }
}