using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class AttributeClassAdditivePercentageTests
    {
        [Test]
        public void ReturnedNullWennAttributeListeLeer()
        {
            AttributeClassAdditivePercentage attributeClass = new AttributeClassAdditivePercentage
                                                              {
                                                                  Name = "TestAttribute",
                                                              };

            Assert.That(attributeClass.Get(0), Is.Null);
        }

        [Test]
        public void ReturnedEinzigesAttributeInAttributeListe()
        {
            AttributeClassAdditivePercentage attributeClass = new AttributeClassAdditivePercentage
                                                              {
                                                                  Name = "TestAttribute",
                                                              };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);

            Assert.That(attributeClass.Get(0), Is.SameAs(attribute), "did not return attribute");
        }

        [Test]
        public void ReturnesPositiveAttributeInAttributeListForValueZero()
        {
            AttributeClassAdditivePercentage attributeClass = new AttributeClassAdditivePercentage
                                                              {
                                                                  Name = "TestAttribute",
                                                              };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "positive");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(0), Is.SameAs(attribute2), "did not return attribute");
        }

        [Test]
        public void ReturnesPositiveAttributeInAttributeListForValueGreaterOne()
        {
            AttributeClassAdditivePercentage attributeClass = new AttributeClassAdditivePercentage
                                                              {
                                                                  Name = "TestAttribute",
                                                              };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "positive");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(.1f), Is.SameAs(attribute2), "did not return attribute");
        }

        [Test]
        public void ReturnesNegativeAttributeInAttributeListForValueLessThenOne()
        {
            AttributeClassAdditivePercentage attributeClass = new AttributeClassAdditivePercentage
                                                              {
                                                                  Name = "TestAttribute",
                                                              };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "negative");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(-.1f), Is.SameAs(attribute2), "did not return attribute");
        }
    }
}