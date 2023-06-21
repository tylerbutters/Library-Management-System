using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LMS
{
    public class Book : INotifyPropertyChanged
    {
        private bool _isReserved;

        public bool isReserved
        {
            get { return _isReserved; }
            set
            {
                if (_isReserved != value)
                {
                    _isReserved = value;
                    OnPropertyChanged("isReserved");
                }
            }
        }
        public string id { get; set; }
        public string cover { get; set; }
        public string title { get; set; }
        public string authorFirstName { get; set; }
        public string authorLastName { get; set; }
        public string tag { get; set; }
        public string summary { get; set; }
        public string isAvailable { get; set; } = "true";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
