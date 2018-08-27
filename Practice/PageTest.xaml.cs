using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Practice
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
    public partial class PageTest : ContentPage
    {
        private const string Url = "https://jsonplaceholder.typicode.com/posts";
        private HttpClient _client = new HttpClient();
        private ObservableCollection<Post> _lstObserCollections;
        public PageTest()
        {
            InitializeComponent();
            //lstViewRest.ItemsSource = new List<Contacts>
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var content = await _client.GetStringAsync(Url);
            var jsonPost = JsonConvert.DeserializeObject<List<Post>>(content);
            _lstObserCollections = new ObservableCollection<Post>(jsonPost);
            lstViewRest.ItemsSource = _lstObserCollections;
        }
        
        private async void BtnAdd_OnClicked(object sender, EventArgs e)
        {
            var post = new Post {Title = "New Title" + DateTime.Now.Ticks};
            _lstObserCollections.Insert(0, post);

            var content = JsonConvert.SerializeObject(post);
            await _client.PostAsync(Url, new StringContent(content));

        }

        private async void BtnUpdate_OnClicked(object sender, EventArgs e)
        {
            var post = _lstObserCollections[0];
            post.Title += "Updated";

            var content = JsonConvert.SerializeObject(post);
            await _client.PutAsync(Url + "/" + post.Id, new StringContent(content));
        }

        private async void BtnDelete_OnClicked(object sender, EventArgs e)
        {
            var post = _lstObserCollections[0];

            _lstObserCollections.Remove(post);

            await _client.DeleteAsync(Url + "/" + post.Id);
        }
    }
}
