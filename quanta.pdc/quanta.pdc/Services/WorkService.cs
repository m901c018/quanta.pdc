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

namespace cns.Services
{

    public class WorkService
    {

        private readonly ApplicationDbContext _context;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly PDC_Member _Member;

        public WorkService(ApplicationDbContext context, PDC_Member Member)
        {
            _context = context;
            _Member = Member;
        }

        public WorkService(IHostingEnvironment hostingEnvironment, ApplicationDbContext context,PDC_Member Member)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _Member = Member;
        }

        /// <summary> 指派功能查詢用
        /// 
        /// </summary>
        /// <param name="PDC_Form">查詢條件</param>
        /// <returns></returns>
        public List<vw_FormQuery> GetFilterFormList(QueryParam PDC_Form)
        {
            //判斷物件
            var Predicate = PredicateBuilder.True<vw_FormQuery>();
            //去掉未送件跟Reject
            Predicate = Predicate.And(a => a.FormStatusCode != FormEnum.Form_Status.NoApply);
            Predicate = Predicate.And(a => a.FormStatusCode != FormEnum.Form_Status.Reject);
            Predicate = Predicate.And(a => a.IsMB == PDC_Form.IsMB);
            //if (PDC_Form.IsMB.HasValue)
            //{
            //    Predicate = Predicate.And(a => a.IsMB == PDC_Form.IsMB);
            //}

            if (!string.IsNullOrWhiteSpace(PDC_Form.CreatorName))
            {
                Predicate = Predicate.And(a => a.CreatorName.Contains(PDC_Form.CreatorName));
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.BoardTypeName))
            {
                Predicate = Predicate.And(a => a.BoardTypeName.Contains(PDC_Form.BoardTypeName));
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.BUCode))
            {
                Predicate = Predicate.And(a => a.BUCode == PDC_Form.BUCode);
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.CompCode))
            {
                Predicate = Predicate.And(a => a.CompCode == PDC_Form.CompCode);
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.PCBLayoutStatus))
            {
                Predicate = Predicate.And(a => a.PCBLayoutStatus == PDC_Form.PCBLayoutStatus);
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.PCBType))
            {
                Predicate = Predicate.And(a => a.PCBType == PDC_Form.PCBType);
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.ProjectName))
            {
                Predicate = Predicate.And(a => a.ProjectName.Contains(PDC_Form.ProjectName));
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.Revision))
            {
                Predicate = Predicate.And(a => a.Revision.Contains(PDC_Form.Revision));
            }

            if (PDC_Form.SearchDate_Start.HasValue)
            {
                Predicate = Predicate.And(a => a.ApplyDate >= Convert.ToDateTime(PDC_Form.SearchDate_Start.Value.ToString("yyyy/MM/dd 00:00:00")));
            }

            if (PDC_Form.SearchDate_End.HasValue)
            {
                Predicate = Predicate.And(a => a.ApplyDate <= Convert.ToDateTime(PDC_Form.SearchDate_End.Value.ToString("yyyy/MM/dd 23:59:59")));
            }

            if (PDC_Form.FormStage != null)
            {
                switch(PDC_Form.FormStage)
                {
                   
                    case FormEnum.Form_Stage.Work:  //已完成
                        Predicate = Predicate.And(a => a.Stage == FormEnum.Form_Stage.Work && a.FormStatusCode == FormEnum.Form_Status.Release);
                        break;
                    default:
                        Predicate = Predicate.And(a => a.Stage == PDC_Form.FormStage);
                        break;
                }
            }

            
            

            List<vw_FormQuery> pDC_FormList = new List<vw_FormQuery>();

            pDC_FormList = _context.vw_FormQuery.Where(Predicate).ToList();

            return pDC_FormList;
        }

        /// <summary> 工作區查詢用
        /// 
        /// </summary>
        /// <param name="PDC_Form">查詢條件</param>
        /// <returns></returns>
        public List<vw_FormQuery> GetFilterWorkFormList(QueryParam PDC_Form)
        {
            //判斷物件
            var Predicate = PredicateBuilder.True<vw_FormQuery>();
            //去掉未送件跟未指派
            Predicate = Predicate.And(a => a.FormStatusCode != FormEnum.Form_Status.NoApply);
            Predicate = Predicate.And(a => a.FormStatusCode != FormEnum.Form_Status.Apply);

            if (!string.IsNullOrWhiteSpace(PDC_Form.PDC_Member))
            {
                Predicate = Predicate.And(a => a.PDC_Member == PDC_Form.PDC_Member);
            }

            if (PDC_Form.Form_Status != null)
            {
                Predicate = Predicate.And(a => a.FormStatusCode == PDC_Form.Form_Status);
            }

            if (PDC_Form.SearchDate_Start.HasValue)
            {
                Predicate = Predicate.And(a => a.ApplyDate >= Convert.ToDateTime(PDC_Form.SearchDate_Start.Value.ToString("yyyy/MM/dd 00:00:00")));
            }

            if (PDC_Form.SearchDate_End.HasValue)
            {
                Predicate = Predicate.And(a => a.ApplyDate <= Convert.ToDateTime(PDC_Form.SearchDate_End.Value.ToString("yyyy/MM/dd 23:59:59")));
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.AppliedFormNo))
            {
                Predicate = Predicate.And(a => a.AppliedFormNo.Contains(PDC_Form.AppliedFormNo));
            }

            
            List<vw_FormQuery> pDC_FormList = new List<vw_FormQuery>();

            pDC_FormList = _context.vw_FormQuery.Where(Predicate).ToList();

            return pDC_FormList;
        }
    }

   
}
