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
        public string id { get; set; }
        public string cover { get; set; }
        public string title { get; set; }
        public string authorFirstName { get; set; }
        public string authorLastName { get; set; }
        public string tag { get; set; }
        public string summary { get; set; }
        public string isAvailable { get; set; } = "true";


        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isReserved;
        private bool _isLoaned;
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
        public bool isLoaned
        {
            get { return _isLoaned; }
            set
            {
                if (_isLoaned != value)
                {
                    _isLoaned = value;
                    OnPropertyChanged("isLoaned");
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
