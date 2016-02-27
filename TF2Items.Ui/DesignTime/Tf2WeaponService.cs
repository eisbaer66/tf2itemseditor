using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TF2Items.Core;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.DesignTime
{
    class Tf2WeaponService : ITf2WeaponService
    {
        private readonly ITf2WeaponService _service;

        public Tf2WeaponService(ITf2WeaponService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<Tf2Weapon>> Get()
        {
            return (await _service.Get()).Take(100);
        }

        public async Task<IDictionary<string, Tf2Attribute>> GetAttributesAsClassDictionary()
        {
            return await _service.GetAttributesAsClassDictionary();
        }

        public async Task<IDictionary<string, AttributeClass>> GetAttributeClassesAsDictionary()
        {
            return await _service.GetAttributeClassesAsDictionary();
        }

        public async Task<IDictionary<int, AttributeClass>> GetAttributeClassesAsAttributeIdDictionary()
        {
            return await _service.GetAttributeClassesAsAttributeIdDictionary();
        }

        public async Task<IEnumerable<AttributeClass>> GetAttributeClasses()
        {
            return (await _service.GetAttributeClasses()).Take(100);
        }

        public async Task<IEnumerable<Tf2Attribute>> GetAttributes()
        {
            return (await _service.GetAttributes()).Take(100);
        }
    }
}