using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static cns.Services.Enum.MemberEnum;

namespace cns.Models
{
    public class vw_PrivilegeQuery
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        public Guid PrivilegeID { get; set; }
        /// <summary>
        /// PDC使用者ID
        /// </summary>
        public Guid MemberID { get; set; }
        
        /// <summary>
        /// 工號
        /// </summary>
        public string EmpNumber { get; set; }
        /// <summary>
        /// 域名 + 工號
        /// </summary>
        public string DomainEmpNumber { get; set; }
        /// <summary>
        /// PDC使用者名稱(英文 + 中文 + 分機號碼)
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 公司別
        /// </summary>
        public string CompCode { get; set; }
        /// <summary>
        /// 事業群代碼
        /// </summary>
        public string BUCode { get; set; }
        /// <summary>
        /// 事業群名稱
        /// </summary>
        public string BUName { get; set; }
        /// <summary>
        /// 事業群顯示名稱
        /// </summary>
        public string ShowName { get; set; }
        /// <summary>
        /// 是否為MB
        /// </summary>
        public string IsMB { get; set; }
        /// <summary>
        /// 是否Mail通知
        /// </summary>
        public string IsMail { get; set; }

        /// <summary> 權限
        /// 
        /// </summary>
        public Role RoleID { get; set; }  
    }
}
