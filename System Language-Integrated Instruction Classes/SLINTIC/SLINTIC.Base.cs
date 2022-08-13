using System.Reflection;

namespace SLINTIC
{
    public class SLINTIC
    {
        ///<summary>
        ///Constantly evaluates the specified condition and invokes when true
        ///</summary>
        public void InvokeWhen(bool condition, MethodInfo method, object ?instance, object[] ?parameters)
        {
            do
            {
                if (condition) { method.Invoke(instance, parameters); break; } continue;
            } while (!condition);
        }
    }
}