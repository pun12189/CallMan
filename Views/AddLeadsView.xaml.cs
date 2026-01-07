using BahiKitab.Models;
using BahiKitab.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for AddLeadsView.xaml
    /// </summary>
    public partial class AddLeadsView : UserControl
    {
        private readonly LeadSourceDataService _sourceDataService;
        private readonly LeadStatusDataService _statusDataService;
        private readonly LeadTagsDataService _tagsDataService;
        private readonly LeadLabelsDataService _labelsDataService;

        public AddLeadsView()
        {
            InitializeComponent();
            _sourceDataService = new LeadSourceDataService();
            _labelsDataService = new LeadLabelsDataService();
            _tagsDataService = new LeadTagsDataService();
            _statusDataService = new LeadStatusDataService();

            this.Loaded += AddLeadsView_Loaded;
        }

        private async void AddLeadsView_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbLabel.ItemsSource = await _labelsDataService.GetAllLeadLabelsAsync();
            this.cbSource.ItemsSource = await _sourceDataService.GetAllLeadSourcesAsync();
            this.cbTags.ItemsSource = await _tagsDataService.GetAllLeadTagsAsync();
            this.cbStatus.ItemsSource = await _statusDataService.GetAllLeadStatussAsync();
        }

        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private string[]? SearchPincode(string pincode)
        {
            string[]? loc = null;

            if (!string.IsNullOrEmpty(pincode))
            {
                HttpClient client = new HttpClient();
                var address = @"https://api.postalpincode.in/pincode/" + pincode;
                client.BaseAddress = new Uri(address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(address).Result;
                if (response.IsSuccessStatusCode)
                {
                    loc = new string[4];
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    JArray data = (JArray)JsonConvert.DeserializeObject(responseBody);
                    var data1 = data.First;
                    var article = (JObject)data1["PostOffice"].Children().First();
                    loc[0] = article.SelectToken("District").Value<string>();
                    loc[1] = article.SelectToken("State").Value<string>();
                    loc[2] = article.SelectToken("Country").Value<string>();
                    loc[3] = article.SelectToken("Name").Value<string>();
                }
            }

            return loc;
        }

        private void tbPincode_LostFocus(object sender, RoutedEventArgs e)
        {
            var data = SearchPincode(this.tbPincode.Text);
            if (data != null)
            {
                this.tbDist.Text = data[0].ToString();
                this.tbState.Text = data[1].ToString();
                this.tbCountry.Text = data[2].ToString();
                this.tbCity.Text = data[3].ToString();
            }
        }
    }
}
