using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Serializable]
    public class Api_postDept
    {
        /// <summary> 公司代碼
        /// 
        /// </summary>
        public string COMCOD { get; set; }
        /// <summary> 部門代碼
        /// 
        /// </summary>
        public string DEPCOD { get; set; }
    }
}
