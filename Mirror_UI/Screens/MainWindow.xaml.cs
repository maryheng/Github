using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Windows.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace Mirror_UI
{
    public partial class MainWindow : UserControl, ISwitchable
    {
        private List<Status> currentTweets;
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
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
            StartClock();
            GetWeather(null, null);
            UpdateWeather();
            GetGoogleCalendar();
        }
        

        private async void GetWeather(object sender, EventArgs e)
        {
            var client = new HttpClient();
            try
            {
                string resourceAddress = "http://api.openweathermap.org/data/2.5/find?q=Singapore&appid=b8ef90a894b4c5b3669df67ac81a4e1b&units=metric";
                var wcfResponse = await client.GetAsync(resourceAddress);
                var returnedJsonWeather = await wcfResponse.Content.ReadAsStringAsync();
                var weatherAsObject = JsonConvert.DeserializeObject<dynamic>(returnedJsonWeather);
                var temp = weatherAsObject.list[0].main.temp;
                var weatherSummary = weatherAsObject.list[0].weather[0].main;
                var country = weatherAsObject.list[0].name;
                var icon = (weatherAsObject.list[0].weather[0].icon).ToString();
                tempTB.Text = temp.ToString() + " °C";
                countryTB.Text = country.ToString();
                weatherTB.Text = weatherSummary.ToString();
                lastUpdatedTB.Text = "Last Updated: " + String.Format("{0:T}", DateTime.Now);
                switch (icon)
                {
                    case "01d":
                        var uri1 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\01d.png");
                        var bitmap1 = new BitmapImage(uri1);
                        weatherIcon.Source = bitmap1;
                        break;
                    case "01n":
                        var uri2 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\01n.png");
                        var bitmap2 = new BitmapImage(uri2);
                        weatherIcon.Source = bitmap2;
                        break;
                    case "02d":
                        var uri3 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\02d.png");
                        var bitmap3 = new BitmapImage(uri3);
                        weatherIcon.Source = bitmap3;
                        break;
                    case "02n":
                        var uri4 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\02n.png");
                        var bitmap4 = new BitmapImage(uri4);
                        weatherIcon.Source = bitmap4;
                        break;
                    case "03d":
                    case "03n":
                    case "04d":
                    case "04n":
                        var uri5 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\03or4.png");
                        var bitmap5 = new BitmapImage(uri5);
                        weatherIcon.Source = bitmap5;
                        break;
                    case "09n":
                    case "09d":
                        var uri6 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\09.png");
                        var bitmap6 = new BitmapImage(uri6);
                        weatherIcon.Source = bitmap6;
                        break;
                    case "10d":
                    case "10n":
                        var uri7 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\09.png");
                        var bitmap7 = new BitmapImage(uri7);
                        weatherIcon.Source = bitmap7;
                        break;
                    case "11d":
                        var uri8 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\11d.png");
                        var bitmap8 = new BitmapImage(uri8);
                        weatherIcon.Source = bitmap8;
                        break;
                    case "11n":
                        var uri9 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\11n.png");
                        var bitmap9 = new BitmapImage(uri9);
                        weatherIcon.Source = bitmap9;
                        break;
                    case "13d":
                    case "13n":
                        var uri10 = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\11n.png");
                        var bitmap10 = new BitmapImage(uri10);
                        weatherIcon.Source = bitmap10;
                        break;
                    default:
                        var uri = new Uri("C:\\Users\\FYP\\Source\\Repos\\Github\\Mirror_UI\\Images\\weatherIcons\\50.png");
                        var bitmap = new BitmapImage(uri);
                        weatherIcon.Source = bitmap;
                        break;
                }
            }
            catch (Exception ex)
            { 
                string messageBoxText = ex.Message;
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }
        private void UpdateWeather()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(600); //10 mins
            timer.Tick += GetWeather;
            timer.Start();
        }
        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Tickevent;
            timer.Start();
        }
        private void Tickevent(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            DateTB.Text = String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now);
            TimeTB.Text = String.Format("{0:T}", DateTime.Now);
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










        private void GetGoogleCalendar()
        {
            eventLB.Items.Clear();
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = System.IO.Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            
            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = String.Format("{0:M/d/yyyy}", eventItem.Start.DateTime);
                    if (String.IsNullOrEmpty(when))
                    {
                        when = String.Format("{0:M/d/yyyy}", eventItem.Start.Date);
                    }
                    ListBoxItem itm = new ListBoxItem();
                    itm.Content = when + ": " + eventItem.Summary;
                    eventLB.Items.Add(itm);

                }
            }
            else
            {
                ListBoxItem itm = new ListBoxItem();
                itm.Content = "No upcoming events found.";
                eventLB.Items.Add(itm);
            }
        }
        
    }
}
