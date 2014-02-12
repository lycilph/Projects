namespace EditableTextBlockV2
{
    public class Pattern : ObservableObject
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        private string _Regex;
        public string Regex
        {
            get { return _Regex; }
            set
            {
                if (_Regex == value) return;
                _Regex = value;
                NotifyPropertyChanged();
            }
        }

        public Pattern() : this(string.Empty, string.Empty) {}
        public Pattern(string name, string regex)
        {
            Name = name;
            Regex = regex;
        }
    }
}
