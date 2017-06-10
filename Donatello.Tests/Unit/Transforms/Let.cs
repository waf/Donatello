namespace UnitTest
{
    public static class LetTest
    {
        static LetTest()
        {
            Constructors.CreateLet(() =>
                {
                    var x = 5;
                    var y = 4;
                    return (x + y);
                }
            );
        }
    }
}