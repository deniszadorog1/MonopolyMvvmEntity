using System.Text;

namespace MonopolyDLL.Services
{
    public static class MoneyConvertingService
    {
        public static string GetConvertedStringWithoutLastK(int price)
        {
            string str = GetConvertedPrice(price);
            str = str.Remove(str.Length - 1);
            return str;
        }

        public static string GetConvertedPrice(int price)
        {
            const char lastLetter = 'k';
            const int divider = 3;
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < price.ToString().Length; i++)
            {
                build.Append(price.ToString()[i]);
            }

            for (int i = price.ToString().Length; i >= 0; i--)
            {
                if (i % divider == 0 && i != 0 && i != price.ToString().Length)
                {
                    build.Insert(price.ToString().Length - i, ",");
                }
            }

            build.Append(lastLetter);
            return build.ToString();
        }
    }
}
