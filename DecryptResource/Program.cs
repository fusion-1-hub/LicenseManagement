var result3 = DecryptResource.DecryptResource.l.Ext(4);


for (int Pnjvg = 0, SKXtE = 0; Pnjvg < 10; Pnjvg++)
{
    SKXtE = result3[Pnjvg];
    SKXtE = ~SKXtE;
    result3 = result3.Substring(0, Pnjvg) + (char)(SKXtE & 0xFFFF) + result3.Substring(Pnjvg + 1);
}
Console.WriteLine(result3);
Console.WriteLine("\npress any key to exit the process...");
Console.ReadKey();

public static class Extensions
{
    public static string Ext(this string str, int n)
    {
        if (String.IsNullOrEmpty(str) || n < 1)
        {
            throw new ArgumentException();
        }
        return string.Concat(Enumerable.Range(0, str.Length / n)
                        .Select(i => char.ConvertFromUtf32(
                            int.Parse(str.Substring(i * n, n), System.Globalization.NumberStyles.HexNumber))));
    }
}