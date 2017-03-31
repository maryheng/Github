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

namespace Mirror_UI
{
    public partial class MainWindow : UserControl, ISwitchable
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void BookMeetingRoom_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BookMeetingRoom());
        }

        private void LoanInventoryItemsButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new LoanInventoryItem());
        }

        private void ItemsOnLoanButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MoreLoanItems());
        }

        private void MoreEventsButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MoreEvents());
        }

        private void BookPrintersCuttersButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BookPrinterAndLaser());
        }
    }
}
