using System;

namespace Architecture.Tools
{
    class Repository
    {
        private readonly Serializing _serializer;

        public Repository(Serializing serializer)
        {
            _serializer = serializer;
        }

        public T Load<T>(Guid id)
        {
            //Load data as String[] from whereever
            var data = new String[] {};

            return _serializer.Deserialize<T>(data);
        }

        public void Save<T>(T toSave)
        {
            var data = _serializer.Serialize(toSave);

            //Save data as String[] from whereever
        }

        public void Delete<T>(T toDelete)
        {
            //Do some deleting magic
        }

    }
}
