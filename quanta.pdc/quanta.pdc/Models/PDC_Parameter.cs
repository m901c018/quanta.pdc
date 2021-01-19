using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Table("PDC_Parameter")]
    public class PDC_Parameter
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ParameterID { get; set; }
        /// <summary>
        /// 參數群組
        /// </summary>
        public string ParameterGroup { get; set; }
        /// <summary>
        /// 參數父層
        /// </summary>
        public Int64 ParameterParentID { get; set; }
        /// <summary>
        /// 參數名稱
        /// </summary>
        [MaxLength(256)]
        public string ParameterName { get; set; }
        /// <summary>
        /// 參數類別
        /// </summary>
        public string ParameterType { get; set; }
        /// <summary>
        /// 參數顯示名稱
        /// </summary>
        [MaxLength(256)]
        public string ParameterText { get; set; }
        /// <summary>
        /// 參數值
        /// </summary>
        public string ParameterValue { get; set; }
        /// <summary>
        /// 參數描述
        /// </summary>
        public string ParameterDesc { get; set; }
        /// <summary>
        /// 參數備註
        /// </summary>
        public string ParameterNote { get; set; }
        /// <summary>
        /// 參數排序
        /// </summary>
        public int OrderNo { get; set; }
        /// <summary>
        /// 是否同步
        /// </summary>
        public bool IsSync { get; set; }
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
