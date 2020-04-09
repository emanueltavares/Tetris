public class MathExt
{
    public static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}