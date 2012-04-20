//-----------------------------------------------------------------------
// <copyright file="MemoModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Models
{
    using System;
    using Microsoft.Practices.Prism.ViewModel;

    /// <summary>
    ///
    /// </summary>
    public class MemoModel : NotificationObject
    {
        /// <summary>
        ///
        /// </summary>
        private DateTime _postDateTime;

        /// <summary>
        ///
        /// </summary>
        private string _content;

        /// <summary>
        ///
        /// </summary>
        public MemoModel()
        {
            this.CreateUtcTime = DateTime.UtcNow;
            this._postDateTime = DateTime.Now;
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public DateTime PostDateTime
        {
            get
            {
                return _postDateTime;
            }

            set
            {
                if (value != _postDateTime)
                {
                    _postDateTime = value;
                    RaisePropertyChanged(() => PostDateTime);
                }
            }
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public DateTime CreateUtcTime
        {
            get;
            private set;
        }

        public string DateTimeString
        {
            get
            {
                return this.PostDateTime.ToString();
            }
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                if (value != _content)
                {
                    _content = value;
                    RaisePropertyChanged(() => Content);
                }
            }
        }
    }
}
