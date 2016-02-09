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

        public async Task<IEnumerable<Tf2Attribute>> GetAttributes()
        {
            return (await _service.GetAttributes()).Take(100);
        }
    }
}