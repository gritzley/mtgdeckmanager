using System;
using System.Collections.Generic;
using System.Text;
using MtgApiManager;

namespace MTGDeckManager
{
    public class Card
    {
        public string Name;
        public string CollectorNr;
        public string SetID;
        public string UniqueID { get { return SetID + CollectorNr; } }

        public Card (string name, string setID, string collectorNr )
        {
            Name = name;
            SetID = setID;
            CollectorNr = collectorNr;
        }
    }
}
