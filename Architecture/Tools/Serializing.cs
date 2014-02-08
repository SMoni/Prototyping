using System;
using System.Collections.Generic;
using System.Linq;

namespace Architecture.Tools 
{
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

        public String[] Serialize<T>(T toSerialize)
        {
            return null;
        }

        public T Deserialize<T>(String[] toDeserialize)
        {
            return default(T);
        }

    }
}
