using MagicTheGathering.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicTheGathering
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Card> _cards = new List<Card>();
        private const string SET = "Amonkhet";

        public MainWindow()
        {
            InitializeComponent();

            colorComboBox.Items.Add("All");
            colorComboBox.Items.Add("Black");
            colorComboBox.Items.Add("Blue");
            colorComboBox.Items.Add("Green");
            colorComboBox.Items.Add("Red");
            colorComboBox.Items.Add("White");
            colorComboBox.Items.Add("Multiple");
            colorComboBox.SelectedIndex = 0;

            rarityComboBox.Items.Add("All");
            rarityComboBox.Items.Add("Common");
            rarityComboBox.Items.Add("Uncommon");
            rarityComboBox.Items.Add("Rare");
            rarityComboBox.Items.Add("Mythic Rare");
            rarityComboBox.SelectedIndex = 0;

            typeComboBox.Items.Add("All");
            typeComboBox.Items.Add("Creature");
            typeComboBox.Items.Add("Sorcery");
            typeComboBox.Items.Add("Instant");
            typeComboBox.Items.Add("Enchantment");
            typeComboBox.Items.Add("Artifact");
            typeComboBox.Items.Add("Land");
            typeComboBox.Items.Add("Planeswalker");
            typeComboBox.SelectedIndex = 0;

            LoadDecksComboBox();
            LoadContent();

            setComboBox.Items.Add("All");
            foreach(var card in _cards)
            {
                if(!setComboBox.Items.Contains(card.SetName))
                {
                    setComboBox.Items.Add(card.SetName);
                }
            }
            setComboBox.SelectedIndex = 0;
        }

        private void LoadContent()
        {
            var handler = new DataHandler();
            var cards = handler.GetCards(SET);
            LoadList(cards);
            _cards = cards;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                var handler = new DataHandler();
                var cards = handler.GetCards(SET);
                foreach (var card in cards)
                {
                    if (card.Name == (string)listBox.SelectedItem)
                    {
                        if (card.ImageUrl != null)
                        {
                            image.Source = new BitmapImage(new Uri(card.ImageUrl, UriKind.Absolute));
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri("https://www.google.nl/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&ca" +
                                "d=rja&uact=8&ved=0ahUKEwixptmGi4bTAhVDrRQKHTIEBDgQjRwIBw&url=http%3A%2F%2Fshchurchullanad.com%2Fformervi" +
                                "cars.php&psig=AFQjCNH6LtqsKAb8o8LgthDCPQ9zyUGO_A&ust=1491233283080580"));
                        }
                        nameText.Text = card.Name;
                        typeText.Text = card.Type;
                        rarityText.Text = card.Rarity;
                        textText.Document.Blocks.Clear();
                        textText.Document.Blocks.Add(new Paragraph(new Run(card.Text)));
                        flavorText.Document.Blocks.Clear();
                        flavorText.Document.Blocks.Add(new Paragraph(new Run(card.Flavor)));
                        if (card.Loyalty != null)
                        {
                            strengthText.Text = card.Loyalty;
                        }
                        else if (card.Power != null)
                        {
                            strengthText.Text = card.Power + "/" + card.Toughness;
                        }
                        else
                        {
                            strengthText.Text = "";
                        }
                        manaCostText.Text = card.ManaCost;
                        break;
                    }
                }
            }
        }

        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var cards = new List<Card>();
            foreach (var card in _cards)
            {
                if (card.Name.ToLower().Contains(searchTextBox.Text.ToLower()))
                {
                    cards.Add(card);
                }
            }
            LoadList(cards);
        }

        private void LoadList(List<Card> cards)
        {
            listBox.Items.Clear();
            foreach (var card in cards)
            {
                listBox.Items.Add(card.Name);
                listBox.IsEnabled = true;
                listBox.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
            }
            resultLabel.Content = listBox.Items.Count + " results";
            if (cards.Count == 0)
            {
                listBox.Items.Add("No cards have been found");
                listBox.IsEnabled = false;
                resultLabel.Content = "0 results";
            }
        }

        private void Filter()
        {
            var handler = new DataHandler();
            var cards = handler.GetCards(SET);

            var color = colorComboBox.SelectedValue;
            switch (color)
            {
                case "All":
                    break;
                case "Black":
                    cards = FilterColor("Black", cards);
                    break;
                case "Blue":
                    cards = FilterColor("Blue", cards);
                    break;
                case "Green":
                    cards = FilterColor("Green", cards);
                    break;
                case "Red":
                    cards = FilterColor("Red", cards);
                    break;
                case "White":
                    cards = FilterColor("White", cards);
                    break;
                case "Multiple":
                    cards = FilterColor("Multiple", cards);
                    break;
            }

            var rarity = rarityComboBox.SelectedValue;
            switch (rarity)
            {
                case "All":
                    break;
                case "Common":
                    cards = FilterRarity("Common", cards);
                    break;
                case "Uncommon":
                    cards = FilterRarity("Uncommon", cards);
                    break;
                case "Rare":
                    cards = FilterRarity("Rare", cards);
                    break;
                case "Mythic Rare":
                    cards = FilterRarity("Mythic Rare", cards);
                    break;
            }

            var type = typeComboBox.SelectedValue;
            switch (type)
            {
                case "All":
                    break;
                case "Creature":
                    cards = FilterType("Creature", cards);
                    break;
                case "Sorcery":
                    cards = FilterType("Sorcery", cards);
                    break;
                case "Instant":
                    cards = FilterType("Instant", cards);
                    break;
                case "Enchantment":
                    cards = FilterType("Enchantment", cards);
                    break;
                case "Artifact":
                    cards = FilterType("Artifact", cards);
                    break;
                case "Land":
                    cards = FilterType("Land", cards);
                    break;
                case "Planeswalker":
                    cards = FilterType("Planeswalker", cards);
                    break;
            }

            if(setComboBox.SelectedValue.ToString() != "All")
            {
                var result = new List<Card>();
                foreach(var card in cards)
                {
                    if(card.SetName == setComboBox.SelectedValue.ToString())
                    {
                        result.Add(card);
                    }
                }
                cards = result;
            }

            LoadList(cards);
        }

        private List<Card> FilterColor(string color, List<Card> cards)
        {
            var result = new List<Card>();
            if (color != "Multiple")
            {
                foreach (var card in cards)
                {
                    if (card.Colors != null)
                    {
                        if (card.Colors.Contains(color))
                        {
                            result.Add(card);
                        }
                    }
                }
            }
            else
            {
                foreach (var card in cards)
                {
                    if (card.Colors == null)
                        continue;

                    if (card.Colors.Count() > 1)
                    {
                        result.Add(card);
                    }
                }
            }
            return result;
        }

        private List<Card> FilterRarity(string rarity, List<Card> cards)
        {
            var result = new List<Card>();
            foreach(var card in cards)
            {
                if(card.Rarity != null)
                {
                    if(card.Rarity == rarity)
                    {
                        result.Add(card);
                    }
                }
            }
            return result;
        }

        private List<Card> FilterType(string type, List<Card> cards)
        {
            var result = new List<Card>();
            foreach (var card in cards)
            {
                if (card.Types != null)
                {
                    if (card.Types.Contains(type))
                    {
                        result.Add(card);
                    }
                }
            }
            return result;
        }

        private void LoadDecksComboBox()
        {
            deckComboBox.Items.Add("<New Deck>");
            var files = Directory.GetFiles("Data/Decks");
            foreach(var file in files)
            {
                var item = file.Replace("Data/Decks\\", "");
                item = item.Replace(".xml", "");
                deckComboBox.Items.Add(item);
            }
            deckComboBox.SelectedIndex = 0;
        }
    }
}
