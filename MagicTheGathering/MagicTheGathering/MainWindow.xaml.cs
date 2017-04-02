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

            var service = new CardService();
            var result = service.Where(x => x.SetName, "Kaladesh").All();
            if(result.IsSuccess)
            {
                var cards = result.Value;

                LoadList(cards);
            }
            else
            {
                var exception = result.Exception;
            }

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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                var service = new CardService();
                var result = service.Where(x => x.Name, listBox.SelectedItem).All();
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
                }
            }
        }

        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void LoadList(List<Card> cards)
        {
            listBox.Items.Clear();
            foreach (var card in cards)
            {
                listBox.Items.Add(card.Name);
            }
        }

        private void Filter()
        {
            var service = new CardService();
            var result = service.Where(x => x.SetName, "Kaladesh").All();
            if (result.IsSuccess)
            {
                var cards = result.Value;

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
                switch(rarity)
                {
                    case "All":
                        break;
                    case "Common":
                        break;
                    case "Uncommon":
                        break;
                    case "Rare":
                        break;
                    case "Mythic Rare":
                        break;
                }

                LoadList(cards);
            }
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
    }
}
