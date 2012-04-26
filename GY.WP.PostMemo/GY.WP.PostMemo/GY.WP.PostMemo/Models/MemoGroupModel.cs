//-----------------------------------------------------------------------
// <copyright file="MemoGroupModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Models
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.Practices.Prism.ViewModel;

    /// <summary>
    ///
    /// </summary>
    public class MemoGroupModel : NotificationObject
    {
        /// <summary>
        ///Gets or sets
        /// </summary>
        public DateTime MemoGroupTitleDate
        {
            get;
            set;
        }

        /// <summary>
        ///Gets
        /// </summary>
        public int MemoCount
        {
            get
            {
                return MemoGroupTitleDate != null ? this.MemoGroupList.Count : 0;
            }
        }

        private ObservableCollection<MemoModel> _memoGroupList;

        /// <summary>
        ///Gets or sets
        /// </summary>
        public ObservableCollection<MemoModel> MemoGroupList
        {
            get
            {
                return this._memoGroupList;
            }
            set
            {
                this._memoGroupList = value;
                RaisePropertyChanged(() => MemoGroupList);
                this._memoGroupList.CollectionChanged += (s, e) => RaisePropertyChanged(() => MemoCount);
            }
        }
    }
}
