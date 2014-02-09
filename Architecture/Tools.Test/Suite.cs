﻿using System;
using System.Collections.Generic;
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
        public void SerializeCard()
        {
            var facade     = Facade.Instance;
            var serializer = new Serializing(FactoryMethods);

            var card = facade.Create<Card>();

            var expectedData = new Dictionary<String, String>
            {
                  {"Id",          card.Id.ToString()}
                , {"Type",        "Card"}
                , {"Description", "Hello World"}
                , {"Requester",   "Le me"}
                , {"Responsible", "Le not me"}
            };

            card.Description = expectedData["Description"];
            card.Requester   = expectedData["Requester"];
            card.Responsible = expectedData["Responsible"];

            var data = serializer.Serialize(card);

            Assert.IsNotNull(data[0]);

            var cardData = data[0];

            CompareActualToExpectedData(expectedData, cardData);
        }

        [Test]
        public void SerializeSection()
        {
            var facade     = Facade.Instance;
            var serializer = new Serializing(FactoryMethods);

            var section = facade.Create<Section>();

            var expectedData = new Dictionary<String, String>
            {
                  {"Id",                  section.Id.ToString()}
                , {"Type",                "Section"}
                , {"Name",                "Le section"}
                , {"WorkInProgressLimit", "2"}
            };

            section.Name                = expectedData["Name"];
            section.WorkInProgressLimit = int.Parse(expectedData["WorkInProgressLimit"]);

            var data = serializer.Serialize(section);

            Assert.IsNotNull(data[0]);

            var sectionData = data[0];

            CompareActualToExpectedData(expectedData, sectionData);            
        }

        [Test]
        public void SerializeBoard()
        {
            var facade     = Facade.Instance;
            var serializer = new Serializing(FactoryMethods);

            var board = facade.Create<Board>();

            var expectedData = new Dictionary<String, String>
            {
                  {"Id",   board.Id.ToString()}
                , {"Type", "Board"}
            };

            var data = serializer.Serialize(board);

            Assert.IsNotNull(data[0]);

            var boardData = data[0];

            CompareActualToExpectedData(expectedData, boardData);
        }

        private static void CompareActualToExpectedData(Dictionary<String, String> expectedData, Dictionary<String, String> actualData)
        {
            foreach (var expectedKeyValuePair in expectedData)
            {
                Assert.IsTrue(actualData.ContainsKey(expectedKeyValuePair.Key));
                Assert.AreEqual(expectedKeyValuePair.Value, actualData[expectedKeyValuePair.Key]);
            }
        }

        [Test]
        public void LoadBoardIncludingSectionsAndCardsById()
        {
            
        }

        [Test]
        public void SaveBoardIncludingSectionsAndCards()
        {
            
        }

        private Dictionary<Type, Func<Object>> _factoryMethods;

        private Dictionary<Type, Func<Object>> FactoryMethods
        {
            get
            {
                return _factoryMethods ?? (_factoryMethods = new Dictionary<Type, Func<Object>>
                {
                     { typeof(Board),   () => new Board() }
                    ,{ typeof(Section), () => new Section() }
                    ,{ typeof(Card),    () => new Card() }
                });
            }
        }


    }
}
