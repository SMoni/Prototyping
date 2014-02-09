using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Core;

namespace Architecture.Tools 
{
    public class SerializedData : List<Dictionary<String, String>> { }

    class Serializing
    {
        private readonly IDictionary<Type, Func<Object>> _factoryMethods;

        public Serializing(IDictionary<Type, Func<Object>> factoryMethods)
        {
            _factoryMethods = factoryMethods;
        }

        private Func<Object> FindFactoryMethodBy(String typeAsString)
        {
            return _factoryMethods.FirstOrDefault(pair => pair.Key.Name == typeAsString).Value;
        }

        private Dictionary<Type, Func<Object, SerializedData>> _serializeMethods;

        private Dictionary<Type, Func<Object, SerializedData>> SerializeMethods
        {
            get
            {
                return _serializeMethods ?? (_serializeMethods = new Dictionary<Type, Func<object, SerializedData>>
                {
                      { typeof(Card),    o => Serialize(o as Card) }
                    , { typeof(Section), o => Serialize(o as Section) }
                    , { typeof(Board),   o => Serialize(o as Board) }
                });
            }
        }

        public SerializedData Serialize<T>(T toSerialize)
        {
            var typeOf = toSerialize.GetType();

            if (!SerializeMethods.ContainsKey(typeOf))
                throw new NotImplementedException();

            return SerializeMethods[typeOf](toSerialize);
        }

        private SerializedData Serialize(Board toSerialize)
        {
            if (toSerialize == null)
                return null;

            var result = new SerializedData
            {
                new Dictionary<string, string>
                {
                      {"Type",        toSerialize.GetType().Name}
                    , {"Id",          toSerialize.Id.ToString()}
                    , {"LastUpdated", toSerialize.LastUpdated.ToString("o")}
                }
            };

            return result;
        }

        private SerializedData Serialize(Section toSerialize)
        {
            if (toSerialize == null)
                return null;

            var result = new SerializedData
            {
                new Dictionary<string, string>
                {
                      {"Type",                toSerialize.GetType().Name}
                    , {"Id",                  toSerialize.Id.ToString()}
                    , {"LastUpdated",         toSerialize.LastUpdated.ToString("o")}
                    , {"Name",                toSerialize.Name}
                    , {"WorkInProgressLimit", toSerialize.WorkInProgressLimit.ToString("0")}
                }
            };

            return result;
        }

        private SerializedData Serialize(Card toSerialize)
        {
            if (toSerialize == null)
                return null;

            var result = new SerializedData
            {
                new Dictionary<string, string>
                {
                      {"Type",        toSerialize.GetType().Name}
                    , {"Id",          toSerialize.Id.ToString()}
                    , {"Created",     toSerialize.Created.ToString("o")}
                    , {"LastUpdated", toSerialize.LastUpdated.ToString("o")}
                    , {"Description", toSerialize.Description}
                    , {"Requester",   toSerialize.Requester}
                    , {"Responsible", toSerialize.Responsible}
                }
            };

            return result;

        }

        public T Deserialize<T>(String[] toDeserialize)
        {
            return default(T);
        }

    }
}
