﻿//-----------------------------------------------------------------------
// <copyright file="MemoFullViewViewModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using GY.WP.PostMemo.Models;
    using Microsoft.Practices.Prism.ViewModel;
    using System.Windows.Media;
    using GY.WP.PostMemo.ValueConverters;

    /// <summary>
    ///
    /// </summary>
    public class MemoFullViewViewModel : NotificationObject
    {
        /// <summary>
        /// LINQ to SQL data context for the local database.
        /// </summary>
        private MemoDataContext memoDB;

        /// <summary>
        ///
        /// </summary>
        public MemoFullViewViewModel()
        {
            memoDB = new MemoDataContext(Constants.DBConnectionString);
            this.LoadDatabase();
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public ObservableCollection<MemoGroupModel> MemoGroupListAll
        {
            get;
            set;
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public ObservableCollection<MemoModel> MemoListDone
        {
            get;
            set;
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public ObservableCollection<MemoModel> MemoListTodo
        {
            get;
            set;
        }

        // Query database and load the collections and list used by the pivot pages.

        /// <summary>
        ///
        /// </summary>
        public void LoadDatabase()
        {
            var memoInDB = from MemoModel memo in memoDB.MemoTable
                           select memo;

            var mempGroup = from MemoModel memo in memoDB.MemoTable
                            group memo by memo.MemoDateTime.Date into g
                            select new MemoGroupModel { MemoGroupTitleDate = g.Key, MemoGroupList = new ObservableCollection<MemoModel>(g) };

            MemoGroupListAll = new ObservableCollection<MemoGroupModel>(mempGroup);
            MemoListDone = new ObservableCollection<MemoModel>(memoInDB.Where(p => p.IsComplete));
            MemoListTodo = new ObservableCollection<MemoModel>(memoInDB.Where(p => !p.IsComplete));
        }

        public SolidColorBrush MemoListTodoColor
        {
            get
            {
                return AccentColorNameToBrushConverter.ColorNameToBrushDictionary.Values.ToArray()[1];
            }
        }

        /// <summary>
        /// Write changes in the data context to the database.
        /// </summary>
        public void SaveChangesToDB()
        {
            memoDB.SubmitChanges();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="memoModel"></param>
        public void DoneMemo(MemoModel memoModel)
        {
            memoModel.IsComplete = true;
            memoDB.SubmitChanges();
            this.MemoListTodo.Remove(memoModel);
            this.MemoListDone.Add(memoModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="memoModel"></param>
        internal void DeleteMemo(MemoModel memoModel)
        {
            this.MemoGroupListAll.First(p => p.MemoGroupTitleDate.Equals(memoModel.MemoDateTime.Date)).MemoGroupList.Remove(memoModel);

            if (memoModel.IsComplete)
            {
                this.MemoListDone.Remove(memoModel);
            }
            else
            {
                this.MemoListTodo.Remove(memoModel);
            }

            memoDB.MemoTable.DeleteOnSubmit(memoModel);
            memoDB.SubmitChanges();
        }
    }
}
