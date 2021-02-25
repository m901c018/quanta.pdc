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

namespace cns.Services
{

    public class FormService
    {

        private readonly ApplicationDbContext _context;


        public FormService(ApplicationDbContext context)
        {
            _context = context;
            
        }



        /// <summary> 取得申請單紀錄
        /// 
        /// </summary>
        /// <param name="FormID">申請單ID</param>
        /// <returns></returns>
        public List<PDC_Form_StageLog> GetForm_StageLogList(Int64 FormID)
        {
            List<PDC_Form_StageLog> Form_StageLogList = new List<PDC_Form_StageLog>();
            Form_StageLogList = _context.PDC_Form_StageLog
                                .Where(x => x.FormID == FormID)
                                .OrderBy(x => x.CreatorDate)
                                .ToList();

            return Form_StageLogList;
        }

        public PDC_Form GetFormOne(Int64 FormID)
        {
            PDC_Form item = new PDC_Form();

            item = _context.PDC_Form.Where(x => x.FormID == FormID).SingleOrDefault();

            return item;
        }

        public List<PDC_Form> GetFilterFormList(PDC_Form PDC_Form)
        {
            //判斷物件
            var Predicate = PredicateBuilder.True<PDC_Form>();

            if(!string.IsNullOrWhiteSpace(PDC_Form.ApplierID))
            {
                Predicate = Predicate.And(a => a.ApplierID.Contains(PDC_Form.ApplierID));
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

            if (PDC_Form.CreatorDate.Year != 1)
            {
                Predicate = Predicate.And(a => a.CreatorDate == PDC_Form.CreatorDate);
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.FormStatus))
            {
                Predicate = Predicate.And(a => a.FormStatus == PDC_Form.FormStatus);
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.AppliedFormNo))
            {
                Predicate = Predicate.And(a => a.AppliedFormNo.Contains(PDC_Form.AppliedFormNo));
            }

            List<PDC_Form> pDC_FormList = new List<PDC_Form>();

            pDC_FormList = _context.PDC_Form.Where(Predicate).ToList();

            return pDC_FormList;
        }


        public bool UpdateForm(PDC_Form NewForm, ref string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            try
            {
                PDC_Form OldForm = GetFormOne(NewForm.FormID);
               
                OldForm.ApplyDate = NewForm.ApplyDate;
                OldForm.BoardTypeName = NewForm.BoardTypeName;
                OldForm.FormStatus = NewForm.FormStatus;
                OldForm.PCBLayoutStatus = NewForm.PCBLayoutStatus;
                OldForm.PCBType = NewForm.PCBType;
                OldForm.ProjectName = NewForm.ProjectName;
                OldForm.Revision = NewForm.Revision;
                OldForm.Modifyer = NewForm.Modifyer;
                OldForm.ModifyerName = NewForm.ModifyerName;
                OldForm.ModifyerDate = NewForm.ModifyerDate;
                _context.SaveChanges();

                ErrorMsg = "儲存成功";
            }
            catch (Exception ex)
            {
                ErrorMsg = "儲存失敗";
                return false;
            }
            return true;
        }

        /// <summary> 新增申請單
        /// 
        /// </summary>
        /// <param name="NewForm">申請單資料</param>
        /// <param name="ErrorMsg">錯誤訊息</param>
        /// <param name="FormID">表單ID</param>
        /// <returns></returns>
        public bool AddForm(PDC_Form NewForm, ref string ErrorMsg, out long FormID)
        {
            ErrorMsg = string.Empty;
            try
            {
                _context.PDC_Form.Add(NewForm);
                _context.SaveChanges();

                FormID = NewForm.FormID;

                NewForm.AppliedFormNo = "CN" + NewForm.FormID.ToString().PadLeft(6, '0');
                _context.SaveChanges();

                ErrorMsg = "儲存成功";
            }
            catch (Exception ex)
            {
                ErrorMsg = "儲存失敗";
                FormID = 0;
                return false;
            }
            return true;
        }

        public decimal GetWorkHour(Enum.FormEnum.Form_Stage form_Stage)
        {
            PDC_Parameter pDC_Parameter = new PDC_Parameter();
            ParameterService parameterService = new ParameterService(_context);

            if((int)form_Stage == 1)
            {
                pDC_Parameter = parameterService.GetParameterOne("ConfigurationFormApply"); //抽單
            }
            else if((int)form_Stage == 2)
            {
                pDC_Parameter = parameterService.GetParameterOne("ConfigurationSendReturn"); //派單退件

            }
            else
            {
                pDC_Parameter = parameterService.GetParameterOne("ConfigurationReject"); //Reject
            }

            decimal WorkHour = 0;

            if (decimal.TryParse(pDC_Parameter.ParameterText, out WorkHour))
                return WorkHour;
            else
                return 0.5M;
        }

        public bool AddForm_StageLog(long FormID, Enum.FormEnum.Form_Stage form_Stage, string result,out long FormStageID, ref string ErrorMsg)
        {
            PDC_Form_StageLog FormStage = new PDC_Form_StageLog();
            ErrorMsg = string.Empty;
            try
            {
                FormStage.FormID = FormID;
                FormStage.Result = result;
                FormStage.Stage = Enum.FormEnum.Form_Stage.Apply;
                FormStage.StageName = FormEnum.GetForm_StageName(Enum.FormEnum.Form_Stage.Apply);
                FormStage.WorkHour = 0;
                FormStage.Creator = "super@admin.com";
                FormStage.CreatorName = "Roger Chao (趙偉智)";
                FormStage.CreatorDate = DateTime.Now;
                _context.PDC_Form_StageLog.Add(FormStage);
                _context.SaveChanges();

                ErrorMsg = "儲存成功";
                FormStageID = FormStage.StageLogID;
            }
            catch (Exception ex)
            {
                ErrorMsg = "儲存失敗";
                FormStageID = 0;
                return false;
            }

            return true;
        }

        
    }

    /// <summary>
    /// 支援函式
    /// 2013.06.03 由 gordon 處技轉
    /// </summary>
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
