using System;
using System.IO;
using System.Threading.Tasks;
using icebear;
using log4net;

namespace TF2Items.ValvePak
{
    public interface IValveTextureFormatService
    {
        Task<string> ConvertVtf(string vtfFilePath);
        Task<string> ConvertVtf(string vtfFilePath, string outputPath);
        Task<string> ConvertVtf(string vtfFilePath, string outputPath, bool overrideExisting);
    }

    public class ValveTextureFormatService : IValveTextureFormatService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ValveTextureFormatService));

        private readonly IConfig _config;

        public ValveTextureFormatService(IConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            _config = config;
        }

        public async Task<string> ConvertVtf(string vtfFilePath)
        {
            return await ConvertVtf(vtfFilePath, Path.GetTempPath(), true);
        }

        public async Task<string> ConvertVtf(string vtfFilePath, string outputPath)
        {
            return await ConvertVtf(vtfFilePath, outputPath, false);
        }

        public async Task<string> ConvertVtf(string vtfFilePath, string outputPath, bool overrideExisting)
        {
            if (vtfFilePath == null)
                throw new ArgumentNullException("vtfFilePath");

            vtfFilePath = vtfFilePath.Trim('\\');
            outputPath = outputPath.Trim('\\');

            string fileName = Path.GetFileName(vtfFilePath);

            using (NDC.Push(string.Format("[converting {0}]", fileName)))
            {
                Log.Debug(string.Format("vtfFilePath: '{0}'", vtfFilePath));
                Log.Debug(string.Format("outputPath: '{0}'", outputPath));

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                string outputFilename = fileNameWithoutExtension + ".png";
                string outputFilePath = Path.Combine(outputPath, outputFilename);
                if (File.Exists(outputFilePath))
                {
                    if (overrideExisting)
                    {
                        Log.Info("overriding existing output-file");
                        File.Delete(outputFilePath);
                    }
                    else
                    {
                        throw new IOException("file alreaday exists: " + outputFilePath);
                    }
                }

                string args = string.Format(@"-file ""{0}"" -output ""{1}"" -exportformat ""png""", vtfFilePath, outputPath);
                string exePath = _config.VtfCmdLocation;

                await ProcessUtil.StartBackgroundProcess(exePath, args, Log);

                return outputFilePath;
            }
        }
    }
}