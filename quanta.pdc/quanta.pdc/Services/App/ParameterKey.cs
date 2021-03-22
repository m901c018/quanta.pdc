using cns.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Services.App
{
    [Serializable]
    public class ParameterKey
    {
        /// <summary>
        /// 組態設定-首頁連結
        /// </summary>
        public const String Configuration_HomeLink = "Configuration_HomeLink";

        /// <summary>
        /// 組態設定-圖文說明
        /// </summary>
        public const String PCBLayoutConstraint = "PCBLayoutConstraint";

        /// <summary>
        /// 組態設定-清單與罐頭
        /// </summary>
        public const String PCBTypeList = "PCBTypeList";

        /// <summary>
        /// 組態設定-公告
        /// </summary>
        public const String ConfigurationAnnouncement = "ConfigurationAnnouncement";

        /// <summary>
        /// CHECK/驗證說明
        /// </summary>
        public const String ConfigurationDescription = "ConfigurationDescription";

        /// <summary>
        /// 申請者抽單工時
        /// </summary>
        public const String ConfigurationApplyDraw = "ConfigurationApplyDraw";

        /// <summary>
        /// 派單退件工時
        /// </summary>
        public const String ConfigurationSendReturn = "ConfigurationSendReturn";

        /// <summary>
        /// 處理-Release工時
        /// </summary>
        public const String ConfigurationRelease = "ConfigurationRelease";

        /// <summary>
        /// 處理-Reject工時
        /// </summary>
        public const String ConfigurationReject = "ConfigurationReject";

        /// <summary>
        /// 表單申請堆疊檢核提醒
        /// </summary>
        public const String ConfigurationFormApply = "ConfigurationFormApply";

        /// <summary>
        /// 工作明細堆疊檢核提醒
        /// </summary>
        public const String ConfigurationWorkDetail = "ConfigurationWorkDetail";

        /// <summary>
        /// Category列舉
        /// </summary>
        public const String SystemCategory = "SystemCategory";

        /// <summary>
        /// 組態設定-清單與罐頭選項
        /// </summary>
        public const String PCBTypeList_Item = "PCBTypeList_Item";

        /// <summary>
        /// 組態設定-圖文說明選項
        /// </summary>
        public const String PCBLayoutConstraint_query_Col = "PCBLayoutConstraint_query_Col";

        /// <summary>
        /// 公司別參數
        /// </summary>
        public const String ConfigurationCompCode = "ConfigurationCompCode";

        /// <summary>
        /// 事業群參數
        /// </summary>
        public const String ConfigurationDeptCode = "ConfigurationDeptCode";
    }
}
