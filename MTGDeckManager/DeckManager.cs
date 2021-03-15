using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MTGDeckManager
{
    public class DeckManager
    {
        private static Regex _DeckFormatRegex = new Regex(@"(Commander\n(?<commander>.+\n)\n)?(Companion\n(?<companion>.+\n)\n)?(Deck\n(?<deck>(.+\n)+))(\nSideboard\n(?<sideboard>(.+\n)+))?");
        private string _SaveDir;
        private JsonSerializer _JsonSerializer;
        private List<Deck> _Decks;

        public static Regex CardRegex = new Regex(@"(?<amount>\d+) (?<name>[\w, ' ]+) \((?<setName>[A-Z0-9]{3})\) (?<collectorNr>\d{1,3})");
        public List<Deck> Decks { get { return _Decks; } }
        public DeckManager()
        {
            _JsonSerializer = new JsonSerializer();

            _SaveDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MTGDeckManager\";
            if(!Directory.Exists(_SaveDir))
            {
                Directory.CreateDirectory(_SaveDir);
            }

            LoadAllDecks();
        }
        public bool verifyDeckList(string list)
        {
            if (!_DeckFormatRegex.IsMatch(list)) return false;
            string[] deconstructedDeck = DeconstructDecklist(list);

            if (!CardRegex.IsMatch(deconstructedDeck[0]) && deconstructedDeck[0] != "") return false;
            if (!CardRegex.IsMatch(deconstructedDeck[1]) && deconstructedDeck[1] != "") return false;
            if (!CardRegex.IsMatch(deconstructedDeck[2])) return false;
            if (!CardRegex.IsMatch(deconstructedDeck[3]) && deconstructedDeck[3] != "") return false;
            return true;
        }
        private string[] DeconstructDecklist(string list)
        {
            Match match = _DeckFormatRegex.Match(list);
            string commander = match.Groups["commander"].Value;
            string companion = match.Groups["companion"].Value;
            string deck = match.Groups["deck"].Value;
            string sideboard = match.Groups["sideboard"].Value;
            return new string[] { commander, companion, deck, sideboard };
        }
        public Deck CreateDeck(string list)
        {
            if (verifyDeckList(list))
            {
                string[] deconstructedDeck = DeconstructDecklist(list);

                Card commander = MakeCard(CardRegex.Match(deconstructedDeck[0]));
                Card companion = MakeCard(CardRegex.Match(deconstructedDeck[1]));
                List<(int, Card)> mainDeck = CreateSubdeck(deconstructedDeck[2]);
                List<(int, Card)> sideboard = CreateSubdeck(deconstructedDeck[3]);
                return new Deck(mainDeck, sideboard, companion, commander);
            }
            return null;
        }

        private List<(int, Card)> CreateSubdeck(string list)
        {
            if (list == null || list.Length == 0) return null;
            return CardRegex
                .Matches(list)
                .Cast<Match>()
                .Select<Match, (int, Card)>(e =>
                    (int.Parse(e.Groups["amount"].Value),
                    MakeCard(e)))
                .ToList();
        }
        public static Card MakeCard(Match match)
        {
            if (match.Success)
            {
                string name = match.Groups["name"].Value;
                string setName = match.Groups["setName"].Value;
                string collectorNr = match.Groups["collectorNr"].Value;
                return new Card(name, setName, collectorNr);
            }
            return null;
        }
        public void SaveDeck(Deck deck)
        {
            if (deck.Name == null)
            {
                throw new InvalidOperationException();
            }
            SaveDeck(deck, deck.Name);
        }
        public void SaveDeck(Deck deck, string name, bool force = false)
        {
            string path = _SaveDir + name + ".json";

            if (File.Exists(path) && !force)
                throw new InvalidOperationException();

            deck.Name = name;
            string json = JsonConvert.SerializeObject(deck);
            File.WriteAllText(path, json);
            _Decks.Add(deck);
        }

        public void DeleteDeck(Deck deck)
        {
            string path = _SaveDir + deck.Name + ".json";

            if (!File.Exists(path))
                throw new InvalidOperationException();

            File.Delete(path);
            _Decks.Remove(deck);
        }

        private void LoadAllDecks()
        {
            _Decks = new List<Deck>();
            string[] deckPaths = Directory.GetFiles(_SaveDir, "*.json");

            foreach (string path in deckPaths)
            {
                _Decks.Add(LoadDeck(path));
            }
        }
        private Deck LoadDeck(string path)
        {
            string json = File.ReadAllText(path);
            JsonTextReader _JsonTextReader = new JsonTextReader(new StringReader(json));
            
            Deck deck = _JsonSerializer.Deserialize<Deck>(_JsonTextReader);

            return deck;
        }
    }
}
