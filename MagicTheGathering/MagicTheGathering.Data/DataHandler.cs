using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MagicTheGathering.Data
{
    public class DataHandler
    {
        public DataHandler()
        {

        }

        public void SaveCards(List<MtgApiManager.Lib.Model.Card> cards, string fileName)
        {
            var magic = new Magic()
            {
                Cards = new List<Card>()
            };

            foreach (var card in cards)
            {
                var cardToAdd = new Card()
                {
                    Name = card.Name,
                    Colors = card.Colors,
                    Rarity = card.Rarity,
                    Set = card.Set,
                    SetName = card.SetName,
                    Type = card.Type,
                    Text = card.Text,
                    ImageUrl = card.ImageUrl.AbsoluteUri,
                    Loyalty = card.Loyalty,
                    ManaCost = card.ManaCost,
                    Power = card.Power,
                    Toughness = card.Toughness,
                    Source = card.Source,
                    SubTypes = card.SubTypes,
                    SuperTypes = card.SuperTypes,
                    Types = card.Types,
                    OriginalType = card.OriginalType,
                    OriginalText = card.OriginalText,
                    Names = card.Names,
                    Flavor = card.Flavor,
                    Number = card.Number
                };

                magic.Cards.Add(cardToAdd);
            }

            var serializer = new XmlSerializer(typeof(Magic));
            using (var writer = new StreamWriter($"Data/{fileName}.xml"))
            {
                serializer.Serialize(writer, magic);
            }
        }

        public List<Card> GetCards(string fileName)
        {
            var deserializer = new XmlSerializer(typeof(Magic));
            using (var reader = new StreamReader($"Data/{fileName}.xml"))
            {
                var data = (Magic)deserializer.Deserialize(reader);
                return data.Cards;
            }
        }
    }
}
