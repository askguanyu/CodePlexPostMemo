//-----------------------------------------------------------------------
// <copyright file="MemoSettingsViewModel.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.PostMemo.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using GY.WP.PostMemo.Localization;
    using GY.WP.PostMemo.Models;
    using GY.WP.ToolKit.ExtensionMethods;
    using GY.WP.ToolKit.Utility;
    using Microsoft.Practices.Prism.ViewModel;
    using Microsoft.Phone.Controls;

    /// <summary>
    ///
    /// </summary>
    public class MemoSettingsViewModel : NotificationObject
    {
        /// <summary>
        ///
        /// </summary>
        private List<ColorModel> _accentColorList = new List<ColorModel>
        {
            new ColorModel{ ColorName="system", ColorDisplayName=AppResources.Settings_Color_system},
            new ColorModel{ ColorName="magenta", ColorDisplayName=AppResources.Settings_Color_magenta},
            new ColorModel{ ColorName="purple", ColorDisplayName=AppResources.Settings_Color_purple},
            new ColorModel{ ColorName="teal", ColorDisplayName=AppResources.Settings_Color_teal},
            new ColorModel{ ColorName="lime", ColorDisplayName=AppResources.Settings_Color_lime},
            new ColorModel{ ColorName="brown", ColorDisplayName=AppResources.Settings_Color_brown},
            new ColorModel{ ColorName="pink", ColorDisplayName=AppResources.Settings_Color_pink},
            new ColorModel{ ColorName="mango", ColorDisplayName=AppResources.Settings_Color_mango},
            new ColorModel{ ColorName="blue", ColorDisplayName=AppResources.Settings_Color_blue},
            new ColorModel{ ColorName="red", ColorDisplayName=AppResources.Settings_Color_red},
            new ColorModel{ ColorName="green", ColorDisplayName=AppResources.Settings_Color_green}
        };

        /// <summary>
        /// An array of all the names of the accent colors.
        /// </summary>
        /// <summary>
        ///Gets
        /// </summary>
        public string AppVersionString
        {
            get
            {
                return "V {0}".FormatWith(Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1]);
            }
        }

        /// <summary>
        /// Returns an array of all the names of the accent colors.
        /// </summary>
        public ReadOnlyCollection<ColorModel> AccentColorList
        {
            get
            {
                return new ReadOnlyCollection<ColorModel>(_accentColorList);
            }
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public int TodoMemoListColorSelectedIndex
        {
            get
            {
                return IsolatedSettingsUtility.GetValue<int>(Constants.KeyTodoMemoListColorSelectedIndex);
            }

            set
            {
                IsolatedSettingsUtility.Update(Constants.KeyTodoMemoListColorSelectedIndex, value);
                IsolatedSettingsUtility.Update(Constants.KeyTodoMemoListColor, AccentColorList[value]);
            }
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public int DoneMemoListColorSelectedIndex
        {
            get
            {
                return IsolatedSettingsUtility.GetValue<int>(Constants.KeyDoneMemoListColorSelectedIndex);
            }

            set
            {
                IsolatedSettingsUtility.Update(Constants.KeyDoneMemoListColorSelectedIndex, value);
                IsolatedSettingsUtility.Update(Constants.KeyDoneMemoListColor, AccentColorList[value]);
            }
        }

        /// <summary>
        ///Gets or sets
        /// </summary>
        public int AllMemoListColorSelectedIndex
        {
            get
            {
                return IsolatedSettingsUtility.GetValue<int>(Constants.KeyAllMemoListColorSelectedIndex);
            }

            set
            {
                IsolatedSettingsUtility.Update(Constants.KeyAllMemoListColorSelectedIndex, value);
                IsolatedSettingsUtility.Update(Constants.KeyAllMemoListColor, AccentColorList[value]);
            }
        }
    }
}
