using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Serializable]
    public class Api_QueryDept
    {
        /// <summary> 部門名稱
        /// 
        /// </summary>
        public string DEPNAM { get; set; }

    }
}
