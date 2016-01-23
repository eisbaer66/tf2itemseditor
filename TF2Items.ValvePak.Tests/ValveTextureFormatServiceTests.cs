using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using log4net;
using NUnit.Framework;

namespace TF2Items.ValvePak.Tests
{
    [TestFixture]
    public class ValveTextureFormatServiceTests
    {
        private ILog _log;

        [SetUp]
        public void Setup()
        {
            LogConfigurator.ForTest();
            _log = LogManager.GetLogger(typeof(ValveTextureFormatServiceTests));

            IEnumerable<string> files = new List<string>
                                        {
                                            Path.Combine(TestHelper.TestDir, "c_bet_rocketlauncher_large.png"),
                                            Path.Combine(Path.GetTempPath(), "c_bet_rocketlauncher_large.png")
                                        };
            foreach (string file in files)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        [Test]
        public void ThrowsExceptionIfConfigIsNull()
        {
            Assert.That(() => new ValveTextureFormatService(null), Throws.ArgumentNullException);
        }

        [Test]
        public async Task IntoGivenDirectory()
        {
            ValveTextureFormatService valvePakService = new ValveTextureFormatService(new Config());
            string filePath = await valvePakService.ConvertVtf(Path.Combine(TestHelper.TestDir, @"c_bet_rocketlauncher_large.vtf"), TestHelper.TestDir);

            AssertFile(filePath);
        }

        [Test]
        public void IntoGivenDirectoryThrowsExeptionIfFileExists()
        {
            File.Create(Path.Combine(TestHelper.TestDir, "c_bet_rocketlauncher_large.png")).Close();

            ValveTextureFormatService valvePakService = new ValveTextureFormatService(new Config());
            Assert.That(async () => await valvePakService.ConvertVtf(Path.Combine(TestHelper.TestDir, @"c_bet_rocketlauncher_large.vtf"), TestHelper.TestDir), Throws.InstanceOf<IOException>());
        }

        [Test]
        public async Task IntoTempDirectory()
        {
            ValveTextureFormatService valvePakService = new ValveTextureFormatService(new Config());
            string filePath = await valvePakService.ConvertVtf(Path.Combine(TestHelper.TestDir, @"c_bet_rocketlauncher_large.vtf"));

            AssertFile(filePath);
            Assert.That(filePath.StartsWith(Path.GetTempPath()), Is.True, filePath + " is not in temp-directory");
        }

        [Test]
        public async Task IntoGivenDirectoryOverridesFile()
        {
            File.Create(Path.Combine(TestHelper.TestDir, "c_bet_rocketlauncher_large.png")).Close();

            ValveTextureFormatService valvePakService = new ValveTextureFormatService(new Config());
            string filePath = await valvePakService.ConvertVtf(Path.Combine(TestHelper.TestDir, @"c_bet_rocketlauncher_large.vtf"));

            AssertFile(filePath);
        }

        [Test]
        public async Task TrimmsBackslashes()
        {
            ValveTextureFormatService valvePakService = new ValveTextureFormatService(new Config());
            string filePath = await valvePakService.ConvertVtf(Path.Combine(TestHelper.TestDir, @"c_bet_rocketlauncher_large.vtf"), TestHelper.TestDir + "\\");

            AssertFile(filePath);
        }

        private void AssertFile(string filePath)
        {
            _log.Debug("checking " + filePath);

            Assert.That(filePath, Is.Not.Null, "filepath is null");
            Assert.That(File.Exists(filePath), Is.True, filePath + " does not exist");

            long size = new FileInfo(filePath).Length;
            Assert.That(size, Is.EqualTo(54197), "size of output is wrong");
        }
    }
}