using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using cns.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.XSSF.UserModel;

namespace cns.ViewModels
{
    //view model for changeroles screen
    [Serializable]
    public class m_ExcelPartial
    {
        /// <summary> 錯誤訊息
        /// 
        /// </summary>
        public string Errmsg { get; set; }

        /// <summary> 檔案名稱
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary> 所有ExcelSheet
        /// 
        /// </summary>
        public List<string> SheetNames { get; set; }

        /// <summary> Stackup欄位
        /// 
        /// </summary>
        public List<PDC_StackupColumn> StackupColumnList { get; set; }

        /// <summary> Stackup資料
        /// 
        /// </summary>
        public List<PDC_StackupDetail> StackupDetalList { get; set; }

        /// <summary> 所有Excel資料
        /// 
        /// </summary>
        public List<DataTable> ExcelSheetDts { get; set; }

        /// <summary> 總版厚
        /// 
        /// </summary>
        public decimal ThicknessTotal { get; set; }

        /// <summary> 快速連結資料
        /// 
        /// </summary>
        public List<PDC_Parameter> FastLinkList { get; set; }

        /// <summary> 快速連結檔案資料
        /// 
        /// </summary>
        public List<PDC_File> FastLinkFileList { get; set; }

        /// <summary> 範本檔案
        /// 
        /// </summary>
        public PDC_File m_CNSSampleFile { get; set; }

        /// <summary> StackupType資料
        /// 
        /// </summary>
        public List<SelectListItem> StackupTypeList { get; set; }

        /// <summary> 組態設定線寬線距規則
        /// 
        /// </summary>
        public PDC_Parameter m_ExcelRule { get; set; }

        /// <summary> 是否表單申請編輯Excel
        /// 
        /// </summary>
        public bool IsFormApplyEdit { get; set; }

        /// <summary> 
        /// 
        /// </summary>
        public List<ExcelRow> SheetData { get; set; }

        public m_ExcelPartial()
        {
            StackupColumnList = new List<PDC_StackupColumn>();
            ExcelSheetDts = new List<DataTable>();
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 1, ColumnCode = "Col_01A", ColumnName = "層別\n(LAYER)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 0, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 2, ColumnCode = "Col_02A", ColumnName = "疊構類別\n(Stack up Type)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 1, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 3, ColumnCode = "Col_03A", ColumnName = "NAME\n(TOP, GND, GND1, IN1, ..,\nVCC, VCC1, BOTTOM)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 2, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 4, ColumnCode = "Col_04A", ColumnName = "HIGH SPEED\nGROUP NAME\n(T, 1, 2, 3…)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 3, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 5, ColumnCode = "Col_05A", ColumnName = "線寬\n(LINE WIDTH)", ColumnType = "文字", DataType = "int", DecimalPlaces = 2, MaxLength = 256, OrderNo = 4, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 6, ColumnCode = "Col_06A", ColumnName = "間距\n(SPACING)", ColumnType = "文字", DataType = "int", DecimalPlaces = 2, MaxLength = 256, OrderNo = 5, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 7, ColumnCode = "Col_07B", ColumnName = "疊構\n(Stack up)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 6, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 8, ColumnCode = "Col_08B", ColumnName = "板厚\n(Thickness)", ColumnType = "文字", DataType = "int", DecimalPlaces = 2, MaxLength = 256, OrderNo = 7, ParentColumnID = 0 });


            StackupTypeList = new List<SelectListItem>();
            StackupTypeList.Add(new SelectListItem { Text = "Conductor", Value = "Conductor" });
            StackupTypeList.Add(new SelectListItem { Text = "Dielectric", Value = "Dielectric" });
            StackupTypeList.Add(new SelectListItem { Text = "Plane", Value = "Plane" });
        }
    }

    public class ExcelRow
    {

        public bool IsFirstRow { get; set; }

        public string Name { get; set; }

        public XSSFRow FirstRow { get; set; }

        public XSSFRow SecondRow { get; set; }

    }
}
