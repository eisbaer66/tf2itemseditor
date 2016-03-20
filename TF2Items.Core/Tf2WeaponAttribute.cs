using System.Diagnostics;

namespace TF2Items.Core
{
    [DebuggerDisplay("{Name}: {Value} ({Class})")]
    public class Tf2WeaponAttribute
    {
        public Tf2WeaponAttribute(string attributeClass, string name, string value)
        {
            Class = attributeClass;
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Class { get; set; }
        public string Value { get; set; }

        public Tf2WeaponAttribute Duplicate()
        {
            return new Tf2WeaponAttribute(Class, Name, Value);
        }
    }
}