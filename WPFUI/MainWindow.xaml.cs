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
using Engine.EventArgs;
using Engine.ViewModels;
using Engine.Models;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameSession _gameSession = new GameSession();
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        public MainWindow()
        {
            InitializeComponent();

            InitializeUserInputActions();

            _gameSession.OnMessageRaised += OnGameMessageRaised;

            DataContext = _gameSession;
        }

        //calls movement selections from GameSession
        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }

        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }

        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }

        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }

        private void OnClick_AttackMonster(object sender, RoutedEventArgs e)
        {
            _gameSession.AttackCurrentMonster();
        }

        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {

            _gameSession.useCurrentConsumable();
        }

        //function to display message
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd();
        }

        //opens trade window
        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            if(_gameSession.currentTrader != null)
            {
                TradeScreen tradeScreen = new TradeScreen();
                tradeScreen.Owner = this;
                tradeScreen.DataContext = _gameSession;
                tradeScreen.ShowDialog();
            }          
        }

        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.craftItem(recipe);
        }

        //assigns inventory, movement, and battle options to keyboard presses
        private void InitializeUserInputActions()
        {
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.Z, () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(Key.C, () => _gameSession.useCurrentConsumable());
            _userInputActions.Add(Key.I, () => SetTabFocus("InventoryTabItem"));
            _userInputActions.Add(Key.Q, () => SetTabFocus("QuestsTabItem"));
            _userInputActions.Add(Key.R, () => SetTabFocus("RecipesTabItem"));
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));
        }

        //invokes events on key presses
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(_userInputActions.ContainsKey(e.Key))
            {
                _userInputActions[e.Key].Invoke();
            }
        }

        //key presses changes bottom left pane to desired tab
        private void SetTabFocus(string tabName)
        {
            foreach(object item in PlayerDataTabControl.Items)
            {             
                if (item is TabItem tabItem)
                {
                    if (tabItem.Name == tabName)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
                }
            }
        }
    }
}
