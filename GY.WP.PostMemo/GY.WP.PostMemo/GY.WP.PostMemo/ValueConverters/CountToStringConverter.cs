//-----------------------------------------------------------------------
// <copyright file="CountToStringConverter.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.ValueConverters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using GY.WP.PostMemo.Localization;

    /// <summary>
    ///
    /// </summary>
    public class CountToStringConverter : IValueConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                throw new ArgumentException(AppResources.InvalidIntArgument);
            }

            int given = (int)value;

            return string.Format(CultureInfo.CurrentCulture, AppResources.StringFormat_Count, given);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">(Not used).</param>
        /// <param name="targetType">(Not used).</param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">(Not used).</param>
        /// <returns>null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
