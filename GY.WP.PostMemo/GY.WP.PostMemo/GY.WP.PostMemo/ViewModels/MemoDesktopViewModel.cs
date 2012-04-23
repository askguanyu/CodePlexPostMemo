//-----------------------------------------------------------------------
// <copyright file="MemoDesktopViewModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.ViewModels
{
    using System.Collections.ObjectModel;
    using GY.WP.PostMemo.Models;
    using Microsoft.Practices.Prism.ViewModel;
    using System.Data.Linq;

    /// <summary>
    ///
    /// </summary>
    public class MemoDesktopViewModel : NotificationObject
    {
        /// <summary>
        /// LINQ to SQL data context for the local database.
        /// </summary>
        private MemoDataContext memoDB;

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        public MemoDesktopViewModel()
        {
            memoDB = new MemoDataContext(Constants.DBConnectionString);
            this.MemoList = new ObservableCollection<MemoModel>();
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
        public void AddMemo(MemoModel memoModel)
        {
            memoDB.MemoTable.InsertOnSubmit(memoModel);
            memoDB.SubmitChanges();
            MemoList.Add(memoModel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="memoModel"></param>
        public void DeleteMemo(MemoModel memoModel)
        {
            MemoList.Remove(memoModel);
            memoDB.MemoTable.DeleteOnSubmit(memoModel);
            memoDB.SubmitChanges();
        }
    }
}
