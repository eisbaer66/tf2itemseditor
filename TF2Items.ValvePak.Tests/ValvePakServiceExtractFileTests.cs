using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using TF2Items.Core;

namespace TF2Items.ValvePak.Tests
{
    [TestFixture]
    [Category("VPK")]
    public class ValvePakServiceExtractFileTests
    {
        private MockRepository _mockRepository;
        private IValveTextureFormatService _vtfService;

        [SetUp]
        public void Setup()
        {
            LogConfigurator.ForTest();

            _mockRepository = new MockRepository();
            _vtfService = _mockRepository.StrictMock<IValveTextureFormatService>();
        }

        [Test]
        public void ThrowsExceptionIfConfigIsNull()
        {
            Assert.That(() => new ValvePakService(null, new SteamConfig(), _vtfService), Throws.ArgumentNullException);
        }

        [Test]
        public void ThrowsExceptionIfSteamConfigIsNull()
        {
            Assert.That(() => new ValvePakService(new Config(), null, _vtfService), Throws.ArgumentNullException);
        }

        [Test]
        public void ThrowsExceptionIfVtfServiceIsNull()
        {
            Assert.That(() => new ValvePakService(new Config(), new SteamConfig(), null), Throws.ArgumentNullException);
        }

        [Test]
        public async Task IntoGivenDirectory()
        {
            ValvePakService valvePakService = new ValvePakService(new Config(), new SteamConfig(), _vtfService);
            string filePath = await valvePakService.ExtractFile(@"root\materials\backpack\weapons\c_models\c_bet_rocketlauncher\c_bet_rocketlauncher_large.vtf", TestHelper.TestDir);

            AssertFile(filePath);
        }

        [Test]
        public async Task IntoTempDirectory()
        {
            ValvePakService valvePakService = new ValvePakService(new Config(), new SteamConfig(), _vtfService);
            string filePath = await valvePakService.ExtractFile(@"root\materials\backpack\weapons\c_models\c_bet_rocketlauncher\c_bet_rocketlauncher_large.vtf");

            AssertFile(filePath);
            Assert.That(filePath.StartsWith(Path.GetTempPath()), Is.True, filePath + " is not in temp-directory");
        }

        [Test]
        public async Task TrimmsBackslashes()
        {
            ValvePakService valvePakService = new ValvePakService(new Config(), new SteamConfig(), _vtfService);
            string filePath = await valvePakService.ExtractFile(@"root\materials\backpack\weapons\c_models\c_bet_rocketlauncher\c_bet_rocketlauncher_large.vtf\", TestHelper.TestDir + "\\");

            AssertFile(filePath);
        }

        private static void AssertFile(string filePath)
        {
            Assert.That(filePath, Is.Not.Null, "filepath is null");
            Assert.That(File.Exists(filePath), Is.True, filePath + " does not exist");

            long size = new FileInfo(filePath).Length;
            Assert.That(size, Is.EqualTo(349784), "size of output is wrong");
        }
    }
}