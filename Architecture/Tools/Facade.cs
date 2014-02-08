using System;
using System.Collections.Generic;
using Architecture.Core;

namespace Architecture.Tools
{
    public class Facade
    {
       private static volatile Facade _instance;
       private static readonly object SyncRoot = new Object();

       private Facade() { }

       public static Facade Instance
       {
          get 
          {
             if (_instance == null) 
             {
                lock (SyncRoot) 
                {
                   if (_instance == null)
                       _instance = new Facade();
                }
             }

             return _instance;
          }
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


        public T Create<T>()
        {
            if(!FactoryMethods.ContainsKey(typeof(T)))
                throw new NotImplementedException();

            return (T)FactoryMethods[typeof(T)]();
        }

        public T Load<T>(Guid id)
        {
            if (!FactoryMethods.ContainsKey(typeof(T)))
                throw new NotImplementedException();

            var result = Instance.Create<T>();

            //Load via Repository

            return result;
        }

        public void Save<T>(T toSave)
        {
            if (!FactoryMethods.ContainsKey(typeof(T)))
                throw new NotImplementedException();
    
            //Save via Repository

        }

        public void Delete<T>(T toDelete)
        {
            if (!FactoryMethods.ContainsKey(typeof(T)))
                throw new NotImplementedException();

            //Delete via Repository
        }

    }

    internal static class ExtensionMethods
    {
        public static void Save<T>(this T toSave)
        {
            Facade.Instance.Save(toSave);
        }

        public static void Delete<T>(this T toDelete)
        {
            Facade.Instance.Delete(toDelete);
        }

    }

}
