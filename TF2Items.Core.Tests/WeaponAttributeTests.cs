using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class WeaponAttributeTests
    {
        [Test]
        public void Tf2ItemsWeaponsConstuctorInitializesMembers()
        {
            WeaponAttribute weapon = new WeaponAttribute(134, 2);

            Assert.That(weapon.Id,          Is.EqualTo(134),    "Id is not initialized");
            Assert.That(weapon.Name,        Is.Null,            "Name is not null");
            Assert.That(weapon.Format,      Is.Null,            "Format is not null");
            Assert.That(weapon.EffectType,  Is.Null,            "EffectType is not null");
            Assert.That(weapon.Value,       Is.EqualTo(2),      "Value is not null");
        }
        [Test]
        public void ItemsGameConstuctorInitializesMembers()
        {
            WeaponAttribute weapon = new WeaponAttribute(134, "attach particle effect", "value_is_particle_index", "unusual");

            Assert.That(weapon.Id,          Is.EqualTo(134),                        "Id is not initialized");
            Assert.That(weapon.Name,        Is.EqualTo("attach particle effect"),   "Name is not initialized");
            Assert.That(weapon.Format,      Is.EqualTo("value_is_particle_index"),  "Format is not initialized");
            Assert.That(weapon.EffectType,  Is.EqualTo("unusual"),                  "EffectType is not initialized");
            Assert.That(weapon.Value,       Is.Null,                                "Value is not null");
        }
        [Test]
        public void ToStringOhneValueIsEmpty()
        {
            WeaponAttribute weapon = new WeaponAttribute(134, "attach particle effect", "value_is_particle_index", "unusual");

            Assert.That(weapon.ToString(), Is.Empty);
        }
        [Test]
        public void ToStringMitValue()
        {
            WeaponAttribute weapon = new WeaponAttribute(134, "attach particle effect", "value_is_particle_index", "unusual")
                                     {
                                         Value =  2
                                     };
            Assert.That(weapon.ToString(), Is.EqualTo("134 ; 2"));
        }
    }
}