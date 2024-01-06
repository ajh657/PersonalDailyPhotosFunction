using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDailyPhotosFunction.Common
{
    public static class Util
    {
        public static string GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);

        public static string DashCaseToSpacedCamelCase(this string dashCase)
        {
            var words = dashCase.Split('-');
            var sb = new StringBuilder();

            for (var i = 0; i < words.Length; i++)
            {
                sb.Append(FirstCharToUpper(words[i]));
                if (i != words.Length - 1)
                {
                    sb.Append(' ');
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
        public static Uri Combine(this Uri uri, string relativeUri)
        {
            return new Uri(uri, relativeUri);
        }
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
