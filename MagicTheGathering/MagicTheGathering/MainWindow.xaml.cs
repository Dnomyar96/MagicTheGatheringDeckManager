using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            colorComboBox.Items.Add("All");
            colorComboBox.Items.Add("Black");
            colorComboBox.Items.Add("Blue");
            colorComboBox.Items.Add("Green");
            colorComboBox.Items.Add("Red");
            colorComboBox.Items.Add("White");
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

            LoadContent();
        }

        private async void LoadContent()
        {
            var cards = new List<Card>();
            var service = new CardService();
            var done = false;
            var count = 1;
            while (!done)
            {
                ShowSpinner();
                var result = await service.Where(x => x.SetName, "Kaladesh").Where(x => x.Page, count).AllAsync();
                if (result.IsSuccess)
                {
                    HideSpinner();
                    if (result.Value.Count > 0)
                    {
                        foreach (var card in result.Value)
                        {
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
            LoadList(cards);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                var service = new CardService();
                ShowSpinner();
                var result = await service.Where(x => x.Name, listBox.SelectedItem).AllAsync();
                if (result.IsSuccess)
                {
                    var cards = result.Value;

                    foreach (var card in cards)
                    {
                        if(card.ImageUrl != null)
                        {
                            image.Source = new BitmapImage(new Uri(card.ImageUrl.AbsoluteUri, UriKind.Absolute));
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri("https://www.google.nl/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&ca" +
                                "d=rja&uact=8&ved=0ahUKEwixptmGi4bTAhVDrRQKHTIEBDgQjRwIBw&url=http%3A%2F%2Fshchurchullanad.com%2Fformervi" +
                                "cars.php&psig=AFQjCNH6LtqsKAb8o8LgthDCPQ9zyUGO_A&ust=1491233283080580"));
                        }
                    }
                    HideSpinner();
                }
            }
        }

        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void ShowSpinner()
        {
            spinner.Visibility = Visibility.Visible;
            listBox.IsEnabled = false;
            button.IsEnabled = false;
            sortButton.IsEnabled = false;
            filterGrid.IsEnabled = false;
        }

        private void HideSpinner()
        {
            spinner.Visibility = Visibility.Collapsed;
            listBox.IsEnabled = true;
            button.IsEnabled = true;
            sortButton.IsEnabled = true;
            filterGrid.IsEnabled = true;
        }

        private void LoadList(List<Card> cards)
        {
            listBox.Items.Clear();
            foreach (var card in cards)
            {
                listBox.Items.Add(card.Name);
            }
        }

        private async void Filter()
        {
            var cards = new List<Card>();
            var service = new CardService();
            var done = false;
            var count = 1;
            while (!done)
            {
                ShowSpinner();
                var result = await service.Where(x => x.SetName, "Kaladesh").Where(x => x.Page, count).AllAsync();
                if (result.IsSuccess)
                {
                    HideSpinner();
                    if (result.Value.Count > 0)
                    {
                        foreach (var card in result.Value)
                        {
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

            LoadList(cards);
        }

        private List<Card> FilterColor(string color, List<Card> cards)
        {
            var result = new List<Card>();
            foreach(var card in cards)
            {
                if (card.Colors != null)
                {
                    if (card.Colors.Contains(color))
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
    }
}
