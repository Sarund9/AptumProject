namespace AptumEngine.Core
{
    public unsafe static class AMath
    {


        /// <summary> Fast Inverse Square Root </summary>
        public static float InvSqrt(in float f)
        {
            const float THREE_HALVES = 1.5f;
            const int INV_SQRT = 0x5f3759df;

            long i;
            float y = f;
            float x2 = f * 0.5f;

            i = *(long*)&y;
            i = INV_SQRT - (i >> 1);
            y = *(float*)&i;

            y *= (THREE_HALVES - (x2 * y * y));

            return y;
        }

        //public static float Abs(in float f)
        //{
        //    const float INV = 0b_10000000_00000000_00000000_00000000;
        //    return f & INV;
        //}

        private static void Test(in float f)
        {
            int S = 0b_0000_0000;

            //int sign = f & *(float*)&S;


        }
    }
}
