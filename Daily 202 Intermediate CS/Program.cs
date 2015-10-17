using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Daily_202_Intermediate_CS
{
    class Program
    {
        static Regex NodePattern = new Regex(@"<pre>.*?</pre>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static Regex DatePattern = new Regex(@"(\d+)(st|nd|rd|th) (April|March) (\d\d\d\d)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        static void Main(string[] args)
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadString("http://tlarsen2.tripod.com/anthonypolumbo/apeasterdates.html");
                var dates = NodePattern.Matches(data).Cast<Match>().Select(match => match.Value)
                    .SelectMany(section => DatePattern.Matches(section).Cast<Match>().Select(match => match.Value
                        .Replace("st", String.Empty)
                        .Replace("nd", String.Empty)
                        .Replace("rd", String.Empty)
                        .Replace("th", String.Empty)))
                    .Select(date => DateTime.ParseExact(
                        date,
                        new[] { "dd MMMM yyyy", "d MMMM yyyy" },
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AdjustToUniversal))
                    .Where(date => date.Year > 2014 && date.Year < 2026)
                    .OrderBy(date => date);

                foreach (var date in dates)
                {
                    Console.WriteLine(date.ToShortDateString());
                }
            }

            // Suspend the console.
            Console.ReadKey();
        }
    }
}
