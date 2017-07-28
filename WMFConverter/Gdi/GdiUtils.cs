using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter.Gdi
{
    /// <summary>
    /// Utils functions.
    /// </summary>
    public class GdiUtils
    {
        #region Local Variables

        private static int[,] FBA_SHIFT_JIS = new int[,] { { 0x81, 0x9F }, { 0xE0, 0xFC } };
        private static int[,] FBA_HANGUL_CHARSET = new int[,] { { 0x80, 0xFF } };
        private static int[,] FBA_JOHAB_CHARSET = new int[,] { { 0x80, 0xFF } };
        private static int[,] FBA_GB2312_CHARSET = new int[,] { { 0x80, 0xFF } };
        private static int[,] FBA_CHINESEBIG5_CHARSET = new int[,] { { 0xA1, 0xFE } };

        #endregion

        #region Public Methods

        /// <summary>
        /// Convert byte array to string using specified charset.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string ConvertString(byte[] chars, int charset)
        {
            string str = null;

            int length = 0;
            while (length < chars.Count() && chars[length] != 0)
            {
                length++;
            }

            try
            {
                str = System.Text.Encoding.GetEncoding(GetCharset(charset)).GetString(chars).Substring(0, length);
            }
            catch (Exception ex)
            {
                try
                {
                    str = System.Text.Encoding.GetEncoding("US-ASCII").GetString(chars).Substring(0, length);
                }
                catch (Exception ex2)
                {
                    throw new Exception(ex2.Message);
                }
            }
            return str;
        }

        /// <summary>
        /// Get specified charset.
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetCharset(int charset)
        {
            switch (charset)
            {
                case (int)FontCharsetEnum.ANSI_CHARSET:
                    return "Cp1252";
                case (int)FontCharsetEnum.SYMBOL_CHARSET:
                    return "Cp1252";
                case (int)FontCharsetEnum.MAC_CHARSET:
                    return "MacRoman";
                case (int)FontCharsetEnum.SHIFTJIS_CHARSET:
                    return "MS932";
                case (int)FontCharsetEnum.HANGUL_CHARSET:
                    return "MS949";
                case (int)FontCharsetEnum.JOHAB_CHARSET:
                    return "Johab";
                case (int)FontCharsetEnum.GB2312_CHARSET:
                    return "MS936";
                case (int)FontCharsetEnum.CHINESEBIG5_CHARSET:
                    return "MS950";
                case (int)FontCharsetEnum.GREEK_CHARSET:
                    return "Cp1253";
                case (int)FontCharsetEnum.TURKISH_CHARSET:
                    return "Cp1254";
                case (int)FontCharsetEnum.VIETNAMESE_CHARSET:
                    return "Cp1258";
                case (int)FontCharsetEnum.HEBREW_CHARSET:
                    return "Cp1255";
                case (int)FontCharsetEnum.ARABIC_CHARSET:
                    return "Cp1256";
                case (int)FontCharsetEnum.BALTIC_CHARSET:
                    return "Cp1257";
                case (int)FontCharsetEnum.RUSSIAN_CHARSET:
                    return "Cp1251";
                case (int)FontCharsetEnum.THAI_CHARSET:
                    return "MS874";
                case (int)FontCharsetEnum.EASTEUROPE_CHARSET:
                    return "Cp1250";
                case (int)FontCharsetEnum.OEM_CHARSET:
                    return "Cp1252";
                default:
                    return "Cp1252";
            }
        }

        /// <summary>
        /// Get language using specified charset.
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetLanguage(int charset)
        {
            switch (charset)
            {
                case (int)FontCharsetEnum.ANSI_CHARSET:
                    return "en";
                case (int)FontCharsetEnum.SYMBOL_CHARSET:
                    return "en";
                case (int)FontCharsetEnum.MAC_CHARSET:
                    return "en";
                case (int)FontCharsetEnum.SHIFTJIS_CHARSET:
                    return "ja";
                case (int)FontCharsetEnum.HANGUL_CHARSET:
                    return "ko";
                case (int)FontCharsetEnum.JOHAB_CHARSET:
                    return "ko";
                case (int)FontCharsetEnum.GB2312_CHARSET:
                    return "zh-CN";
                case (int)FontCharsetEnum.CHINESEBIG5_CHARSET:
                    return "zh-TW";
                case (int)FontCharsetEnum.GREEK_CHARSET:
                    return "el";
                case (int)FontCharsetEnum.TURKISH_CHARSET:
                    return "tr";
                case (int)FontCharsetEnum.VIETNAMESE_CHARSET:
                    return "vi";
                case (int)FontCharsetEnum.HEBREW_CHARSET:
                    return "iw";
                case (int)FontCharsetEnum.ARABIC_CHARSET:
                    return "ar";
                case (int)FontCharsetEnum.BALTIC_CHARSET:
                    return "bat";
                case (int)FontCharsetEnum.RUSSIAN_CHARSET:
                    return "ru";
                case (int)FontCharsetEnum.THAI_CHARSET:
                    return "th";
                case (int)FontCharsetEnum.EASTEUROPE_CHARSET:
                    return null;
                case (int)FontCharsetEnum.OEM_CHARSET:
                    return null;
                default:
                    return null;
            }
        }


        /// <summary>
        /// Get the first byte area specified charset.
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static int[,] GetFirstByteArea(int charset)
        {
            switch (charset)
            {
                case (int)FontCharsetEnum.SHIFTJIS_CHARSET:
                    return FBA_SHIFT_JIS;
                case (int)FontCharsetEnum.HANGUL_CHARSET:
                    return FBA_HANGUL_CHARSET;
                case (int)FontCharsetEnum.JOHAB_CHARSET:
                    return FBA_JOHAB_CHARSET;
                case (int)FontCharsetEnum.GB2312_CHARSET:
                    return FBA_GB2312_CHARSET;
                case (int)FontCharsetEnum.CHINESEBIG5_CHARSET:
                    return FBA_CHINESEBIG5_CHARSET;
                default:
                    return null;
            }
        }

        #endregion
    }
}
