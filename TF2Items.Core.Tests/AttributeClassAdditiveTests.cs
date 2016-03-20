using NUnit.Framework;

namespace TF2Items.Core.Tests
{
    [TestFixture]
    public class AttributeClassAdditiveTests
    {
        [Test]
        public void ReturnedNullWennAttributeListeLeer()
        {
            AttributeClassAdditive attributeClass = new AttributeClassAdditive
                                                    {
                                                        Name = "TestAttribute",
                                                    };

            Assert.That(attributeClass.Get(0, string.Empty), Is.Null);
        }

        [Test]
        public void ReturnedEinzigesAttributeInAttributeListe()
        {
            AttributeClassAdditive attributeClass = new AttributeClassAdditive
                                                    {
                                                        Name = "TestAttribute",
                                                    };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);

            Assert.That(attributeClass.Get(0, string.Empty), Is.SameAs(attribute), "did not return attribute");
        }

        [Test]
        public void ReturnesPositiveAttributeInAttributeListForValueZero()
        {
            AttributeClassAdditive attributeClass = new AttributeClassAdditive
                                                    {
                                                        Name = "TestAttribute",
                                                    };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "positive");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(0, string.Empty), Is.SameAs(attribute2), "did not return attribute");
        }

        [Test]
        public void ReturnesPositiveAttributeInAttributeListForValueGreaterOne()
        {
            AttributeClassAdditive attributeClass = new AttributeClassAdditive
                                                    {
                                                        Name = "TestAttribute",
                                                    };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "positive");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(.1f, string.Empty), Is.SameAs(attribute2), "did not return attribute");
        }

        [Test]
        public void ReturnesNegativeAttributeInAttributeListForValueLessThenOne()
        {
            AttributeClassAdditive attributeClass = new AttributeClassAdditive
                                                    {
                                                        Name = "TestAttribute",
                                                    };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "negative");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(-.1f, string.Empty), Is.SameAs(attribute2), "did not return attribute");
        }
    }
}