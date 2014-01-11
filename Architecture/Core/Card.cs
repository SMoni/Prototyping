using System;
using System.ComponentModel;

namespace Architecture.Core 
{
    public class Card
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Card()
        {
            _id          = Guid.NewGuid();
            _created     = DateTime.Now;
            _lastUpdated = Created;
            
            _description = String.Empty;
            _responsible = String.Empty;
            _requester   = String.Empty;
        }

        private readonly Guid _id;

        public Guid Id
        {
            get { return _id; }
        }

        private readonly DateTime _created;

        public DateTime Created
        {
            get { return _created; }
        }

        private DateTime _lastUpdated;

        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
        }

        private String _description;

        public String Description
        {
            get { return _description; }
            set
            {
                if(_description.Equals(value))
                    return;

                _description = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        private String _responsible;

        public String Responsible
        {
            get { return _responsible; }
            set
            {
                if (_responsible.Equals(value))
                    return;

                _responsible = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Responsible"));
            }
        }

        private string _requester;

        public String Requester
        {
            get { return _requester; }
            set
            {
                if (_requester.Equals(value))
                    return;

                _requester = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Requester"));
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            _lastUpdated = DateTime.Now;
            
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, eventArgs);
        }

    }
}
