using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtgDeckManagerView.Logic
{
    public class DeckEntry
    {
        public string Name { get; set; }
        public string Decklist { get; set; }

        public DeckEntry(string title, string decklist)
        {
            Name = title;
            Decklist = decklist;
        }
    }
}
