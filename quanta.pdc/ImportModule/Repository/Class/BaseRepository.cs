using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportModule.Repository.Interface;
using System.Text.RegularExpressions;

namespace ImportModule.Repository.Class
{
    [Serializable]
    public class BaseRepository:IBaseRepository
    {
       
        /// <summary>
        /// 驗證資料表欄位
        /// </summary>
        /// <param name="unit">ImportModel</param>
        public void Validate(ImportModel unit)
        {

            BaseRepository _BaseRepository = new BaseRepository();
            #region ==== 驗證NULL、必填、長度 ====
            if (unit.Col_Value == null)
            {
                unit.Col_IsPass = false;
                unit.Col_Error = "欄位不可為 NULL";
                return;
            }
            if (unit.Col_Must && unit.Col_Value == "")
            {
                unit.Col_IsPass = false;
                unit.Col_Error = "欄位不可空白";
            }
            if (unit.Col_Type == ImportModule.ImportFormatEnum.eColType.colString && unit.Col_Value.Length > unit.Col_Length)
            {
                unit.Col_IsPass = false;
                unit.Col_Error = "欄位不可超過: " + unit.Col_Length + "字元";
            }
            #endregion
            try
            {
                #region ==== 驗證特殊用途 ====
                if (unit.Col_Value != "")
                {
                    switch (unit.Col_ValidateType)
                    { 
                        case ImportFormatEnum.eColValidateType.Normal:
                            break;
                        case ImportFormatEnum.eColValidateType.E_mail:
                            if (!_BaseRepository.IsMailAddress(unit.Col_Value))
                            {
                                unit.Col_Error = "信箱格式錯誤,範例:XXX@gmail.com";
                                unit.Col_IsPass = false;
                            }
                            break;
                        case ImportFormatEnum.eColValidateType.Tel:
                            if (!_BaseRepository.IsPhoneNumber(unit.Col_Value))
                            {
                                unit.Col_Error = "電話格式錯誤,範例:04-12345678或0921-999999";
                                unit.Col_IsPass = false;
                            }
                            break;
                    }
                }
                #endregion

                #region ==== 驗證欄位型別 ====
                switch (unit.Col_Type)
                {
                    case ImportFormatEnum.eColType.colBoolean:
                        Convert.ToBoolean(unit.Col_Value);
                        break;
                    case ImportFormatEnum.eColType.colInt16:
                        Convert.ToInt16(unit.Col_Value);
                        break;
                    case ImportFormatEnum.eColType.colInt32:
                        Convert.ToInt32(unit.Col_Value);
                        break;
                    case ImportFormatEnum.eColType.colInt64:
                        Convert.ToInt64(unit.Col_Value);
                        break;
                    case ImportFormatEnum.eColType.colNumeric:
                        Convert.ToDecimal(unit.Col_Value);
                        break;
                    case ImportFormatEnum.eColType.colDouble:
                        Convert.ToDouble(unit.Col_Value);
                        break;
                    case ImportFormatEnum.eColType.colString:
                        Convert.ToString(unit.Col_Value);
                        break;
                    case ImportFormatEnum.eColType.colDate:
                        Convert.ToDateTime(unit.Col_Value);
                        break;
                }
                #endregion
            }
            catch (System.FormatException)
            {
                unit.Col_IsPass = false;
                unit.Col_Error = "數值格式應為:" + GetColFormat(unit.Col_Type);
            }
            catch (System.OverflowException)
            {
                unit.Col_IsPass = false;
                unit.Col_Error = "數值內容有誤(溢位)";
            }
        }

        /// <summary>
        /// 取得欄位類型
        /// </summary>
        /// <param name="eType">類型列舉</param>
        /// <returns></returns>
        public String GetColFormat(ImportModule.ImportFormatEnum.eColType eType)
        {
            String sResult = "";
            switch (eType)
            {
                case ImportModule.ImportFormatEnum.eColType.colBoolean:
                    sResult = "布林值";
                    break;
                case ImportModule.ImportFormatEnum.eColType.colInt16:
                case ImportModule.ImportFormatEnum.eColType.colInt32:
                case ImportModule.ImportFormatEnum.eColType.colInt64:
                    sResult = "整數";
                    break;
                case ImportModule.ImportFormatEnum.eColType.colNumeric:
                case ImportModule.ImportFormatEnum.eColType.colDouble:
                    sResult = "浮點數";
                    break;
                case ImportModule.ImportFormatEnum.eColType.colDate:
                    sResult = "日期(yyyy-MM-dd hh:mm:ss)";
                    break;
                case ImportModule.ImportFormatEnum.eColType.colString:
                    sResult = "字串";
                    break;
            }

            return sResult;
        }

        /// <summary>
        /// 判斷字串是否為正確的電子郵件地址
        /// </summary>
        /// <param name="mailAddress">電子郵件地址</param>
        /// <returns>傳回布林值</returns>
        public bool IsMailAddress(string mailAddress)
        {
            bool bln;
            try
            {
                System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(mailAddress);
                bln = true;
            }
            catch
            {
                bln = false;
            }

            return bln;
        }

        /// <summary>
        /// 判斷字串是否為正確的電話\手機
        /// </summary>
        /// <param name="Tel">電話號碼</param>
        /// <returns>傳回布林值</returns>
        public bool IsPhoneNumber(string Tel)
        {
            bool bln;
            try
            {
                //建立一個Regex物件，規則直接在建構子傳入
                Regex checkTel = new Regex(@"^(\d{2,4})\-(\d{7,8})$");
                bool _tel = checkTel.IsMatch(Tel);
                Regex checkPhone = new Regex(@"^09(\d{2})\-(\d{6})$");
                bool _phone = checkPhone.IsMatch(Tel);
                if (_tel || _phone)
                {
                    bln = true;
                }
                else
                {
                    bln = false;
                }
            }
            catch
            {
                bln = false;
            }

            return bln;
        }
    }
}
