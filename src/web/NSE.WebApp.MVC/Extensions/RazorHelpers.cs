using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;

namespace NSE.WebApp.MVC.Extensions
{
    public static class RazorHelpers
    {
        public static string StockMessage(this RazorPage page, int amount)
        {
            return amount > 0 ? $"Just {amount} in stock!" : "Product unavailable";
        }

        public static string FormatCurrency(this RazorPage page, decimal value)
        {
            return value > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", value) : "Free";
        }

        public static string HashEmailForGravatar(this RazorPage page, string email)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
            var sBuilder = new StringBuilder();

            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
