using System;
using System.Linq;

namespace FillingArrayWithLoop {
    class Program {
        static void Main() {

            const int numberOfColumns      = 2;
            const int numberofRows         = 2;
            const int numberOfPlanes       = 2;
            const int numberOfCuboids      = 2;
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

            var rank               = thisArray.Rank;
            var indices            = new int[rank];
            var dimensions         = GetDimensionsFrom(thisArray);
            var lengthOfDimensions = dimensions.Length;
            var numberOfElements   = dimensions.Last();

            Func<int, int[]> indicesBy = index =>
            {
                var indexForDimension = lengthOfDimensions - 1;
                var parameter         = 0;

                while (indexForDimension > 0) {
                    indices[parameter++] = index % dimensions[indexForDimension--] / dimensions[indexForDimension];
                }

                return indices;
            };

            for (var index = 0; index < numberOfElements; index++) {
                thisArray.SetValue(withThat(), indicesBy(index));
            }
        }

        private static int[] GetDimensionsFrom(Array thisArray)
        {
            var rank   = thisArray.Rank;
            var length = thisArray.Rank + 1;

            var result = new int[length];

            result[0] = 1;

            for (var index = 1; index < length; index++)
            {
                result[index] = result[index - 1] * thisArray.GetLength(rank - index);
            }

            return result;

        }
    }
}
