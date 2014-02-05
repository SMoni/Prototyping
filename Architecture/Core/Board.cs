using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Architecture.Core 
{
    public class Board
    {
        public event ListChangedEventHandler SectionAdded;
        public event ListChangedEventHandler SectionRemoved;

        public Board()
        {
            _id          = Guid.NewGuid();
            _lastUpdated = DateTime.Now;
        }

        private readonly Guid _id;

        public Guid Id {
            get { return _id; }
        }

        private DateTime _lastUpdated;

        public DateTime LastUpdated {
            get { return _lastUpdated; }
        }

        private readonly List<Section> _sections = new List<Section>();

        public IEnumerable<Section> Sections {
            get { return _sections.ToArray(); }
        }

        public void Add(Section section)
        {
            if (_sections.Exists(toFind => section.Id == toFind.Id))
                return;

            _sections.Add(section);

            var eventArgs = new ListChangedEventArgs(
                ListChangedType.ItemAdded,
                _sections.IndexOf(section)
            );

            OnSectionAdded(eventArgs);
        }

        private void OnSectionAdded(ListChangedEventArgs listChangedEventArgs)
        {
            _lastUpdated = DateTime.Now;

            if (SectionAdded == null)
                return;

            SectionAdded.Invoke(this, listChangedEventArgs);
        }

        public void Remove(Section section)
        {
            var sectionToRemove = _sections.SingleOrDefault(toFind => section.Id == toFind.Id);

            if (sectionToRemove == null)
                return;

            var eventArgs = new ListChangedEventArgs(
                ListChangedType.ItemDeleted,
                -1,
                _sections.IndexOf(sectionToRemove)
            );

            _sections.Remove(sectionToRemove);

            OnSectionRemoved(eventArgs);
        }

        private void OnSectionRemoved(ListChangedEventArgs listChangedEventArgs)
        {
            _lastUpdated = DateTime.Now;

            if (SectionRemoved == null)
                return;

            SectionRemoved.Invoke(this, listChangedEventArgs);
        }
    }
}
