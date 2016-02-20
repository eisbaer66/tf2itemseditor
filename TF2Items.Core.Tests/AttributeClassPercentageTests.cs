using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class AttributeClassPercentageTests
    {
        [Test]
        public void ReturnedNullWennAttributeListeLeer()
        {
            AttributeClassPercentage attributeClass = new AttributeClassPercentage
                                                      {
                                                          Name = "TestAttribute",
                                                      };

            Assert.That(attributeClass.Get(0), Is.Null);
        }

        [Test]
        public void ReturnedEinzigesAttributeInAttributeListe()
        {
            AttributeClassPercentage attributeClass = new AttributeClassPercentage
                                                      {
                                                          Name = "TestAttribute",
                                                      };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);

            Assert.That(attributeClass.Get(0), Is.SameAs(attribute), "did not return attribute");
        }

        [Test]
        public void ReturnesPositiveAttributeInAttributeListForValueOne()
        {
            AttributeClassPercentage attributeClass = new AttributeClassPercentage
                                                      {
                                                          Name = "TestAttribute",
                                                      };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "positive");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(1), Is.SameAs(attribute2), "did not return attribute");
        }

        [Test]
        public void ReturnesPositiveAttributeInAttributeListForValueGreaterOne()
        {
            AttributeClassPercentage attributeClass = new AttributeClassPercentage
                                                      {
                                                          Name = "TestAttribute",
                                                      };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "positive");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(1.1f), Is.SameAs(attribute2), "did not return attribute");
        }

        [Test]
        public void ReturnesNegativeAttributeInAttributeListForValueLessThenOne()
        {
            AttributeClassPercentage attributeClass = new AttributeClassPercentage
                                                      {
                                                          Name = "TestAttribute",
                                                      };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "negative");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(.9f), Is.SameAs(attribute2), "did not return attribute");
        }
    }
}