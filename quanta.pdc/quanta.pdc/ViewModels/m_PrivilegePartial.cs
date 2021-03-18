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
    public class m_PrivilegePartial
    {
        
        /// <summary> PDC處理員
        /// 
        /// </summary>
        public List<vw_PrivilegeQuery> vw_ProcessQueryList { get; set; }

        /// <summary> PDC指派員
        /// 
        /// </summary>
        public List<vw_PrivilegeQuery> vw_AssignQueryList { get; set; }

        /// <summary> PDC管理員
        /// 
        /// </summary>
        public List<vw_PrivilegeQuery> vw_AdminQueryList { get; set; }


        /// <summary> 工號
        /// 
        /// </summary>
        public string EmpNumber { get; set; }

        public m_PrivilegePartial()
        {
            vw_ProcessQueryList = new List<vw_PrivilegeQuery>();
            vw_AssignQueryList = new List<vw_PrivilegeQuery>();
            vw_AdminQueryList = new List<vw_PrivilegeQuery>();
        }
    }
}
