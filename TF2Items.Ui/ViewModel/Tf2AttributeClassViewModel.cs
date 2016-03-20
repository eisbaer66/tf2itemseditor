using System;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using TF2Items.Core;

namespace TF2Items.Ui.ViewModel
{
    public class Tf2AttributeClassViewModel : ViewModelBase
    {
        private AttributeClass _class;
        private Tf2Attribute _model;

        public AttributeClass Class
        {
            get { return _class; }
            set
            {
                _class = value;
                _model = value.Get(1, value.Name);

                RaisePropertyChanged(() => Class);
                RaisePropertyChanged(() => Image);
                RaisePropertyChanged(() => Format);
            }
        }

        public string Image
        {
            get
            {
                switch (_model.EffectType)
                {
                    case "positive":
                        return "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_plus.png";
                    case "negative":
                        return "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_minus.png";
                    default:
                        return "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_neutral.png";
                }
            }
        }
        public string Format
        {
            get
            {
                switch (_model.Format)
                {
                    case "value_is_percentage":
                        return "%";
                    case "value_is_inverted_percentage":
                        return "%";
                    case "value_is_additive_percentage":
                        return "+%";
                    case "value_is_additive":
                        return "+";
                    case "value_is_or":
                        return "OR";
                    case "value_is_particle_index":
                        return "PI";
                    default:
                    {
                        Debug.WriteLine(_model.Format);
                        return _model.Format.Replace("_", Environment.NewLine);
                    }
                }
            }
        }
    }
}