using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Table("PDC_StackupDetail")]
    public class PDC_StackupDetail
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 StackupDetailID { get; set; }
        /// <summary>
        /// StackupColumn主鍵
        /// </summary>
        [Required]
        public Int64 StackupColumnID { get; set; }
        /// <summary>
        /// 欄位類別
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string DataType { get; set; }
        /// <summary>
        /// 欄位型態
        /// </summary>
        [MaxLength(64)]
        public string ColumnType { get; set; }
        /// <summary>
        /// 第幾筆資料
        /// </summary>
        [Required]
        public int IndexNo { get; set; }
        /// <summary>
        /// 欄位內容
        /// </summary>
        public string ColumnValue { get; set; }
        /// <summary>
        /// 該欄位是否唯讀
        /// </summary>
        [Required]
        public bool IsDisabled { get; set; }
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
