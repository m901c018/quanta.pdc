using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Serializable]
    public class Api_postUserInfo
    {
        /// <summary> 域名 + 工號
        /// 
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary> 信箱
        /// 
        /// </summary>
        public string EMail { get; set; }
    }
}
