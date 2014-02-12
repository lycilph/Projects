using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Loader
{
    public class ImageViewModel : ObservableObject
    {
        private string filename;
        public string Filename
        {
            get { return filename; }
            set
            {
                if (filename != value)
                {
                    filename = value;
                    NotifyPropertyChanged("Filename");
                }
            }
        }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set 
            {
                if (selected != value)
                {
                    selected = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        public ImageViewModel(string filename)
        {
            Filename = filename;
            Selected = false;
        }
    }
}
