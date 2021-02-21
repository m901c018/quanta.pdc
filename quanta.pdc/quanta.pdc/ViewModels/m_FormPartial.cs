using System;
using System.Collections.Generic;
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

        /// <summary> BRD壓縮檔
        /// 
        /// </summary>
        public IFormFile m_UplpadBRDFile { get; set; }

        /// <summary> Constraint Excel
        /// 
        /// </summary>
        public IFormFile m_UplpadExcelFile { get; set; }

        /// <summary> pstchip.dat
        /// 
        /// </summary>
        public IFormFile m_UplpadpstchipFile { get; set; }

        /// <summary> pstxnet.dat
        /// 
        /// </summary>
        public IFormFile m_UplpadpstxnetFile { get; set; }

        /// <summary> pstxprt.dat
        /// 
        /// </summary>
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

        public m_FormPartial()
        {
            
        }
    }
}
