using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace CSharpSnackisDB.Services
{
    public static class FilteredWordsCheck
    {
        public static string Filter(string input, string[] badWords)
        {

            var re = new Regex(
                @"("
                + string.Join("|", badWords.Select(word =>
                    string.Join(@"\s*", word.ToCharArray())))
                + @")", RegexOptions.IgnoreCase);

            return re.Replace(input, match =>
            {
                return new string('*', match.Length);
            });
        }
        public static string[] SeedWords()
        {
            string[] WordArr = File.ReadAllLines(@"Files\filteredwords.txt");
            return WordArr;

        }
    }
}
