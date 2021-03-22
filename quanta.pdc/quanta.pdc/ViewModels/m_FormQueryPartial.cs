using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using cns.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using static cns.Services.Enum.FormEnum;

namespace cns.ViewModels
{
    //view model for changeroles screen
    [Serializable]
    public class m_FormQueryPartial
    {
        
        /// <summary> 查詢資料
        /// 
        /// </summary>
        public List<vw_FormQuery> vw_FormQueryList { get; set; }

        /// <summary> 查詢用
        /// 
        /// </summary>
        public DateTime? SearchDate { get; set; }

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

        

        public string rootPath { get; set; }

        public m_FormQueryPartial()
        {
            QueryParam = new QueryParam();
            PicDescriptionList = new List<PDC_Parameter>();
            PicDescriptionFileList = new List<PDC_File>();
        }
    }

    public class QueryParam
    {
        public string ApplierID { get; set; }
        public string AppliedFormNo { get; set; }
        public string FormStatus { get; set; }
        public string PCBLayoutStatus { get; set; }
        public string PCBType { get; set; }
        public string ProjectName { get; set; }
        public string BoardTypeName { get; set; }
        public string Revision { get; set; }
        public string CompCode { get; set; }
        public string BUCode { get; set; }
        public string BUName { get; set; }
        public string CreatorName { get; set; }
        /// <summary> 負責人
        /// 
        /// </summary>
        public string PDC_Member { get; set; }
        public Boolean? IsMB { get; set; }
        /// <summary> 目前關卡
        /// 
        /// </summary>
        public Form_Stage? FormStage { get; set; }

        /// <summary> 表單狀態
        /// 
        /// </summary>
        public Form_Status? Form_Status { get; set; }

        /// <summary> 查詢用
        /// 
        /// </summary>
        public DateTime? SearchDate_Start { get; set; }

        /// <summary> 查詢用
        /// 
        /// </summary>
        public DateTime? SearchDate_End { get; set; }
    }
}
