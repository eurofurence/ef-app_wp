using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace Eurofurence.Companion.Data
{
    public class NavigationDataItem
    {
        public NavigationDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Content = content;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }


    public sealed class NavigationDataSource
    {
        private ObservableCollection<NavigationDataItem> _mainMenu = new ObservableCollection<NavigationDataItem>();
        public ObservableCollection<NavigationDataItem> MainMenu
        {
            get { return this._mainMenu; }
        }

        public async Task LoadAsync()
        {
            Uri dataUri = new Uri("ms-appx:///DataModel/NavigationData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);

            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["MainMenu"].GetArray();

            foreach (JsonValue menuItem in jsonArray)
            {
                JsonObject menuObject = menuItem.GetObject();
                NavigationDataItem group = new NavigationDataItem(menuObject["UniqueId"].GetString(),
                                                            menuObject["Title"].GetString(),
                                                            menuObject["Subtitle"].GetString(),
                                                            menuObject["ImagePath"].GetString(),
                                                            menuObject["Description"].GetString(), "Foo!");
                
                this.MainMenu.Add(group);
            }
        }
    }
}