//-----------------------------------------------------------------------
// <copyright file="MemoDesktop.xaml.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Views
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using GY.WP.PostMemo.Localization;
    using GY.WP.PostMemo.Models;
    using GY.WP.ToolKit.ExtensionMethods;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    ///
    /// </summary>
    public partial class MemoDesktop : PhoneApplicationPage
    {
        /// <summary>
        ///
        /// </summary>
        public MemoDesktop()
        {
            InitializeComponent();
            InitializeAppBar();

            this.MemoList = new ObservableCollection<MemoModel>();
            this.listBoxMemoList.ItemsSource = this.MemoList;
        }

        private void InitializeAppBar()
        {
            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.Opacity = 0.5;

            ApplicationBarIconButton appBarPost = new ApplicationBarIconButton(new Uri("/Images/appbar.send.rest.png", UriKind.Relative)) { Text = AppResources.AppBar_Post };
            appBarPost.Click += appBarPost_Click;
            this.ApplicationBar.Buttons.Add(appBarPost);

            ApplicationBarMenuItem appBarFullView = new ApplicationBarMenuItem(AppResources.AppBar_FullView);
            this.ApplicationBar.MenuItems.Add(appBarFullView);
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public ObservableCollection<MemoModel> MemoList
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (!string.IsNullOrEmpty(this.chatBubbleTextBoxMemo.Text))
            {
                this.chatBubbleTextBoxMemo.Text = string.Empty;
                e.Cancel = true;
            }
            else
            {
                switch (MessageBox.Show("", "Exit ?", MessageBoxButton.OKCancel))
                {
                    case MessageBoxResult.Cancel:
                    case MessageBoxResult.No:
                    case MessageBoxResult.None:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.OK:
                    case MessageBoxResult.Yes:
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void appBarPost_Click(object sender, EventArgs e)
        {
            if (!this.chatBubbleTextBoxMemo.Text.IsNullOrEmpty())
            {
                var newMemo = new MemoModel { Content = this.chatBubbleTextBoxMemo.Text };
                this.MemoList.Add(newMemo);
                this.listBoxMemoList.ScrollIntoView(newMemo);
                this.chatBubbleTextBoxMemo.Text = string.Empty; 
            }
        }
    }
}