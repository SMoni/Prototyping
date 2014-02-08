using NUnit.Framework;
using Architecture.Core;

namespace Architecture.Tools.Test
{
    [TestFixture]
    class Suite
    {
        [Test]
        public void CreateCardByFacade()
        {
            var facade = Facade.Instance;

            var createdCard = facade.Create<Card>();

            Assert.IsNotNull(createdCard);

        }

        [Test]
        public void CreateSectionByFacade()
        {
            var facade = Facade.Instance;

            var createdSection = facade.Create<Section>();

            Assert.IsNotNull(createdSection);

        }

        [Test]
        public void CreateBoardByFacade()
        {
            var facade = Facade.Instance;

            var createdBoard = facade.Create<Board>();

            Assert.IsNotNull(createdBoard);

        }

        [Test]
        public void LoadBoardIncludingSectionsAndCardsById()
        {
            
        }

        [Test]
        public void SaveBoardIncludingSectionsAndCards()
        {
            
        }

    }
}
