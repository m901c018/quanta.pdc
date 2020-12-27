using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportModule.Repository.Interface
{
    public interface IBaseRepository
    {
        /// <summary>
        /// 驗證資料表欄位
        /// </summary>
        /// <param name="unit">ImportModel</param>
        void Validate(ImportModel unit);

        /// <summary>
        /// 取得欄位類型
        /// </summary>
        /// <param name="eType">類型列舉</param>
        /// <returns></returns>
        String GetColFormat(ImportModule.ImportFormatEnum.eColType eType);

        /// <summary>
        /// 判斷字串是否為正確的電子郵件地址
        /// </summary>
        /// <param name="mailAddress">電子郵件地址</param>
        /// <returns>傳回布林值</returns>
        bool IsMailAddress(string mailAddress);

        /// <summary>
        /// 判斷字串是否為正確的電話\手機
        /// </summary>
        /// <param name="Tel">電話號碼</param>
        /// <returns>傳回布林值</returns>
        bool IsPhoneNumber(string Tel);
    }
}
