using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Services.App
{
    [Serializable]
    public class SessionKey
    {
        /// <summary>
        /// 使用者資訊
        /// </summary>
        public const String usrInfo = "usrInfo";

        /// <summary>
        /// 選單URL
        /// </summary>
        public const String MenuUrl = "MenuUrl";

        /// <summary>
        /// Excel檔案ID
        /// </summary>
        public const String SessionFileID = "SessionFileID";

        /// <summary>
        /// Excel檔案內容
        /// </summary>
        public const String SessionExcelData = "SessionExcelData";

        /// <summary>
        /// Excel檔案名稱
        /// </summary>
        public const String SessionFileName = "SessionFileName";
        
    }
}
