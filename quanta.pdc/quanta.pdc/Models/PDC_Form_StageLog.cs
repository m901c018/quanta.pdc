using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static cns.Services.Enum.FormEnum;

namespace cns.Models
{
    [Table("PDC_Form_StageLog")]
    public class PDC_Form_StageLog
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 StageLogID { get; set; }
        /// <summary>
        /// 表單主鍵
        /// </summary>
        [Required]
        public Int64 FormID { get; set; }
        /// <summary>
        /// 關卡
        /// </summary>
        [Required]
        public Form_Stage Stage { get; set; }

        /// <summary>
        /// 負責人
        /// </summary>
        public string PDC_Member { get; set; }

        /// <summary>
        /// 關卡名稱
        /// </summary>
        [MaxLength(128)]
        public string StageName { get; set; }
        /// <summary>
        /// 結果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 工時
        /// </summary>
        [Required]
        public Decimal WorkHour { get; set; }
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
