using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class Tf2AttributeTests
    {
        [Test]
        public void ItemsGameConstuctorInitializesMembers()
        {
            Tf2Attribute weapon = new Tf2Attribute(134, "set_attached_particle", "attach particle effect", "value_is_particle_index", "unusual");

            Assert.That(weapon.Id, Is.Not.Null, "Id is not initialized");
            Assert.That(weapon.Id.Value, Is.EqualTo(134), "Id is not initialized");
            Assert.That(weapon.Name, Is.EqualTo("attach particle effect"), "Name is not initialized");
            Assert.That(weapon.Class, Is.EqualTo("set_attached_particle"), "Class is not initialized");
            Assert.That(weapon.Format, Is.EqualTo("value_is_particle_index"), "Format is not initialized");
            Assert.That(weapon.EffectType, Is.EqualTo("unusual"), "EffectType is not initialized");
        }
    }
    [TestFixture]
    public class Tf2WeaponAttributeTests
    {
        [Test]
        public void ItemsGameConstuctorInitializesMembers()
        {
            Tf2WeaponAttribute weapon = new Tf2WeaponAttribute("set_attached_particle", "attach particle effect", "2");
            
            Assert.That(weapon.Name,        Is.EqualTo("attach particle effect"),   "Name is not initialized");
            Assert.That(weapon.Class,       Is.EqualTo("set_attached_particle"),    "Class is not initialized");
            Assert.That(weapon.Value,       Is.EqualTo("2"),                        "Value is not null");
        }
    }

    [TestFixture]
    public class WeaponAttributeTests
    {
        [Test]
        public void Tf2ItemsWeaponsConstuctorInitializesMembers()
        {
            ConfigWeaponAttribute weapon = new ConfigWeaponAttribute(134, 2);

            Assert.That(weapon.Id,          Is.Not.Null,        "Id is not initialized");
            Assert.That(weapon.Id.Value,    Is.EqualTo(134),    "Id is not initialized");
            Assert.That(weapon.Value,       Is.EqualTo("2"),    "Value is not null");
        }
        [Test]
        public void ToStringIsEmptyIfValueNull()
        {
            ConfigWeaponAttribute weapon = new ConfigWeaponAttribute(134, null);

            Assert.That(weapon.ToString(), Is.Empty);
        }
        [Test]
        public void ToStringIsEmptyIfValueEmpty()
        {
            ConfigWeaponAttribute weapon = new ConfigWeaponAttribute(134, string.Empty);

            Assert.That(weapon.ToString(), Is.Empty);
        }
        [Test]
        public void ToStringMitValue()
        {
            ConfigWeaponAttribute weapon = new ConfigWeaponAttribute(134, 2);
            Assert.That(weapon.ToString(), Is.EqualTo("134 ; 2"));
        }
    }
}