using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ImportModule
{
    /// <summary>
    /// 匯入欄位屬性定義
    /// </summary>
    [Serializable]
    public class ImportModel
    {
        #region === Property ===
        /// <summary>
        /// 欄位代碼
        /// </summary>
        public string ColumnCode { get; set; }
        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 欄位英文名
        /// </summary>
        public string Col_EName { get; set; }
        /// <summary>
        /// 欄位中文名
        /// </summary>
        public string Col_CName { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Col_Must { get; set; }
        /// <summary>
        /// 欄位長度
        /// </summary>
        public int Col_Length { get; set; }
        /// <summary>
        /// 欄位型別(String,int...)
        /// </summary>
        public ImportModule.ImportFormatEnum.eColType Col_Type { get; set; }
        /// <summary>
        /// 欄位用途(信箱,電話...)
        /// </summary>
        public ImportModule.ImportFormatEnum.eColValidateType Col_ValidateType { get; set; }
        /// <summary>
        /// 欄位值(私有)
        /// </summary>
        private string _Col_Value;
        /// <summary>
        /// 欄位值
        /// </summary>
        public string Col_Value
        {
            get { return _Col_Value; }
            set
            {
                if (value.Trim() == "")
                {
                    Col_IsEmpty = true;
                }
                else
                {
                    Col_IsEmpty = false;
                }
                _Col_Value = value;
            }
        }
        /// <summary>
        /// 驗證是否通過
        /// </summary>
        public bool Col_IsPass { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Col_Error { get; set; }
        /// <summary>
        /// 是否為空值
        /// </summary>
        public bool Col_IsEmpty { get; set; }
        #endregion

        public ImportModel()
        {
            this.Col_Error = "";
            this.Col_Value = "";
            this.Col_IsPass = true;
            this.Col_ValidateType = ImportFormatEnum.eColValidateType.Normal;
        }

        /// <summary>
        /// 帶入初始設定
        /// </summary>
        /// <param name="CName">中文名</param>
        /// <param name="EName">英文名</param>
        /// <param name="Length">欄位長度</param>
        /// <param name="Must">必填</param>
        /// <param name="Type">類型</param>
        /// <param name="Type">欄位用途</param>
        public ImportModel(string CName, string EName, int Length, bool Must, ImportModule.ImportFormatEnum.eColType Type, ImportModule.ImportFormatEnum.eColValidateType ValidateType)
        {
            this.Col_Error = "";
            this.Col_Value = "";
            this.Col_IsPass = true;
            this.Col_CName = CName;
            this.Col_EName = EName;
            this.Col_Length = Length;
            this.Col_Must = Must;
            this.Col_Type = Type;
            this.Col_ValidateType = ValidateType;
        }
        
    }

}
