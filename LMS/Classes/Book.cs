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
        private bool _isLoaned;
        private bool _isReservedByUser;
        public string id { get; set; }
        public string cover { get; set; }
        public string title { get; set; }
        public string authorFirstName { get; set; }
        public string authorLastName { get; set; }
        public string subject { get; set; }
        public string summary { get; set; }
        public bool isLoanedByUser { get; set; } //doesnt need property chanaged even cuz it doesnt update on click 
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
        public bool isReservedByUser
        {
            get { return _isReservedByUser; }
            set
            {
                if (_isReservedByUser != value)
                {
                    _isReservedByUser = value;
                    OnPropertyChanged("isReservedByUser");
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
