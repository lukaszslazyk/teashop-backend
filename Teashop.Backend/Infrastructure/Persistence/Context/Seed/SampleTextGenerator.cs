using System.Linq;

namespace Teashop.Backend.Infrastructure.Persistence.Context.Seed
{
    public static class SampleTextGenerator
    {
        private static readonly string _loremIpsum =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." +
            " Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris" +
            " nisi ut aliquip ex ea commodo consequat." +
            " Duis aute irure dolor in reprehenderit in voluptate velit esse" +
            " cillum dolore eu fugiat nulla pariatur." +
            " Excepteur sint occaecat cupidatat non proident, sunt in culpa qui" +
            " officia deserunt mollit anim id est laborum.";

        public static string Generate(int numberOfWords)
        {
            return Normalize(AssembleFromLorepIpsum(numberOfWords));
        }

        private static string AssembleFromLorepIpsum(int numberOfWords)
        {
            return Enumerable.Range(0, _loremIpsum.Length / numberOfWords + 1)
                .SelectMany(x => GetLoremIpsumWords())
                .Take(numberOfWords)
                .Aggregate((x, y) => x + " " + y);
        }

        private static string Normalize(string text)
        {
            if (text.EndsWith(','))
                return text.TrimEnd(',') + '.';
            if (!text.EndsWith('.'))
                return text + '.';
            return text;
        }

        private static string[] GetLoremIpsumWords()
        {
            return _loremIpsum.Split(" ");
        }
    }
}
