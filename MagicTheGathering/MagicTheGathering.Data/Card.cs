using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicTheGathering.Data
{
    public class Card
    {
        public string Name { get; set; }

        public string[] Colors { get; set; }

        public string Rarity { get; set; }

        public string Set { get; set; }

        public string SetName { get; set; }

        public string Type { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public string Loyalty { get; set; }

        public string ManaCost { get; set; }

        public string Power { get; set; }

        public string Toughness { get; set; }

        public string Source { get; set; }

        public string[] SubTypes { get; set; }

        public string[] SuperTypes { get; set; }

        public string[] Types { get; set; }

        public string OriginalType { get; set; }

        public string OriginalText { get; set; }

        public string[] Names { get; set; }

        public string Flavor { get; set; }

        public string Number { get; set; }
    }
}
