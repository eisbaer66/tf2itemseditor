using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using icebear;
using log4net;
using Nito.AsyncEx;
using TF2Items.Core;

namespace TF2Items.ValvePak
{
    public class ValvePakService : IValvePakService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ValvePakService));
        private static readonly AsyncLock Lock = new AsyncLock();

        private readonly TempFileCollection _tempFileCollection;
        private readonly IConfig _config;
        private readonly ISteamConfig _steamconfig;
        private readonly IValveTextureFormatService _vtfService;

        public ValvePakService(IConfig config, ISteamConfig steamconfig, IValveTextureFormatService vtfService)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (steamconfig == null)
                throw new ArgumentNullException("steamconfig");
            if (vtfService == null)
                throw new ArgumentNullException("vtfService");
            _config = config;
            _steamconfig = steamconfig;
            _vtfService = vtfService;
        }

        public string TextureVpk { get { return Path.Combine(_steamconfig.TeamFortress2Directory, "tf", "tf2_textures_dir.vpk"); }}

        public async Task<string> ExtractWeaponIcon(string imageInventory)
        {
            string path = await ExtractWeaponIcon(imageInventory, _tempFileCollection.TempDir);
            if (String.IsNullOrEmpty(path))
                return null;
            _tempFileCollection.AddFile(path, false);
            return path;
        }

        public async Task<string> ExtractWeaponIcon(string imageInventory, string outputPath)
        {
            imageInventory = imageInventory.Trim('\\');
            outputPath = outputPath.Trim('\\');
            string extension = ".vtf";
            string pathInPackage = "root/materials/" + imageInventory + extension;

            string vtfFilePath = await ExtractFile(pathInPackage, outputPath);
            if (!File.Exists(vtfFilePath))
                return null;

            string pngFilePath = await _vtfService.ConvertVtf(vtfFilePath, outputPath, true);

            File.Delete(vtfFilePath);

            return pngFilePath;
        }

        public async Task<string> ExtractFile(string pathInPackage)
        {
            string path = await ExtractFile(pathInPackage, _tempFileCollection.TempDir);
            _tempFileCollection.AddFile(path, false);
            return path;
        }

        public async Task<string> ExtractFile(string pathInPackage, string outputPath)
        {
            if (pathInPackage == null)
                throw new ArgumentNullException("pathInPackage");

            pathInPackage = pathInPackage.Trim('\\');
            outputPath = outputPath.Trim('\\');

            string fileName = Path.GetFileName(pathInPackage);

            using (await Lock.LockAsync())
            using (NDC.Push(string.Format("[extracting {0}]", fileName)))
            {
                Log.Debug(string.Format("pathInPackage: '{0}'", pathInPackage));
                Log.Debug(string.Format("outputPath: '{0}'", outputPath));

                string args = string.Format(@"-p ""{0}"" -e ""{1}"" -d ""{2}""", TextureVpk, pathInPackage, outputPath);
                string exePath = _config.HlExtractLocation;

                await ProcessUtil.StartBackgroundProcess(exePath, args, Log);

                return Path.Combine(outputPath, fileName);
            }
        }
    }
}