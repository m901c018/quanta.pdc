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
    public class m_HomePartial
    {
        

        /// <summary> 快速連結資料
        /// 
        /// </summary>
        public List<PDC_Parameter> HomeLinkList { get; set; }

        /// <summary> 快速連結檔案資料
        /// 
        /// </summary>
        public List<PDC_File> HomeLinkFileList { get; set; }

        /// <summary> 範本檔案
        /// 
        /// </summary>
        public PDC_File m_CNSSampleFile { get; set; }

        /// <summary> 首頁公告訊息
        /// 
        /// </summary>
        public PDC_Parameter Announcement { get; set; }

        public m_HomePartial()
        {
            
        }
    }
}
