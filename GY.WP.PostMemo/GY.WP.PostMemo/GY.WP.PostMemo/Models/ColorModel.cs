//-----------------------------------------------------------------------
// <copyright file="ColorModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.Models
{
    using GY.WP.PostMemo.Localization;

    /// <summary>
    ///
    /// </summary>
    public class ColorModel
    {
        /// <summary>
        ///
        /// </summary>
        public ColorModel()
        {
            this.ColorName = "system";
            this.ColorDisplayName = AppResources.Settings_Color_system;
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public string ColorName
        {
            get;
            set;
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public string ColorDisplayName
        {
            get;
            set;
        }
    }
}
