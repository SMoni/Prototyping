using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private Dictionary<Type, Func<Object, SerializedData>> _serializeMethods;

        private Dictionary<Type, Func<Object, SerializedData>> SerializeMethods
        {
            get
            {
                return _serializeMethods ?? (_serializeMethods = new Dictionary<Type, Func<Object, SerializedData>>
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

        private Dictionary<Type, Func<SerializedData, Object>> _deserializeMethods;

        private Dictionary<Type, Func<SerializedData, Object>> DeserializeMethods
        {
            get
            {
                return _deserializeMethods ?? (_deserializeMethods = new Dictionary<Type, Func<SerializedData, Object>>
                {
                      { typeof(Card),    DeserializeCard }
                    , { typeof(Section), DeserializeSection }
                    , { typeof(Board),   DeserializeBoard }
                });
            }
        }

        public T Deserialize<T>(SerializedData toDeserialize)
        {
            var typeOfT = typeof(T);

            var firstEntry = toDeserialize.FirstOrDefault();

            if (firstEntry == null)
                throw new ArgumentNullException();

            if (!firstEntry.ContainsKey("Type"))
                throw new ArgumentException();

            var typeName = firstEntry["Type"];

            if (typeOfT.Name != typeName)
                throw new ArgumentException();

            if (!DeserializeMethods.ContainsKey(typeOfT))
                throw new NotImplementedException();

            return (T)DeserializeMethods[typeOfT](toDeserialize);
        }

        private Board DeserializeBoard(SerializedData toDeserialize)
        {
            var result = _factoryMethods[typeof(Board)]() as Board;

            return result;
        }

        private Section DeserializeSection(SerializedData toDeserialize)
        {
            var result = _factoryMethods[typeof(Section)]() as Section;

            return result;
        }

        private Card DeserializeCard(SerializedData toDeserialize)
        {
            var dataForCard = toDeserialize.FirstOrDefault();

            if (dataForCard == null)
                throw new ArgumentNullException();
            
            var result = _factoryMethods[typeof(Card)]() as Card;

            if (result == null)
                throw new ArgumentNullException();

            GetFieldInfoBy("Id",          result.GetType()).SetValue(result, Guid.Parse(dataForCard["Id"]));
            GetFieldInfoBy("Created",     result.GetType()).SetValue(result, DateTime.Parse(dataForCard["Created"]));
            GetFieldInfoBy("LastUpdated", result.GetType()).SetValue(result, DateTime.Parse(dataForCard["LastUpdated"]));
            GetFieldInfoBy("Description", result.GetType()).SetValue(result, dataForCard["Description"]);
            GetFieldInfoBy("Requester",   result.GetType()).SetValue(result, dataForCard["Requester"]);
            GetFieldInfoBy("Responsible", result.GetType()).SetValue(result, dataForCard["Responsible"]);

            return result;
        }

        protected static String       MEMBER_PREFIX = "_";
        protected static BindingFlags MEMBER_BINDINGFLAGS = BindingFlags.Instance |
                                                              BindingFlags.NonPublic;

        protected static BindingFlags PROPERTY_BINDINGFLAGS = BindingFlags.Instance |
                                                              BindingFlags.Public |
                                                              BindingFlags.FlattenHierarchy;

        protected virtual FieldInfo GetFieldInfoBy(String key, Type thisType)
        {

            FieldInfo result = null;

            while ((thisType != null) && (result == null))
            {
                result = thisType.GetField(MEMBER_PREFIX + key.ToLower(), MEMBER_BINDINGFLAGS);
                thisType = thisType.BaseType;
            }

            return result;

        }

    }
}
