using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static cns.Services.Enum.FormEnum;

namespace cns.Models
{
    public class vw_FormQuery
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        public Int64 FormID { get; set; }
        /// <summary>
        /// 申請者編號
        /// </summary>
        public string AppliedFormNo { get; set; }
        
        /// <summary>
        /// 單號順序
        /// </summary>
        public Int64? ApplyOrder { get; set; }
        /// <summary>
        /// 申請單狀態代碼
        /// </summary>
        public Form_Status? FormStatusCode { get; set; }
        /// <summary>
        /// 申請單狀態
        /// </summary>
        public string FormStatus { get; set; }
        /// <summary>
        /// 公司別
        /// </summary>
        public string CompCode { get; set; }
        /// <summary>
        /// 部門
        /// </summary>
        public string BUCode { get; set; }
        /// <summary>
        /// 申請者
        /// </summary>
        public string ApplierID { get; set; }
        /// <summary>
        /// 申請日期
        /// </summary>
        public DateTime ApplyDate { get; set; }
        /// <summary>
        /// PCBLayoutStatus
        /// </summary>
        public string PCBLayoutStatus { get; set; }
        /// <summary>
        /// 是否為MB
        /// </summary>
        public Boolean IsMB { get; set; }
        /// <summary>
        /// PCBType
        /// </summary>
        public string PCBType { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// BoardTypeName
        /// </summary>
        public string BoardTypeName { get; set; }
        /// <summary>
        /// Rev
        /// </summary>
        public string Revision { get; set; }
        /// <summary>
        /// 建立者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 建立者名稱
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreatorDate { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifyer { get; set; }
        /// <summary>
        /// 修改者名稱
        /// </summary>
        public string ModifyerName { get; set; }
        /// <summary>
        /// 修改者日期
        /// </summary>
        public DateTime? ModifyerDate { get; set; }
        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime? StageDate { get; set; }

        /// <summary>
        /// 關卡名稱
        /// </summary>
        public string StageName { get; set; }

        /// <summary>
        /// 關卡
        /// </summary>
        public Form_Stage? Stage { get; set; }

        /// <summary>
        /// PDC負責人
        /// </summary>
        public string PDC_Member { get; set; }

        /// <summary>
        /// PDC負責人名稱
        /// </summary>
        public string PDC_MemberName { get; set; }

        /// <summary> 部門名稱
        /// 
        /// </summary>
        public string BUName { get; set; }
    }
}
