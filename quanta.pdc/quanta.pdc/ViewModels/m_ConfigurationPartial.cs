using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using cns.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cns.ViewModels
{
    //view model for changeroles screen
    public class m_ConfigurationPartial
    {

        /// <summary> Stackup欄位
        /// 
        /// </summary>
        public PDC_File CNS_Sample { get; set; }

        /// <summary> Stackup資料
        /// 
        /// </summary>
        public List<PDC_StackupDetail> StackupDetalList { get; set; }

        /// <summary> 首頁連結資料
        /// 
        /// </summary>
        public List<PDC_Parameter> HomeLinkList { get; set; }

        /// <summary> 首頁連結檔案相關
        /// 
        /// </summary>
        public List<PDC_File> HomeLinkFileList { get; set; }

        /// <summary> 檢驗訊息
        /// 
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary> 顯示文字
        /// 
        /// </summary>
        public string ParameterText { get; set; }

        /// <summary> 是否同步
        /// 
        /// </summary>
        public int IsSync { get; set; }

        /// <summary> 連結
        /// 
        /// </summary>
        public string Link { get; set; }

        /// <summary> 排序
        /// 
        /// </summary>
        public int HomeLinkOrderNo { get; set; }

        /// <summary> 新增首頁連結資料
        /// 
        /// </summary>
        public PDC_Parameter m_HomeLink { get; set; }

        /// <summary> 新增首頁連結檔案
        /// 
        /// </summary>
        public PDC_File m_HomeLinkFile { get; set; }

        /// <summary> 圖文說明-路徑下拉
        /// 
        /// </summary>
        public List<SelectListItem> PCBTypeList { get; set; }

        /// <summary> 圖文說明-欄位下拉
        /// 
        /// </summary>
        public List<SelectListItem> PCBTypeItemList { get; set; }

        /// <summary> 圖文說明欄位
        /// 
        /// </summary>
        public PDC_Parameter m_PCBParameter { get; set; }

        /// <summary> 圖文說明檔案清單
        /// 
        /// </summary>
        public List<PDC_File> PCBFileList { get; set; }

        /// <summary> 清單與罐頭資料
        /// 
        /// </summary>
        public List<SelectListItem> PCBParameterList { get; set; }

        public m_ConfigurationPartial()
        {
            CNS_Sample = new PDC_File();
            HomeLinkList = new List<PDC_Parameter>();
            HomeLinkFileList = new List<PDC_File>();
            m_HomeLink = new PDC_Parameter();
            m_HomeLinkFile = new PDC_File();

            PCBTypeList = new List<SelectListItem>();
            PCBTypeItemList = new List<SelectListItem>();
        }
    }
}
