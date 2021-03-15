using System;
using System.Collections.Generic;
using System.Text;
using MtgApiManager;
using MtgApiManager.Lib.Service;
using MtgApiManager.Lib.Model;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Net;
using QuickType;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MTGDeckManager
{   
    public static class APILoader
    {
        private static IMtgServiceProvider serviceProvider = new MtgServiceProvider();

        private static string baseURL = "https://api.magicthegathering.io/v1/cards";
        public static async Task<CardSearchResult> GetCard(string name)
        {
            
            string url = $"{baseURL}?name=\"{name}\"";

            if (name.Contains("\'"))
            {
                url.Replace("\"", ""); // mtg api has a weird issue where the exact search will not return anythign if the card name contains apostrophes.
            }
            WebRequest request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            JsonTextReader json = new JsonTextReader(reader);

            JsonSerializer jsonSerializer = new JsonSerializer();

            CardSearchResult cards = jsonSerializer.Deserialize<CardSearchResult>(json);

            response.Close();

            return cards;
        }
    }
}
