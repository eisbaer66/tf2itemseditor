using System.Diagnostics;

namespace TF2Items.Core
{
    [DebuggerDisplay("{Id} {Name}")]
    public class Tf2Attribute
    {
        public Tf2Attribute(int id, string attributeClass, string name, string format, string effectType)
        {
            Id = id;
            Class = attributeClass;
            Name = name;
            Format = format;
            EffectType = effectType;
        }

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Format { get; set; }
        public string EffectType { get; set; }
    }
}