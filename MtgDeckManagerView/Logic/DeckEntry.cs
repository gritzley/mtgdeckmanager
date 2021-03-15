using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtgDeckManagerView.Logic
{
    public class DeckEntry
    {
        public string Title { get; set; }
        public string Decklist { get; set; }

        public List<string> DecklistDivided
        {
            get
            {
                return Decklist.Split('\n').ToList();
            }
            private set
            {
                Decklist = String.Join("\n", value);
            }
        }

        public DeckEntry(string title, string decklist)
        {
            Title = title;
            Decklist = decklist;
        }
    }
}
