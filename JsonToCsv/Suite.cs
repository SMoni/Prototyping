using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using Newtonsoft.Json.Linq;
using Ninject.Infrastructure.Language;
using NUnit.Framework;

namespace JsonToCsv
{
    [TestFixture]
    public class Suite
    {
        JArray GetObjects()
        {
            return new JArray
            {
                  GetObject("01")
                , GetObject("02")
            };

        }

        JObject GetObject()
        {
            return GetObject(String.Empty);
        }

        JObject GetObject(String prefix)
        {
            var result = new JObject
            {
                  {"Property01", prefix + "01"}
                , {"Property02", prefix + "02"}
            };

            return result;
        }

        [Test]
        public void PlainTests()
        {
            var objectToSerialize = GetObject();
            var child = GetObject("01");

            objectToSerialize.Add("Children", new JArray
            {
                child
            });

            var expected = new StringBuilder();

            expected.AppendLine("Property01:01;Property02:02;Property01:0101;");
            expected.AppendLine("Property02:0102;");

            var actual = new StringBuilder();

            objectToSerialize.TraverseThrough((@this, leaf) => @this.Properties()
                .Where(property => property.Value is JValue)
                .Map(property =>
                {
                    actual.Append(property.Name + ":" + property.Value + ";");
                    actual.Append(leaf ? "\r\n" : String.Empty);
                })
            );

            Assert.AreEqual(expected.ToString(), actual.ToString());


        }

        [Test]
        public void OneEmptyArray()
        {
            var firstLevel = new JObject
            {
                  {"Level", "01"}
            };

            var secondLevel01 = new JObject
            {
                  {"Level", "01.01"}
            };

            var secondLevel02 = new JObject
            {
                  {"Level", "01.02"}
            };

            var thirdLevel = new JObject
            {
                  {"Level", "01.02.01"}
            };

            firstLevel.Add("Sub", new JArray
            {
                secondLevel01,
                secondLevel02
            });

            secondLevel02.Add("Sub", new JArray
            {
                thirdLevel
            });

            var actual = new StringBuilder();

            firstLevel.TraverseThrough((@this, leaf) => @this.Properties()
                .Where(property => property.Value is JValue)
                .Map(property =>
                {
                    actual.Append(property.Name + ":" + property.Value + ";");
                    actual.Append(leaf ? "\r\n" : String.Empty);
                })
            );

            Console.WriteLine(actual.ToString());

        }

    }

    public static class JsonNetExtensions
    {
        public delegate void DoThat(JObject withThis, Boolean untilIsLeaf);

        public static void TraverseThrough(this JObject thisObject, DoThat doingThat)
        {
            (new JArray { thisObject }).TraverseThrough(doingThat);
        }

        public static void TraverseThrough(this IEnumerable<JToken> thisArray, DoThat doingThat)
        {
            thisArray.TraverseThrough(() => { /* First Level... nothing to do... */ }, doingThat);
        }

        private static void TraverseThrough(this IEnumerable<JToken> thisArray, Action doThisBefore, DoThat doingThat)
        {
            const bool isLeaf = true;
            
            Func<JProperty, Boolean> isValid = property => property.Value is JArray;

            thisArray.Map(entry =>
            {
                var asJObject = entry as JObject;

                if (asJObject == null)
                    return;

                Action<Boolean> test = b =>
                {
                    doThisBefore();
                    doingThat(asJObject, b);
                };

                var arrays = asJObject.Properties()
                    .Where(isValid)
                    .Select(property => property.Value)
                    .Cast<JArray>()
                    .ToArray();

                Action doThis = ()=> test(isLeaf);

                if (arrays.Length == 0)
                    doThis();

                Action doThisHereBefore = () => test(!isLeaf);

                arrays.Map(thatArray =>
                {
                    if (thatArray.Count < 1)
                        doThis();
                    else
                        thatArray.TraverseThrough(
                            doThisHereBefore,
                            doingThat
                            );
                });

            });
        }

    }
}
