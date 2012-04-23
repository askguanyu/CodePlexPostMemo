//-----------------------------------------------------------------------
// <copyright file="MemoDesktop.xaml.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Views
{
    using System;
    using System.Linq;
    using System.Windows;
    using GY.WP.PostMemo.Localization;
    using GY.WP.PostMemo.Models;
    using GY.WP.PostMemo.ViewModels;
    using GY.WP.ToolKit.ExtensionMethods;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    ///
    /// </summary>
    public partial class MemoDesktop : PhoneApplicationPage
    {
        public MemoDesktopViewModel ViewModel { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public MemoDesktop()
        {
            InitializeComponent();
            InitializeAppBar();

            this.ViewModel = new MemoDesktopViewModel();
            this.DataContext = this.ViewModel;
        }

        private void InitializeAppBar()
        {
            this.ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarPost = new ApplicationBarIconButton(new Uri("/Images/appbar.send.rest.png", UriKind.Relative)) { Text = AppResources.AppBar_Post };
            appBarPost.Click += appBarPost_Click;
            this.ApplicationBar.Buttons.Add(appBarPost);

            ApplicationBarMenuItem appBarFullView = new ApplicationBarMenuItem(AppResources.AppBar_FullView);
            this.ApplicationBar.MenuItems.Add(appBarFullView);
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
                switch (MessageBox.Show("", AppResources.MsgBox_Exit, MessageBoxButton.OKCancel))
                {
                    case MessageBoxResult.Cancel:
                    case MessageBoxResult.No:
                    case MessageBoxResult.None:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.OK:
                    case MessageBoxResult.Yes:
                        while (NavigationService.BackStack.Any())
                        {
                            NavigationService.RemoveBackEntry();
                        }
                        e.Cancel = false;
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
                var newMemo = new MemoModel { MemoContent = this.chatBubbleTextBoxMemo.Text };
                this.ViewModel.AddMemo(newMemo);
                this.listBoxMemoList.ScrollIntoView(newMemo);
                this.chatBubbleTextBoxMemo.Text = string.Empty;
            }
        }

        private void menuItemDelete_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            this.ViewModel.DeleteMemo(memoModel);
        }

        private void menuItemCopy_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            Clipboard.SetText(memoModel.MemoContent);
        }
    }
}