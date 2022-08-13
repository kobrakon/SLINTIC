using System.Reflection;

// imagine adding protection levels for objects just to add reflection
namespace SLINTIC.Reflection
{
    public class Reflection
    {
        ///<summary>
        ///Retrieves MethodBase from the specified method
        ///</summary>
        public static MethodBase ?ReturnMethodFromClass<T>(string methodName) { return typeof(T).GetMethod(methodName); }
        //Overload 1 for BindingFlags
        public static MethodBase ?ReturnMethodFromClass<T>(string methodName, BindingFlags flags) { return typeof(T).GetMethod(methodName, flags); }

        ///<summary>
        ///Accesses the given method and invokes it
        ///</summary>
        public static void GetMethodAndInvoke<T>(string methodName, object ?instance, object[] ?parameters)
        {
            MethodInfo ?method = typeof(T).GetMethod(methodName);
            if (method is null) return;
            method.Invoke(instance, parameters);
        }
        // Overload 1 for BindingFlags
        public static void GetMethodAndInvoke<T>(string methodName, object ?instance, object[] ?parameters, BindingFlags flags)
        {
            MethodInfo ?method = typeof(T).GetMethod(methodName, flags);
            if (method is null) return;
            method.Invoke(instance, parameters);
        }
    }
}