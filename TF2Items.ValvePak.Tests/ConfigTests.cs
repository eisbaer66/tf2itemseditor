using NUnit.Framework;

namespace TF2Items.ValvePak.Tests
{
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        public void ReadsHlExtractLocation()
        {
            Assert.That(new Config().HlExtractLocation, Is.EqualTo("..\\..\\..\\tools\\HLExtract.exe"));
        }
        [Test]
        public void ReadsVtfCmdLocation()
        {
            Assert.That(new Config().VtfCmdLocation, Is.EqualTo("..\\..\\..\\tools\\VTFCmd.exe"));
        }
    }
}
