using MTGDeckManager;
using System;
using System.Linq;
using System.Windows;

namespace MTGDeckManagerConsole
{
    static class Program
    {
        static DeckManager deckManager;
        [STAThread]
        static void Main(string[] args)
        {
            System.Console.WriteLine("test");
            System.Console.ReadLine();
            deckManager = new DeckManager();
            bool running = true;
            do
            {
                string input = Read().ToLower();
                switch (input)
                {
                    case "x":
                    case "exit":
                        running = false;
                        break;
                    case "v":
                    case "verify":
                        VerifyList(ClipboardText);
                        break;
                    case "h":
                    case "help":
                        Paste.HelpMenue();
                        break;
                    case "s":
                    case "save":
                        SaveList(ClipboardText);
                        break;
                    case "l":
                    case "list":
                        ListDecks();
                        break;
                    case "w":
                    case "show":
                        WriteDeck();
                        break;
                    case "d":
                    case "delete":
                        DeleteDeck();
                        break;
                    case "t":
                    case "test":
                        TestMethod();
                        break;
                }
            }
            while (running);
        }

        private static async void TestMethod()
        {
            string s = (await APILoader.GetCard("The Birth of Meletis", "THB")).ForeignNames[0].Name;

            Console.WriteLine(s);
        }

        private static void DeleteDeck()
        {
            Deck deck;
            if (SelectDeck(out deck))
            {
                deckManager.DeleteDeck(deck);
            }
        }

        private static void WriteDeck()
        {
            Deck deck;
            if (SelectDeck(out deck))
            {
                Console.WriteLine(deck.GetDeckList());
                Paste.CopyPrompt();
                if (Read().ToLower() == "y")
                {
                    Clipboard.SetText(deck.GetDeckList());
                    Paste.SuccessfullCopy();
                }
            }
        }

        private static bool SelectDeck(out Deck deck)
        {
            Paste.DeckSelectionPrompt();
            string name = Read();
            int index = Array.IndexOf(deckManager
                .Decks
                .Select(d => d.Name)
                .ToArray(), name);

            if (index == -1)
            {
                Paste.DeckDoesNotExistError(name);
                deck = null;
                return false;
            }
            deck =  deckManager.Decks[index];
            return true;
        }

        private static void ListDecks()
        {
            foreach(Deck deck in deckManager.Decks)
            {
                Console.WriteLine(deck.Name);
            }
        }

        static string Read()
        {
            Console.Write(">");
            return Console.ReadLine();
        }

        static string ClipboardText
        {
            get { return Clipboard.GetText().Replace("\r", ""); }
        }

        static void VerifyList (string list) 
        {
            if (deckManager.verifyDeckList(list))
            {
                Paste.ValidDeck();
                if (Read().ToLower() != "n")
                {
                    SaveList(list);
                }
            }
            else
            {
                Paste.FormatInformation();   
            }
        }

        static void SaveList(string list)
        {
            if (deckManager.verifyDeckList(list))
            {
                Deck deck = deckManager.CreateDeck(list);
                Paste.NamePrompt();
                string name = Read();

                try
                {
                    deckManager.SaveDeck(deck, name);
                    Paste.SuccessfullSave();
                }
                catch (InvalidOperationException)
                {
                    Paste.DeckExistsError(name);
                    if (Read().ToLower() == "y")
                    {
                        deckManager.SaveDeck(deck, name, true);
                    }
                }
            }
            else
            {
                Paste.FormatInformation();
            }
            
        }
    }

    static class Paste
    {
        public static void ValidDeck()
        {
            Console.WriteLine("Your clipboard contains a valid Deck. Save? [Y/n]");
        }
        public static void FormatInformation()
        {
            Console.WriteLine("Your clipboard doesn't contains a valid Deck.");
            Console.WriteLine("In MTGA, click on a deck and then export.");
            Console.WriteLine("This will automatically copy the deck to your clipboard in the correct format.");
        }

        public static void DeckSelectionPrompt()
        {
            Console.WriteLine("Which deck do you want to select?");
        }

        public static void CopyPrompt()
        {
            Console.WriteLine("Do you want to copy the Deck to the clipboard? [y/N]");
        }

        public static void HelpMenue()
        {
            Console.WriteLine("e[x]it - exit the Program");
            Console.WriteLine("[v]erify - verify if the current clipboard is a valid deck");
        }

        public static void NamePrompt()
        {
            Console.WriteLine("How should the deck be called?");
        }

        public static void SuccessfullSave()
        {
            Console.WriteLine("Saved successfully");
        }
        public static void SuccessfullCopy()
        {
            Console.WriteLine("Copied to clipboard successfully");
        }

        public static void DeckExistsError(string name)
        {
            Console.WriteLine($"A Deck with the name {name} already exists. Overwrite? [y/N]");
        }

        internal static void DeckDoesNotExistError(string name)
        {
            Console.WriteLine($"A deck with the name {name} does not exist");
        }
    }
}
