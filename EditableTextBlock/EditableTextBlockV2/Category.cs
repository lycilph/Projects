﻿using System.Collections.ObjectModel;

namespace EditableTextBlockV2
{
    public class Category : ObservableObject
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

        private ObservableCollection<Pattern> _Patterns = new ObservableCollection<Pattern>();
        public ObservableCollection<Pattern> Patterns
        {
            get { return _Patterns; }
            set
            {
                if (_Patterns == value) return;
                _Patterns = value;
                NotifyPropertyChanged();
            }
        }
    }
}
