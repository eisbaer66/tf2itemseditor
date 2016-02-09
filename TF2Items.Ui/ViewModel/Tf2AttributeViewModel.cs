using System;
using System.Diagnostics;
using TF2Items.Core;

namespace TF2Items.Ui.ViewModel
{
    public class Tf2AttributeViewModel
    {
        public Tf2Attribute Model { get; set; }

        public string Image
        {
            get
            {
                return Model.EffectType == "positive"
                    ? "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_plus.png"
                    : "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_minus.png";
            }
        }
        public string Format
        {
            get
            {
                switch (Model.Format)
                {
                    case "value_is_percentage":
                        return "%";
                    case "value_is_inverted_percentage":
                        return "-%";
                    case "value_is_additive_percentage":
                        return "+%";
                    case "value_is_additive":
                        return "+";
                    case "value_is_or":
                        return "OR";
                    case "value_is_particle_index":
                        return "OR";
                    default:
                    {
                        Debug.WriteLine(Model.Format);
                        return Model.Format.Replace("_", Environment.NewLine);
                    }
                }
            }
        }
    }
}