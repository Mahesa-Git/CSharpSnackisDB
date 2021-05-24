using System;

namespace CSharpSnackisDB.Entities
{
    public class FilteredWords
    {
        public string ID { get; set; }
        public string Words { get; set; }

        public FilteredWords()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}
