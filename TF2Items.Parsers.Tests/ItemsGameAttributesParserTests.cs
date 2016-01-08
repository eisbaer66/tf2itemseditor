using System.Collections.Generic;
using log4net.Config;
using NUnit.Framework;
using TF2Items.Core;

namespace TF2Items.Parsers.Tests
{
    [TestFixture]
    public class ItemsGameAttributesParserTests
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
            IList<WeaponAttribute> attributes = new ItemsGameAttributesParser().Parse(filepath);

            Assert.That(attributes, Is.Not.Null);
            Assert.That(attributes.Count, Is.Not.EqualTo(0));
        }
    }
}