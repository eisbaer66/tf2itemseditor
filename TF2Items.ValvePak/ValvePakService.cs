using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;
using icebear;
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

            return await _vtfService.ConvertVtf(vtfFilePath, outputPath);
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

                await ProcessUtil.StartBackgroundProcess(exePath, args, Log);

                return Path.Combine(outputPath, fileName);
            }
        }
    }
}