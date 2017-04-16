using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;

namespace DataPopulater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var setService = new SetService();
            var result = setService.All();
            if (result.IsSuccess)
            {
                var sets = result.Value;
                var sortedSets = sets.OrderBy(x => x.Name).ToList();
                foreach (var set in sortedSets)
                {
                    comboBox.Items.Add(set.Name);
                }
            }
            comboBox.SelectedIndex = 0;
        }

        public async Task SaveData(string setName)
        {
            var service = new CardService();
            var cards = new List<Card>();
            var done = false;
            var count = 1;
            while (!done)
            {
                var result = await service.Where(x => x.SetName, setName).Where(x => x.Page, count).AllAsync();
                if (result.IsSuccess)
                {
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
            var handler = new MagicTheGathering.Data.DataHandler();
            handler.SaveCards(cards, setName);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            spinner.Visibility = Visibility.Visible;
            await SaveData(comboBox.SelectedValue.ToString());
            spinner.Visibility = Visibility.Hidden;
            doneL.Visibility = Visibility.Visible;
        }
    }
}
