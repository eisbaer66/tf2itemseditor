using System.IO;
using System.Linq;

namespace TF2Items.Parsers.Tests
{
    public class TestDeployment
    {
        public static string GetFile(string filename)
        {
            string dir = Directory.GetCurrentDirectory();

            string foundFile = SearchFile(filename, dir);
            if (foundFile == null)
                throw new FileNotFoundException("test-file was not found", filename);
            return foundFile;
        }

        private static string SearchFile(string filename, string dir)
        {
            string foundFile = Directory.GetFiles(dir).FirstOrDefault(f => Path.GetFileName(f) == filename);
            if (foundFile != null)
                return foundFile;
            foreach (string subDir in Directory.GetDirectories(dir))
            {
                foundFile = SearchFile(filename, subDir);
                if (foundFile != null)
                    return foundFile;
            }

            return null;
        }
    }
}