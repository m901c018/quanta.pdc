﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static cns.Services.Helper.ExcelHepler;

namespace cns.ViewModels
{
    //view model for changeroles screen
    public class m_ExcelPartial
    {
        /// <summary> 錯誤訊息
        /// 
        /// </summary>
        public string Errmsg { get; set; }

        /// <summary> 所有ExcelSheet
        /// 
        /// </summary>
        public List<string> SheetNames { get; set; }

        /// <summary> Stackup欄位
        /// 
        /// </summary>
        public List<StackupColumnModel> StackupColumnList { get; set; }

        /// <summary> 所有Excel資料
        /// 
        /// </summary>
        public List<DataTable> ExcelSheetDts { get; set; }

        public m_ExcelPartial()
        {
            StackupColumnList = new List<StackupColumnModel>();
            ExcelSheetDts = new List<DataTable>();
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 1, ColumnCode = "Col_01A", ColumnName = "層別\n(LAYER)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 0, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 2, ColumnCode = "Col_02A", ColumnName = "疊構類別\n(Stack up Type)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 1, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 3, ColumnCode = "Col_03A", ColumnName = "NAME\n(TOP, GND, GND1, IN1, ..,\nVCC, VCC1, BOTTOM)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 2, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 4, ColumnCode = "Col_04A", ColumnName = "HIGH SPEED\nGROUP NAME\n(T, 1, 2, 3…)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 3, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 5, ColumnCode = "Col_05A", ColumnName = "線寬\n(LINE WIDTH)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 4, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 6, ColumnCode = "Col_06A", ColumnName = "間距\n(SPACING)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 5, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 7, ColumnCode = "Col_07B", ColumnName = "疊構\n(Stack up)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 6, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 8, ColumnCode = "Col_08B", ColumnName = "板厚\n(Thickness)", ColumnType = "文字", DataType = "數字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 7, ParentColumnID = 0 });
        }
    }
}