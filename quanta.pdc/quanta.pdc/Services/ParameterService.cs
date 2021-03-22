using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cns.Services
{
    
    public  class ParameterService
    {

        private readonly ApplicationDbContext _context;

        private readonly PDC_Member _Member;

        public ParameterService(ApplicationDbContext context, PDC_Member Member)
        {
            _context = context;
            _Member = Member;
        }


        public List<SelectListItem> GetSelectList(string ParameterGroup,Boolean IsValue_ByID = true)
        {
            List<SelectListItem> ParameterSelectList = new List<SelectListItem>();
            if(IsValue_ByID)
            {
                ParameterSelectList = _context.PDC_Parameter
                                .Where(x => x.ParameterGroup == ParameterGroup)
                                .OrderBy(x => x.CreatorDate)
                                .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.ParameterText, Value = x.ParameterID.ToString() }).ToList();
            }
            else
            {
                ParameterSelectList = _context.PDC_Parameter
                                .Where(x => x.ParameterGroup == ParameterGroup)
                                .OrderBy(x => x.CreatorDate)
                                .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.ParameterText, Value = x.ParameterValue }).ToList();
            }

            return ParameterSelectList;
        }

        public List<SelectListItem> GetSelectList(Int64 ParameterParentID)
        {
            List<SelectListItem> ParameterSelectList = new List<SelectListItem>();
            ParameterSelectList = _context.PDC_Parameter
                                .Where(x => x.ParameterParentID == ParameterParentID)
                                .OrderBy(x => x.CreatorDate)
                                .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.ParameterText, Value = x.ParameterID.ToString() }).ToList();

            return ParameterSelectList;
        }

        public PDC_Parameter GetParameterOne(Int64 ParameterID)
        {
            PDC_Parameter item = new PDC_Parameter();

            item = _context.PDC_Parameter.Where(x => x.ParameterID == ParameterID).FirstOrDefault();

            return item;
        }

        public PDC_Parameter GetParameterOne(string ParameterGroup)
        {
            PDC_Parameter item = new PDC_Parameter();

            item = _context.PDC_Parameter.Where(x => x.ParameterGroup == ParameterGroup).FirstOrDefault();

            return item;
        }

        public List<PDC_Parameter> GetParameterList(string ParameterGroup)
        {
            List<PDC_Parameter> item = new List<PDC_Parameter>();

            item = _context.PDC_Parameter.Where(x => x.ParameterGroup == ParameterGroup).OrderBy(x => x.OrderNo).ToList();

            return item;
        }

        public List<PDC_Parameter> GetParameterList(Int64 ParameterParentID)
        {
            List<PDC_Parameter> item = new List<PDC_Parameter>();

            item = _context.PDC_Parameter.Where(x => x.ParameterParentID == ParameterParentID).OrderBy(x => x.OrderNo).ToList();

            return item;
        }

        public bool UpdateParameter(PDC_Parameter NewParameter, ref string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            try
            {
                PDC_Parameter OldParameter = GetParameterOne(NewParameter.ParameterID); 
                OldParameter.ParameterValue = NewParameter.ParameterValue;
                OldParameter.ParameterType = NewParameter.ParameterType;
                OldParameter.ParameterText = NewParameter.ParameterText;
                OldParameter.ParameterParentID = NewParameter.ParameterParentID;
                OldParameter.ParameterNote = NewParameter.ParameterNote;
                OldParameter.ParameterName = NewParameter.ParameterName;
                OldParameter.ParameterGroup = NewParameter.ParameterGroup;
                OldParameter.ParameterDesc = NewParameter.ParameterDesc;
                OldParameter.OrderNo = NewParameter.OrderNo;
                OldParameter.Modifyer = _Member.MemberID.ToString();
                OldParameter.ModifyerName = _Member.UserEngName;
                OldParameter.ModifyerDate = DateTime.Now;
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

        public bool DeleteParameter(Int64 ParameterID, ref string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            try
            {
                PDC_Parameter Parameter = GetParameterOne(ParameterID);
                _context.Remove(Parameter);
                _context.SaveChanges();

                ErrorMsg = "刪除成功";
            }
            catch (Exception ex)
            {
                ErrorMsg = "刪除失敗";
                return false;
            }
            return true;
        }

        public bool AddParameter(ref PDC_Parameter NewParameter, ref string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            try
            {
                NewParameter.Creator = _Member.MemberID.ToString();
                NewParameter.CreatorName = _Member.UserEngName;
                NewParameter.CreatorDate = DateTime.Now;
                _context.PDC_Parameter.Add(NewParameter);
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
    }
}
