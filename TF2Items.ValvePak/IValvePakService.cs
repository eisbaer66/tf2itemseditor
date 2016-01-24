using System.Threading.Tasks;

namespace TF2Items.ValvePak
{
    public interface IValvePakService
    {
        string TextureVpk { get; }
        Task<string> ExtractWeaponIcon(string imageInventory);
        Task<string> ExtractWeaponIcon(string imageInventory, string outputPath);
        Task<string> ExtractFile(string pathInPackage);
        Task<string> ExtractFile(string pathInPackage, string outputPath);
    }
}