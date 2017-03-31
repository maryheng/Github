using LinqToTwitter;
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
        private List<Status> currentTweets;

        private SingleUserAuthorizer authorizer = new SingleUserAuthorizer
        {

            CredentialStore = new SingleUserInMemoryCredentialStore
            {
                ConsumerKey =
          "3iNWHoZlJoS5Uwl9AlRY99ENI",
                ConsumerSecret =
         "Nk59Jds8QVs30hZCHwXLlOu1S9nslETCq9H6gvrwD5AHYyfX9C",
                AccessToken =
         "3606448034-0vXRk7NZmHKtoIP2ztyB7i9R2oDY46YSTJsNRi3",
                AccessTokenSecret =
         "54yXjdZOI1KwzU1ZC9II3d2EuTpGJ8GX9fqwrwodLSGzL"
            }
        };
        public MainWindow()
        {
            InitializeComponent();

            GetMostRecent200HomeTimeLine();
            TweetList.Items.Clear();
            currentTweets.ForEach(tweet =>
               TweetList.Items.Add(tweet.Text));
        }

        private void GetMostRecent200HomeTimeLine()
        {
            var twitterContext = new TwitterContext(authorizer);

            var tweets = from tweet in twitterContext.Status
                         where tweet.Type == StatusType.Home &&
                         tweet.Count == 200
                         select tweet;

            currentTweets = tweets.ToList();
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
