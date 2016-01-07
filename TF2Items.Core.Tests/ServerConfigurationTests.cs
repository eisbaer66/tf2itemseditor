using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class ServerConfigurationTests
    {
        [Test]
        public void ConstuctorInitializesListOfWeaponCollections()
        {
            ServerConfiguration serverConfiguration = new ServerConfiguration();

            Assert.That(serverConfiguration.WeaponCollections, Is.Not.Null, "WeaponCollections is null");
        }
    }
}
