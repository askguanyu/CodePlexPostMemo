//-----------------------------------------------------------------------
// <copyright file="MemoModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Models
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    /// <summary>
    ///
    /// </summary>
    [Table]
    public class MemoModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public MemoModel()
        {
            this._memoDateTime = DateTime.Now;
            this._isComplete = false;
        }

        // Define ID: private field, public property, and database column.
        private int _memoId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int MemoId
        {
            get { return _memoId; }
            set
            {
                if (_memoId != value)
                {
                    NotifyPropertyChanging("MemoId");
                    _memoId = value;
                    NotifyPropertyChanged("MemoId");
                }
            }
        }

        // Define Content: private field, public property, and database column.
        private string _memoContent;

        [Column]
        public string MemoContent
        {
            get { return _memoContent; }
            set
            {
                if (_memoContent != value)
                {
                    NotifyPropertyChanging("MemoContent");
                    _memoContent = value;
                    NotifyPropertyChanged("MemoContent");
                }
            }
        }

        // Define Create DateTime: private field, public property, and database column.
        private DateTime _memoDateTime;

        [Column]
        public DateTime MemoDateTime
        {
            get { return _memoDateTime; }
            set
            {
                if (_memoDateTime != value)
                {
                    NotifyPropertyChanging("MemoDateTime");
                    _memoDateTime = value;
                    NotifyPropertyChanged("MemoDateTime");
                }
            }
        }

        // Define completion value: private field, public property, and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;


        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
    }
}
