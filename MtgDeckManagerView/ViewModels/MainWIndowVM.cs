using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTGDeckManager;
using MtgDeckManagerView.Logic;
using MtgDeckManagerView.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Core;
using QuickType;

namespace MtgDeckManagerView.ViewModels
{
    [POCOViewModel]
    public class MainWindowVM
    {
        private DeckManager deckManager;

        static string ClipboardText
        {
            get
            {
                return Clipboard
                    .GetText()
                    .Replace("\r", "");
            }
            set
            {
                Clipboard.SetText(value);
            }
        }

        public virtual ObservableCollection<DeckEntry> DeckEntries { get; set; }
        public virtual DeckEntry SelectedDeck { get; set; }
        public virtual string SelectedCard { get; set; }
        public virtual string ImageSource { get; set; }

        public MainWindowVM()
        {
            deckManager = new DeckManager();
            UpdateDeckEntries();
        }

        public async void LoadImage()
        {
            if (SelectedCard == null)
                return;
            Regex r = DeckManager.CardRegex;
            Match match = r.Match(SelectedCard);
            if (!match.Success)
                return;
            Card card = DeckManager.MakeCard(match);

            CardSearchResult cardData = await APILoader.GetCard(card.Name);

            for (int i = 0; i < cardData.Cards.Length; i++)
            {
                if (cardData.Cards[i].ImageUrl != null)
                {
                    ImageSource = cardData.Cards[i].ImageUrl.AbsoluteUri;
                    return;
                }
            }
            //ImageSource = "";
        }
        public void StartLoadImage()
        {
            LoadImage();
        }
        public void UpdateDeckEntries()
        {
            DeckEntries = new ObservableCollection<DeckEntry>(deckManager
                .Decks
                .Select(d => new DeckEntry(d.Name, d.GetDeckList()))
                .ToList());
        }

        public void ImportFromClipboard()
        {
            string list = ClipboardText;
            if (deckManager.verifyDeckList(list))
            {
                Deck deck = deckManager.CreateDeck(list);
                bool nameIsValid = false;
                string name = "";
                while(!nameIsValid)
                {
                    name = PromptVM.ShowDialog("What should the Deck be called?", "Name");

                    if (name == null)
                        return;
                    if (name.Trim(' ') == "")
                    {
                        MessageBox.Show("Name can not be empty");
                        continue;
                    }
                    nameIsValid = true;
                }
                try
                {
                    deckManager.SaveDeck(deck, name);
                    UpdateDeckEntries();
                    MessageBox.Show($"Deck {name} saved successfully!");
                }
                catch (InvalidOperationException)
                {
                    bool overwrite = PromptVM.YesNo($"Deck {name} already exists. Do you want to overwrite it?", "Overwrite");
                    if (overwrite)
                    {
                        DeleteDeck(name);
                        deckManager.SaveDeck(deck, name, true);
                        UpdateDeckEntries();
                    }
                }
            }
            else
            {
                MessageBox.Show("Your Clipboard does not contain a valid Deck");
            }
        }

        public void ExportToClipboard()
        {
            if (SelectedDeck == null)
            {
                MessageBox.Show("No Deck selected!");
                return;
            }
            ClipboardText = SelectedDeck.Decklist;
            MessageBox.Show("Deck Copied To Clipboard. In MTG Arena, go to decks and click the 'Import' button.");
        }

        public void DeleteCurrentDeck()
        {
            bool delete = PromptVM.YesNo("Are you sure?", "Delete Deck");

            if (!delete)
                return;

            string name = SelectedDeck.Title;

            DeleteDeck(name);

            UpdateDeckEntries();

            MessageBox.Show($"Deck {name} deleted succesfully");
        }

        public void DeleteDeck(string name)
        {
            Deck d = deckManager
                .Decks
                .Where(e => e.Name == name)
                .First();

            deckManager.DeleteDeck(d);
        }
    }
}
