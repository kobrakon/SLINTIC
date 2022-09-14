using System.ComponentModel;
using SLINTIC.Logger;
using SLINTIC.Exceptions;
using System.Diagnostics.Contracts;

namespace SLINTIC.Math
{
    public static class Math
    {
        internal static System.Timers.Timer TimeOutTimer = new System.Timers.Timer(30000); // Timeout for while() statements to prevent endless calculation

        /// <summary>
        /// Low latency floating point inverse square root operation for calculating normalized vectors in a 3D environment
        /// </summary>
        [Description("Low latency floating point inverse square root operation for calculating normalized vectors in a 3D environment"),Category("Vector Calculations")]
        public static unsafe float FastInvSqrt(float number)
        {   // we all know this one
            long i;
            float x2, y;
            const float threehalfs = 1.5F;

            x2 = number * 0.5F;
            y = number;
            i = * ( long * ) &y; // initiate memory fuck
            i = 0x5f3759df - (i >> 1); // hell
            y = * ( float * ) &i;
            y = y * (threehalfs - (x2 * y * y));
            return y;
        }

        /// <returns>
        /// The square root of a given number
        /// </returns>
        public static float SqrtOf(float number)
        {
            if (number.ToString().StartsWith("-"))
            {
                throw new MathOperationException($"SLINTIC.Math Invalid Math Operation: The square root of a negative ({number}) was requested, which will always return undefined.");
            }
            return number.ToPowerOf(0.5f);
        }

        /// <returns>An integer equivalent to the floating point value with the decimal places removed to negate calculative entropy</returns>
        public static int DeEntroponize(this float num) => num.ToString().Split(".")[0].Cast<int>().ToArray().Concatenate();

        // Overload 1: float extension
        /// <returns>
        /// The square root of a given number
        /// </returns>
        public static float Sqrt(this float number) => SqrtOf(number);

        /// <summary>
        /// High-precision floating point rounding calculation
        /// </summary>
        /// <returns>Floating point value rounded to the most precise position</returns>
        public static float Round(float number)
        {   // wow turns out trying to teach a computer how to round floating point values really sucks
            if (!number.ToString().Contains(".")) return RoundWhole(number);

            string[] nstring = number.ToString().Split("."); // gets the number in the parameter and splits it into 2 strings divided by the decimal 
            List<int> leftsplit = nstring[0].ToCharArray().Cast<int>().ToList(); // using lists so I can remove entries
            List<int> rightsplit = nstring[1].ToCharArray().Cast<int>().ToList();
            int shift = 1;

            // example 345.6834
            if (rightsplit[0] >= 5)
            {
                return leftsplit.ToArray().Concatenate() + 1; // returns 346
            } else if (rightsplit[0] != 0)
            {   // example 345.345
                return leftsplit.ToArray().Concatenate() - 1; // returns 344
            } else
            {   // example : 232.00253
                foreach (int c in rightsplit) // I know I could use a for loop but that would just be slower
                {
                    // if the right of the decimal at the location of c + shift, where c represents the pointer of the char being evaluated, is 0
                    if (rightsplit[shift] == 0)
                    {   // skip
                        shift++;
                        continue;
                    } else if (rightsplit[shift] >= 5) // if the right of the decimal at the location of c + shift is greater or equal to 5
                    {
                        // increment previous number by 1
                        rightsplit[shift - 1]++;
                        // 232.00353
                        // 543.0000055385

                        // seeks numbers after rounded number and removes them | output => 232.003

                        // a bug in C# has resulted in me slowly losing my sanity
                        // (rightsplit.Count - shift - 1).Destructure().ForEach((int i) => rightsplit.RemoveAt(i + shift);
                        // cant do this for SOME fucking reason
                        // even tho Destructure() returns an array ForEach() doesn't recognize it
                        // so for the past fucking hour I've been slamming my head on my desk wondering why it wasn't recognizing
                        // my lambda notation action only to realize that it wasn't recognizing my array
                        // I fucking hate it here
                        Array.ForEach((rightsplit.Count - shift - 1).Destructure(), (int i) => rightsplit.RemoveAt(i + shift)); // I'm genuienly gonna fucking kill someone
                        return (new int[] {leftsplit.ToArray().Concatenate(), rightsplit.ToArray().Concatenate()}).Concatenate();
                    } else // if 0 < n < 5
                    {
                        // if number before shift is 0, skip
                        if (rightsplit[shift - 1] == 0)
                        {
                            shift++;
                            continue;
                        }
                        // if number before shift is not 0, deincrement number
                        rightsplit[shift - 1]--;

                        Array.ForEach((rightsplit.Count - shift - 1).Destructure(), (int i) => rightsplit.RemoveAt(i + shift));
                        return (new int[] {leftsplit.ToArray().Concatenate(), rightsplit.ToArray().Concatenate()}).Concatenate();
                    }
                }
                return (new int[] {leftsplit.Cast<int>().ToArray().Concatenate(), rightsplit.Cast<int>().ToArray().Concatenate()}).Concatenate();
            }
        }

        /// <returns>
        /// An equivalent whole number rounded to the tens position
        /// </returns>
        public static float RoundWhole(float number)
        {
            if (number.ToString().Contains(".")) return Round(number);
            int[] num = number.ToString().ToCharArray().Cast<int>().ToArray();

            if (num[num.Length - 1] >= 5)
            {
                num[num.Length - 2] += 1;
                num[num.Length - 1] = 0;
                return num.Concatenate();
            } else { num[num.Length - 1] = 0; return num.Concatenate(); }
        }

        // Overload 1 : float extension
        /// <returns>
        /// An equivalent floating point value rounded to the most precise position
        /// </returns>
        public static float Rounded(this float number) => Round(number);

        // floating point extension
        /// <summary>
        /// Low latency floating point inverse square root operation for calculating normalized vectors in a 3D environment
        /// </summary>
        [Description("Low latency floating point inverse square root operation for calculating normalized vectors in a 3D environment"),Category("Vector Calculations")]
        public static float ToInvSqrt(this float number) => FastInvSqrt(number);

        /// <returns>
        /// The result of a given number to the power of an exponent
        /// </returns>
        [Description("Calculates the result of a given number to the power of an exponent"),Category("Arithmetic")]
        public static float ToPower(this float baseNum, float exponent)
        {
            if (exponent is 1) return baseNum;
            float result = 1;
            baseNum.Destructure().ToList().ForEach((_) => result *= baseNum);
            return result;
        }

        // Float extension
        /// <summary>
        /// Calculates the result of a given number to the power of an exponent
        /// </summary>
        [Description("Calculates the result of a given number to the power of an exponent"),Category("Arithmetic")]
        public static float ToPowerOf(this float baseNum, float exponent) => ToPower(baseNum, exponent);
    }

    public struct Coord2
    {
        /// <summary>
        /// Value consisting of 2 floating point numbers to represent locations in a 2D environment (X,Y)
        /// </summary>
        [Description("Value consisting of 2 floating point numbers to represent locations in a 2D environment (X,Y)"),Category("Algebra")]
        public Coord2(float x, float y, string tag, C2Env? env)
        {
            this.x = x;
            this.y = y;
            this.tag = tag;
            this.env = env;
        }

        /// <summary>
        /// Returns the geometric distance between 2 Coord2 values in a 2D environment
        /// </summary>
        public float Distance(Coord2 nbase, Coord2 ncomparative) => ((ncomparative.x - nbase.x).ToPowerOf(2) + (ncomparative.y - nbase.x).ToPowerOf(2)).Sqrt();

        /// <summary>
        /// Returns the geometric midpoint between 2 Coord2 values
        /// </summary>
        public Coord2 Midpoint(Coord2 nbase, Coord2 ncomparative)
        {
            float xdiff = (ncomparative.x - nbase.x);
            float ydiff = (ncomparative.y - nbase.y);
            return new Coord2(xdiff/2, ydiff/2, $"MPOF{nbase} TO {ncomparative}", nbase.env);
        }

        /// <summary>
        /// ID to differenciate Coord2 instances
        /// </summary>
        public string tag;
        [Description("Represents the relative X coordinate (left/right)"),Category("2D Positioning")]
        public float x;
        [Description("Represents the relative Y coordinate (up/down)"),Category("2D Positioning")]
        public float y;
        /// <summary>
        /// Represents the geometric environment the Coord2 value is in
        /// </summary>
        public C2Env? env;
    }

    public struct Coord3
    {
        /// <summary>
        /// Value consisting of 3 floating point numbers to represent locations in a 3D environment (X,Y,Z)
        /// </summary>
        [Description("Value consisting of 3 floating point numbers to represent locations in a 3D environment (X,Y,Z)"),Category("3D Positioning")]
        public Coord3(float x, float y, float z, string tag)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.tag = tag;
        }

        /// <summary>
        /// Returns the geometric distance between 2 Coord3 values in a 3D environment
        /// </summary>
        public float Distance(Coord3 nbase, Coord3 ncomparative) => ((ncomparative.x - nbase.x).ToPowerOf(2) + (ncomparative.y - nbase.y).ToPowerOf(2) + (ncomparative.z - nbase.z).ToPowerOf(2)).Sqrt();

        /// <summary>
        /// ID to differenciate Coord3 instances
        /// </summary>
        public string tag;
        /// <summary>
        /// Represents the relative X coordinate (left/right)
        /// </summary>
        [Description("Represents the relative X coordinate (left/right)"),Category("3D Positioning")]
        public float x;
        /// <summary>
        /// Represents the relative Y coordinate (near/far)
        /// </summary>
        [Description("Represents the relative Y coordinate (near/far)"),Category("3D Positioning")]
        public float y;
        /// <summary>
        /// Represents the relative Z coordinate (height)
        /// </summary>
        [Description("Represents the relative Z coordinate (height)"),Category("3D Positioning")]
        public float z;
        /// <summary>
        /// Represents the true center of the dimentional plane
        /// </summary>
        [Description("Represents the true center of a dimentional plane"),Category("3D Positioning")]
        public static Coord3 Origin = new Coord3(0, 0, 0, "Origin");
    }

    public struct C2Env
    {
        /// <summary>
        /// Represents an instance of a 2D geometric environment
        /// </summary>
        public C2Env(Coord2[] initialpoints, string tag)
        {
            this.tag = tag;
            var instance = this.points;
            instances.Add(this);
            initialpoints.ToList().ForEach((Coord2 point) => instance.Add(point));
        }

        /// <summary>
        /// Places a Coord2 instance within a C2Env instance
        /// </summary>
        public void Cast(Coord2 point)
        {
            if (this.points.Contains(point)) LocalConsole.Log($"SLINTIC.Math C2Env Warning: Ignored request to cast point {point} to instance {tag} because it already exists.");
            this.points.Add(point);
        }

        /// <summary>
        /// Removes a Coord2 instance from a C2Env instance
        /// </summary>
        public void Destroy(Coord2 point)
        {
            if (!this.points.Contains(point)) LocalConsole.Log($"SLINTIC.Math C2Env Warning: Ignored request to remove point {point} from instance {tag} because it doesn't exist.");
            this.points.Remove(point);
            LocalConsole.Log($"SLINTIC.Math C2Env: Removed point {point} from {this.tag}");
        }

        public List<Coord2> points = new List<Coord2>();
        public string tag;
        public static List<object> instances = new List<object>();
    }

    public struct Vector
    {
        /// <summary>
        /// 3-point value that represents a vector force acting on an object
        /// </summary>
        public Vector(float xforce, float yforce, float decay)
        {
            Contract.Requires<ArgumentOutOfRangeException>(decay < 1, "SLINTIC.Math Vector Error: Cannot define Vector, decay value cannot be greater than 1.");
            this.xforce = xforce;
            this.yforce = yforce;
            this.decay = decay;
        }

        public float xforce;
        public float yforce;
        public float decay;
    }

    public struct Slope
    {
        /// <summary>
        /// 2-point value representative of the vertical and horizontal steps between different Coord2 vectors
        /// </summary>
        [Description("2-point value representative of the vertical and horizontal steps between different Coord2 vectors"),Category("Algebra")]
        public Slope(int rise, int run)
        {
            this.rise = rise;
            this.run = run;
        }

        /// <summary>
        /// Calculates the vertical and horizontal steps between different Coord2 vectors and returns them as a 2-point Slope value
        /// </summary>
        [Description("Calculates the vertical and horizontal steps between different Coord2 vectors and returns them as a 2-point Slope value"),Category("Algebra")]
        public Slope CalcSlope(Coord2 baseC, Coord2 comparativeC)
        {
            Slope result = new Slope();

            if (baseC.x == comparativeC.x)
            {
                result.run = 0;
                if (baseC.y == comparativeC.y)
                {
                    result.rise = 0;
                    return result;
                }

                while(baseC.y != comparativeC.y)
                {
                    baseC.y++;
                }
                result.rise = baseC.y;
                return result;
            } else if (baseC.y == comparativeC.y)
            {
                result.rise = baseC.y;

                while (baseC.x != comparativeC.x)
                {
                    baseC.x++;
                }
                result.run = baseC.x;
                return result;
            } else 
            {
                while (baseC.x != comparativeC.x)
                {
                    baseC.
                }
            }
        }

        /// <summary>
        /// Represents the amount of vertical shift between 2 Y coordinates
        /// </summary>
        [Description("Represents the amount of vertical shift between 2 Y coordinates"),Category("Algebra")]
        public float rise;
        /// <summary>
        /// Represents the amount of horizontal shift between 2 X coordinates
        /// </summary>
        [Description("Represents the amount of horizontal shift between 2 X coordinates"),Category("Algebra")]
        public float run;
    }
}