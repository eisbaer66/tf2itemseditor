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
            IList<Tf2Attribute> attributes = new Tf2AttributesParser().Parse(filepath);

            Assert.That(attributes, Is.Not.Null);
            Assert.That(attributes.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void ParsesItemsGameAsDictionary()
        {
            string filepath = TestDeployment.GetFile("items_game.txt");
            IDictionary<int, Tf2Attribute> attributes = new Tf2AttributesParser().ParseAsDictionary(filepath);

            Assert.That(attributes, Is.Not.Null);
            Assert.That(attributes.Count, Is.Not.EqualTo(0));
        }
    }
}