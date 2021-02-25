using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Table("PDC_Form")]
    public class PDC_Form
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 FormID { get; set; }
        /// <summary>
        /// 申請者編號
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [MaxLength(64)]
        public string AppliedFormNo { get; set; }
        /// <summary>
        /// 申請單狀態
        /// </summary>
        [Required(ErrorMessage ="必填")]
        [MaxLength(10)]
        public string FormStatus { get; set; }
        /// <summary>
        /// 公司別
        /// </summary>
        [MaxLength(32)]
        public string CompCode { get; set; }
        /// <summary>
        /// 事業群
        /// </summary>
        [MaxLength(32)]
        public string BUCode { get; set; }
        /// <summary>
        /// 申請者
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public string ApplierID { get; set; }
        /// <summary>
        /// 申請日期
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public DateTime ApplyDate { get; set; }
        /// <summary>
        /// PCBLayoutStatus
        /// </summary>
        [MaxLength(32)]
        public string PCBLayoutStatus { get; set; }
        /// <summary>
        /// 是否為MB
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public Boolean IsMB { get; set; }
        /// <summary>
        /// PCBType
        /// </summary>
        public string PCBType { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        [MaxLength(64)]
        [Required(ErrorMessage = "必填")]
        public string ProjectName { get; set; }
        /// <summary>
        /// BoardTypeName
        /// </summary>
        [MaxLength(64)]
        [Required(ErrorMessage = "必填")]
        public string BoardTypeName { get; set; }
        /// <summary>
        /// Rev
        /// </summary>
        [MaxLength(256)]
        [Required(ErrorMessage = "必填")]
        public string Revision { get; set; }
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
        /// <summary>
        /// 修改者
        /// </summary>
        [MaxLength(128)]
        public string Modifyer { get; set; }
        /// <summary>
        /// 修改者名稱
        /// </summary>
        [MaxLength(128)]
        public string ModifyerName { get; set; }
        /// <summary>
        /// 修改者日期
        /// </summary>
        public DateTime? ModifyerDate { get; set; }
    }
}
