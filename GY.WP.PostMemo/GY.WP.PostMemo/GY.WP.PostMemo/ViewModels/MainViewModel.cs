//-----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using GY.WP.PostMemo.Models;
    using Microsoft.Practices.Prism.ViewModel;

    /// <summary>
    ///
    /// </summary>
    public class MainViewModel : NotificationObject
    {
        /// <summary>
        ///
        /// </summary>
        public MainViewModel()
        {
            this.MemoList = new ObservableCollection<MemoModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MemoModel> MemoList
        {
            get;
            private set;
        }

        /// <summary>
        ///Gets or sets a value indicating whether
        /// </summary>
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Load data

            this.IsDataLoaded = true;
        }
    }
}