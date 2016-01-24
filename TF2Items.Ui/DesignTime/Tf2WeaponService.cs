using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Tf2Weapon> Get()
        {
            return _service.Get().Take(100);
        }
    }
}