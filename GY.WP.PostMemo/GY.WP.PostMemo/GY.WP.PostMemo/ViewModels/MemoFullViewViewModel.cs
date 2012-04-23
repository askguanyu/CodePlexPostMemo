using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.ViewModel;
using GY.WP.PostMemo.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace GY.WP.PostMemo.ViewModels
{
    public class MemoFullViewViewModel : NotificationObject
    {
        /// <summary>
        /// LINQ to SQL data context for the local database.
        /// </summary>
        private MemoDataContext memoDB;

        public MemoFullViewViewModel()
        {
            memoDB = new MemoDataContext(Constants.DBConnectionString);
            this.LoadDatabase();
        }

        public ObservableCollection<MemoModel> MemoListAll
        {
            get;
            set;
        }

        public ObservableCollection<MemoModel> MemoListDone
        {
            get;
            set;
        }

        public ObservableCollection<MemoModel> MemoListTodo
        {
            get;
            set;
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadDatabase()
        {

            var memoInDB = from MemoModel memo in memoDB.MemoTable
                           select memo;

            MemoListAll = new ObservableCollection<MemoModel>(memoInDB);
            MemoListDone = new ObservableCollection<MemoModel>(memoInDB.Where(p => p.IsComplete));
            MemoListTodo = new ObservableCollection<MemoModel>(memoInDB.Where(p => !p.IsComplete));
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

        internal void DeleteMemo(MemoModel memoModel)
        {
            this.MemoListAll.Remove(memoModel);

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
