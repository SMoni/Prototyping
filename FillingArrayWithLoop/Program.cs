using System;
using System.Linq;

namespace FillingArrayWithLoop {
    class Program {
        static void Main() {

            const int numberOfColumns = 10;
            const int numberofRows = 10;
            const int numberOfPlanes = 2;
            const int numberOfCuboids = 2;
            const int numberOfHyperCuboids = 2;

            var myArray = new int[
                numberOfHyperCuboids,
                numberOfCuboids,
                numberOfPlanes,
                numberofRows,
                numberOfColumns
            ];

            var index = 0;

            Fill(myArray, () => index++);
        }

        private static void Fill<T>(Array thisArray, Func<T> withThat) {
            var rank = thisArray.Rank;
            var indices = new int[rank];
            var dimensions = new int[rank + 1];
            var lengthOfDimensions = dimensions.Length;

            dimensions[0] = 1;

            for (var index = 1; index < dimensions.Length; index++) {
                dimensions[index] = dimensions[index - 1] * thisArray.GetLength(rank - index);
            }

            var numberOfElements = dimensions.Last();

            for (var index = 0; index < numberOfElements; index++) {
                var indexForDimension = lengthOfDimensions - 1;
                var parameter = 0;

                while (indexForDimension > 0) {
                    indices[parameter++] = index % dimensions[indexForDimension--] / dimensions[indexForDimension];
                }

                thisArray.SetValue(withThat(), indices);

            }
        }
    }
}
