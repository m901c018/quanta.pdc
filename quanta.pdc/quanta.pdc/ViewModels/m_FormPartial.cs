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
    [Serializable]
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

        /// <summary> 意見
        /// 
        /// </summary>
        public string m_Result { get; set; }

        /// <summary> 最後一筆LOG資料
        /// 
        /// </summary>
        public PDC_Form_StageLog m_PDC_Form_StageLog { get; set; }

        /// <summary> PCBType-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> PCBTypeList { get; set; }

        /// <summary> PCB Layout Status-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> PCBLayoutStatusList { get; set; }

        /// <summary> 申請單意見罐頭
        /// 
        /// </summary>
        public List<PDC_Parameter> FormApplyResultList { get; set; }

        /// <summary> BRD壓縮檔(編輯用)
        /// 
        /// </summary>
        public IFormFile m_UplpadBRDFile2 { get; set; }

        /// <summary> Constraint Excel(編輯用)
        /// 
        /// </summary>
        public IFormFile m_UplpadExcelFile2 { get; set; }

        /// <summary> pstchip.dat(編輯用)
        /// 
        /// </summary>
        public IFormFile m_UplpadpstchipFile2 { get; set; }

        /// <summary> pstxnet.dat(編輯用)
        /// 
        /// </summary>
        public IFormFile m_UplpadpstxnetFile2 { get; set; }

        /// <summary> pstxprt.dat(編輯用)
        /// 
        /// </summary>
        public IFormFile m_UplpadpstxprtFile2 { get; set; }

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
        public List<PDC_File> m_OtherFileList { get; set; }

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
        public List<vw_FormQuery> vw_FormQueryList { get; set; }

        /// <summary> 查詢資料
        /// 
        /// </summary>
        public vw_FormQuery vw_FormQuery { get; set; }

        /// <summary> 查詢用
        /// 
        /// </summary>
        public DateTime? SearchDate { get; set; }

        /// <summary> 臨時戳記
        /// 
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PDC_Member Member { get; set; }

        /// <summary> 組態設定線寬線距規則
        /// 
        /// </summary>
        public PDC_Parameter m_ExcelRule { get; set; }

        public m_FormPartial()
        {
            m_PDC_Form = new PDC_Form();
            vw_FormQueryList = new List<vw_FormQuery>();
            Member = new PDC_Member();
        }
    }

    
}
