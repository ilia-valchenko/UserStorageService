using System;
using System.Reflection;

namespace ServiceApplication
{
    public class Proxy : MarshalByRefObject
    {
        object CallInternal(string dll, string typename, object instance, string method, object[] parameters)
        {
            Assembly a = Assembly.LoadFile(dll);
            Type t = a.GetType(typename);
            MethodInfo m = t.GetMethod(method);
            return m.Invoke(instance, parameters);
        }

        public static object Call(AppDomain domain, string dll, string typename, object instance, string method, params object[] parameters)
        {
            Proxy proxy = (Proxy)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(Proxy).FullName);
            object result = proxy.CallInternal(dll, typename, instance, method, parameters);
            return result;
        }
    }
}
