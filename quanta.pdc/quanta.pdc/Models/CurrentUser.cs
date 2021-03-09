using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Models
{
    [Serializable]
    public class CurrentUser
    {
        public PDC_Member User { get; set; }

        public List<Menu> MenuList { get; set; }
        /// <summary>
        /// 建構式
        /// </summary>
        public CurrentUser()
        {
            User = new PDC_Member();
        }
    }
}
