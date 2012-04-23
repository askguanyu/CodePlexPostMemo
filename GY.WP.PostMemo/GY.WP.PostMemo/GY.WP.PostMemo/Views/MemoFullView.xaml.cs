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
using GY.WP.PostMemo.ViewModels;
using GY.WP.PostMemo.Models;

namespace GY.WP.PostMemo.Views
{
    public partial class MemoFullView : PhoneApplicationPage
    {
        public MemoFullViewViewModel ViewModel { get; private set; }

        public MemoFullView()
        {
            InitializeComponent();
            this.ViewModel = new MemoFullViewViewModel();
            this.DataContext = this.ViewModel;
        }

        private void menuItemDone_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            this.ViewModel.DoneMemo(memoModel);
        }

        private void menuItemCopy_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            Clipboard.SetText(memoModel.MemoContent);
        }

        private void menuItemDelete_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var memoModel = ((MemoModel)(((MenuItem)(sender)).CommandParameter));
            this.ViewModel.DeleteMemo(memoModel);
        }
    }
}