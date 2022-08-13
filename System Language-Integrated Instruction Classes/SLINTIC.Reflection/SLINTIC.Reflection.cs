using System.Reflection;

// imagine adding protection levels for objects just to add reflection
namespace SLINTIC.Reflection
{
    public static class Reflection
    {
        ///<summary>
        ///Retrieves MethodBase from the specified method
        ///</summary>
        public static MethodBase ?ReturnMethodBase<T>(string methodName) => typeof(T).GetMethod(methodName);
        //Overload 1 for BindingFlags
        public static MethodBase ?ReturnMethodBase<T>(string methodName, BindingFlags flags) => typeof(T).GetMethod(methodName, flags);

        ///<summary>
        /// Retrieves MethodInfo from the specified method
        ///</summary>
        public static MethodInfo ?ReturnMethodInfo<T>(string methodName) => typeof(T).GetMethod(methodName);
        //Overload 1 for BindingFlags
        public static MethodInfo ?ReturnMethodInfo<T>(string methodName, BindingFlags flags) => typeof(T).GetMethod(methodName, flags);

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

        ///<summary>
        /// Gets requested field and replaces its value
        ///</summary>
        public static void Impose<T>(string fieldName, object ?instance, object ?fieldImport) => typeof(T).GetField(fieldName)?.SetValue(instance, fieldImport);
        // Overload 1 for BindingFlags
        public static void Impose<T>(string fieldName, BindingFlags flags, object ?instance, object ?fieldImport) => typeof(T).GetField(fieldName, flags)?.SetValue(instance, fieldImport);
    }
}
