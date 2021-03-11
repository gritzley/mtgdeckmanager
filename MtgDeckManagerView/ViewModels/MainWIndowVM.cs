using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTGDeckManager;
using MtgDeckManagerView.Logic;
using System.Collections.ObjectModel;

namespace MtgDeckManagerView.ViewModels
{
    [POCOViewModel]
    public class MainWindowVM
    {
        private DeckManager deckManager;

        public virtual ObservableCollection<DeckEntry> DeckEntries { get; set; }

        public MainWindowVM()
        {
            deckManager = new DeckManager();

            DeckEntries = new ObservableCollection<DeckEntry>(deckManager
                .Decks
                .Select(d => new DeckEntry(d.Name, d.GetDeckList()))
                .ToList());
        }
    }
}
