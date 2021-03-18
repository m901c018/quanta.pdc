using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static cns.Services.Enum.MemberEnum;

namespace cns.Models
{
    [Table("PDC_Department")]
    public class PDC_Department
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DepartmentID { get; set; }
        /// <summary>
        /// 公司別
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [MaxLength(64)]
        public string CompCode { get; set; }
        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CompName { get; set; }
        /// <summary>
        /// 部門代碼
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [MaxLength(64)]
        public string BUCode { get; set; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public string BUName { get; set; }
        /// <summary>
        /// 顯示部門名稱
        /// </summary>
        public string ShowName { get; set; }
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
