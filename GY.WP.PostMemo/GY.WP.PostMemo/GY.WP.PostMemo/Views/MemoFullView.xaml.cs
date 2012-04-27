//-----------------------------------------------------------------------
// <copyright file="MemoFullView.xaml.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Views
{
    using System;
    using System.Windows;
    using GY.WP.PostMemo.Localization;
    using GY.WP.PostMemo.Models;
    using GY.WP.PostMemo.ViewModels;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    ///
    /// </summary>
    public partial class MemoFullView : PhoneApplicationPage
    {
        /// <summary>
        ///
        /// </summary>
        public MemoFullView()
        {
            this.InitializeComponent();
            this.InitializeAppBar();
            this.ViewModel = new MemoFullViewViewModel();
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public MemoFullViewViewModel ViewModel
        {
            get;
            private set;
        }

        /// <summary>
        ///
        /// </summary>
        private void InitializeAppBar()
        {
            this.ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarHelp = new ApplicationBarIconButton(new Uri("/Images/appbar.feature.settings.rest.png", UriKind.Relative)) { Text = AppResources.AppBar_Help };
            appBarHelp.Click += new EventHandler(appBarHelp_Click);
            this.ApplicationBar.Buttons.Add(appBarHelp);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void appBarHelp_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MemoSettings", UriKind.Relative));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDone_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            this.ViewModel.DoneMemo(memoModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemCopy_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            Clipboard.SetText(memoModel.MemoContent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDelete_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            this.ViewModel.DeleteMemo(memoModel);
        }
    }
}