using System.IO;

namespace RstInvent
{
    internal static class GeneratorId
    {
        public static string Generate()
        {
            string hex;
            using (var sr = new StreamReader("LastGeneratedNumber.txt"))
            {
                hex = sr.ReadToEnd();
            }

            long number = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            number++;

            hex = number.ToString("X24");

            using (var fs = new StreamWriter("LastGeneratedNumber.txt", false))
            {
                fs.Write(hex);
            }

            return hex;
        }
    }
}