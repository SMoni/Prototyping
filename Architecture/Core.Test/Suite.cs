using System.Threading;
using NUnit.Framework;

namespace Architecture.Core.Test {
    [TestFixture]
    class Suite {

        [Test]
        public void PropertyChangeTriggersLastUpdateChange()
        {
            var card = new Card();

            Assert.AreEqual(card.Created, card.LastUpdated);

            Thread.Sleep(1000);

            card.Description = "Hello World";

            Assert.AreNotEqual(card.Created, card.LastUpdated);
        }

    }
}
