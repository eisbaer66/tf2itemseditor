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

            Assert.That(attributeClass.Get(0, string.Empty), Is.Null);
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

            Assert.That(attributeClass.Get(0, string.Empty), Is.SameAs(attribute), "did not return attribute");
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

            Assert.That(attributeClass.Get(0, string.Empty), Is.SameAs(attribute), "did not return attribute");
        }
    }

    [TestFixture]
    public class AttributeClassSetTests
    {
        [Test]
        public void ReturnedNullWennAttributeListeLeer()
        {
            AttributeClassSet attributeClass = new AttributeClassSet
                                            {
                                                Name = "TestAttribute",
                                            };

            Assert.That(attributeClass.Get(0, string.Empty), Is.Null);
        }

        [Test]
        public void ReturnedEinzigesAttributeInAttributeListe()
        {
            AttributeClassSet attributeClass = new AttributeClassSet
                                            {
                                                Name = "TestAttribute",
                                            };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);

            Assert.That(attributeClass.Get(0, string.Empty), Is.SameAs(attribute), "did not return attribute");
        }

        [Test]
        public void ReturnedFirstAttributeInAttributeList()
        {
            AttributeClassSet attributeClass = new AttributeClassSet
                                            {
                                                Name = "TestAttribute",
                                            };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(0, string.Empty), Is.SameAs(attribute), "did not return attribute");
        }

        [Test]
        public void ReturnedAttributeWithMatchingName()
        {
            AttributeClassSet attributeClass = new AttributeClassSet
                                            {
                                                Name = "TestAttribute",
                                            };
            Tf2Attribute attribute = new Tf2Attribute(1, "TestAttribute", "Test-Attribute1", "formatIstEgal", "effectTypeIstEgal");
            Tf2Attribute attribute2 = new Tf2Attribute(2, "TestAttribute", "Test-Attribute2", "formatIstEgal", "effectTypeIstEgal");
            attributeClass.Attributes.Add(attribute);
            attributeClass.Attributes.Add(attribute2);

            Assert.That(attributeClass.Get(0, "Test-Attribute2"), Is.SameAs(attribute2), "did not return attribute");
        }
    }
}