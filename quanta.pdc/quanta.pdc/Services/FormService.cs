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

    public class FormService
    {

        private readonly ApplicationDbContext _context;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly PDC_Member _Member;

        public FormService(ApplicationDbContext context, PDC_Member Member)
        {
            _context = context;
            _Member = Member;
        }

        public FormService(IHostingEnvironment hostingEnvironment, ApplicationDbContext context,PDC_Member Member)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _Member = Member;
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

        /// <summary> 取得申請單
        /// 
        /// </summary>
        /// <param name="FormID">申請單ID</param>
        /// <returns></returns>
        public PDC_Form GetFormOne(Int64 FormID)
        {
            PDC_Form item = new PDC_Form();

            item = _context.PDC_Form.Where(x => x.FormID == FormID).SingleOrDefault();

            return item;
        }

        /// <summary> 取得申請單
        /// 
        /// </summary>
        /// <param name="FormNo">申請單編號</param>
        /// <returns></returns>
        public PDC_Form GetFormOne(String FormNo)
        {
            PDC_Form item = new PDC_Form();

            item = _context.PDC_Form.Where(x => x.AppliedFormNo == FormNo).SingleOrDefault();

            return item;
        }

        public List<vw_FormQuery> GetFilterFormList(QueryParam PDC_Form)
        {
            //判斷物件
            var Predicate = PredicateBuilder.True<vw_FormQuery>();

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

            if (PDC_Form.SearchDate_Start.HasValue)
            {
                Predicate = Predicate.And(a => a.ApplyDate >= Convert.ToDateTime(PDC_Form.SearchDate_Start.Value.ToString("yyyy/MM/dd 00:00:00")));
            }

            if (PDC_Form.SearchDate_End.HasValue)
            {
                Predicate = Predicate.And(a => a.ApplyDate <= Convert.ToDateTime(PDC_Form.SearchDate_End.Value.ToString("yyyy/MM/dd 23:59:59")));
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.FormStatus))
            {
                Predicate = Predicate.And(a => a.FormStatus == PDC_Form.FormStatus);
            }

            if (!string.IsNullOrWhiteSpace(PDC_Form.AppliedFormNo))
            {
                Predicate = Predicate.And(a => a.AppliedFormNo.Contains(PDC_Form.AppliedFormNo));
            }

            List<vw_FormQuery> pDC_FormList = new List<vw_FormQuery>();

            pDC_FormList = _context.vw_FormQuery.Where(Predicate).ToList();

            return pDC_FormList;
        }


        public bool UpdateForm(PDC_Form NewForm, List<PDC_File> FileList, ref string ErrorMsg)
        {
            FileHelper fileHelper = new FileHelper(_hostingEnvironment);
            ErrorMsg = string.Empty;
            try
            {
                PDC_Form OldForm = GetFormOne(NewForm.FormID);
               
                OldForm.ApplyDate = NewForm.ApplyDate;
                OldForm.BoardTypeName = NewForm.BoardTypeName.ToUpper();
                OldForm.FormStatus = NewForm.FormStatus;
                OldForm.PCBLayoutStatus = NewForm.PCBLayoutStatus;
                OldForm.PCBType = NewForm.PCBType;
                OldForm.ProjectName = NewForm.ProjectName.ToUpper();
                OldForm.Revision = NewForm.Revision.ToUpper();
                OldForm.FormStatusCode = NewForm.FormStatusCode;
                OldForm.Result = NewForm.Result;
                OldForm.Modifyer = _Member.MemberID.ToString();
                OldForm.ModifyerName = _Member.UserEngName;
                OldForm.ModifyerDate = DateTime.Now;

                List<PDC_File> NewFileList = new List<PDC_File>();

                foreach (PDC_File item in FileList)
                {
                    PDC_File File = _context.PDC_File.Where(x => x.FileID == item.FileID).SingleOrDefault();
                    File.SourceID = NewForm.FormID;
                    File.FileDescription = "";
                    NewFileList.Add(File);
                }

                _context.SaveChanges();

                //把檔案從Temp移到FileUpload
                foreach (PDC_File item in NewFileList)
                {
                    fileHelper.RemoveFile(item.FileFullName);
                }

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
        public bool AddForm(PDC_Form NewForm,List<PDC_File> FileList, ref string ErrorMsg, out long FormID)
        {
            FileHelper fileHelper = new FileHelper(_hostingEnvironment);
            ErrorMsg = string.Empty;
            try
            {
                NewForm.ProjectName = NewForm.ProjectName.ToUpper();
                NewForm.Revision = NewForm.Revision.ToUpper();
                NewForm.BoardTypeName = NewForm.BoardTypeName.ToUpper();
                NewForm.Creator = _Member.MemberID.ToString();
                NewForm.CreatorName = _Member.UserEngName;
                NewForm.CreatorDate = DateTime.Now;

                _context.PDC_Form.Add(NewForm);
                _context.SaveChanges();

                FormID = NewForm.FormID;

                List<PDC_File> NewFileList = new List<PDC_File>();

                foreach(PDC_File item in FileList)
                {
                    PDC_File File = _context.PDC_File.Where(x => x.FileID == item.FileID).SingleOrDefault();
                    File.SourceID = FormID;
                    File.FileDescription = "";
                    NewFileList.Add(File);
                }

                NewForm.AppliedFormNo = "C" + NewForm.FormID.ToString().PadLeft(7, '0');
                _context.SaveChanges();
                //把檔案從Temp移到FileUpload
                foreach (PDC_File item in NewFileList)
                {
                    fileHelper.RemoveFile(item.FileFullName);
                }

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
            ParameterService parameterService = new ParameterService(_context,_Member);

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

        public bool AddForm_StageLog(long FormID, Enum.FormEnum.Form_Stage form_Stage, string result, List<PDC_File> FileList, out long FormStageID, ref string ErrorMsg)
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
                FormStage.Creator = _Member.MemberID.ToString();
                FormStage.CreatorName = _Member.UserEngName.ToString();
                FormStage.CreatorDate = DateTime.Now;
                _context.PDC_Form_StageLog.Add(FormStage);
                _context.SaveChanges();

                FormStageID = FormStage.StageLogID;

                List<PDC_File> NewFileList = new List<PDC_File>();
                foreach(PDC_File item in FileList)
                {
                    PDC_File OldFile = _context.PDC_File.Where(x => x.FileID == item.FileID).SingleOrDefault();
                    PDC_File NewFile = new PDC_File();
                    NewFile.Creator = OldFile.Creator;
                    NewFile.CreatorName = OldFile.CreatorName;
                    NewFile.CreatorDate = DateTime.Now;
                    NewFile.FileCategory = OldFile.FileCategory;
                    NewFile.FileDescription = OldFile.FileDescription;
                    NewFile.FileExtension = OldFile.FileExtension;
                    NewFile.FileFullName = OldFile.FileFullName;
                    NewFile.FileName = OldFile.FileName;
                    NewFile.FileSize = OldFile.FileSize;
                    NewFile.FileType = OldFile.FileType;
                    NewFile.FileNote = OldFile.FileNote;
                    NewFile.FileRemark = OldFile.FileRemark;
                    NewFile.FunctionName = "FormStage";
                    NewFile.SourceID = FormStageID;
                    NewFileList.Add(NewFile);
                    _context.Add(NewFile);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorMsg = "儲存失敗";
                FormStageID = 0;
                return false;
            }

            return true;
        }

        /// <summary> 新增表單紀錄
        /// 
        /// </summary>
        /// <param name="FormID">表單ID</param>
        /// <param name="form_Stage">關卡</param>
        /// <param name="result">意見</param>
        /// <param name="PDC_Member">負責人</param>
        /// <param name="FormStageID">表單紀錄ID</param>
        /// <param name="ErrorMsg">錯誤訊息</param>
        /// <returns></returns>
        public bool AddForm_StageLog(long FormID, Enum.FormEnum.Form_Stage form_Stage, string result,string PDC_Member, out long FormStageID, ref string ErrorMsg,decimal WorkHour = 0)
        {
            PDC_Form_StageLog FormStage = new PDC_Form_StageLog();
            ErrorMsg = string.Empty;
            try
            {
                PDC_Form pDC_Form = _context.PDC_Form.Where(x => x.FormID == FormID).SingleOrDefault();
                switch(form_Stage)
                {
                    case FormEnum.Form_Stage.Apply:
                        pDC_Form.FormStatusCode = FormEnum.Form_Status.Apply;
                        break;
                    case FormEnum.Form_Stage.Assign:
                        pDC_Form.FormStatusCode = FormEnum.Form_Status.Work;
                        break;
                    case FormEnum.Form_Stage.End:
                        pDC_Form.FormStatusCode = FormEnum.Form_Status.End;
                        break;
                    case FormEnum.Form_Stage.Reject:
                        pDC_Form.FormStatusCode = FormEnum.Form_Status.Reject;
                        break;
                    case FormEnum.Form_Stage.Release:
                        pDC_Form.FormStatusCode = FormEnum.Form_Status.Release;
                        break;
                    case FormEnum.Form_Stage.Work:
                        pDC_Form.FormStatusCode = FormEnum.Form_Status.Work;
                        break;
                }
                pDC_Form.FormStatus = FormEnum.GetForm_StatusDic()[(int)pDC_Form.FormStatusCode];

                FormStage.FormID = FormID;
                FormStage.Result = result;
                FormStage.Stage = form_Stage;
                FormStage.StageName = FormEnum.GetForm_StageName(form_Stage);
                FormStage.WorkHour = WorkHour;
                FormStage.Creator = _Member.MemberID.ToString();
                FormStage.CreatorName = _Member.UserEngName.ToString();
                FormStage.CreatorDate = DateTime.Now;
                FormStage.PDC_Member = PDC_Member;
                _context.PDC_Form_StageLog.Add(FormStage);
                _context.SaveChanges();

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

        public bool DeleteForm_StageLog(long StageLogID)
        {
            try
            {
                PDC_Form_StageLog Stage = _context.PDC_Form_StageLog.Where(x => x.StageLogID == StageLogID).SingleOrDefault();
                _context.PDC_Form_StageLog.Remove(Stage);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary> 抽單
        /// 
        /// </summary>
        /// <param name="FormID">申請單ID</param>
        /// <param name="ErrorMsg">錯誤訊息</param>
        /// <returns></returns>
        public bool CloseFormApply(long FormID, ref string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            try
            {
                PDC_Form Form = _context.PDC_Form.Where(x => x.FormID == FormID).SingleOrDefault();
                if(Form.FormStatusCode == FormEnum.Form_Status.Apply || Form.FormStatusCode == FormEnum.Form_Status.Apply)
                {
                    Form.FormStatus = FormEnum.GetForm_StatusDic()[(int)FormEnum.Form_Status.End];
                    Form.FormStatusCode = FormEnum.Form_Status.End;
                    Form.Modifyer = _Member.MemberID.ToString();
                    Form.ModifyerName = _Member.UserEngName;
                    Form.ModifyerDate = DateTime.Now;
                    

                    PDC_Form_StageLog FormStage = new PDC_Form_StageLog();
                    FormStage.FormID = FormID;
                    FormStage.Result = "已抽單";
                    FormStage.Stage = Enum.FormEnum.Form_Stage.End;
                    FormStage.StageName = FormEnum.GetForm_StageName(Enum.FormEnum.Form_Stage.End);
                    FormStage.WorkHour = GetWorkHour(FormEnum.Form_Stage.End);
                    FormStage.Creator = _Member.MemberID.ToString();
                    FormStage.CreatorName = _Member.UserEngName;
                    FormStage.CreatorDate = DateTime.Now;
                    _context.PDC_Form_StageLog.Add(FormStage);

                    _context.SaveChanges();
                }
                else
                {
                    ErrorMsg = "該申請單已處理完成";
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorMsg = "儲存失敗";
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
