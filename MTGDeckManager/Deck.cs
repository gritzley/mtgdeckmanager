using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MTGDeckManager
{
    public class Deck
    {
        private List<(int, Card)> _mainDeck;
        private List<(int, Card)> _sideBoard;
        private Card _companion;
        private Card _commander;

        public string Name;

        public List<(int, Card)> MainDeck { get { return _mainDeck; } }
        public List<(int, Card)> SideBoard { get { return _sideBoard; } }
        public Card Companion { get { return _companion; } }
        public Card Commander { get { return _commander; } }

        public String GetMaindeckString()
        {
            return GetListString(_mainDeck);
        }
        public String GetSideboardString()
        {
            return GetListString(_sideBoard);
        }
        public String GetCompanionString()
        {
            if (_companion != null)
            {
                return GetCardString(_companion);
            }
            return "";
        }
        public String GetCommanderString()
        {
            if (_commander != null)
            {
                return GetCardString(_commander); 
            }
            return "";
        }
        public String GetDeckList()
        {
            string CommanderString = "";
            if (_commander != null)
            {
                CommanderString = $"Commander\n1 {GetCommanderString()}\n";
            }
            string CompanionString = "";
            if (_companion != null)
            {
                CompanionString = $"Companion\n1 {GetCompanionString()}\n";
            }
            string SideboardString = "";
            if (_sideBoard.Count > 0)
            {
                SideboardString = $"Sideboard\n{GetSideboardString()}\n";
            }
            return $"{CommanderString}{CompanionString}Deck\n{GetMaindeckString()}\n{SideboardString}";
        }

        private String GetCardString (Card card)
        { 
            return $"{card.Name} ({card.SetID}) {card.CollectorNr}\n";
        }
        private String GetListString(List<(int, Card)> list)
        {
            if (list.Count > 0)
            {
                return String.Join("", list
                    .Select(e =>
                    {
                        int i = e.Item1;
                        Card c = e.Item2;
                        return $"{i.ToString()} {GetCardString(c)}";
                    })
                    .ToArray());
            }
            return "";
        }

        public Deck(
            List<(int, Card)> mainDeck,
            List<(int, Card)> sideBoard = null,
            Card companion = null,
            Card commander = null)
        {
            if (sideBoard != null)
            {
                _sideBoard = sideBoard;
            }
            else
            {
                _sideBoard = new List<(int, Card)>();
            }
            _mainDeck = mainDeck;

            _companion = companion;
            _commander = commander;
        }
    }
}
