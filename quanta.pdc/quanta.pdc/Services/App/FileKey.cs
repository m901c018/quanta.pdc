using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Services.App
{
    [Serializable]
    public class FileKey
    {
        /// <summary>
        /// Excel範本
        /// </summary>
        public const String ConfigurationSample = "ConfigurationSample";

        /// <summary>
        /// PCB對應檔案
        /// </summary>
        public const String Configuration_PCBFile = "Configuration_PCBFile";

        /// <summary>
        /// 檔案上傳正式資料夾
        /// </summary>
        public const String FileUpload = "FileUpload";

        /// <summary>
        /// 檔案上傳暫存資料夾
        /// </summary>
        public const String Temp = "Temp";

        /// <summary>
        /// 組態設定-WorkDetail檔案
        /// </summary>
        public const String ConfigurationWorkDetail = "ConfigurationWorkDetail";

        /// <summary>
        /// 組態設定-首頁連結檔案
        /// </summary>
        public const String Configuration_HomeLink = "Configuration_HomeLink";

        /// <summary>
        /// 申請單-BRD檔案
        /// </summary>
        public const String FormApplyBRD = "FormApplyBRD";

        /// <summary>
        /// 申請單-Excel檔案
        /// </summary>
        public const String FormApplyExcel = "FormApplyExcel";

        /// <summary>
        /// 申請單-其他檔案
        /// </summary>
        public const String FormApplyOther = "FormApplyOther";

        /// <summary>
        /// 申請單-pstchip檔案
        /// </summary>
        public const String FormApplypstchip = "FormApplypstchip";

        /// <summary>
        /// 申請單-pstxnet檔案
        /// </summary>
        public const String FormApplypstxnet = "FormApplypstxnet";

        /// <summary>
        /// 申請單-pstxprt檔案
        /// </summary>
        public const String FormApplypstxprt = "FormApplypstxprt";

        /// <summary>
        /// 申請單關卡檔案
        /// </summary>
        public const String FormStage = "FormStage";
        
    }
}
