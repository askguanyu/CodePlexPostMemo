//-----------------------------------------------------------------------
// <copyright file="MemoFullViewViewModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Media;
    using GY.WP.PostMemo.Models;
    using GY.WP.PostMemo.ValueConverters;
    using GY.WP.ToolKit.Utility;
    using Microsoft.Practices.Prism.ViewModel;

    /// <summary>
    ///
    /// </summary>
    public class MemoFullViewViewModel : NotificationObject
    {
        /// <summary>
        /// LINQ to SQL data context for the local database.
        /// </summary>
        private MemoDataContext _memoDB;

        /// <summary>
        ///
        /// </summary>
        private SolidColorBrush _memoListTodoBackground;

        /// <summary>
        ///
        /// </summary>
        private SolidColorBrush _memoListDoneBackground;

        /// <summary>
        ///
        /// </summary>
        private SolidColorBrush _memoListAllBackground;

        /// <summary>
        ///
        /// </summary>
        public MemoFullViewViewModel()
        {
            _memoDB = new MemoDataContext(Constants.DBConnectionString);
            this.LoadDatabase();
            this.InitializeColors();
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

        /// <summary>
        ///Gets or sets
        /// </summary>
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

        /// <summary>
        ///Gets or sets
        /// </summary>
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

        /// <summary>
        ///Gets or sets
        /// </summary>
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
        ///
        /// </summary>
        public void InitializeColors()
        {
            this.MemoListTodoBackground = AccentColorNameToBrushConverter.ColorNameToBrushDictionary[(IsolatedSettingsUtility.GetValue<ColorModel>(Constants.KeyTodoMemoListColor) ?? new ColorModel()).ColorName];
            this.MemoListDoneBackground = AccentColorNameToBrushConverter.ColorNameToBrushDictionary[(IsolatedSettingsUtility.GetValue<ColorModel>(Constants.KeyDoneMemoListColor) ?? new ColorModel()).ColorName];
            this.MemoListAllBackground = AccentColorNameToBrushConverter.ColorNameToBrushDictionary[(IsolatedSettingsUtility.GetValue<ColorModel>(Constants.KeyAllMemoListColor) ?? new ColorModel()).ColorName];
        }

        /// <summary>
        /// Query database and load the collections and list used by the pivot pages.
        /// </summary>
        public void LoadDatabase()
        {
            var memoInDB = from MemoModel memo in _memoDB.MemoTable
                           select memo;

            var memoGroup = from MemoModel memo in _memoDB.MemoTable
                            group memo by memo.MemoDateTime.Date into g
                            select new MemoGroupModel { MemoGroupTitleDate = g.Key, MemoGroupList = new ObservableCollection<MemoModel>(g) };

            MemoGroupListAll = new ObservableCollection<MemoGroupModel>(memoGroup);
            MemoListDone = new ObservableCollection<MemoModel>(memoInDB.Where(p => p.IsComplete));
            MemoListTodo = new ObservableCollection<MemoModel>(memoInDB.Where(p => !p.IsComplete));
        }

        /// <summary>
        /// Write changes in the data context to the database.
        /// </summary>
        public void SaveChangesToDB()
        {
            _memoDB.SubmitChanges();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="memoModel"></param>
        public void DoneMemo(MemoModel memoModel)
        {
            memoModel.IsComplete = true;
            _memoDB.SubmitChanges();
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

            _memoDB.MemoTable.DeleteOnSubmit(memoModel);
            _memoDB.SubmitChanges();
        }
    }
}
