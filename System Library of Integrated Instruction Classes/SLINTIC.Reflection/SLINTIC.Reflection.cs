using System.Linq;
using System.Reflection;
using SLINTIC.Exceptions;
using System.Collections.Generic;

// imagine adding protection levels for objects just to add reflection
namespace SLINTIC.Reflection
{
    public static class Reflection
    {
        internal static Dictionary<string, object> ImposedFields = new();
        internal static Dictionary<string, object> ImposedFieldInstances = new ();

        ///<summary>
        ///Retrieves MethodBase from the specified method
        ///</summary>
        public static MethodBase? ReturnMethodBase<T>(string methodName) => typeof(T).GetMethod(methodName);
        //Overload 1 for BindingFlags
        public static MethodBase? ReturnMethodBase<T>(string methodName, BindingFlags flags) => typeof(T).GetMethod(methodName, flags);

        ///<summary>
        /// Retrieves MethodInfo from the specified method
        ///</summary>
        public static MethodInfo? ReturnMethodInfo<T>(string methodName) => typeof(T).GetMethod(methodName);
        //Overload 1 for BindingFlags
        public static MethodInfo? ReturnMethodInfo<T>(string methodName, BindingFlags flags) => typeof(T).GetMethod(methodName, flags);

        ///<summary>
        ///Accesses the given method and invokes it
        ///</summary>
        public static void GetMethodAndInvoke<T>(string methodName, object? instance, object[]? parameters)
        {
            MethodInfo? method = typeof(T).GetMethod(methodName);
            if (method is null) return;
            method.Invoke(instance, parameters);
        }
        // Overload 1 for BindingFlags
        public static void GetMethodAndInvoke<T>(string methodName, object? instance, object[]? parameters, BindingFlags flags)
        {
            MethodInfo? method = typeof(T).GetMethod(methodName, flags);
            if (method is null) return;
            method.Invoke(instance, parameters);
        }

        ///<summary>
        /// Gets requested field and replaces its value
        ///</summary>
        public static void Impose<T>(string fieldName, object? instance, object? fieldImport)
        {
            object? value = typeof(T).GetField(fieldName)?.GetValue(instance);
            typeof(T).GetField(fieldName)?.SetValue(instance, fieldImport);

            ImposedFields.Add(fieldName, value);
            ImposedFieldInstances.Add(fieldName, instance);
        }
        // Overload 1 for BindingFlags
        public static void Impose<T>(string fieldName, BindingFlags flags, object? instance, object? fieldImport)
        {
            typeof(T).GetField(fieldName, flags)?.SetValue(instance, fieldImport);
        }

        /// <summary>
        /// Returns a previously Imposed value to it's original value
        /// </summary>
        public static void Revoke(string fieldName)
        {
            var field = from Field in ImposedFields where Field.Key == fieldName select Field;

            if (field is null)
            {
                throw new RevokeNonImposedMemberException("Revoke is not applicable to non-imposed members");
            }

            var instance = from Instance in ImposedFieldInstances where Instance.Key == fieldName select Instance;

            if (instance is null) { throw new RevokeUndefinedImposedFieldInstanceException("Imposed field cannot be revoked because it's instance, or the instance it was imposed from, is null."); }
        }
    }
}