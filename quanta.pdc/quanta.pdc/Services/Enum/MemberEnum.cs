using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Services.Enum
{
    public class MemberEnum
    {

        public enum Role
        {
            /// <summary>
            /// EE使用者
            /// </summary>
            EE_User = 1,
            /// <summary>
            /// PDC指派員
            /// </summary>
            PDC_Designator = 2,
            /// <summary>
            /// PDC處理員
            /// </summary>
            PDC_Processor = 3,
            /// <summary>
            /// PDC管理員
            /// </summary>
            PDC_Administrator = 4,
            /// <summary>
            /// 開發者
            /// </summary>
            PDC_Programmer = 5,
        }

        public static string GetForm_StageName(Role Role)
        {
            Dictionary<int, string>  Dic_Form_Stage = GetRoleDic();

            return Dic_Form_Stage[(int)Role];
        }


        public static Dictionary<int, string> GetRoleDic()
        {
            Dictionary<int, string> Dic_Role = new Dictionary<int, string>();
            //Dic_Role.Add(1, "EE使用者");
            Dic_Role.Add(2, "PDC指派員");
            Dic_Role.Add(3, "PDC處理員");
            Dic_Role.Add(4, "PDC管理員");

            return Dic_Role;
        }
    }
}
