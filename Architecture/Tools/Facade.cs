using System;

namespace Architecture.Tools {
    public class Facade
    {
        public T Create<T>()
        {
            return default(T);
        }

        public T Load<T>(Guid id)
        {
            return default(T);
        }
    }

    internal static class ExtensionMethods
    {
        public static void Save<T>(this T toSave)
        {

        }

        public static void Delete<T>(this T toDelete)
        {

        }
    }

}
