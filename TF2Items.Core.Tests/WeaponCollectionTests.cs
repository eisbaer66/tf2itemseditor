using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class WeaponCollectionTests
    {
        [Test]
        public void DefaultConstuctorInitializesMembers()
        {
            UserIdentifier users = UserIdentifier.Any();
            WeaponCollection weaponCollection = new WeaponCollection();

            Assert.That(weaponCollection.Users, Is.EqualTo(users), "Users is not initialized");
            Assert.That(weaponCollection.Weapons, Is.Not.Null, "Weapons is null");
        }

        [Test]
        public void UsersConstuctorInitializesMembers()
        {
            UserIdentifier users = UserIdentifier.Any();
            WeaponCollection weaponCollection = new WeaponCollection(users);

            Assert.That(weaponCollection.Users, Is.SameAs(users), "Users is not initialized");
            Assert.That(weaponCollection.Weapons, Is.Not.Null, "Weapons is null");
        }
    }
}