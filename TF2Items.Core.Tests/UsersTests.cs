using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class UsersTests
    {
        [Test]
        public void FactoryMethodAnyCreatesAnyUsers()
        {
            UserIdentifier users = UserIdentifier.Any();

            Assert.That(users.ToString(), Is.EqualTo("*"), "wrong ToString() output");
        }

        [Test]
        public void FactoryMethodFromStreamIdsCreatesSteamUsers()
        {
            UserIdentifier users = UserIdentifier.FromStreamIds("STEAM_0:1:13776935", "STEAM_0:1:17919459");

            Assert.That(users.ToString(), Is.EqualTo("STEAM_0:1:13776935 ; STEAM_0:1:17919459"), "wrong ToString() output");
        }

        [Test]
        public void FactoryMethodFromStreamIdsCreatesAnyUsersIfListIsEmpty()
        {
            UserIdentifier users = UserIdentifier.FromStreamIds();

            Assert.That(users.ToString(), Is.EqualTo("*"), "wrong ToString() output");
        }

        [Test]
        public void FactoryMethodFromStreamIdsCreatesAnyUsersIfTheOnlyIdIsEmpty()
        {
            UserIdentifier users = UserIdentifier.FromStreamIds();

            Assert.That(users.ToString(), Is.EqualTo("*"), "wrong ToString() output");
        }

        [Test]
        public void FactoryMethodFromStreamIdsCreatesAnyUsersIfTheOnlyIdIsStar()
        {
            UserIdentifier users = UserIdentifier.FromStreamIds("*");

            Assert.That(users.ToString(), Is.EqualTo("*"), "wrong ToString() output");
        }

        [Test]
        public void AnyUsersAreEqual()
        {
            UserIdentifier usersA = UserIdentifier.Any();
            UserIdentifier usersB = UserIdentifier.Any();

            Assert.That(usersA, Is.EqualTo(usersB));
        }

        [Test]
        public void UsersWithSameSteamIdsAreEqual()
        {
            UserIdentifier usersA = UserIdentifier.FromStreamIds("STEAM_0:1:13776935");
            UserIdentifier usersB = UserIdentifier.FromStreamIds("STEAM_0:1:13776935");

            Assert.That(usersA, Is.EqualTo(usersB));
        }

        [Test]
        public void UsersWithMultipleSameSteamIdsAreEqual()
        {
            UserIdentifier usersA = UserIdentifier.FromStreamIds("STEAM_0:1:13776935", "STEAM_0:1:17919459");
            UserIdentifier usersB = UserIdentifier.FromStreamIds("STEAM_0:1:17919459", "STEAM_0:1:13776935");

            Assert.That(usersA, Is.EqualTo(usersB));
        }

        [Test]
        public void AnyUsersAndSteamIdsUserAreNotEqual()
        {
            UserIdentifier usersA = UserIdentifier.Any();
            UserIdentifier usersB = UserIdentifier.FromStreamIds("STEAM_0:1:13776935");

            Assert.That(usersA, Is.Not.EqualTo(usersB));
        }

        [Test]
        public void UsersWithDifferentSteamIdsAreNotEqual()
        {
            UserIdentifier usersA = UserIdentifier.FromStreamIds("STEAM_0:1:13776935");
            UserIdentifier usersB = UserIdentifier.FromStreamIds("STEAM_0:1:17919459");

            Assert.That(usersA, Is.Not.EqualTo(usersB));
        }

        [Test]
        public void UsersWithSubsetOfSteamIdsAreNotEqual()  //Maybe?
        {
            UserIdentifier usersA = UserIdentifier.FromStreamIds("STEAM_0:1:13776935", "STEAM_0:1:17919459");
            UserIdentifier usersB = UserIdentifier.FromStreamIds("STEAM_0:1:13776935");

            Assert.That(usersA, Is.Not.EqualTo(usersB));
        }
    }
}