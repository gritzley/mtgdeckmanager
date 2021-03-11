using System;
using System.Collections.Generic;
using System.Text;
using MtgApiManager;
using MtgApiManager.Lib.Service;
using MtgApiManager.Lib.Model;
using System.Threading.Tasks;
using MtgApiManager.Lib.Core;
using System.Linq;

namespace MTGDeckManager
{
    public static class APILoader
    {
        private static IMtgServiceProvider serviceProvider = new MtgServiceProvider();
        public static async Task<ICard> GetCard(string name, string setID)
        {
            ICardService service = serviceProvider.GetCardService();
            Exceptional<List<ICard>> result = await service.Where(x => x.Set, setID)
                .Where(x => x.Name, name)
                .AllAsync();

            if (result.IsSuccess)
            {
                return result.Value[0];
            }
            else
            {
                return null;
            }
        }
    }
}
