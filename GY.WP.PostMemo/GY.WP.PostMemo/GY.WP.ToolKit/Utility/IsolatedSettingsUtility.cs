//-----------------------------------------------------------------------
// <copyright file="IsolatedSettingsUtility.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.ToolKit.Utility
{
    using System.IO.IsolatedStorage;

    /// <summary>
    ///
    /// </summary>
    public static class IsolatedSettingsUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Update(string key, object value)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
            }

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Remove(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(string key)
        {
            T result;

            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out result))
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factor"></param>
        public static void Increase(string key, int factor)
        {
            IsolatedSettingsUtility.Update(key, IsolatedSettingsUtility.GetValue<int>(key) + factor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factor"></param>
        public static void Decrease(string key, int factor)
        {
            IsolatedSettingsUtility.Update(key, IsolatedSettingsUtility.GetValue<int>(key) - factor);
        }
    }
}
