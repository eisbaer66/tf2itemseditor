using System.Collections.Generic;
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
        public IList<Tf2Attribute> Attributes { get; set; }

        public virtual Tf2Attribute Get(float value)
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
    }

    public class AttributeClassAdditive : AttributeClass
    {
        public override Tf2Attribute Get(float value)
        {
            if (value < 0)
                return GetNegativeAttribute();
            return GetPositiveAttribute();
        }
    }

    public class AttributeClassAdditivePercentage : AttributeClass
    {
        public override Tf2Attribute Get(float value)
        {
            if (value < 0)
                return GetNegativeAttribute();
            return GetPositiveAttribute();
        }
    }

    public class AttributeClassInvertedPercentage : AttributeClass
    {
        public override Tf2Attribute Get(float value)
        {
            if (value < 1)
                return GetPositiveAttribute();
            return GetNegativeAttribute();
        }
    }

    public class AttributeClassPercentage : AttributeClass
    {
        public override Tf2Attribute Get(float value)
        {
            if (value < 1)
                return GetNegativeAttribute();
            return GetPositiveAttribute();
        }
    }
}