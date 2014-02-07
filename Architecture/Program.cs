using System;
using Architecture.Tools;
using Architecture.Core;

namespace Architecture {
    class Program {
        static void Main(string[] args)
        {
            var facade = new Facade();

            var loadedBoard = facade.Load<Board>(Guid.NewGuid());

            loadedBoard.Save();
            loadedBoard.Delete();

            var createdBoard = facade.Create<Board>();

            createdBoard.Save();

            var createdSection = facade.Create<Section>();

            createdSection.Name = "Hello World";

            createdBoard.Add(createdSection);

            var createdCardNo1 = facade.Create<Card>();

            createdCardNo1.Description = "Do something";
            createdCardNo1.Responsible = "Me";
            createdCardNo1.Requester   = "You";

            createdSection.Add(createdCardNo1);

            var createdCardNo2 = facade.Create<Card>();

            createdCardNo2.Description = "Do something else...";
            createdCardNo2.Responsible = "You";
            createdCardNo2.Requester   = "Me";

            createdSection.Add(createdCardNo2);

            createdBoard.Delete();

        }
    }
}
