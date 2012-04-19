//-----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.ToolKit.ExtensionMethods
{
    using System;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Media.Imaging;

    /// <summary>
    ///
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] EncryptData(this byte[] source, byte[] optional = null)
        {
            return ProtectedData.Protect(source, optional);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] DecryptData(this byte[] source, byte[] optional = null)
        {
            return ProtectedData.Unprotect(source, optional);
        }

        /// <summary>
        /// Base64 decodes a string
        /// </summary>
        /// <param name="source">A base64 encoded string</param>
        /// <returns>Decoded string</returns>
        public static string Base64Decode(this string source)
        {
            byte[] buffer = Convert.FromBase64String(source);

            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Base64 encodes a string
        /// </summary>
        /// <param name="source">String to encode</param>
        /// <returns>A base64 encoded string</returns>
        public static string Base64Encode(this string source)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(source);

            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string source)
        {
            return Encoding.Unicode.GetBytes(source);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncryptString(this string source)
        {
            HashAlgorithm crypto = new SHA256Managed();

            //return Convert.ToBase64String(crypto.ComputeHash(Encoding.UTF8.GetBytes(source)));
            return crypto.ComputeHash(Encoding.UTF8.GetBytes(source)).ToHexString(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="addSpace"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] source, bool addSpace = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (source.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder(source.Length * 2);

            if (addSpace)
            {
                foreach (byte hex in source)
                {
                    result.AppendFormat("{0:X2}", hex);
                    result.Append(" ");
                }
            }
            else
            {
                foreach (byte hex in source)
                {
                    result.AppendFormat("{0:X2}", hex);
                }
            }

            return result.ToString().Trim();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string FormatWith(this string source, params object[] parameters)
        {
            return string.Format(source, parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToTimeSpanString(this TimeSpan source)
        {
            if (source.TotalSeconds < 60)
            {
                return "{0}秒".FormatWith(source.Seconds);
            }

            if (source.TotalMinutes < 60)
            {
                return "{0}分{1}秒".FormatWith(source.Minutes, source.Seconds);
            }

            if (source.Hours < 24)
            {
                return "{0}时{1}分{2}秒".FormatWith(source.Hours, source.Minutes, source.Seconds);
            }
            else
            {
                return "{0}天{1}时{2}分{3}秒".FormatWith(source.Days, source.Hours, source.Minutes, source.Seconds);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object InvokeMethod(this object source, string method, object[] parameters)
        {
            MethodInfo methodInfo = source.GetType().GetMethod(method);
            return methodInfo.Invoke(source, parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToYesNoString(this bool source)
        {
            return source ? "是" : "否";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToCHS(this DayOfWeek source)
        {
            switch (source)
            {
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期天";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Only accept 0 360 -360 270 -270 90 -90 -180 180 逆时针旋转
        /// </summary>
        /// <param name="source">WriteableBitmap</param>
        /// <param name="counterclockwiseAngle">Counterclockwise Angle</param>
        /// <returns>WriteableBitmap rotated without cropping</returns>
        public static WriteableBitmap Rotate(this WriteableBitmap source, int counterclockwiseAngle)
        {
            int[, ] writeableBitmapArray = new int[source.PixelWidth, source.PixelHeight];

            for (int c = 0; c < source.PixelWidth; c++)
            {
                for (int r = 0; r < source.PixelHeight; r++)
                {
                    writeableBitmapArray[c, r] = source.Pixels[source.PixelWidth * r + c];
                }
            }

            WriteableBitmap result;
            int[, ] resultWriteableBitmapArray;

            if (counterclockwiseAngle == 0 || counterclockwiseAngle == 360 || counterclockwiseAngle == -360)
            {
                result = new WriteableBitmap(source.PixelWidth, source.PixelHeight);
                resultWriteableBitmapArray = new int[source.PixelWidth, source.PixelHeight];
                resultWriteableBitmapArray = writeableBitmapArray;
            }
            else if (counterclockwiseAngle == 90 || counterclockwiseAngle == -270)
            {
                result = new WriteableBitmap(source.PixelHeight, source.PixelWidth);
                resultWriteableBitmapArray = new int[source.PixelHeight, source.PixelWidth];
                for (int c = 0; c < writeableBitmapArray.GetLength(0); c++)
                {
                    for (int r = 0; r < writeableBitmapArray.GetLength(1); r++)
                    {
                        resultWriteableBitmapArray[r, writeableBitmapArray.GetLength(0) - c - 1] = writeableBitmapArray[c, r];
                    }
                }
            }
            else if (counterclockwiseAngle == 180 || counterclockwiseAngle == -180)
            {
                result = new WriteableBitmap(source.PixelWidth, source.PixelHeight);
                resultWriteableBitmapArray = new int[source.PixelWidth, source.PixelHeight];
                for (int c = 0; c < writeableBitmapArray.GetLength(0); c++)
                {
                    for (int r = 0; r < writeableBitmapArray.GetLength(1); r++)
                    {
                        resultWriteableBitmapArray[writeableBitmapArray.GetLength(0) - c - 1, writeableBitmapArray.GetLength(1) - r - 1] = writeableBitmapArray[c, r];
                    }
                }
            }
            else if (counterclockwiseAngle == 270 || counterclockwiseAngle == -90)
            {
                result = new WriteableBitmap(source.PixelHeight, source.PixelWidth);
                resultWriteableBitmapArray = new int[source.PixelHeight, source.PixelWidth];
                for (int c = 0; c < writeableBitmapArray.GetLength(0); c++)
                {
                    for (int r = 0; r < writeableBitmapArray.GetLength(1); r++)
                    {
                        resultWriteableBitmapArray[writeableBitmapArray.GetLength(1) - r - 1, c] = writeableBitmapArray[c, r];
                    }
                }
            }
            else
            {
                return null;
            }

            for (int r = 0; r < resultWriteableBitmapArray.GetLength(1); r++)
            {
                for (int c = 0; c < resultWriteableBitmapArray.GetLength(0); c++)
                {
                    result.Pixels[result.PixelWidth * r + c] = resultWriteableBitmapArray[c, r];
                }
            }

            return result;
        }
    }
}
