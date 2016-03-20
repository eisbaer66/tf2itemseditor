using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TF2Items.Core
{
    public class AttributeClass
    {
        public AttributeClass()
        {
            Name = string.Empty;
            Attributes = new List<Tf2Attribute>();
        }

        public string Name { get; set; }

        public virtual string PrettyName
        {
            get
            {
                Tf2Attribute attribute = Attributes.FirstOrDefault();
                if (attribute == null)
                    return Name;
                return attribute.Name;
            }
        }

        public IList<Tf2Attribute> Attributes { get; set; }
        public virtual AttributeEditing EditMode { get{ return AttributeEditing.Numerical;} }


        public Tf2Attribute Get(string value, string name)
        {
            float f;
            if (float.TryParse(value, out f))
                return Get(f, name);

            throw new ArgumentException("value can not be recognized as float", "value");
        }

        public virtual Tf2Attribute Get(float value, int? id)
        {
            return Get(value, string.Empty);
        }

        public virtual Tf2Attribute Get(float value, string name)
        {
            return Attributes.FirstOrDefault();
        }

        protected Tf2Attribute GetPositiveAttribute()
        {
            string effectType = "positive";
            return GetAttribute(effectType);
        }

        protected Tf2Attribute GetNegativeAttribute()
        {
            string effectType = "negative";
            return GetAttribute(effectType);
        }

        private Tf2Attribute GetAttribute(string effectType)
        {
            Tf2Attribute attribute = Attributes.FirstOrDefault(a => a.EffectType == effectType);
            if (attribute == null)
                return Attributes.FirstOrDefault();

            return attribute;
        }

        public virtual ConfigWeaponAttribute GetDefaultWeaponAttribute()
        {
            Tf2Attribute attribute = Get(1, string.Empty);
            return new ConfigWeaponAttribute(attribute.Id.Value, "1");
        }

        public virtual string Format(float value)
        {
            return value.ToString();
        }

        public virtual float Deformat(string value, float oldValue)
        {
            if (string.IsNullOrEmpty(value))
                value = GetDefaultWeaponAttribute().Value;

            float f;
            if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out f))
                return oldValue;

            return f;
        }
    }

    public class AttributeClassSet : AttributeClass
    {
        public override string PrettyName
        {
            get { return Name.Replace('_', ' '); }
        }

        public override AttributeEditing EditMode
        {
            get { return AttributeEditing.Set; }
        }

        public override Tf2Attribute Get(float value, int? id)
        {
            return Attributes.FirstOrDefault(a => a.Id == id) ?? Attributes.FirstOrDefault();
        }
        public override Tf2Attribute Get(float value, string name)
        {
            return Attributes.FirstOrDefault(a => a.Name == name) ?? Attributes.FirstOrDefault();
        }
    }

    public class AttributeClassAdditive : AttributeClass
    {
        public override Tf2Attribute Get(float value, string name)
        {
            if (value < 0)
                return GetNegativeAttribute();
            return GetPositiveAttribute();
        }

        public override ConfigWeaponAttribute GetDefaultWeaponAttribute()
        {
            Tf2Attribute attribute = GetPositiveAttribute();
            return new ConfigWeaponAttribute(attribute.Id.Value, "0");
        }
    }

    public class AttributeClassPercentage : AttributeClass
    {
        public override Tf2Attribute Get(float value, string name)
        {
            if (value < 1)
                return GetNegativeAttribute();
            return GetPositiveAttribute();
        }

        public override string Format(float value)
        {
            return (value*100).ToString();
        }

        public override float Deformat(string value, float oldValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return base.Deformat(value, oldValue);
            }

            float f;
            if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out f))
                return oldValue;

            return f / 100;
        }
    }
}