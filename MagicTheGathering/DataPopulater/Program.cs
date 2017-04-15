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

            SaveData();
            Console.ReadLine();
        }

        public static async void SaveData()
        {
            var service = new CardService();
            var cards = new List<Card>();
            var done = false;
            var count = 1;
            while (!done)
            {
                Console.WriteLine("Downloading info...");
                var result = await service.Where(x => x.SetName, "Kaladesh").Where(x => x.Page, count).AllAsync();
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
            handler.SaveCards(cards, "Kaladesh");
            Console.WriteLine("Done");
        }
    }
}
