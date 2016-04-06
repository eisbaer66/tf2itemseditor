using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TF2Items.Core;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    [DebuggerDisplay("{Class.Name}: {Value}")]
    public class WeaponDetailsSetAttributeViewModel : WeaponDetailsNumericalAttributeViewModel
    {
        private void Set(Tf2Attribute attribute)
        {
            Attribute = attribute;
            Editing = false;
        }

        public WeaponDetailsSetAttributeViewModel(ITf2WeaponService service)
            : base(service)
        {}

        public IList<WeaponDetailsSetAttributeOptionViewModel> Options { get { return Class.Attributes.Select(a => WeaponDetailsSetAttributeOptionViewModel.From(a, Set)).ToList(); } }
    }
}