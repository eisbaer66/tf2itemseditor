using System.IO;
using System.Reflection;

namespace TF2Items.ValvePak.Tests
{
    public class TestHelper
    {
        public static string TestDir { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }}
    }
}