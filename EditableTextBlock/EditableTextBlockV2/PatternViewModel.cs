using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditableTextBlockV2
{
    public enum PatterEditType
    {
        None,
        Name,
        Regex
    }

    public class PatternViewModel : ViewModelBase<Pattern>
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        public string Regex
        {
            get { return Model.Regex; }
            set { Model.Regex = value; }
        }

        private PatterEditType _EditType = PatterEditType.None;
        public PatterEditType EditType
        {
            get { return _EditType; }
            set
            {
                if (_EditType == value) return;
                _EditType = value;
                NotifyPropertyChanged();
            }
        }

        public PatternViewModel(Pattern pattern) : base(pattern) {}

        public void ToggleEditType()
        {
            switch (EditType)
            {
                case PatterEditType.None:
                    EditType = PatterEditType.Name;
                    break;
                case PatterEditType.Name:
                    EditType = PatterEditType.Regex;
                    break;
                case PatterEditType.Regex:
                    EditType = PatterEditType.Name;
                    break;
                default:
                    break;
            }
        }
    }
}
