﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using cns.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cns.ViewModels
{
    //view model for changeroles screen
    [Serializable]
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

        /// <summary> 選單
        /// 
        /// </summary>
        public List<Menu> MenuList { get; set; }

        /// <summary> 登入者
        /// 
        /// </summary>
        public PDC_Member User { get; set; }

        /// <summary> 所有會員資料
        /// 
        /// </summary>
        public List<PDC_Member> MemberList { get; set; }

        public m_HomePartial()
        {
            
        }
    }
}
