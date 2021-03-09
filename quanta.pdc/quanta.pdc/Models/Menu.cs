using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    public class Menu
    {
        /// <summary>
        /// 主鍵
        /// </summary>
        public Int32 MenuID { get; set; }
        /// <summary>
        /// 上層Menu主鍵
        /// </summary>
        public Int32 ParentMenuID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Order { get; set; }
    }
}
