namespace SLINTIC.Math
{
    public class Math
    {
        ///<summary>
        /// Low latency floating point inverse square root operation for calculating normalized vectors in a 3D environment
        ///</summary>
        public static unsafe float FastInvSqrt(float number)
        {   // we all know this one
            long i;
	        float x2, y;
	        const float threehalfs = 1.5F;

            x2 = number * 0.5F;
            y  = number;
            i = * ( long * ) &y; // initiate memory fuck
            i = 0x5f3759df - (i >> 1); // hell
            y = * ( float * ) &i;
            y  = y * ( threehalfs - ( x2 * y * y ) );
            return y;
        }
    }
}