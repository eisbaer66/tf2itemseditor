using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class AttributeClassTests
    {
        [Test]
        public void ReturnedNullWennAttributeListeLeer()
        {
            AttributeClass attributeClass = new AttributeClass
                                            {
                                                Name = "TestAttribute",
                                            };

            Assert.That(attributeClass.Get(0), Is.Null);
        }

        [Test]
        public void ReturnedEinzigesAttributeInAttributeListe()
        {
            AttributeClass attributeClass = new AttributeClass
                                            {
                                                Name = "TestAttribute",
                                            };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);

            Assert.That(attributeClass.Get(0), Is.SameAs(attribute), "did not return attribute");
        }

        [Test]
        public void ReturnedFirstAttributeInAttributeList()
        {
            AttributeClass attributeClass = new AttributeClass
                                            {
                                                Name = "TestAttribute",
                                            };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(0), Is.SameAs(attribute), "did not return attribute");
        }
    }
}