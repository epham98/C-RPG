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
using System.Windows.Shapes;
using Engine.ViewModels;
using Engine.Models;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        //wraps tradescreen's datacontext as a gamesession object
        public GameSession Session => DataContext as GameSession;
        public TradeScreen()
        {
            InitializeComponent();
        }

        //function to sell items
        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                Session.currentPlayer.receiveGold(groupedInventoryItem.item.price);
                Session.currentTrader.AddItemToInventory(groupedInventoryItem.item);
                Session.currentPlayer.RemoveItemFromInventory(groupedInventoryItem.item);
            }
        }

        //function to buy items
        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                if(Session.currentPlayer.gold >= groupedInventoryItem.item.price)
                {
                    Session.currentPlayer.spendGold(groupedInventoryItem.item.price);
                    Session.currentTrader.RemoveItemFromInventory(groupedInventoryItem.item);
                    Session.currentPlayer.AddItemToInventory(groupedInventoryItem.item);
                }
                else
                {
                    MessageBox.Show("Insufficient Macca.");
                }
            }
        }

        //closes trade window
        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
