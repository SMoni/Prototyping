using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Architecture.Core 
{
    public class Section
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event ListChangedEventHandler     CardAdded;
        public event ListChangedEventHandler     CardRemoved;
        public event EventHandler                WorkInProgressLimitExceeded;

        public Section()
        {
            _id                  = Guid.NewGuid();
            _lastUpdated         = DateTime.Now;
            _name                = String.Empty;
            _workInProgressLimit = 0;
        }

        private readonly Guid _id;

        public Guid Id {
            get { return _id; }
        }

        private DateTime _lastUpdated;

        public DateTime LastUpdated {
            get { return _lastUpdated; }
        }

        private String _name;

        public String Name
        {
            get { return _name; }
            set
            {
                if (_name.Equals(value))
                    return;

                _name = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        private int _workInProgressLimit;

        /// <summary>
        /// Defines how many card should be maximum in this section.
        /// 0 defines an unlimited amount of cards.
        /// Less than 0 throws an exception.
        /// </summary>
        public int WorkInProgressLimit
        {
            get { return _workInProgressLimit; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Limit could NOT be less than 0!!!");
                
                if(_workInProgressLimit.Equals(value))
                    return;

                _workInProgressLimit = value;

                OnPropertyChanged(new PropertyChangedEventArgs("WorkInProgressLimit"));

            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _lastUpdated = DateTime.Now;

            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, propertyChangedEventArgs);
        }

        private readonly List<Card> _cards = new List<Card>();

        public IEnumerable<Card> Cards
        {
            get { return _cards.ToArray(); }
        }

        public void Add(Card card)
        {
            if (_cards.Exists(toFind => card.Id == toFind.Id))
                return;

            _cards.Add(card);

            var eventArgs = new ListChangedEventArgs(
                ListChangedType.ItemAdded,
                _cards.IndexOf(card)
            );

            OnCardAdded(eventArgs);

            if (_cards.Count > WorkInProgressLimit)
                OnWorkInProgressLimitExceeded(eventArgs);
        }

        private void OnCardAdded(ListChangedEventArgs listChangedEventArgs)
        {
            _lastUpdated = DateTime.Now;
            
            if (CardAdded == null)
                return;

            CardAdded.Invoke(this, listChangedEventArgs);
        }

        private void OnWorkInProgressLimitExceeded(EventArgs eventArgs)
        {
            if (WorkInProgressLimitExceeded == null)
                return;

            WorkInProgressLimitExceeded.Invoke(this, eventArgs);
        }

        public void Remove(Card card)
        {
            var cardToRemove = _cards.SingleOrDefault(toFind => card.Id == toFind.Id);
            
            if (cardToRemove == null)
                return;

            var eventArgs = new ListChangedEventArgs(
                ListChangedType.ItemDeleted,
                -1,
                _cards.IndexOf(cardToRemove)
            );

            _cards.Remove(cardToRemove);

            OnCardRemoved(eventArgs);
        }

        private void OnCardRemoved(ListChangedEventArgs listChangedEventArgs)
        {
            _lastUpdated = DateTime.Now;

            if (CardRemoved == null)
                return;

            CardRemoved.Invoke(this, listChangedEventArgs);
        }
    }
}
