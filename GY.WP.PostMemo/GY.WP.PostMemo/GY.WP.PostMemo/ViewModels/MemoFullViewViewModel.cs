//-----------------------------------------------------------------------
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
    using GY.WP.ToolKit.Utility;

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
            this.InitializeColors();
        }

        public void InitializeColors()
        {
            this.MemoListTodoBackground = AccentColorNameToBrushConverter.ColorNameToBrushDictionary[(IsolatedSettingsUtility.GetValue<ColorModel>(Constants.KeyTodoMemoListColor) ?? new ColorModel()).ColorName];
            this.MemoListDoneBackground = AccentColorNameToBrushConverter.ColorNameToBrushDictionary[(IsolatedSettingsUtility.GetValue<ColorModel>(Constants.KeyDoneMemoListColor) ?? new ColorModel()).ColorName];
            this.MemoListAllBackground = AccentColorNameToBrushConverter.ColorNameToBrushDictionary[(IsolatedSettingsUtility.GetValue<ColorModel>(Constants.KeyAllMemoListColor) ?? new ColorModel()).ColorName];
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

        private SolidColorBrush _memoListTodoBackground;

        public SolidColorBrush MemoListTodoBackground
        {
            get
            {
                return this._memoListTodoBackground;
            }
            set
            {
                this._memoListTodoBackground = value;
                RaisePropertyChanged(() => MemoListTodoBackground);
            }
        }

        private SolidColorBrush _memoListDoneBackground;

        public SolidColorBrush MemoListDoneBackground
        {
            get
            {
                return this._memoListDoneBackground;
            }
            set
            {
                this._memoListDoneBackground = value;
                RaisePropertyChanged(() => MemoListDoneBackground);
            }
        }

        private SolidColorBrush _memoListAllBackground;

        public SolidColorBrush MemoListAllBackground
        {
            get
            {
                return this._memoListAllBackground;
            }
            set
            {
                this._memoListAllBackground = value;
                RaisePropertyChanged(() => MemoListAllBackground);
            }
        }

        /// <summary>
        /// Query database and load the collections and list used by the pivot pages.
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
