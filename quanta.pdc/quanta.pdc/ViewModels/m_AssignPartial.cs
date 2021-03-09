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
    public class m_AssignPartial
    {
        
        /// <summary> 查詢資料
        /// 
        /// </summary>
        public List<vw_FormQuery> vw_FormQueryList { get; set; }


        /// <summary> 圖文說明清單
        /// 
        /// </summary>
        public List<PDC_Parameter> PicDescriptionList { get; set; }

        /// <summary> 圖文說明檔案清單
        /// 
        /// </summary>
        public List<PDC_File> PicDescriptionFileList { get; set; }

        /// <summary> PCBType-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> PCBTypeList { get; set; }

        /// <summary> PCB Layout Status-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> PCBLayoutStatusList { get; set; }

        /// <summary> 查詢欄位
        /// 
        /// </summary>
        public QueryParam QueryParam { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, string> m_FormStage { get; set; }

        /// <summary> 處理員清單-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> m_ProcessorList { get; set; }

        /// <summary> 公司別清單-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> m_CompCodeList { get; set; }

        /// <summary> 部門清單-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> m_DeptCodeList { get; set; }

        /// <summary> 目前登入者ID
        /// 
        /// </summary>
        public string MemberID { get; set; }

        public m_AssignPartial()
        {
            vw_FormQueryList = new List<vw_FormQuery>();
            QueryParam = new QueryParam();
            PicDescriptionList = new List<PDC_Parameter>();
            PicDescriptionFileList = new List<PDC_File>();
        }
    }

    
}
