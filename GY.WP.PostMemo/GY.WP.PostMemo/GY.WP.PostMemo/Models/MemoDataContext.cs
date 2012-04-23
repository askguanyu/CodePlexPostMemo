//-----------------------------------------------------------------------
// <copyright file="MemoDataContext.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Models
{
    using System.Data.Linq;

    /// <summary>
    ///
    /// </summary>
    public class MemoDataContext : DataContext
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString"></param>
        public MemoDataContext(string connectionString)
            : base(connectionString)
        {
            if (!this.DatabaseExists())
            {
                // Create the local database.
                this.CreateDatabase();
            }
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public Table<MemoModel> MemoTable;
    }
}
