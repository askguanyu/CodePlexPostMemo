using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using GY.WP.PostMemo.Models;

namespace GY.WP.PostMemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            //this.listBoxMemoList.ScrollIntoView(listBoxMemoList.Items[listBoxMemoList.Items.Count - 1]);
        }

        private void appBarPost_Click(object sender, EventArgs e)
        {
            var newMemo = new MemoModel { Content = this.chatBubbleTextBoxMemo.Text };
            App.ViewModel.MemoList.Add(newMemo);
            this.listBoxMemoList.ScrollIntoView(newMemo);
            this.chatBubbleTextBoxMemo.Text = string.Empty;
        }

        private void chatBubbleTextBoxMemo_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (this.scrollViewer.VerticalOffset < this.scrollViewer.ScrollableHeight)
            //{
            //    this.scrollViewer.ScrollToVerticalOffset(this.scrollViewer.ScrollableHeight);
            //}
        }

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
    }
}