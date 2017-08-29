﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using WeCode_Next.DataModel;
using Windows.UI.Xaml.Controls;

namespace WeCode_Next.Pages
{
    public sealed partial class WindowsBlog : Page
    {
        public WindowsBlog()
        {
            this.InitializeComponent();
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                NC.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                main.Visibility = Windows.UI.Xaml.Visibility.Visible;
                NewsLoad(false);
                NewsLoad(true);
            }
            else {
                NC.Visibility = Windows.UI.Xaml.Visibility.Visible;
                main.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

        }

        private async void NewsLoad(bool type)
        {
            string uri = (type) ? "https://blogs.windows.com/buildingapps/feed/" : "https://blogs.windows.com/windowsexperience/tag/windows-insider-program/feed/";
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(uri));
            XDocument news = XDocument.Load(await response.Content.ReadAsStreamAsync());
            var data = from item in news.Descendants("item")
                       select new News
                       {
                           Title = (string)item.Element("title"),
                           PubDate = (string)item.Element("pubDate"),
                           Link = (string)item.Element("link")
                       };
            if (type)
            {
                wd.ItemsSource = data;
            }
            else
            {
                wip.ItemsSource = data;
            }
        }
    }
}