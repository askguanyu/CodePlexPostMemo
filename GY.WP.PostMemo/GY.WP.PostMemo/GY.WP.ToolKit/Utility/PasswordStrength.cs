//-----------------------------------------------------------------------
// <copyright file="PasswordStrength.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.ToolKit.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///
    /// </summary>
    public class PasswordStrength
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly int MAX_SCORE = 100;

        /// <summary>
        ///
        /// </summary>
        public static readonly int MIN_SCORE = 0;

        /// <summary>
        ///
        /// </summary>
        private StrengthAlgorithm mAlgo = null;

        /// <summary>
        ///
        /// </summary>
        LinkedList<BadReason> mBadReasons = new LinkedList<BadReason>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="cipherData"></param>
        /// <returns></returns>
        public bool IsValid(byte[] cipherData)
        {
            if (cipherData.GetLength(0) < 8)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strCipherData"></param>
        /// <returns></returns>
        public int GetStrength(string strCipherData)
        {
            mAlgo = new StrengthAlgorithm(strCipherData);
            int score = mAlgo.GetScore();
            if (MAX_SCORE < score)
            {
                score = MAX_SCORE;
            }
            else if (MIN_SCORE > score)
            {
                score = MIN_SCORE;
            }

            return score;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<BadReason> GetBadReasons()
        {
            if (null != mAlgo)
            {
                return mAlgo.GetBadReasons();
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cipherData"></param>
        /// <returns></returns>
        private int GetStrength(byte[] cipherData)
        {
            mAlgo = new StrengthAlgorithm(cipherData);
            int score = mAlgo.GetScore();
            if (MAX_SCORE < score)
            {
                score = MAX_SCORE;
            }
            else if (MIN_SCORE > score)
            {
                score = MIN_SCORE;
            }

            return score;
        }

        /// <summary>
        ///
        /// </summary>
        private class StrengthAlgorithm
        {
            //rule from password.mx500.com

            /// <summary>
            ///
            /// </summary>

            //private readonly string TAG = "StrengthAlgorithm";

            /// <summary>
            ///
            /// </summary>
            private readonly int MIN_COUNT = 8;

            /// <summary>
            ///
            /// </summary>
            private byte[] mDataChecked;

            /// <summary>
            ///
            /// </summary>
            private string mStrDataChecked;

            /// <summary>
            ///
            /// </summary>
            private int mScore = 0;

            /// <summary>
            ///
            /// </summary>
            private int mCount = 0;

            /// <summary>
            ///
            /// </summary>
            private int mCapLetterCount = 0;

            /// <summary>
            ///
            /// </summary>
            private int mLetterCount = 0;

            /// <summary>
            ///
            /// </summary>
            private int mNumberCount = 0;

            /// <summary>
            ///
            /// </summary>
            private int mSpecialCount = 0;

            /// <summary>
            ///
            /// </summary>
            /// <param name="checkedData"></param>
            public StrengthAlgorithm(byte[] checkedData)
            {
                mDataChecked = checkedData;
                mCount = mDataChecked.GetLength(0);
                GetGeneralCount();
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="checkedData"></param>
            public StrengthAlgorithm(string checkedData)
            {
                mStrDataChecked = checkedData;
                Encoding ec = new UTF8Encoding(true, true);
                mDataChecked = ec.GetBytes(mStrDataChecked);
                mCount = mDataChecked.GetLength(0);
                GetGeneralCount();
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public int GetCount()
            {
                return mCount;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public int GetScore()
            {
                mScore = 0;
                mScore += GetCountScore();
                mScore += GetCapLetterScore();
                mScore += GetLetterScore();
                mScore += GetNumberScore();
                mScore += GetSpecialScore();
                mScore += GetMidNumbLetterScore();
                mScore += GetBasicStandScore();
                mScore += GetNeighborCapitalLettersScore();
                mScore += GetNeighborLowerCaseLettersScore();
                mScore += GetNeighborNumberScore();
                mScore += GetOnlyLettersOrNumberScore();
                mScore += GetRepeatScore();
                mScore += GetContinuousCountScore();
                return mScore;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public List<BadReason> GetBadReasons()
            {
                List<BadReason> badReasons = new List<BadReason>();
                if (0 > GetNeighborNumberScore())
                {
                    badReasons.Add(new BadReason(BadReason.REASON_NEIGHBOR_NUMBERS));
                }

                if ((0 > GetNeighborLowerCaseLettersScore()) ||
                    (0 > GetNeighborCapitalLettersScore()))
                {
                    badReasons.Add(new BadReason(BadReason.REASON_NEIGHBOR_LETTERS));
                }

                if (0 > GetOnlyLettersOrNumberScore())
                {
                    if (0 == mNumberCount)
                    {
                        badReasons.Add(new BadReason(BadReason.REASON_ONLY_LETTERS));
                    }
                    else
                    {
                        badReasons.Add(new BadReason(BadReason.REASON_ONLY_NUMBERS));
                    }
                }

                if (0 > GetRepeatScore())
                {
                    badReasons.Add(new BadReason(BadReason.REASON_REPEAT));
                }

                if (0 > GetContinuousCountScore())
                {
                    badReasons.Add(new BadReason(BadReason.REASON_CONTINUOUS));
                }

                if (0 == GetBasicStandScore())
                {
                    if (mCount < MIN_COUNT)
                    {
                        badReasons.Add(new BadReason(BadReason.REASON_TOO_SHORT));
                    }
                }

                return badReasons;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetCountScore()
            {
                return GetCount() * 4;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetCapLetterScore()
            {
                if (mCapLetterCount > 0)
                {
                    return (mCount - mCapLetterCount) * 2;
                }
                else
                {
                    return 0;
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetLetterScore()
            {
                if (mLetterCount > 0)
                {
                    return (mCount - mLetterCount) * 2;
                }
                else
                {
                    return 0;
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNumberScore()
            {
                if (mNumberCount != mCount)
                {
                    return (mNumberCount * 4);
                }
                else
                {
                    return 0;
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetSpecialScore()
            {
                return mSpecialCount * 6;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetMidNumbLetterScore()
            {
                int n = GetMidNumSpecialCount();
                return (n * 2);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetBasicStandScore()
            {
                int score = 0;

                if (mCount < MIN_COUNT)
                {
                    return 0;
                }

                score++;

                if (mLetterCount > 0)
                {
                    score++;
                }

                if (mCapLetterCount > 0)
                {
                    score++;
                }

                if (mNumberCount > 0)
                {
                    score++;
                }

                if (mSpecialCount > 0)
                {
                    score++;
                }

                if (score < 4)
                {
                    return 0;
                }

                return (score * 2);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetOnlyLettersOrNumberScore()
            {
                if (0 < mSpecialCount)
                {
                    return 0;
                }

                if (mNumberCount == 0)
                {
                    //only have letters
                    return -(mLetterCount + mCapLetterCount);
                }

                if ((0 == mLetterCount) && (0 == mCapLetterCount))
                {
                    return -(mNumberCount);
                }

                return 0;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNeighborNumberScore()
            {
                int n = GetNeighborNumber();
                return -(n * 2);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNeighborCapitalLettersScore()
            {
                int n = GetNeighborCapitalLetters();
                return -(n * 2);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNeighborLowerCaseLettersScore()
            {
                int n = GetNeighborLowerCaseLetters();
                return -(n * 2);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNeighborNumber()
            {
                string p = @"[^0-9]*([0-9]{2,})[^0-9]*";
                return GetNeighbors(mStrDataChecked, p, @"[0-9]{2,}");
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNeighborCapitalLetters()
            {
                string p = @"[^A-Z]*([A-Z]{2,})[^A-Z]*";
                return GetNeighbors(mStrDataChecked, p, @"[A-Z]{2,}");
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNeighborLowerCaseLetters()
            {
                string p = @"[^a-z]*([a-z]{2,})[^a-z]*";
                return GetNeighbors(mStrDataChecked, p, @"[a-z]{2,}");
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetNeighborLetters()
            {
                string p = @"[^a-zA-Z]*([a-zA-Z]{2,})[^a-zA-Z]*";
                return GetNeighbors(mStrDataChecked, p, @"[a-zA-Z]{2,}");
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="inputstring"></param>
            /// <param name="p"></param>
            /// <param name="p2"></param>
            /// <returns></returns>
            private int GetNeighbors(string inputstring, string p, string p2 = "")
            {
                int nResult = 0;

                MatchCollection mc = Regex.Matches(inputstring, p);
                foreach (Match m in mc)
                {
                    string sought;

                    if ("" != p2)
                    {
                        // remove the non-matching part in this group
                        sought = Regex.Match(m.Value, p2).Value;
                    }
                    else
                    {
                        sought = m.Value;
                    }

                    nResult += sought.Length - 1;
                }

                return nResult;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetRepeatScore()
            {
                byte[] sorted = (byte[])(mDataChecked.Clone());
                Array.Sort(sorted);

                byte curData = 0;
                int repeatCount = 1;
                int nSum = 0;
                for (int i = 0; i < sorted.GetLength(0); ++i)
                {
                    if (curData != sorted[i])
                    {
                        if (IsLetter(curData) &&
                            (Math.Abs(curData - sorted[i]) == Math.Abs('a' - 'A')))
                        {
                            repeatCount++;
                        }
                        else
                        {
                            curData = sorted[i];
                            if (repeatCount > 1)
                            {
                                nSum += repeatCount * (repeatCount - 1);
                            }

                            repeatCount = 1;
                        }
                    }
                    else
                    {
                        repeatCount++;
                    }
                }

                if (1 < repeatCount)
                {
                    nSum += repeatCount * (repeatCount - 1);
                }

                return -nSum;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            private bool IsLetter(byte c)
            {
                return IsCapitalLetter(c) || IsLowcaseLetter(c);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            private bool IsCapitalLetter(byte c)
            {
                return ('A' <= c && 'Z' >= c);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            private bool IsLowcaseLetter(byte c)
            {
                return ('a' <= c && 'z' >= c);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            private bool IsNumber(byte c)
            {
                return ('0' <= c && '9' >= c);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            private bool IsSpecial(byte c)
            {
                return (('!' <= c && '/' >= c) ||
                       (':' <= c && '@' >= c) ||
                       ('[' <= c && '`' >= c) ||
                        ('{' <= c && '~' >= c));
            }

            /// <summary>
            ///
            /// </summary>
            private void GetGeneralCount()
            {
                int i = 0;
                for (i = 0; i < mDataChecked.GetLength(0); ++i)
                {
                    byte c = mDataChecked[i];
                    if (IsCapitalLetter(c))
                    {
                        mCapLetterCount++;
                    }
                    else if (IsLowcaseLetter(c))
                    {
                        mLetterCount++;
                    }
                    else if (IsNumber(c))
                    {
                        mNumberCount++;
                    }
                    else if (IsSpecial(c))
                    {
                        mSpecialCount++;
                    }
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetMidNumSpecialCount()
            {
                if (mCount <= 2)
                {
                    return 0;
                }

                int midNumSpecialCount = 0;
                for (int i = 1; i < mCount - 1; ++i)
                {
                    Byte c = mDataChecked[i];
                    if (IsNumber(c) || IsSpecial(c))
                    {
                        midNumSpecialCount++;
                    }
                }

                return midNumSpecialCount;
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetContinuousCountScore()
            {
                int n = GetContinuousCount();
                return -(n * 3);
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            private int GetContinuousCount()
            {
                Stack<Byte> stack = new Stack<Byte>();
                int nSum = 0;
                for (int i = 0; i < mDataChecked.GetLength(0); ++i)
                {
                    byte c = mDataChecked[i];
                    if (IsLetter(c))
                    {
                        char lowcaseC = Char.ToLower((char)c);
                        if (stack.Count == 0)
                        {
                            stack.Push((byte)lowcaseC);
                        }
                        else
                        {
                            Byte prev = stack.Peek();
                            if (!IsLetter(prev))
                            {
                                stack.Clear();
                            }
                            else if (Math.Abs(prev - lowcaseC) != 1)
                            {
                                stack.Clear();
                            }

                            stack.Push((byte)lowcaseC);
                        }
                    }
                    else if (IsNumber(c))
                    {
                        if (stack.Count == 0)
                        {
                            stack.Push(c);
                        }
                        else
                        {
                            Byte prev = stack.Peek();
                            if (!IsNumber(prev))
                            {
                                stack.Clear();
                            }
                            else if (Math.Abs(prev - c) != 1)
                            {
                                stack.Clear();
                            }

                            stack.Push(c);
                        }
                    }
                    else if (IsSpecial(c))
                    {
                        stack.Clear();
                    }

                    if (3 == stack.Count)
                    {
                        Byte third = stack.Pop();
                        Byte second = stack.Pop();
                        Byte first = stack.Pop();
                        if (first != third)
                        {
                            nSum++;
                        }

                        stack.Push(second);
                        stack.Push(third);
                    }
                }

                return nSum;
            }
        }
    }

    /// <summary>
    /// Helper class
    /// </summary>
    public class BadReason
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly int REASON_TOO_SHORT = 0;

        /// <summary>
        ///
        /// </summary>
        public static readonly int REASON_ONLY_LETTERS = 1;

        /// <summary>
        ///
        /// </summary>
        public static readonly int REASON_ONLY_NUMBERS = 2;

        /// <summary>
        ///
        /// </summary>
        public static readonly int REASON_CONTINUOUS = 3;

        /// <summary>
        ///
        /// </summary>
        public static readonly int REASON_NEIGHBOR_LETTERS = 4;

        /// <summary>
        ///
        /// </summary>
        public static readonly int REASON_NEIGHBOR_NUMBERS = 5;

        /// <summary>
        ///
        /// </summary>
        public static readonly int REASON_REPEAT = 6;

        /// <summary>
        ///
        /// </summary>
        private static readonly string REASON_TOO_SHORT_CH = "密码太短";

        /// <summary>
        ///
        /// </summary>
        private static readonly string REASON_ONLY_LETTERS_CH = "只有英文字元";

        /// <summary>
        ///
        /// </summary>
        private static readonly string REASON_ONLY_NUMBERS_CH = "只有数字字元";

        /// <summary>
        ///
        /// </summary>
        public static readonly string REASON_CONTINUOUS_CH = "连续字母/数字超过3个";

        /// <summary>
        ///
        /// </summary>
        public static readonly string REASON_NEIGHBOR_LETTERS_CH = "连续英文字元";

        /// <summary>
        ///
        /// </summary>
        public static readonly string REASON_NEIGHBOR_NUMBERS_CH = "连续数字字元";

        /// <summary>
        ///
        /// </summary>
        public static readonly string REASON_REPEAT_CH = "重复字元";

        /// <summary>
        ///
        /// </summary>
        private Dictionary<int, string> m_ReasonTxt = new Dictionary<int, string>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="reasonCode"></param>
        public BadReason(int reasonCode)
        {
            m_ReasonTxt.Add(REASON_TOO_SHORT, REASON_TOO_SHORT_CH);
            m_ReasonTxt.Add(REASON_ONLY_LETTERS, REASON_ONLY_LETTERS_CH);
            m_ReasonTxt.Add(REASON_ONLY_NUMBERS, REASON_ONLY_NUMBERS_CH);
            m_ReasonTxt.Add(REASON_CONTINUOUS, REASON_CONTINUOUS_CH);
            m_ReasonTxt.Add(REASON_NEIGHBOR_LETTERS, REASON_NEIGHBOR_LETTERS_CH);
            m_ReasonTxt.Add(REASON_NEIGHBOR_NUMBERS, REASON_NEIGHBOR_NUMBERS_CH);
            m_ReasonTxt.Add(REASON_REPEAT, REASON_TOO_SHORT_CH);
        }

        /// <summary>
        /// Return the reason text, if found
        /// </summary>
        /// <param name="ReasonCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNotFoundException">if reasonCode is not valid</exception>
        public string GetReasonExplain(int ReasonCode)
        {
            return (string)(m_ReasonTxt[ReasonCode]);
        }
    }
}
