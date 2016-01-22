using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using log4net;
using TF2Items.Core;

namespace TF2Items.ValvePak
{
    public class ValvePakService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ValvePakService));

        private readonly TempFileCollection _tempFileCollection;
        private readonly IConfig _config;
        private readonly ISteamConfig _steamconfig;

        public ValvePakService(IConfig config, ISteamConfig steamconfig)
        {
            _config = config;
            _steamconfig = steamconfig;
            _tempFileCollection = new TempFileCollection(Path.GetTempPath(), false);
        }

        public string TextureVpk { get { return Path.Combine(_steamconfig.TeamFortress2Directory, "tf", "tf2_textures_dir.vpk"); }}

        public async Task<string> ExtractWeaponIcon(string imageInventory)
        {
            return await ExtractWeaponIcon(imageInventory, _tempFileCollection.TempDir);
        }
        public async Task<string> ExtractWeaponIcon(string imageInventory, string outputPath)
        {
            imageInventory = imageInventory.Trim('\\');
            outputPath = outputPath.Trim('\\');
            string extension = ".vtf";
            string filename = Path.GetDirectoryName(imageInventory) + extension;
            string pathInPackage = Path.Combine("root", "materials", imageInventory, filename);

            string vtfFilePath = await ExtractFile(pathInPackage, _tempFileCollection.TempDir);
            _tempFileCollection.AddFile(vtfFilePath, false);

            return await ConvertVtf(vtfFilePath, outputPath);
        }

        public async Task<string> ExtractFile(string pathInPackage)
        {
            return await ExtractFile(pathInPackage, _tempFileCollection.TempDir);
        }

        public async Task<string> ExtractFile(string pathInPackage, string outputPath)
        {
            if (pathInPackage == null)
                throw new ArgumentNullException("pathInPackage");

            pathInPackage = pathInPackage.Trim('\\');
            outputPath = outputPath.Trim('\\');

            string fileName = Path.GetFileName(pathInPackage);

            using (NDC.Push(string.Format("[extracting {0}]", fileName)))
            {
                Log.Debug(string.Format("pathInPackage: '{0}'", pathInPackage));
                Log.Debug(string.Format("outputPath: '{0}'", outputPath));

                string args = string.Format(@"-p ""{0}"" -e ""{1}"" -d ""{2}""", TextureVpk, pathInPackage, outputPath);
                string exePath = _config.HlExtractLocation;

                using (NDC.Push("[running HLExtract.exe]"))
                {
                    await StartProcess(exePath, args);
                }

                return Path.Combine(outputPath, fileName);
            }
        }

        public async Task<string> ConvertVtf(string vtfFilePath)
        {
            return await ConvertVtf(vtfFilePath, _tempFileCollection.TempDir, true);
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
                        throw new IOException("file alreaday exists: " +outputFilePath);
                    }
                }

                string args = string.Format(@"-file ""{0}"" -output ""{1}"" -exportformat ""png""", vtfFilePath, outputPath);
                string exePath = _config.VtfCmdLocation;

                using (NDC.Push("[running VTFCmd.exe]"))
                {
                    await StartProcess(exePath, args);
                }

                return outputFilePath;
            }
        }

        private static async Task<ProcessResult> StartProcess(string exePath, string args)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo(exePath, args) { CreateNoWindow = true },
            };

            ProcessResult result = await process
                .InitializeLogging()
                .StartAsync();
            return result
                .CaptureLogging(Log);
        }
    }

    public static class Processextensions
    {
        public static Process InitializeLogging(this Process process)
        {
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            return process;
        }
        public static ProcessResult CaptureLogging(this ProcessResult result, ILog log)
        {

            if (!string.IsNullOrEmpty(result.StandardOutput))
                log.Debug(result.StandardOutput);
            if (!string.IsNullOrEmpty(result.ErrorOutput))
                log.Error(result.ErrorOutput);

            string exitMessage = "exited with ExitCode " + result.ExitCode;
            if (result.ExitCode == 0)
                log.Debug(exitMessage);
            else
                log.Error(exitMessage);

            return result;
        }
        public static Task<ProcessResult> StartAsync(this Process process)
        {
            TaskCompletionSource<ProcessResult> taskCompletionSource = new TaskCompletionSource<ProcessResult>();
            Stack ndcStack = NDC.CloneStack();

            process.EnableRaisingEvents = true;
            process.Exited += (s, a) =>
                              {
                                  NDC.Inherit(ndcStack);
                                  ProcessResult result = new ProcessResult
                                                                {
                                                                    ExitCode = process.ExitCode,
                                                                    StandardOutput = process.StandardOutput.ReadToEnd(),
                                                                    ErrorOutput = process.StandardError.ReadToEnd(),
                                                                };

                                  taskCompletionSource.SetResult(result);
                                  process.Dispose();
                              };

            process.Start();

            return taskCompletionSource.Task;
        }
    }

    public class ProcessResult
    {
        public int ExitCode { get; set; }
        public string StandardOutput { get; set; }
        public string ErrorOutput { get; set; }
    }
}