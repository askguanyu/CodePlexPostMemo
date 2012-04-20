//-----------------------------------------------------------------------
// <copyright file="DateTimeToStringConverter.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.ValueConverters
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Data;
    using GY.WP.PostMemo.Localization;

    /// <summary>
    ///
    /// </summary>
    public class DateTimeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a
        /// <see cref="T:System.DateTime"/>
        /// object into a string appropriately formatted to
        /// display full date and time information.
        /// This format can be found in email.
        /// </summary>
        /// <remarks>
        /// This format never displays the year.
        /// </remarks>
        /// <param name="value">The given date and time.</param>
        /// <param name="targetType">
        /// The type corresponding to the binding property, which must be of
        /// <see cref="T:System.String"/>.
        /// </param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">(Not used).</param>
        /// <returns>The given date and time as a string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Target value must be a System.DateTime object.
            if (!(value is DateTime))
            {
                throw new ArgumentException(AppResources.InvalidDateTimeArgument);
            }

            StringBuilder result = new StringBuilder(string.Empty);

            DateTime given = (DateTime)value;

            if (DateTimeFormatHelper.IsCurrentCultureJapanese() || DateTimeFormatHelper.IsCurrentCultureKorean())
            {
                result.AppendFormat(CultureInfo.CurrentCulture, "{0} {1} {2}",
                                        DateTimeFormatHelper.GetMonthAndDay(given),
                                        DateTimeFormatHelper.GetAbbreviatedDay(given),
                                        DateTimeFormatHelper.GetShortTime(given));
            }
            else if (DateTimeFormatHelper.IsCurrentCultureTurkish())
            {
                result.AppendFormat(CultureInfo.CurrentCulture, "{0}, {1} {2}",
                                        DateTimeFormatHelper.GetMonthAndDay(given),
                                        DateTimeFormatHelper.GetAbbreviatedDay(given),
                                        DateTimeFormatHelper.GetShortTime(given));
            }
            else
            {
                result.AppendFormat(CultureInfo.CurrentCulture, "{0} {1}, {2}",
                                        DateTimeFormatHelper.GetAbbreviatedDay(given),
                                        DateTimeFormatHelper.GetMonthAndDay(given),
                                        DateTimeFormatHelper.GetShortTime(given));
            }

            if (DateTimeFormatHelper.IsCurrentUICultureFrench())
            {
                result.Replace(",", string.Empty);
            }

            return result.ToString();
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
