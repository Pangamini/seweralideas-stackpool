#if UNITY_5_3_OR_NEWER
#define UNITY
using UnityEngine;
#endif

using System;
using System.Collections.Generic;


namespace SeweralIdeas.Pooling
{

    public class StackPoolLibrary
    {
        [ThreadStatic]
        private static StackPoolLibrary s_instance;
        private static Dictionary<Type, Type> s_cachedPoolTypes = new Dictionary<Type, Type>();
        private static object s_sharedLock = new object();
        private Dictionary<Type, StackPoolBase> m_pools = new Dictionary<Type, StackPoolBase>();

        private static Dictionary<Type, Type> s_directPoolTypes = new Dictionary<Type, Type>();
        private static Dictionary<string, Type> s_genericPoolTypes = new Dictionary<string, Type>();

        private StackPoolLibrary()
        {
        }

        static StackPoolLibrary()
        {
            ///search through the entire domain for possible stackPool types
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var baseType = typeof(StackPoolBase);
            var baseGenDef = typeof(StackPool<>);
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (System.Reflection.ReflectionTypeLoadException e)
                {
                    Console.Error.Write(e.Message);
                    continue;
                }
                foreach (var type in types)
                {
                    try
                    {
                        if (!baseType.IsAssignableFrom(type))
                            continue;
                        if (type.IsAbstract)
                            continue;

                        Type innerType = null;
                        if (type.IsGenericTypeDefinition)
                        {
                            var searchType = type.BaseType;
                            while (searchType != null)
                            {
                                if (searchType.IsGenericTypeDefinition) continue;
                                var searchGenDef = searchType.GetGenericTypeDefinition();
                                if (searchGenDef == baseGenDef)
                                {
                                    innerType = searchType.GetGenericArguments()[0];
                                    break;
                                }
                                searchType = searchType.BaseType;
                            }

                            if (innerType == null)
                            {
                                Console.Error.WriteLine(string.Format("Failed to register StackPool of type {0}", type));
                            }
                            else
                            {
                                s_genericPoolTypes.Add(innerType.Name, type);
                            }
                        }

                        //non-generics
                        else
                        {
                            var instance = (StackPoolBase)System.Activator.CreateInstance(type);
                            innerType = instance.GetElementType();

                            if (innerType == null)
                            {
                                Console.Error.WriteLine(string.Format("Failed to register StackPool of type {0}", type));
                            }
                            else
                            {
                                s_directPoolTypes.Add(innerType, type);
                            }
                        }
                    }
                    catch (Exception e)
                    { Console.Error.WriteLine(e); }
                }
            }
        }

        public StackPool<T> FindPool<T>() where T : class
        {
            StackPoolBase pool;
            if (!m_pools.TryGetValue(typeof(T), out pool))
            {
                Type poolType = FindPoolType(typeof(T));
                if (poolType == null)
                    throw new System.InvalidOperationException(string.Format("Cannot find StackPool type for {0}", typeof(T).ToString()));
                pool = (StackPoolBase)System.Activator.CreateInstance(poolType);
            }

            return (StackPool<T>)pool;
        }

        private static Type FindPoolType(Type objType)
        {
            lock (s_sharedLock)
            {
                Type poolType;
                if (s_cachedPoolTypes.TryGetValue(objType, out poolType))
                    return poolType;

                //Try find direct
                s_directPoolTypes.TryGetValue(objType, out poolType);
                if (poolType == null)
                {
                    var genTypeDef = objType.GetGenericTypeDefinition();
                    if (s_genericPoolTypes.TryGetValue(genTypeDef.Name, out poolType))
                    {
                        var genArgs = objType.GetGenericArguments();
                        poolType = poolType.MakeGenericType(genArgs);
                    }
                }

                s_cachedPoolTypes.Add(objType, poolType);   //cache the result
                return poolType;
            }
        }

        //ensures per-thread instance
        public static StackPoolLibrary GetInstance()
        {
            if (s_instance == null)
                s_instance = new StackPoolLibrary();
            return s_instance;
        }

    }
}