using ProjetoFinal.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Kinect;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace ProjetoFinal
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            List<GameNew> list = new List<GameNew>();
            list = ReadRSS();
            itemsViewSource.Source = list;
        }
        
        /// <summary>
        /// Lê o RSS de http://store.steampowered.com/feeds/news.xml e preenche a lista com os dados da notícia
        /// </summary>
        /// <returns>
        /// Retorna uma lista do tipo GameNew com os dados noticiados
        /// Valores default:
        ///     GameNew.Image = "logo/notfound.png"
        ///     GameNew.GameID = "0";
        /// </returns>
        List<GameNew> ReadRSS(){
            List<GameNew> list = new List<GameNew>();
            GameNew gnew = new GameNew();
            try
            {
                XmlReader reader;
                try
                {
                    reader = XmlReader.Create("http://store.steampowered.com/feeds/news.xml");
                } catch(Exception){
                    Debug.WriteLine("Erro, carregando do cache");
                    reader = XmlReader.Create("news.xml");
                }
                while(reader.Read()){
                    if(reader.NodeType == XmlNodeType.Element){
                        switch(reader.Name){
                            case "item":
                                gnew = new GameNew();
                                break;
                            case "title":
                                gnew.Title = reader.ReadElementContentAsString();
                                break;
                            case "link":
                                gnew.Link = reader.ReadElementContentAsString();
                                break;
                            case "dc:date":
                                gnew.Date = reader.ReadElementContentAsString().Replace("-0700", string.Empty).Replace("T", " ");
                                break;
                            case "author":
                                gnew.Author = reader.ReadElementContentAsString();
                                break;
                            case "content:encoded":
                                string aux = reader.ReadElementContentAsString();
                                gnew.Content = Regex.Replace(aux, @"<[^>]*>|&.+?;", String.Empty);
                                gnew.Image = Regex.Match(aux, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                                gnew.GameID = Regex.Match(aux, "(?:http://store.steampowered.com/app/)([0-9]{1,7})", RegexOptions.Singleline).Groups[1].Value;
                                if(String.IsNullOrEmpty(gnew.Image)){
                                    gnew.Image = "logo/notfound.png";
                                }
                                if(String.IsNullOrEmpty(gnew.GameID)){
                                    gnew.GameID = "0";
                                }
                                list.Add(gnew);
                                break;
                        }
                    }
                }
            } catch(Exception ex){
                Debug.WriteLine("ERRO: " + ex);
            }
            return list;
        }
        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            GameNew output = e.ClickedItem as GameNew;
            this.Frame.Navigate(typeof(GroupDetailPage1), output);
        }
    }
}
