using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Config;
using NUnit.Framework;

namespace TF2Items.Parsers.Tests
{
    [TestFixture]
    public class ItemsGamePrefabsParserTests
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
            IList<Tf2Prefab> weapons = await new ItemsGamePrefabsParser(new StatsParser()).Parse(filepath);

            Tf2Prefab weaponWithAttributes = weapons.FirstOrDefault(w => w.Attributes.Count > 0);

            Assert.That(weapons, Is.Not.Null);
            Assert.That(weapons.Count, Is.Not.EqualTo(0));
            Assert.That(weaponWithAttributes, Is.Not.Null);
        }

        [Test]
        public async Task ParsesItemsGameAsDictionary()
        {
            string filepath = TestDeployment.GetFile("items_game.txt");
            IDictionary<string, Tf2Prefab> weapons = await new ItemsGamePrefabsParser(new StatsParser()).ParseAsDictionary(filepath);

            Tf2Prefab weaponWithAttributes = weapons.FirstOrDefault(w => w.Value.Attributes.Count > 0).Value;

            Assert.That(weapons, Is.Not.Null);
            Assert.That(weapons.Count, Is.Not.EqualTo(0));
            Assert.That(weaponWithAttributes, Is.Not.Null);
        }
    }
}