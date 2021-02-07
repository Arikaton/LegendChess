namespace LegendChess
{
    public static class MathfExtensions
    {
        public static int GetSign(int value)
        {
            if (value == 0)
                return 0;
            return value > 0 ? 1 : -1;
        }
    }
}