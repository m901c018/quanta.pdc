using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cns.Services.Enum
{
    public class FormEnum
    {

        public enum Form_Stage
        {
            /// <summary>
            /// 申請
            /// </summary>
            Apply = 1,
            /// <summary>
            /// 派單
            /// </summary>
            Assign = 2,
            /// <summary>
            /// 處理
            /// </summary>
            Work = 3,
            /// <summary>
            /// 完成(Release
            /// </summary>
            Release = 4,
            /// <summary>
            /// 完成(Reject
            /// </summary>
            Reject = 5,
            /// <summary>
            /// 已結束(抽單
            /// </summary>
            End = 6,
        }

        public static string GetForm_StageName(Form_Stage Form_Stage)
        {
            Dictionary<int, string>  Dic_Form_Stage = new Dictionary<int, string>();
            Dic_Form_Stage.Add(1, "Apply");
            Dic_Form_Stage.Add(2, "Assign");
            Dic_Form_Stage.Add(3, "Work");
            Dic_Form_Stage.Add(4, "Release");
            Dic_Form_Stage.Add(5, "Reject");
            Dic_Form_Stage.Add(6, "End");

            return Dic_Form_Stage[(int)Form_Stage];
        }

        public enum Form_Status
        {
            /// <summary>
            /// 未送件(暫存)
            /// </summary>
            NoApply = 1,
            /// <summary>
            /// 已送件(申請)
            /// </summary>
            Apply = 2,
            /// <summary>
            /// 處理中
            /// </summary>
            Work = 3,
            /// <summary>
            /// 已完成(Release)
            /// </summary>
            Release = 4,
            /// <summary>
            /// 已完成(Reject)
            /// </summary>
            Reject = 5,
            /// <summary>
            /// 已結束(抽單
            /// </summary>
            End = 6,
        }

        public static Dictionary<int, string> GetForm_StatusDic()
        {
            Dictionary<int, string> Dic_Form_Status = new Dictionary<int, string>();
            Dic_Form_Status.Add(1, "未送件");
            Dic_Form_Status.Add(2, "未派單");
            Dic_Form_Status.Add(3, "處理中");
            Dic_Form_Status.Add(4, "Release");
            Dic_Form_Status.Add(5, "Reject");
            Dic_Form_Status.Add(6, "已抽單");

            return Dic_Form_Status;
        }

        public enum Assign_Form_Stage
        {
            /// <summary>
            /// 申請(未指派)
            /// </summary>
            Apply = 1,
            /// <summary>
            /// 派單(已指派)
            /// </summary>
            Assign = 2,
            /// <summary>
            /// 處理(已完成)
            /// </summary>
            Work = 3,
            /// <summary>
            /// 已結束(已抽單)
            /// </summary>
            End = 6,
        }

        public static Dictionary<int, string> GetAssign_Form_StageDic()
        {
            Dictionary<int, string> Dic_Form_Status = new Dictionary<int, string>();
            Dic_Form_Status.Add(1, "未指派");
            Dic_Form_Status.Add(2, "未處理");
            Dic_Form_Status.Add(3, "已完成");
            Dic_Form_Status.Add(6, "已抽單");

            return Dic_Form_Status;
        }

        public static Dictionary<int, string> GetWorkArea_Form_StageDic()
        {
            Dictionary<int, string> Dic_Form_Status = new Dictionary<int, string>();
            Dic_Form_Status.Add(3, "未處理");
            Dic_Form_Status.Add(4, "Release");
            Dic_Form_Status.Add(5, "Reject");
            Dic_Form_Status.Add(6, "已抽單");

            return Dic_Form_Status;
        }
    }
}
