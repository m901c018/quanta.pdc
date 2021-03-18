using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static cns.Services.Enum.MemberEnum;

namespace cns.Models
{
    [Table("PDC_Privilege")]
    public class PDC_Privilege
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        public Guid PrivilegeID { get; set; }
        /// <summary>
        /// PDC員工ID
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public Guid MemberID { get; set; }
        /// <summary>
        /// 權限
        /// </summary>
        [Required(ErrorMessage ="必填")]
        public Role RoleID { get; set; }
        
        /// <summary>
        /// 類別(MB/其他)
        /// </summary>
        public bool? IsMB { get; set; }
      
        /// <summary>
        /// Mail通知
        /// </summary>
        public bool? IsMail { get; set; }
        
        /// <summary>
        /// 建立者
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Creator { get; set; }
        /// <summary>
        /// 建立者名稱
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string CreatorName { get; set; }
        /// <summary>
        /// 建立日期
        /// </summary>
        [Required]
        public DateTime CreatorDate { get; set; }
    }
}
