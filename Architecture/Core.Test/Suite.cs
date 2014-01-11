using System;
using System.Threading;
using NUnit.Framework;

namespace Architecture.Core.Test {
    [TestFixture]
    class Suite {

        [Test]
        public void CardPropertyChangeTriggersLastUpdateChange()
        {
            var card = new Card();

            Assert.AreEqual(card.Created, card.LastUpdated);

            Thread.Sleep(1000);

            card.Description = "Hello World";

            Assert.AreNotEqual(card.Created, card.LastUpdated);
        }

        [Test]
        public void CardPropertiesChangeTriggersEvent()
        {
            var card = new Card();
            var hasEventOccurred = false;

            card.PropertyChanged += (sender, args) => hasEventOccurred = true;

            card.Description = "Hello World";

            Assert.IsTrue(hasEventOccurred);

            hasEventOccurred = false;

            card.Responsible = "Hello World";

            Assert.IsTrue(hasEventOccurred);

            hasEventOccurred = false;

            card.Requester = "Hello World";

            Assert.IsTrue(hasEventOccurred);

        }

        [Test]
        public void SectionPropertiesChangeTriggersEvent()
        {
            var section = new Section();
            var hasEventOccured = false;

            section.PropertyChanged += (sender, args) => hasEventOccured = true;

            section.Name = "Hello World";

            Assert.IsTrue(hasEventOccured);

            hasEventOccured = false;

            section.WorkInProgressLimit = 10;

            Assert.IsTrue(hasEventOccured);

        }

        [Test]
        public void TriggerExceptionWorkInProgressLimitLessThanZero() {
            var section = new Section();

            try
            {
                section.WorkInProgressLimit = -1;

                Assert.Fail("Should not be reached!!!");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void AddCardIntoSection() {
            var section = new Section();
            var card = new Card();

            var hasCardBeenAdded = false;

            section.CardAdded += (sender, args) => hasCardBeenAdded = true;

            section.Add(card);

            Assert.IsTrue(hasCardBeenAdded);

        }

        [Test]
        public void RemoveCardFromSection() {
            var section = new Section();
            var card = new Card();

            var hasCardBeenRemoved = false;

            section.CardRemoved += (sender, args) => hasCardBeenRemoved = true;

            section.Add(card);

            section.Remove(card);

            Assert.IsTrue(hasCardBeenRemoved);

        }


    }
}
