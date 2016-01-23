using System.IO;
using System.Threading.Tasks;
using log4net;
using NUnit.Framework;
using TF2Items.Core;

namespace TF2Items.ValvePak.Tests
{
    [TestFixture]
    [Category("VPK")]
    public class ValvePakServiceExtractFileTests
    {
        private ILog _log;

        [SetUp]
        public void Setup()
        {
            LogConfigurator.ForTest();
        }

        [Test]
        public async Task IntoGivenDirectory()
        {
            ValvePakService valvePakService = new ValvePakService(new Config(), new SteamConfig());
            string filePath = await valvePakService.ExtractFile(@"root\materials\backpack\weapons\c_models\c_bet_rocketlauncher\c_bet_rocketlauncher_large.vtf", TestHelper.TestDir);

            AssertFile(filePath);
        }

        [Test]
        public async Task IntoTempDirectory()
        {
            ValvePakService valvePakService = new ValvePakService(new Config(), new SteamConfig());
            string filePath = await valvePakService.ExtractFile(@"root\materials\backpack\weapons\c_models\c_bet_rocketlauncher\c_bet_rocketlauncher_large.vtf");

            AssertFile(filePath);
            Assert.That(filePath.StartsWith(Path.GetTempPath()), Is.True, filePath + " is not in temp-directory");
        }

        [Test]
        public async Task TrimmsBackslashes()
        {
            ValvePakService valvePakService = new ValvePakService(new Config(), new SteamConfig());
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