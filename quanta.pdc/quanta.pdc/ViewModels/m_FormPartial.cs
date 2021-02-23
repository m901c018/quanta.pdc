using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using cns.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cns.ViewModels
{
    //view model for changeroles screen
    public class m_FormPartial
    {
        

        /// <summary> 表單資料
        /// 
        /// </summary>
        public PDC_Form m_PDC_Form { get; set; }

        /// <summary> 表單LOG資料
        /// 
        /// </summary>
        public List<PDC_Form_StageLog> PDC_Form_StageLogList { get; set; }

        /// <summary> 表單LOG檔案資料
        /// 
        /// </summary>
        public List<PDC_File> PDC_Form_StageLogFileList { get; set; }

        /// <summary> BRD壓縮檔
        /// 
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public IFormFile m_UplpadBRDFile { get; set; }

        /// <summary> Constraint Excel
        /// 
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public IFormFile m_UplpadExcelFile { get; set; }

        /// <summary> pstchip.dat
        /// 
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public IFormFile m_UplpadpstchipFile { get; set; }

        /// <summary> pstxnet.dat
        /// 
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public IFormFile m_UplpadpstxnetFile { get; set; }

        /// <summary> pstxprt.dat
        /// 
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public IFormFile m_UplpadpstxprtFile { get; set; }

        /// <summary> 其他檔案
        /// 
        /// </summary>
        public IFormFile m_UplpadOtherFile { get; set; }

        /// <summary> BRD壓縮檔
        /// 
        /// </summary>
        public PDC_File m_BRDFile { get; set; }

        /// <summary> Constraint Excel
        /// 
        /// </summary>
        public PDC_File m_ExcelFile { get; set; }

        /// <summary> pstchip.dat
        /// 
        /// </summary>
        public PDC_File m_pstchipFile { get; set; }

        /// <summary> pstxnet.dat
        /// 
        /// </summary>
        public PDC_File m_pstxnetFile { get; set; }

        /// <summary> pstxprt.dat
        /// 
        /// </summary>
        public PDC_File m_pstxprtFile { get; set; }

        /// <summary> 其他檔案
        /// 
        /// </summary>
        public PDC_File m_Other { get; set; }

        /// <summary> 是否直接申請
        /// 
        /// </summary>
        public bool IsSendApply { get; set; }

        /// <summary> 意見
        /// 
        /// </summary>
        public string Result { get; set; }

        /// <summary> Excel內容
        /// 
        /// </summary>
        public DataTable ExcelDt { get; set; }

        /// <summary> 驗證結果
        /// 
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary> 查詢資料
        /// 
        /// </summary>
        public List<PDC_Form> PDC_FormList { get; set; }

        /// <summary> 查詢用
        /// 
        /// </summary>
        public DateTime? SearchDate { get; set; }


        public string AppliedFormNo { get; set; }
        public string FormStatus { get; set; }
        public string PCBLayoutStatus { get; set; }
        public string PCBType { get; set; }
        public string ProjectName { get; set; }
        public string BoardTypeName { get; set; }
        public string Revision { get; set; }
        public string CompCode { get; set; }
        public string BUCode { get; set; }
        public string CreatorName { get; set; }

        public m_FormPartial()
        {
            m_PDC_Form = new PDC_Form();
            PDC_FormList = new List<PDC_Form>();
        }
    }

    
}
