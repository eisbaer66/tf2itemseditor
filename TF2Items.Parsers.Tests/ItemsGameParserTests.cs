using System.Collections.Generic;
using System.Linq;
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
        public void ParsesItemsGame()
        {
            string filepath = TestDeployment.GetFile("items_game.txt");
            IList<Weapon> weapons = new ItemsGameParser().Parse(filepath);

            Weapon weaponWithAttributes = weapons.FirstOrDefault(w => w.Attributes.Count >0);

            Assert.That(weapons, Is.Not.Null);
            Assert.That(weapons.Count, Is.Not.EqualTo(0));
            Assert.That(weaponWithAttributes, Is.Not.Null);
        }
    }
}