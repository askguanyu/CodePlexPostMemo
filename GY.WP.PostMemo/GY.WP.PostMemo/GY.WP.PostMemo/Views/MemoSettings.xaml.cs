//-----------------------------------------------------------------------
// <copyright file="MemoSettings.xaml.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Views
{
    using System.Reflection;
    using System.Windows;
    using GY.WP.PostMemo.Localization;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Tasks;

    /// <summary>
    ///
    /// </summary>
    public partial class MemoSettings : PhoneApplicationPage
    {
        /// <summary>
        ///
        /// </summary>
        public MemoSettings()
        {
            InitializeComponent();
            Dispatcher.BeginInvoke(() => { textBlockVersion.Text = "V " + Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1]; });
            Dispatcher.BeginInvoke(() => { textBlockRoadmap.Text = AppResources.AppRoadmap; });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFeedback_Click(object sender, RoutedEventArgs e)
        {
            new MarketplaceReviewTask().Show();
        }
    }
}