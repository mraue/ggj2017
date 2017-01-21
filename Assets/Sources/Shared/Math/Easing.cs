namespace CreatingDust.Shared.Math
{
    public static class Easing
    {
        public static float Unity(float x)
        {
            return x;
        }

        public static float QuadIn(float x)
        {
            return x * x;
        }

        public static float QuadOut(float x)
        {
            return 2 * x - x * x;
        }

        public static float QuadInOut(float x)
        {
            return x < 0.5f
                ?  QuadIn(x * 2f) / 2f
                : QuadOut((x - 0.5f) * 2f) / 2f + 0.5f;
        }
    }
}