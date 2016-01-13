using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class Tf2WeaponTests
    {
        [Test]
        public void ConstuctorInitializesMembers()
        {
            WeaponIdentifier id = WeaponIdentifier.FromId(12);
            Tf2Weapon weapon = new Tf2Weapon(id);

            Assert.That(weapon.Id,          Is.SameAs(id),  "Id is not initialized");
            Assert.That(weapon.Name,        Is.Null,        "Name is not null");
            Assert.That(weapon.Quality,     Is.Null,        "Quality is not null");
            Assert.That(weapon.Level,       Is.Null,        "Level is not null");
            Assert.That(weapon.AdminFlags,  Is.Null,        "AdminFlags is not null");
            Assert.That(weapon.Attributes,  Is.Not.Null,    "Attributes is null");
        }
    }
    [TestFixture]
    public class ConfigWeaponTests
    {
        [Test]
        public void ConstuctorInitializesMembers()
        {
            WeaponIdentifier id = WeaponIdentifier.FromId(12);
            ConfigWeapon weapon = new ConfigWeapon(id);

            Assert.That(weapon.Id,          Is.SameAs(id),  "Id is not initialized");
            Assert.That(weapon.Name,        Is.Null,        "Name is not null");
            Assert.That(weapon.Quality,     Is.Null,        "Quality is not null");
            Assert.That(weapon.Level,       Is.Null,        "Level is not null");
            Assert.That(weapon.AdminFlags,  Is.Null,        "AdminFlags is not null");
            Assert.That(weapon.Attributes,  Is.Not.Null,    "Attributes is null");
        }
    }
}