using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest.ViewModels
{
    public class FlyoutBase : ObservableObject
    {
        private string _Header;
        public string Header
        {
            get { return _Header; }
            set
            {
                if (_Header == value) return;
                _Header = value;
                NotifyPropertyChanged();
            }
        }

        private bool _IsOpen;
        public bool IsOpen
        {
            get { return _IsOpen; }
            set
            {
                if (_IsOpen == value) return;
                _IsOpen = value;
                NotifyPropertyChanged();
            }
        }

        private Position _Position;
        public Position Position
        {
            get { return _Position; }
            set
            {
                if (_Position == value) return;
                _Position = value;
                NotifyPropertyChanged();
            }
        }

        public FlyoutBase(string header, Position position)
        {
            _Header = header;
            _Position = position;
            _IsOpen = false;
        }

        public void Toggle()
        {
            IsOpen = !IsOpen;
        }
    }
}
