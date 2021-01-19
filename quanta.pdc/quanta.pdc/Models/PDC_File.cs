using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Table("PDC_File")]
    public class PDC_File
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 FileID { get; set; }
        /// <summary>
        /// 實體檔名
        /// </summary>
        [MaxLength(256)]
        public string FileFullName { get; set; }
        /// <summary>
        /// 檔案名稱
        /// </summary>
        [MaxLength(128)]
        public string FileName { get; set; }
        /// <summary>
        /// 副檔名
        /// </summary>
        [MaxLength(10)]
        public string FileExtension { get; set; }
        /// <summary>
        /// 檔案大小
        /// </summary>
        public Int64 FileSize { get; set; }
        /// <summary>
        /// 檔案類別(1:檔案、2:連結)
        /// </summary>
        [Required]
        public int FileCategory { get; set; }
        /// <summary>
        /// 檔案類別(1:圖片、2:文件、3:影片、4:壓縮、5:其他)
        /// </summary>
        [Required]
        public int FileType { get; set; }
        /// <summary>
        /// 對應資料表
        /// </summary>
        [Required]
        public string FunctionName { get; set; }
        /// <summary>
        /// 對應資料表主鍵
        /// </summary>
        public Int64 SourceID { get; set; }
        /// <summary>
        /// 檔案紀錄
        /// </summary>
        public string FileNote { get; set; }
        /// <summary>
        /// 檔案備註
        /// </summary>
        public string FileRemark { get; set; }
        /// <summary>
        /// 檔案描述
        /// </summary>
        public string FileDescription { get; set; }
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
