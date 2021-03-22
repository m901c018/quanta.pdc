using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Serializable]
    public class Api_QueryUserInfo
    {
        /// <summary> 公司代碼
        /// 
        /// </summary>
        public string COMCOD { get; set; }
        /// <summary> 部門代碼
        /// 
        /// </summary>
        public string DEPCOD { get; set; }
        /// <summary> 部門名稱
        /// 
        /// </summary>
        public string DEPNAM { get; set; }
        /// <summary> 工號
        /// 
        /// </summary>
        public string EMPLID { get; set; }
        /// <summary> 英文名字
        /// 
        /// </summary>
        public string ENGNAM { get; set; }
        /// <summary> 中文名字
        /// 
        /// </summary>
        public string CHINAM { get; set; }
        /// <summary> 分機號碼
        /// 
        /// </summary>
        public string MVPNUM { get; set; }
        /// <summary> 信箱
        /// 
        /// </summary>
        public string EMAILD { get; set; }

    }
}
