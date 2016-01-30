using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Config;
using NUnit.Framework;
using TF2Items.Core;

namespace TF2Items.Parsers.Tests
{
    [TestFixture]
    public class ItemsGameParserTests
    {
        [SetUp]
        public void Setup()
        {
            BasicConfigurator.Configure();
        }

        [Test]
        public async Task ParsesItemsGame()
        {
            string filepath = TestDeployment.GetFile("items_game.txt");
            IList<Tf2Weapon> weapons = await new ItemsGameWeaponsParser().Parse(filepath);

            Tf2Weapon weaponWithAttributes = weapons.FirstOrDefault(w => w.Attributes.Count >0);

            Assert.That(weapons, Is.Not.Null);
            Assert.That(weapons.Count, Is.Not.EqualTo(0));
            Assert.That(weaponWithAttributes, Is.Not.Null);
        }

        [Test]
        public async Task ParsesItemsGameAsDictionary()
        {
            string filepath = TestDeployment.GetFile("items_game.txt");
            IDictionary<WeaponIdentifier, Tf2Weapon> weapons = await new ItemsGameWeaponsParser().ParseAsDictionary(filepath);

            Tf2Weapon weaponWithAttributes = weapons.FirstOrDefault(w => w.Value.Attributes.Count >0).Value;

            Assert.That(weapons, Is.Not.Null);
            Assert.That(weapons.Count, Is.Not.EqualTo(0));
            Assert.That(weaponWithAttributes, Is.Not.Null);
        }
    }
}