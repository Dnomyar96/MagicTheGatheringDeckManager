using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPopulater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 'Enter' to start...");
            Console.ReadLine();

            Sets();
            Console.ReadLine();
        }

        public static async void Sets()
        {
            var setService = new SetService();
            var result = await setService.AllAsync();

            if(result.IsSuccess)
            {
                var sets = result.Value;
                foreach(var set in sets)
                {
                    if (!set.Name.Contains("Collector's Edition") && !set.Name.ToLower().Contains("limited edition") && set.Name != "Arabian Nights")
                        await SaveData(set.Name);
                }
            }
            Console.WriteLine("All done");
        }

        public static async Task SaveData(string setName)
        {
            var service = new CardService();
            var cards = new List<Card>();
            var done = false;
            var count = 1;
            while (!done)
            {
                Console.WriteLine("Downloading info (" + setName + ")...");
                var result = await service.Where(x => x.SetName, setName).Where(x => x.Page, count).AllAsync();
                if (result.IsSuccess)
                {
                    if (result.Value.Count > 0)
                    {
                        Console.WriteLine($"Adding page {count}");
                        foreach(var card in result.Value)
                        {
                            Console.WriteLine($"Adding card: {card.Name}");
                            cards.Add(card);
                        }
                    }
                    else
                    {
                        done = true;
                    }
                }
                count++;
            }
            Console.WriteLine("Creating XML file...");
            var handler = new MagicTheGathering.Data.DataHandler();
            handler.SaveCards(cards, setName);
            Console.WriteLine("Done: " + setName);
        }
    }
}
