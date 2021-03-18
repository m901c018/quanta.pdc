using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using cns.Services.Enum;
using cns.Services.Helper;
using static cns.Services.Enum.MemberEnum;

namespace cns.Services
{

    public class PrivilegeService
    {

        private readonly ApplicationDbContext _context;

        private readonly PDC_Member _Member;

        public PrivilegeService(ApplicationDbContext context, PDC_Member Member)
        {
            _context = context;
            _Member = Member;
        }

        public PrivilegeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<vw_PrivilegeQuery> GetFilterPrivilegeList(string EmpNumber,Role? role)
        {
            //判斷物件
            var Predicate = PredicateBuilder.True<vw_PrivilegeQuery>();

            if (!string.IsNullOrWhiteSpace(EmpNumber))
            {
                Predicate = Predicate.And(a => a.EmpNumber == EmpNumber || a.DomainEmpNumber == EmpNumber);
            }

            if (role != null)
            {
                Predicate = Predicate.And(a => a.RoleID == role);
            }

            List<vw_PrivilegeQuery> PrivilegeList = new List<vw_PrivilegeQuery>();

            PrivilegeList = _context.vw_PrivilegeQuery.Where(Predicate).ToList();

            return PrivilegeList;
        }

        public List<PDC_Privilege> GetPrivilegeList(Guid MemberID)
        {
            List<PDC_Privilege> pDC_Privileges = new List<PDC_Privilege>();

            pDC_Privileges = _context.PDC_Privilege.Where(x => x.MemberID == MemberID).ToList();

            return pDC_Privileges;
        }

        public List<SelectListItem> GetProcessorList()
        {
            List<vw_PrivilegeQuery> PrivilegeList = new List<vw_PrivilegeQuery>();

            PrivilegeList = _context.vw_PrivilegeQuery.Where(x=>x.RoleID == Role.PDC_Processor).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems = PrivilegeList.Select(x => new SelectListItem() { Text = x.MemberName.Split("/#")[0], Value = x.MemberID.ToString() }).Distinct().ToList();

            return selectListItems;
        }


        public bool AddPrivilege(Guid MemberID, Role role,ref string ErrorMsg ,bool? IsMB,bool? IsMail)
        {
            ErrorMsg = string.Empty;
            try
            {
                if (!_context.PDC_Privilege.Where(x => x.MemberID == MemberID && x.RoleID == role && x.IsMB == IsMB).Any()) 
                {
                    PDC_Privilege Privilege = new PDC_Privilege();
                    Privilege.RoleID = role;
                    Privilege.PrivilegeID = Guid.NewGuid();
                    Privilege.MemberID = MemberID;
                    Privilege.IsMB = IsMB;
                    Privilege.IsMail = IsMail;
                    Privilege.Creator = _Member.MemberID.ToString();
                    Privilege.CreatorName = _Member.UserEngName;
                    Privilege.CreatorDate = DateTime.Now;
                    _context.PDC_Privilege.Add(Privilege);
                    _context.SaveChanges();
                }
                else
                {
                    ErrorMsg = "該權限已新增";
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorMsg = "新增權限失敗";
                return false;
            }
            return true;
        }

        public bool DeletePrivilege(Guid PrivilegeID)
        {
            try
            {
                PDC_Privilege Privilege = _context.PDC_Privilege.Where(x => x.PrivilegeID == PrivilegeID).SingleOrDefault();
                _context.PDC_Privilege.Remove(Privilege);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
