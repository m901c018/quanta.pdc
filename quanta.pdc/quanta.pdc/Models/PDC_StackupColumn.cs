using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Table("PDC_StackupColumn")]
    public class PDC_StackupColumn
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 StackupColumnID { get; set; }
        /// <summary>
        /// 欄位代碼
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string ColumnCode { get; set; }
        /// <summary>
        /// 欄位名稱
        /// </summary>
        [MaxLength(128)]
        public string ColumnName { get; set; }
        /// <summary>
        /// 欄位類別
        /// </summary>
        [MaxLength(64)]
        public string ColumnType { get; set; }
        /// <summary>
        /// 欄位型態
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string DataType { get; set; }
        /// <summary>
        /// 欄位長度
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// 欄位順序(0開始)
        /// </summary>
        public int OrderNo { get; set; }
        /// <summary>
        /// 小數點幾位數
        /// </summary>
        public int DecimalPlaces { get; set; }
        /// <summary>
        /// 上層節點
        /// </summary>
        public Int64 ParentColumnID { get; set; }
        /// <summary>
        /// 資料內容(View傳送用)
        /// </summary>
        public string ColumnValue { get; set; }
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
