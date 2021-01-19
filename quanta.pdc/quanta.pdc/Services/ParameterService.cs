using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cns.Services
{
    
    public  class ParameterService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _context;

        public ParameterService(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }


        public List<SelectListItem> GetSelectList(string ParameterGroup)
        {
            List<SelectListItem> ParameterSelectList = new List<SelectListItem>();
            ParameterSelectList = _context.PDC_Parameter
                                .Where(x => x.ParameterGroup == ParameterGroup)
                                .OrderBy(x => x.CreatorDate)
                                .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.ParameterText, Value = x.ParameterID.ToString() }).ToList();

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
                OldParameter.Modifyer = "c5805dbf-dac5-41e6-bb72-5eb0b449134d";
                OldParameter.ModifyerName = "super@admin.com";
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

        
    }
}
