using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportModule
{
    /// <summary>
    /// 匯入格式列舉
    /// </summary>
    [Serializable]
    public class ImportFormatEnum
    {
        #region === 列舉 ====
        /// <summary>
        /// 欄位型別
        /// </summary>
        public enum eColType
        {
            /// <summary>
            /// 布林值
            /// </summary>
            colBoolean,
            /// <summary>
            /// 整數(16)
            /// </summary>
            colInt16,
            /// <summary>
            /// 整數(32)
            /// </summary>
            colInt32,
            /// <summary>
            /// 整數(64)
            /// </summary>
            colInt64,
            /// <summary>
            /// 數值
            /// </summary>
            colNumeric,
            /// <summary>
            /// 浮點數
            /// </summary>
            colDouble,
            /// <summary>
            /// 字串
            /// </summary>
            colString,
            /// <summary>
            /// 日期
            /// </summary>
            colDate
        }

        /// <summary>
        /// 欄位用途
        /// </summary>
        public enum eColValidateType
        {
            /// <summary>
            /// 信箱
            /// </summary>
            E_mail,
            /// <summary>
            /// 電話
            /// </summary>
            Tel,
            /// <summary>
            /// 無特殊用途
            /// </summary>
            Normal
        }
        #endregion
    }
}
