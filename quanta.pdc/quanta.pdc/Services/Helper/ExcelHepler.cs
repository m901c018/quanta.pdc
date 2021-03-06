﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Util;
using cns.ViewModels;
using cns.Models;

namespace cns.Services.Helper
{
    public class ExcelHepler
    {
        public static XSSFWorkbook workbook = new XSSFWorkbook();
        //紀錄Stackup的HeaderStyle
        public static ICellStyle StackupHeaderStyle;
        public static ICellStyle StackupHeaderStyle2;
        //紀錄Stackup的Header欄位
        public static List<PDC_StackupColumn> StackupColumnList;
        public static List<PDC_StackupDetail> stackupDetals;
        public static List<string> StackupType;

        public static string SamplePath;
        public static string ExportPath;
        public static string rootPath;

        
        public class StackupStyle
        {
            public BorderStyle BorderBottom { get; set; }
            public BorderStyle BorderLeft { get; set; }
            public BorderStyle BorderRight { get; set; }
            public BorderStyle BorderTop { get; set; }
            public short BorderDiagonalColor { get; set; }
            public HorizontalAlignment Align { get; set; }
        }

        public ExcelHepler(IHostingEnvironment _hostingEnvironment)
        {
            //讀取範例檔案
            SamplePath = _hostingEnvironment.WebRootPath + "\\File\\CNS_v172.xlsx";
            ExportPath = _hostingEnvironment.WebRootPath + "\\File\\CNS_Sample.xlsx";
            rootPath = _hostingEnvironment.WebRootPath;

            StackupType = new List<string>();
            StackupType.Add("Conductor");
            StackupType.Add("Dielectric");
            StackupType.Add("Plane");

            StackupColumnList = new List<PDC_StackupColumn>();
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 1, ColumnCode = "Col_01A", ColumnName = "層別\n(LAYER)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 0, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 2, ColumnCode = "Col_02A", ColumnName = "疊構類別\n(Stack up Type)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 1, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 3, ColumnCode = "Col_03A", ColumnName = "NAME\n(TOP, GND, GND1, IN1, ..,\nVCC, VCC1, BOTTOM)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 2, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 4, ColumnCode = "Col_04A", ColumnName = "HIGH SPEED\nGROUP NAME\n(T, 1, 2, 3…)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 3, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 5, ColumnCode = "Col_05A", ColumnName = "線寬\n(LINE WIDTH)", ColumnType = "文字", DataType = "int", DecimalPlaces = 0, MaxLength = 256, OrderNo = 4, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 6, ColumnCode = "Col_06A", ColumnName = "間距\n(SPACING)", ColumnType = "文字", DataType = "int", DecimalPlaces = 0, MaxLength = 256, OrderNo = 5, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 7, ColumnCode = "Col_07B", ColumnName = "疊構\n(Stack up)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 6, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 8, ColumnCode = "Col_08B", ColumnName = "板厚\n(Thickness)", ColumnType = "文字", DataType = "int", DecimalPlaces = 0, MaxLength = 256, OrderNo = 7, ParentColumnID = 0 });

            stackupDetals = new List<PDC_StackupDetail>();
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 0, ColumnValue = "Solder Mask", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 1, IndexNo = 1, ColumnValue = "L1", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 2, IndexNo = 1, ColumnValue = "Conductor", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 3, IndexNo = 1, ColumnValue = "TOP", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 4, IndexNo = 1, ColumnValue = "T", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 1, ColumnValue = "Cu + Plating", DataType = "int" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 2, IndexNo = 2, ColumnValue = "Dielectric", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 1, IndexNo = 3, ColumnValue = "L2", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 2, IndexNo = 3, ColumnValue = "Conductor", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 3, IndexNo = 3, ColumnValue = "BOT", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 4, IndexNo = 3, ColumnValue = "B", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 3, ColumnValue = "Cu + Plating", DataType = "int" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 4, ColumnValue = "Solder Mask", DataType = "string" });
        }

        public ExcelHepler(IHostingEnvironment _hostingEnvironment,string SampleFilePath)
        {
            //讀取範例檔案
            SamplePath = _hostingEnvironment.WebRootPath + "\\File\\CNS_v172.xlsx";
            if(!string.IsNullOrEmpty(SampleFilePath))
                ExportPath = _hostingEnvironment.WebRootPath + "\\FileUpload\\" + SampleFilePath;
            else
                ExportPath = _hostingEnvironment.WebRootPath + "\\File\\CNS_Sample.xlsx";

            rootPath = _hostingEnvironment.WebRootPath;

            StackupType = new List<string>();
            StackupType.Add("Conductor");
            StackupType.Add("Dielectric");
            StackupType.Add("Plane");

            StackupColumnList = new List<PDC_StackupColumn>();
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 1, ColumnCode = "Col_01A", ColumnName = "層別\n(LAYER)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 0, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 2, ColumnCode = "Col_02A", ColumnName = "疊構類別\n(Stack up Type)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 1, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 3, ColumnCode = "Col_03A", ColumnName = "NAME\n(TOP, GND, GND1, IN1, ..,\nVCC, VCC1, BOTTOM)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 2, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 4, ColumnCode = "Col_04A", ColumnName = "HIGH SPEED\nGROUP NAME\n(T, 1, 2, 3…)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 3, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 5, ColumnCode = "Col_05A", ColumnName = "線寬\n(LINE WIDTH)", ColumnType = "文字", DataType = "int", DecimalPlaces = 0, MaxLength = 256, OrderNo = 4, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 6, ColumnCode = "Col_06A", ColumnName = "間距\n(SPACING)", ColumnType = "文字", DataType = "int", DecimalPlaces = 0, MaxLength = 256, OrderNo = 5, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 7, ColumnCode = "Col_07B", ColumnName = "疊構\n(Stack up)", ColumnType = "文字", DataType = "string", DecimalPlaces = 0, MaxLength = 256, OrderNo = 6, ParentColumnID = 0 });
            StackupColumnList.Add(new PDC_StackupColumn() { StackupColumnID = 8, ColumnCode = "Col_08B", ColumnName = "板厚\n(Thickness)", ColumnType = "文字", DataType = "int", DecimalPlaces = 0, MaxLength = 256, OrderNo = 7, ParentColumnID = 0 });

            stackupDetals = new List<PDC_StackupDetail>();
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 0, ColumnValue = "Solder Mask", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 1, IndexNo = 1, ColumnValue = "L1", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 2, IndexNo = 1, ColumnValue = "Conductor", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 3, IndexNo = 1, ColumnValue = "TOP", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 4, IndexNo = 1, ColumnValue = "T", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 1, ColumnValue = "Cu + Plating", DataType = "int" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 2, IndexNo = 2, ColumnValue = "Dielectric", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 1, IndexNo = 3, ColumnValue = "L2", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 2, IndexNo = 3, ColumnValue = "Conductor", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 3, IndexNo = 3, ColumnValue = "BOT", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 4, IndexNo = 3, ColumnValue = "B", DataType = "string" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 3, ColumnValue = "Cu + Plating", DataType = "int" });
            stackupDetals.Add(new PDC_StackupDetail() { StackupColumnID = 7, IndexNo = 4, ColumnValue = "Solder Mask", DataType = "string" });
        }

        /// <summary> 讀取檔案轉為NPOI
        /// 
        /// </summary>
        /// <param name="excel">NetCore檔案類別</param>
        /// <returns></returns>
        public XSSFWorkbook LoadExcel(IFormFile excel)
        {
            Stream templateStream = new MemoryStream();
            //轉為Steam
            templateStream = excel.OpenReadStream();
            //轉NPOI類型
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateStream);

            return templateWorkbook;
        }

        /// <summary> 儲存檔案並返回檔案路徑
        /// 
        /// </summary>
        /// <param name="excel">NetCore檔案類別</param>
        /// <returns></returns>
        public string SaveAndGetExcelPath(IFormFile file)
        {
            //隨機產生檔案名
            var FilePath = rootPath + "\\FileUpload\\" + Guid.NewGuid().ToString("N") + ".xlsx";

            try
            {
                using (Stream fileStream = new FileStream(FilePath, FileMode.CreateNew))
                {
                    file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {

                return "Error";
            }

            return FilePath;
        }

        /// <summary> 把Sheet資料轉為DataTable
        /// 
        /// </summary>
        /// <param name="xSSFSheet">Excel Sheet</param>
        /// <param name="IsStackup">是否為Stackup</param>
        /// <returns></returns>
        public DataTable GetDataTableFromExcel(ISheet xSSFSheet, Boolean IsStackup)
        {
            DataTable dtExcelRecords = new DataTable();
            if (IsStackup)
            {
                foreach (PDC_StackupColumn StackupColumn in StackupColumnList.OrderBy(x => x.OrderNo))
                {
                    dtExcelRecords.Columns.Add(StackupColumnList.IndexOf(StackupColumn).ToString());
                }

                XSSFRow FirstRow = (XSSFRow)xSSFSheet.GetRow(4);
                int index = 5;
                //判斷第一筆資料是不是TOP
                if(string.IsNullOrWhiteSpace(GetCellValue(FirstRow, StackupColumnList.Where(x => x.ColumnCode == "Col_03A").FirstOrDefault().OrderNo)))
                {
                    DataRow FirstDataRow = dtExcelRecords.NewRow();
                    for (int j = 0; j <= StackupColumnList.Count - 1; j++) //對工作表每一列 
                    {
                        string cellValue = GetCellValue(FirstRow, j); //獲取i行j列數據 
                        FirstDataRow[j] = cellValue;
                    }
                    dtExcelRecords.Rows.Add(FirstDataRow);
                }
                else
                {
                    index = 4;
                }
                for (int i = index; i <= xSSFSheet.LastRowNum - 1; i+= 2)
                {
                    bool IsEnd = false;
                    //每筆有兩列
                    XSSFRow Row = (XSSFRow)xSSFSheet.GetRow(i);
                    XSSFRow Row2 = (XSSFRow)xSSFSheet.GetRow(i + 1);


                    DataRow dr = dtExcelRecords.NewRow();
                    DataRow dr2 = dtExcelRecords.NewRow();
                    for (int j = 0; j <= StackupColumnList.Count - 1; j++) //對工作表每一列 
                    {
                        string cellValue = GetCellValue(Row, j); //獲取i行j列數據 
                        dr[j] = cellValue;
                        string cellValue2 = GetCellValue(Row2, j); //獲取i行j列數據 
                        dr2[j] = cellValue2;

                        string NameValue = GetCellValue(Row, StackupColumnList.Where(x => x.ColumnCode == "Col_03A").FirstOrDefault().OrderNo);

                        //判斷資料是否為最後一筆
                        if (string.IsNullOrWhiteSpace(NameValue))
                            IsEnd = true;
                    }
                    //資料取到Name為null就停
                    if (IsEnd)
                    {
                        return dtExcelRecords;
                    }
                    dtExcelRecords.Rows.Add(dr);
                    dtExcelRecords.Rows.Add(dr2);
                }
            }

            return dtExcelRecords;
        }

        /// <summary> 把Sheet資料轉為ExcelRow
        /// 
        /// </summary>
        /// <param name="xSSFSheet">Excel Sheet</param>
        /// <param name="IsStackup">是否為Stackup</param>
        /// <returns></returns>
        public List<ExcelRow> GetExcelRowFromExcel(ISheet xSSFSheet, Boolean IsStackup)
        {
            List<ExcelRow> dtExcelRecords = new List<ExcelRow>();
            if (IsStackup)
            {

                XSSFRow FirstRow = (XSSFRow)xSSFSheet.GetRow(4);
                int index = 5;
                //判斷第一筆資料是不是TOP
                if (string.IsNullOrWhiteSpace(GetCellValue(FirstRow, StackupColumnList.Where(x => x.ColumnCode == "Col_03A").FirstOrDefault().OrderNo)))
                {
                    ExcelRow firstRow = new ExcelRow();
                    firstRow.IsFirstRow = true;
                    firstRow.FirstRow = FirstRow;
                    dtExcelRecords.Add(firstRow);
                }
                else
                {
                    index = 4;
                }
                for (int i = index; i <= xSSFSheet.LastRowNum - 1; i += 2)
                {
                    
                    
                    bool IsEnd = false;
                    //每筆有兩列
                    XSSFRow Row = (XSSFRow)xSSFSheet.GetRow(i);
                    XSSFRow Row2 = (XSSFRow)xSSFSheet.GetRow(i + 1);

                    ExcelRow excelRow = new ExcelRow();
                    excelRow.IsFirstRow = false;
                    excelRow.FirstRow = Row;
                    excelRow.SecondRow = Row2;

                    for (int j = 0; j <= StackupColumnList.Count - 1; j++) //對工作表每一列 
                    {
                        string NameValue = GetCellValue(Row, StackupColumnList.Where(x => x.ColumnCode == "Col_03A").FirstOrDefault().OrderNo);
                        excelRow.Name = NameValue;
                        //判斷資料是否為最後一筆
                        if (string.IsNullOrWhiteSpace(NameValue))
                            IsEnd = true;
                    }
                    //資料取到Name為null就停
                    if (IsEnd)
                    {
                        return dtExcelRecords;
                    }
                    dtExcelRecords.Add(excelRow);
                }
            }

            return dtExcelRecords;
        }

        /// <summary> 取Excel Row的欄位值
        /// 
        /// </summary>
        /// <param name="Row">Excel Row</param>
        /// <param name="Cellindex">第幾個欄位(0為起始)</param>
        /// <returns></returns>
        public string GetCellValue(XSSFRow Row, int Cellindex)
        {
            string result = string.Empty;

            //取得欄位的值，如果為null或空白則返回空值
            result = Row.GetCell(Cellindex, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString() ?? "";

            return result;
        }

        /// <summary> 驗證Stackup資料
        /// 
        /// </summary>
        /// <param name="ExcelDt">Stackup資料</param>
        /// <param name="model">返回ViewModel</param>
        public string ExcelStackupCheck(DataTable ExcelDt)
        {
            string ErrorMsg = string.Empty;

            List<string> NameList = new List<string>();
            List<string> GroupNameList = new List<string>();
            //StackupType檢查結果
            //bool StackupTypeCheck = false;
            //Top檢查結果
            bool TopCheck = false;
            //Bot檢查結果
            bool BotCheck = false;
            //VCC檢查結果
            bool VCCCheck = false;
            //GND檢查結果
            bool GNDCheck = false;
            //INx檢查結果
            bool INxCheck = false;
            //INxNum檢查結果
            bool INxNumCheck = false;
            //End檢查結果
            bool EndCheck = false;

            bool SxCheck = false;

            decimal ThicknessTotal = 0;

            int ColIndex = 0;
            //第二行開始才是資料
            for (int i = 1; i <= ExcelDt.Rows.Count - 1; i += 2)
            {
                //當前筆數
                ColIndex += 1;
                //每筆有兩列
                DataRow ColFirst = ExcelDt.Rows[i];
                DataRow ColSecond = ExcelDt.Rows[i + 1];


                decimal Thickness1 = 0;
                if(Decimal.TryParse(ColFirst[7].ToString(),out Thickness1))
                {
                    ThicknessTotal += Thickness1;
                    ColFirst[7] = Math.Round(Thickness1, 2, MidpointRounding.AwayFromZero);
                }

                //設定Layer欄位
                ColFirst[0] = "L" + ColIndex;
                ColFirst[2] = ColFirst[2].ToString().ToUpper();
                //數字欄位改為顯示小數2位數
                decimal LineNum;
                if (decimal.TryParse(ColFirst[4].ToString(), out LineNum))
                {
                    ColFirst[4] = Math.Round(LineNum, 2, MidpointRounding.AwayFromZero).ToString("N2");
                }

                if (decimal.TryParse(ColFirst[5].ToString(), out LineNum))
                {
                    ColFirst[5] = Math.Round(LineNum, 2, MidpointRounding.AwayFromZero).ToString("N2");
                }

                decimal ThicknessNum;
                if (decimal.TryParse(ColFirst[7].ToString(), out ThicknessNum))
                {
                    ColFirst[7] = Math.Round(ThicknessNum, 2, MidpointRounding.AwayFromZero).ToString("N2");
                }

                if (decimal.TryParse(ColSecond[7].ToString(), out ThicknessNum))
                {
                    ColSecond[7] = Math.Round(ThicknessNum, 2, MidpointRounding.AwayFromZero).ToString("N2");
                }

                //最後BOT代表結束
                if (ColFirst[2].ToString() == "BOT" || ColSecond[2].ToString() == "BOT")
                {
                    if(ColFirst[2].ToString() == "BOT")
                    {
                        NameList.Add(ColFirst[2].ToString());
                        ColFirst[1] = "Conductor";
                        ColFirst[3] = "B";
                        ColFirst[6] = "Cu + Plating";
                        ColSecond[6] = "Solder Mask";
                        ColSecond[0] = "";
                        if (string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                            BotCheck = true;

                        GroupNameList.Add(ColFirst[3].ToString());
                        break;

                    }
                    else
                    {
                        NameList.Add(ColSecond[2].ToString());
                        ColSecond[1] = "Conductor";
                        ColSecond[3] = "B";
                        ColSecond[6] = "Cu + Plating";

                        decimal Thickness = 0;
                        if (Decimal.TryParse(ColSecond[7].ToString(), out Thickness))
                        {
                            ThicknessTotal += Thickness;
                            ColSecond[7] = Math.Round(Thickness, 2, MidpointRounding.AwayFromZero).ToString("N2");
                        }
                        if (string.IsNullOrWhiteSpace(ColSecond[4].ToString()) || string.IsNullOrWhiteSpace(ColSecond[5].ToString()))
                            BotCheck = true;

                        GroupNameList.Add(ColSecond[3].ToString());

                        if (i == (ExcelDt.Rows.Count - 1))
                        {
                            
                            break;
                        }
                        else
                        {
                            EndCheck = true;
                            break;
                        }
                    }
                }
                

                decimal Thickness2 = 0;
                if (Decimal.TryParse(ColSecond[7].ToString(), out Thickness2))
                {
                    ThicknessTotal += Thickness2;
                    ColSecond[7] = Math.Round(Thickness2, 2, MidpointRounding.AwayFromZero).ToString("N2");
                }

                if (!string.IsNullOrWhiteSpace(ColFirst[2].ToString()))
                {
                    NameList.Add(ColFirst[2].ToString());
                    //固定值『TOP』B欄對應『Conductor』且欄位不可編輯，D欄對應『T』且欄位不可編輯，E欄 & F欄必須有值。
                    if (ColFirst[2].ToString() == "TOP")
                    {
                        ColFirst[1] = "Conductor";
                        ColSecond[1] = "Dielectric";
                        ColFirst[3] = "T";
                        if (string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                            TopCheck = true;
                    }
                    //固定值『BOT』B欄對應『Conductor』且欄位不可編輯，D欄對應『B』且欄位不可編輯，E欄 & F欄必須有值。
                    if (ColFirst[2].ToString() == "BOT")
                    {
                        ColFirst[1] = "Conductor";
                        ColFirst[3] = "B";
                        if (string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                            BotCheck = true;
                    }

                    //C欄=『INx』B欄對應『Conductor』且欄位不可編輯，D欄對應『數字』且欄位不可編輯， E欄 & F欄必須有值。（IN1=1, IN2=2, …最多到 9），不得重複。
                    if (ColFirst[2].ToString().StartsWith("IN"))
                    {
                        ////判斷INx數字跟D欄『數字』是否對應
                        //if (System.Text.RegularExpressions.Regex.Replace(ColFirst[2].ToString(), @"[^0-9]+", "").ToString() != ColFirst[3].ToString())
                        //    INxNumCheck = true;

                        ColFirst[1] = "Conductor";
                        ColFirst[3] = System.Text.RegularExpressions.Regex.Replace(ColFirst[2].ToString(), @"[^0-9]+", "").ToString();
                        ColSecond[1] = "Dielectric";

                        if (string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                            INxCheck = true;
                    }

                    //C欄=『VCCx』B欄對應『Conductor』且欄位不可編輯，D欄對應『不填值』且欄位不可編輯， E欄 & F欄必須有值。（VCC, VCC1 …最多到 9），不得重複。
                    if (ColFirst[2].ToString().StartsWith("VCC"))
                    {
                        ColFirst[1] = "Conductor";
                        ColSecond[1] = "Dielectric";
                        ColFirst[3] = "";

                        if (string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                            VCCCheck = true;
                    }

                    //C欄=『GNDx』B欄對應『Conductor』且欄位不可編輯，D欄對應『不填值』且欄位不可編輯， E欄 & F欄必須有值。（GND, GND1 …最多到 9），不得重複。
                    if (ColFirst[2].ToString().StartsWith("GND"))
                    {
                        ColFirst[1] = "Conductor";
                        ColSecond[1] = "Dielectric";
                        ColFirst[3] = "";

                        if (string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                            GNDCheck = true;
                    }

                    //C欄=『SVCCx、SGNDx』B欄對應『Plane』且欄位不可編輯，D欄對應『不填值』且欄位不可編輯， E欄 & F欄『不填值』且欄位不可編輯。（SVCC, SVCC1 …最多到 9），不得重複。
                    if (ColFirst[2].ToString().StartsWith("S"))
                    {
                        ColFirst[1] = "Plane";
                        ColSecond[1] = "Dielectric";
                        ColFirst[3] = "";
                        //ColFirst[4] = "";
                        //ColFirst[5] = "";
                        if (!string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || !string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                            SxCheck = true;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ColFirst[3].ToString()))
                    GroupNameList.Add(ColFirst[3].ToString());

                
                
                //判斷疊構類別只能有三種選項
                //if (!StackupType.Where(x => x.Contains(ColFirst[1].ToString())).Any() || (!StackupType.Where(x => x.Contains(ColSecond[1].ToString())).Any() && ColFirst[2].ToString() != "BOT"))
                //    StackupTypeCheck = true;

                //每個『Conductoe』or『Plane』的下一列必須固定帶上『Dielecatric』。
                if (ColFirst[2].ToString() != "BOT" && (ColFirst[1].ToString() == "Conductor" || ColFirst[1].ToString() == "Plane"))
                    ColSecond[1] = "Dielectric";
         
            }

            //第一筆資料是起始
            if (ExcelDt.Rows[0][6].ToString() != "Solder Mask")
            {
                if (ExcelDt.Rows[0][2].ToString() != "")
                    ErrorMsg += "第一筆起始疊構(Stack up)必需為Solder Mask\n";
                else
                    ExcelDt.Rows[0][6] = "Solder Mask";
            }
               

            //if (StackupTypeCheck)
            //    ErrorMsg += "疊構類別：只有 Conductor、Dielectric、Plane三種項目。\n";

            if (TopCheck)
                ErrorMsg += "固定值『TOP』，線寬 & 間距必須有值。\n";

            if (BotCheck)
                ErrorMsg += "固定值『BOT』，線寬 & 間距必須有值。\n";

            if (VCCCheck)
                ErrorMsg += "Name=『VCCx』正片設計：線寬 & 間距必須有值。\n";

            if (NameList.Where(x => x.StartsWith("VCC")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any())
                ErrorMsg += "VCC, VCC1 …最多到 9。\n";

            if (GNDCheck)
                ErrorMsg += "Name=『GNDx』正片設計：線寬 & 間距必須有值。\n";

            if (SxCheck)
                ErrorMsg += "負片設計：依規則需以『S』開頭填寫。\n";

            if (NameList.Where(x => x.StartsWith("GND")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any())
                ErrorMsg += "GND, GND1 …最多到 9。\n";

            //if (SVCCCheck)
            //    ErrorMsg += "Name=『SVCCx』疊構類別對應『Plane』且欄位不可編輯，HIGH SPEED GROUP NAME對應『不填值』且欄位不可編輯， 線寬 & 間距『不填值』且欄位不可編輯。\n";

            if (NameList.Where(x => x.StartsWith("SVCC")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any()) 
                ErrorMsg += "SVCC, SVCC1 …最多到 9。\n";

            //if (SGNDCheck)
            //    ErrorMsg += "Name=『SGNDx』疊構類別對應『Plane』且欄位不可編輯，HIGH SPEED GROUP NAME對應『不填值』且欄位不可編輯， 線寬 & 間距『不填值』且欄位不可編輯。\n";

            if (NameList.Where(x => x.StartsWith("SGND")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any()) 
                ErrorMsg += "SGND, SGND1, …最多到 9。\n";

            if (INxCheck)
                ErrorMsg += "Name=『INx』， 線寬 & 間距必須有值。\n";

            if (INxNumCheck)
                ErrorMsg += "Name=『INx』，HIGH SPEED GROUP NAME對應『數字』且欄位不可編輯。\n";

            if (NameList.Where(x => x.StartsWith("IN")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any())
                ErrorMsg += "IN1=1, IN2=2, …最多到 9。\n";


            if (GroupNameList.Count > 0 && GroupNameList[0] != "T")
            {
                ErrorMsg += "HIGH SPEED GROUP NAME規則 開頭應為 T。\n";
            }

            if (GroupNameList.Count > 0 && GroupNameList[GroupNameList.Count - 1] != "B")
            {
                ErrorMsg += "HIGH SPEED GROUP NAME規則 結尾應為 B。\n";
            }
            List<string> NameType = new List<string>();
            NameType.Add("IN");
            NameType.Add("VCC");
            NameType.Add("GND");
            NameType.Add("SVCC");
            NameType.Add("SGND");

            foreach(string item in NameType)
            {
                bool NameNumCheck = false;
                int NameNum1 = 0;
                List<string> NameTypeNumList = NameList.Where(x => x.StartsWith(item)).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "")).OrderBy(o => o).ToList();
                for (int i = 0; i <= NameTypeNumList.Count - 1; i++)
                {
                    if (Int32.TryParse(NameTypeNumList[i], out NameNum1))
                    {
                        if(item == "IN")
                        {
                            if ((i + 1) != NameNum1)
                            {
                                NameNumCheck = true;
                                break;
                            }
                        }
                        else
                        {
                            if (i != NameNum1)
                            {
                                NameNumCheck = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (NameTypeNumList[i] == "")
                            NameNum1 = 0;

                        if (i != NameNum1)
                        {
                            NameNumCheck = true;
                            break;
                        }
                    }
                }

                if(NameNumCheck)
                    ErrorMsg += "NAME規則『" + item+ "』數字不可跳號。\n";

            }
            
            //判斷Name是否有重複
            foreach(var item in NameList.GroupBy(x => x).Select(group => new { name = group.Key, count = group.Count() }).Where(z => z.count > 1))
            {
                ErrorMsg += "Name : " + item.name +" 值不可重複。\n";
            }

            NameType.Add("TOP");
            NameType.Add("BOT");
            foreach (string item in NameList)
            {
                bool NameCheck = true;
                foreach (string NameTypeitem in NameType)
                {
                    if (System.Text.RegularExpressions.Regex.Replace(item, @"[0-9]", "") == NameTypeitem)
                    {
                        NameCheck = false;
                    }
                }
                if (NameCheck)
                    ErrorMsg += "Name : " + item + " 值有錯誤。\n";
            }
            //if (NameList.GroupBy(x => x).Select(group => new { name=group.Key,count =group.Count() }).Where(z=>z.count > 1).Any())
            //    ErrorMsg += "Name(Top,GND,IN1,VCC,BOTTOM) 值不可重複。\n";

            if (EndCheck)
                ErrorMsg += "最後一筆起始疊構(Stack up)必需為Solder Mask。\n";

            

            return ErrorMsg;
        }


        public bool ExcelSampleCheck(XSSFWorkbook Sample)
        {
            bool IsCheck = true;

            List<string> sheetCheck = new List<string>();
            sheetCheck.Add("Stackup");
            sheetCheck.Add("MA");

            for (int i = 0; i <= workbook.NumberOfSheets - 1; i++)
            {
                //驗證Excel工作表
                if (!sheetCheck.Contains(Sample.GetSheetAt(i).SheetName))
                    IsCheck = false;
            }

            return IsCheck;
        }

        /// <summary> 把StackupDetail資料轉為DataTable
        /// 
        /// </summary>
        /// <param name="xSSFSheet">Excel Sheet</param>
        /// <param name="IsStackup">是否為Stackup</param>
        /// <returns></returns>
        public DataTable GetDataTableFromStackupDetail(List<PDC_StackupDetail> StackupDetalList)
        {
            DataTable dtExcelRecords = new DataTable();
            //取得有幾筆資料
            int TotalCount = StackupDetalList.Select(x => x.IndexNo).Max();

            foreach (PDC_StackupColumn StackupColumn in StackupColumnList.OrderBy(x => x.OrderNo))
            {
                dtExcelRecords.Columns.Add(StackupColumn.ColumnName);
            }
            for (int i = 0; i <= TotalCount; i++)
            {
                DataRow dr = dtExcelRecords.NewRow();

                for (int j = 0; j < StackupColumnList.Count - 1; j++) //對工作表每一列 
                {
                    Int64 StackupColumnID = StackupColumnList.Where(x => x.OrderNo == j).Select(x => x.StackupColumnID).First();

                    if (StackupDetalList.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID).Any())
                    {
                        string stackupData = StackupDetalList.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID)
                                                          .Select(x => x.ColumnValue)
                                                          .FirstOrDefault();
                        dr[j] = stackupData;
                    }
                }
                dtExcelRecords.Rows.Add(dr);
            }

            return dtExcelRecords;
        }


        public ISheet GetSheetSample()
        {
            XSSFWorkbook workbookExport = new XSSFWorkbook(ExportPath);
            MemoryStream ms = new MemoryStream();

            List<PDC_StackupDetail> PDC_StackupDetails = stackupDetals;

            #region == 取得範例樣式style ==
            //轉NPOI類型
            XSSFWorkbook Sample = new XSSFWorkbook(SamplePath);

            var StackupSheet = Sample.GetSheet("Stackup");
            //紀錄範例檔案style
            XSSFCellStyle StackupHeaderStyle = (XSSFCellStyle)workbookExport.CreateCellStyle();
            StackupHeaderStyle.CloneStyleFrom(StackupSheet.GetRow(3).GetCell(1).CellStyle);
            XSSFCellStyle ThicknessHeaderStyle = (XSSFCellStyle)workbookExport.CreateCellStyle();
            ThicknessHeaderStyle.CloneStyleFrom(StackupSheet.GetRow(3).GetCell(7).CellStyle);
            XSSFCellStyle ThicknessHeaderTotalStyle = (XSSFCellStyle)workbookExport.CreateCellStyle();
            ThicknessHeaderTotalStyle.CloneStyleFrom(StackupSheet.GetRow(1).GetCell(8).CellStyle);
            //一般欄位樣式
            XSSFCellStyle DataStyle = (XSSFCellStyle)workbookExport.CreateCellStyle();
            DataStyle.CloneStyleFrom(StackupSheet.GetRow(5).GetCell(1).CellStyle);
            //數字欄位樣式
            XSSFCellStyle NumberStyle = (XSSFCellStyle)workbookExport.CreateCellStyle();
            NumberStyle.CloneStyleFrom(StackupSheet.GetRow(5).GetCell(1).CellStyle);
            //NumberStyle.DataFormat = XSSFDataFormat.GetBuiltinFormat("0.00");
            XSSFDataFormat format = (XSSFDataFormat)workbookExport.CreateDataFormat();
            NumberStyle.SetDataFormat(format.GetFormat("0.00"));

            //取得版厚欄位排序
            Int32 ThicknessNum = StackupColumnList.Where(x => x.ColumnCode == "Col_08B").FirstOrDefault().OrderNo;
            //取得Excel版厚欄位
            string ThicknessCol = Char.ConvertFromUtf32(ThicknessNum + 64 + 1);
            #endregion

            // 新增試算表。
            ISheet sheet = workbookExport.GetSheet("Stackup");
            
            sheet.HorizontallyCenter = true;
            //更新有公式的欄位
            sheet.ForceFormulaRecalculation = true;
            //取得有幾筆資料
            int TotalCount = PDC_StackupDetails.Select(x => x.IndexNo).Max();

            for (int i = 0; i <= TotalCount; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 4);
                ICell[] Datacell = new ICell[StackupColumnList.Count];

                for (int j = 0; j <= StackupColumnList.Count - 1; j++)
                {
                    Datacell[j] = dataRow.CreateCell(j);
                    //設定資料樣式
                    if (StackupColumnList[j].DataType == "int")
                        Datacell[j].CellStyle = NumberStyle;
                    else
                        Datacell[j].CellStyle = DataStyle;

                    Int64 StackupColumnID = StackupColumnList.Where(x => x.OrderNo == j).Select(x => x.StackupColumnID).First();

                    if (PDC_StackupDetails.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID).Any())
                    {
                        string stackupData = PDC_StackupDetails.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID)
                                                               .Select(x => x.ColumnValue)
                                                               .FirstOrDefault();
                        Datacell[j].SetCellValue(stackupData);
                    }
                }
            }

            return sheet;
        }
        /// <summary> 匯出Excel範例
        /// 
        /// </summary>
        /// <param name="ExcelSheets"></param>
        /// <param name="headerCtyle"></param>
        /// <param name="DataCtyle"></param>
        /// <returns></returns>
        public MemoryStream ExportExcelSample(List<PDC_StackupDetail> stackupDetalsList = null)
        {

            XSSFWorkbook workbookExport = new XSSFWorkbook(ExportPath);
            MemoryStream ms = new MemoryStream();

            List<PDC_StackupDetail> PDC_StackupDetails = new List<PDC_StackupDetail>();
            if (stackupDetalsList != null)
                PDC_StackupDetails = stackupDetalsList;
            else
                PDC_StackupDetails = stackupDetals;

            #region == 取得範例樣式style ==
            //轉NPOI類型
            XSSFWorkbook Sample = new XSSFWorkbook(SamplePath);

            var StackupSheet = Sample.GetSheet("Stackup");
            
            #endregion

            // 新增試算表。
            ISheet sheet = (XSSFSheet)workbookExport.GetSheet("Stackup");
            ICell[] Thicknesscell = new ICell[StackupColumnList.Count];
            ICell[] Thicknesscell2 = new ICell[StackupColumnList.Count];

            //IRow ThicknessRow1 = sheet.CreateRow(2);
            //Thicknesscell[ThicknessNum] = ThicknessRow1.CreateCell(ThicknessNum);
            //Thicknesscell[ThicknessNum].CellStyle = ThicknessHeaderTotalStyle;
            //Thicknesscell[ThicknessNum].SetCellValue("總板厚");

           
            //取得有幾筆資料
            int TotalCount = PDC_StackupDetails.Select(x => x.IndexNo).Max();

            for (int i = 0; i <= TotalCount; i++)
            {
                IRow dataRow;
                if (sheet.LastRowNum >= (i + 4))
                    dataRow = sheet.GetRow(i + 4);
                else
                    dataRow = sheet.CreateRow(i + 4);

                ICell[] Datacell = new ICell[StackupColumnList.Count];

                for (int j = 0; j <= StackupColumnList.Count - 1; j++)
                {
                    if(dataRow.GetCell(j) == null)
                        Datacell[j] = dataRow.CreateCell(j);
                    else
                        Datacell[j] = dataRow.GetCell(j);
                    //設定資料樣式
                    //if (StackupColumnList[j].DataType == "int")
                    //    Datacell[j].CellStyle = NumberStyle;
                    //else
                    //    Datacell[j].CellStyle = DataStyle;

                    Int64 StackupColumnID = StackupColumnList.Where(x => x.OrderNo == j).Select(x => x.StackupColumnID).First();

                    if (PDC_StackupDetails.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID).Any())
                    {
                        string stackupData = PDC_StackupDetails.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID)
                                                               .Select(x => x.ColumnValue)
                                                               .FirstOrDefault();
                        if (StackupColumnList[j].ColumnCode == "Col_08B" || StackupColumnList[j].ColumnCode == "Col_05A" || StackupColumnList[j].ColumnCode == "Col_06A")
                        {
                            decimal value = 0;
                            if(decimal.TryParse(stackupData, out value))
                            {
                                Datacell[j].SetCellValue(value.ToString("N2"));
                                continue;
                            }
                        }
                        Datacell[j].SetCellValue(stackupData);
                    }
                }
            }

            workbookExport.Write(ms);

            ms.Close();
            ms.Dispose();

            return ms;
        }

        /// <summary> 匯出Excel範例
        /// 
        /// </summary>
        /// <param name="ExcelSheets"></param>
        /// <param name="headerCtyle"></param>
        /// <param name="DataCtyle"></param>
        /// <returns></returns>
        public MemoryStream ExportExcelSample(List<ExcelRow> excelRows, DataTable StackupDetalDt)
        {

            XSSFWorkbook workbookExport = new XSSFWorkbook(ExportPath);
            MemoryStream ms = new MemoryStream();

            #region == 取得範例樣式style ==
            //轉NPOI類型
            XSSFWorkbook Sample = new XSSFWorkbook(SamplePath);

            var StackupSheet = Sample.GetSheet("Stackup");

            IRow SampleRow = StackupSheet.GetRow(5);
            IRow SampleRow2 = StackupSheet.GetRow(6);
            #endregion

            // 新增試算表。
            //ISheet sheet = (XSSFSheet)workbookExport.GetSheet("Stackup");
            //ICell[] Thicknesscell = new ICell[StackupColumnList.Count];
            //ICell[] Thicknesscell2 = new ICell[StackupColumnList.Count];
            ISheet sheet = workbookExport.GetSheet("Stackup");
            
            //IRow ThicknessRow1 = sheet.CreateRow(2);
            //Thicknesscell[ThicknessNum] = ThicknessRow1.CreateCell(ThicknessNum);
            //Thicknesscell[ThicknessNum].CellStyle = ThicknessHeaderTotalStyle;
            //Thicknesscell[ThicknessNum].SetCellValue("總板厚");
            Int32 ColIndex = 0;
            //第二行開始才是資料
            for (int i = 1; i <= StackupDetalDt.Rows.Count - 1; i += 2)
            {
                //當前筆數
                ColIndex += 1;
                //每筆有兩列
                DataRow ColFirst = StackupDetalDt.Rows[i];
                DataRow ColSecond = StackupDetalDt.Rows[i + 1];

                IRow ExportRow1 = sheet.GetRow(i + 4);
                IRow ExportRow2 = sheet.GetRow(i + 5);

                IRow dataRow1;
                IRow dataRow2;
                if (excelRows.Where(x => x.Name == ColFirst[2].ToString()).Any())
                {
                    ExcelRow excelRow = excelRows.Where(x => x.Name == ColFirst[2].ToString()).FirstOrDefault();

                    dataRow1 = excelRow.FirstRow;
                    dataRow2 = excelRow.SecondRow;

                    CopyRows(dataRow1, ref ExportRow1);
                    CopyRows(dataRow2, ref ExportRow2);
                }



                for (int j = 0; j <= StackupColumnList.Count - 1; j++)
                {
                    ExportRow1.GetCell(j).SetCellValue(ColFirst[j].ToString());
                    ExportRow2.GetCell(j).SetCellValue(ColSecond[j].ToString());
                }

                
            }

            InsertRows(ref sheet, 10, 2);
            //    for (int i = 0; i <= TotalCount; i++)
            //{


            workbookExport.Write(ms);

            ms.Close();
            ms.Dispose();

            return ms;
        }

        public void InsertRows(ref ISheet sheet1, int fromRowIndex, int rowCount)
        {
            sheet1.ShiftRows(fromRowIndex, sheet1.LastRowNum, rowCount);

            for (int rowIndex = fromRowIndex; rowIndex < fromRowIndex + rowCount; rowIndex++)
            {
                IRow rowSource = sheet1.GetRow(rowIndex + rowCount);
                IRow rowInsert = sheet1.CreateRow(rowIndex);
                rowInsert.Height = rowSource.Height;
                for (int colIndex = 0; colIndex < rowSource.LastCellNum; colIndex++)
                {
                    ICell cellSource = rowSource.GetCell(colIndex);
                    ICell cellInsert = rowInsert.CreateCell(colIndex);
                    if (cellSource != null)
                    {
                        cellInsert.CellStyle = cellSource.CellStyle;
                    }
                }
            }
        }
        public void CopyRows(IRow rowSource, ref IRow rowInsert)
        {
            for (int colIndex = 0; colIndex < rowSource.LastCellNum; colIndex++)
            {
                ICell cellSource = rowSource.GetCell(colIndex);
                ICell cellInsert = rowInsert.GetCell(colIndex);
                if (cellSource != null)
                {
                    //cellInsert.CellStyle.CloneStyleFrom(cellSource.CellStyle);
                    switch (cellSource.CellType)
                    {
                        case CellType.String:
                            cellInsert.SetCellValue(cellSource.StringCellValue);
                            break;
                        case CellType.Numeric:
                            cellInsert.SetCellValue(cellSource.NumericCellValue.ToString());
                            break;
                        default:
                            
                            break;
                    }
                }
            }
        }
       

        /// <summary> 匯出Excel範例
        /// 
        /// </summary>
        /// <param name="ExcelSheets"></param>
        /// <param name="headerCtyle"></param>
        /// <param name="DataCtyle"></param>
        /// <returns></returns>
        public bool SaveExcel(Stream stream,string FilePath, List<PDC_StackupDetail> stackupDetalsList = null)
        {

            //轉NPOI類型
            XSSFWorkbook workbookExport = new XSSFWorkbook(stream);

            MemoryStream ms = new MemoryStream();

            List<PDC_StackupDetail> PDC_StackupDetails = new List<PDC_StackupDetail>();
            if (stackupDetalsList != null)
                PDC_StackupDetails = stackupDetalsList;
            else
                PDC_StackupDetails = stackupDetals;

            #region == 取得範例樣式style ==
            //轉NPOI類型
            XSSFWorkbook Sample = new XSSFWorkbook(SamplePath);

            var StackupSheet = Sample.GetSheet("Stackup");
            
          
            #endregion

            // 新增試算表。
            ISheet sheet = (XSSFSheet)workbookExport.GetSheet("Stackup");


            //取得有幾筆資料
            int TotalCount = PDC_StackupDetails.Select(x => x.IndexNo).Max();

            for (int i = 0; i <= TotalCount; i++)
            {
                IRow dataRow;
                if (sheet.LastRowNum >= (i + 4))
                    dataRow = sheet.GetRow(i + 4);
                else
                    dataRow = sheet.CreateRow(i + 4);

                ICell[] Datacell = new ICell[StackupColumnList.Count];

                for (int j = 0; j <= StackupColumnList.Count - 1; j++)
                {
                    if (dataRow.GetCell(j) == null)
                        Datacell[j] = dataRow.CreateCell(j);
                    else
                        Datacell[j] = dataRow.GetCell(j);
                    //設定資料樣式
                    //if (StackupColumnList[j].DataType == "int")
                    //    Datacell[j].CellStyle = NumberStyle;
                    //else
                    //    Datacell[j].CellStyle = DataStyle;

                    Int64 StackupColumnID = StackupColumnList.Where(x => x.OrderNo == j).Select(x => x.StackupColumnID).First();

                    if (PDC_StackupDetails.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID).Any())
                    {
                        string stackupData = PDC_StackupDetails.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID)
                                                               .Select(x => x.ColumnValue)
                                                               .FirstOrDefault();
                        if (StackupColumnList[j].ColumnCode == "Col_08B" || StackupColumnList[j].ColumnCode == "Col_05A" || StackupColumnList[j].ColumnCode == "Col_06A")
                        {
                            decimal value = 0;
                            if (decimal.TryParse(stackupData, out value))
                            {
                                Datacell[j].SetCellValue(value.ToString("N2"));
                                continue;
                            }
                        }
                        Datacell[j].SetCellValue(stackupData);
                    }
                }
            }


            sheet.HorizontallyCenter = true;
            //更新有公式的欄位
            sheet.ForceFormulaRecalculation = true;

            try
            {
                stream.Close();
                stream.Dispose();
                workbookExport.Write(ms);

                if(FileHelper.DeleteFile(FilePath))
                {
                    using (Stream fileStream = new FileStream(FilePath, FileMode.CreateNew))
                    {
                        //file.CopyToAsync(fileStream);
                        fileStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
                    }
                }
                //CopyStream(ms, stream);
                

                ms.Close();
                ms.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ms.Close();
                ms.Dispose();
                return false;
            }
        }

        /// <summary> 匯出Excel
        /// 
        /// </summary>
        /// <param name="ExcelSheets"></param>
        /// <param name="headerCtyle"></param>
        /// <param name="DataCtyle"></param>
        /// <returns></returns>
        public MemoryStream ExportExcelStream(List<DataTable> ExcelSheets, ICellStyle headerCtyle, ICellStyle DataCtyle)
        {
            MemoryStream ms = new MemoryStream();

            foreach (DataTable item in ExcelSheets)
            {
                // 新增試算表。
                ISheet sheet = workbook.CreateSheet(item.TableName);
                IRow headerRow = sheet.CreateRow(0);
                ICell[] headercell = new ICell[item.Columns.Count];
                for (int i = 0; i <= item.Columns.Count - 1; i++)
                {
                    headercell[i] = headerRow.CreateCell(i);
                    headercell[i].CellStyle = headerCtyle;  //  設定標題樣式
                    headercell[i].SetCellValue(item.Columns[i].ColumnName);
                }
                sheet.HorizontallyCenter = true;

                for (int i = 1; i <= item.Rows.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i);
                    ICell[] Datacell = new ICell[item.Columns.Count];

                    for (int j = 0; j <= item.Columns.Count - 1; j++)
                    {
                        Datacell[j] = dataRow.CreateCell(j);
                        Datacell[j].CellStyle = DataCtyle;  //  設定標題樣式
                        Datacell[j].SetCellValue(item.Rows[i][j].ToString());
                    }
                }

                //自動調整欄位寬度
                for (int j = 0; j < ExcelSheets[0].Columns.Count - 1; j++)
                {
                    sheet.AutoSizeColumn(j);
                }
            }
            workbook.Write(ms);

            ms.Close();
            ms.Dispose();

            return ms;
        }
    }
}
