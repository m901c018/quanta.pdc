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
        /// <summary>
        /// 
        /// </summary>
        public string COMCOD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DEPCOD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DEPNAM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EMPLID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ENGNAM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MVPNUM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EMAILD { get; set; }
        
    }
}
