using System;
using System.Diagnostics;
using System.Globalization;

namespace TF2Items.Core
{
    [DebuggerDisplay("{Id}: {Value}")]
    public class ConfigWeaponAttribute
    {
        public ConfigWeaponAttribute(int id, float value)
            : this(id, value.ToString(new CultureInfo("en-US")))
        { }
        public ConfigWeaponAttribute(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsPredefined { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Value))
                return String.Empty;
            return string.Format("{0} ; {1}", Id, Value);
        }
    }
}