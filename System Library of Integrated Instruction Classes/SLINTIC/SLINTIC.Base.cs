using System;
using System.Linq;
using SLINTIC.Math;
using SLINTIC.Exceptions;
using System.Diagnostics.Contracts;
using System.CodeDom;

namespace SLINTIC
{
    /// <summary>
    /// Boolean event-based class that asynchronously invokes a given method delegate when the given condition is true
    /// </summary>
    public class ConditionalInvoker
    {
        /// <summary>
        /// Creates a new ConditionalInvoker instance
        /// </summary>
        public ConditionalInvoker(bool condition, Action method)
        {
            this.condition = condition;
            this.method = method;
        }

        /// <summary>
        /// Starts asynchronous boolean event
        /// </summary>
        public async void StartAwait()
        {
            running = true;
            while (condition ^ running) // legit the first time I've used XOR
            {
                await Task.Yield();
            }
            if (!running) return;
            method();
        }

        /// <summary>
        /// Cancels the asynchronous boolean event
        /// </summary>
        public void BreakOp()
        {
            running = false;
        }

        public bool condition;
        public Action method;
        private bool running;
    }

    public static class Extensions
    {
        /// <summary>
        /// Sets a StringPointer at an index location
        /// </summary>
        /// <returns>A new StringPointer at the indexed location</returns>
        public static StringPointer Point(this string str, int index)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index <= str.Length && index >= 0);
            char[] c = str.ToCharArray();
            return new StringPointer(str, index, c[index]);
        }

        /// <summary>
        /// Removes a pointed character from a string
        /// </summary>
        /// <returns>A new string with the pointed char removed</returns>
        public static string Pluck(this StringPointer str, int ind)
        {
            List<char> c = str.container.ToCharArray().ToList();
            c.RemoveAt(ind);
            return c.ToArray().Concatenate().ToString();
        }

        /// <summary>
        /// Takes a StringPointer, replaces its value, and returns a new string
        /// </summary>
        /// <returns>A new string with pointer index value replaced</returns>
        public static string Impose(this StringPointer ptr, char val)
        {
            char[] b = ptr.container.ToCharArray();
            b[ptr.index] = val;
            return b.Concatenate().ToString();
        }

        // example 5.Destructure() == int[0, 1, 2, 3, 4, 5]
        /// <returns>An array of all real numbers within the given value</returns>
        public static int[] Destructure(this object num)
        {
            Contract.Requires<MathOperationException>(num is double || num is float || num is int && num != null, "SLINTIC Destructure Error: Cannot destructure a value that is not a number");
            float result = (float)num;
            if (result.ToString().Contains(".")) result = result.DeEntroponize();
            List<int> output = new();
            int iterator = 0;
            while (result != 0) { output.Add(iterator); iterator++; result--; }
            return output.ToArray();
        }

        /// <returns>All values of an array joined into a single value</returns>
        public static object Concatenate(this object[] t)
        {
            string output = "";
            foreach (object o in t) { output += o.ToString(); }
            return output;
        }

        // overload galore
        /// <returns>All values of an array joined into a single value</returns>
        public static int Concatenate(this int[] t) => Concatenate(t);
        /// <returns>All values of an array joined into a single value</returns>
        public static double Concatenate(this double[] t) => Concatenate(t);
        /// <returns>All values of an array joined into a single value</returns>
        public static float Concatenate(this float[] t) => Concatenate(t);
        /// <returns>All values of an array joined into a single value</returns>
        public static string Concatenate(this string[] t) => Concatenate(t);
        /// <returns>All values of an array joined into a single value</returns>
        public static char Concatenate(this char[] t) => Concatenate(t);
    }

    /// <summary>Represents a pointer to a character at the index of a string</summary>
    public struct StringPointer
    {
        /// <returns>A pointer to a character at the index of a string</returns>
        public StringPointer(string container, int index, char character)
        {
            this.container = container;
            this.index = index;
            this.character = character;
        }

        /// <summary>
        /// The string literal the pointer is attached to
        /// </summary>
        public readonly string container;
        /// <summary>
        /// The position in the string literal that is being pointed at=
        /// </summary>
        public readonly int index;
        /// <summary>
        /// The character that is being pointed
        /// </summary>
        public readonly char character;
    }
}