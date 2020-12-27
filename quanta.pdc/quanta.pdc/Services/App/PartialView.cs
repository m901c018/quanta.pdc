using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cns
{
    //static class to avoid magic string when call partial view name
    public static class PartialView
    {
        public static string StatusMessage => "_StatusMessage";
    }
}
