using System;

namespace CSharpSnackisDB.Entities
{
    public class FilteredWords
    {
        public Guid ID { get; set; }
        public string Words { get; set; }

        public FilteredWords()
        {
            ID = Guid.NewGuid();
        }
    }
}
