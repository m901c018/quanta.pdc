using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace cns.Services.Helper
{
    public class ExcelHepler
    {
        public static HSSFWorkbook workbook = new HSSFWorkbook();
        //紀錄Stackup的HeaderStyle
        public static ICellStyle StackupHeaderStyle;
        public static ICellStyle StackupHeaderStyle2;
        //紀錄Stackup的Header欄位
        public static List<StackupColumnModel> StackupColumnList = new List<StackupColumnModel>();
        public static List<string[]> StackupSampleData;
        public static List<StackupDetalModel> stackupDetals;

        public static string SamplePath;
        public static string ExportPath;

        public class StackupColumnModel
        {
            /// <summary>
            /// 主鍵
            /// </summary>
            public Int64 StackupColumnID { get; set; }
            /// <summary>
            /// 欄位代碼
            /// </summary>
            public string ColumnCode { get; set; }
            /// <summary>
            /// 欄位名稱
            /// </summary>
            public string ColumnName { get; set; }
            /// <summary>
            /// 欄位類別
            /// </summary>
            public string ColumnType { get; set; }
            /// <summary>
            /// 欄位型態
            /// </summary>
            public string DataType { get; set; }
            /// <summary>
            /// 欄位長度
            /// </summary>
            public int MaxLength { get; set; }
            /// <summary>
            /// 欄位順序(0開始)
            /// </summary>
            public int OrderNo { get; set; }
            /// <summary>
            /// 小數點幾位數
            /// </summary>
            public int DecimalPlaces { get; set; }
            /// <summary>
            /// 上層節點
            /// </summary>
            public Int64 ParentColumnID { get; set; }
        }

        public class StackupDetalModel
        {
            /// <summary>
            /// 主鍵
            /// </summary>
            public Int64 StackupDetailID { get; set; }
            /// <summary>
            /// StackupColumn主鍵
            /// </summary>
            public Int64 StackupColumnID { get; set; }
            /// <summary>
            /// 欄位類別
            /// </summary>
            public string DataType { get; set; }
            /// <summary>
            /// 欄位型態
            /// </summary>
            public string ColumnType { get; set; }
            /// <summary>
            /// 第幾筆資料
            /// </summary>
            public int IndexNo { get; set; }
            /// <summary>
            /// 欄位內容
            /// </summary>
            public string ColumnValue { get; set; }
        }

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
            ExportPath = _hostingEnvironment.WebRootPath + "\\File\\CNS_ExportSample.xlsx";



            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 1, ColumnCode = "Col_01A", ColumnName = "層別\n(LAYER)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 0, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 2, ColumnCode = "Col_02A", ColumnName = "疊構類別\n(Stack up Type)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 1, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 3, ColumnCode = "Col_03A", ColumnName = "NAME\n(TOP, GND, GND1, IN1, ..,\nVCC, VCC1, BOTTOM)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 2, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 4, ColumnCode = "Col_04A", ColumnName = "HIGH SPEED\nGROUP NAME\n(T, 1, 2, 3…)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 3, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 5, ColumnCode = "Col_05A", ColumnName = "線寬\n(LINE WIDTH)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 4, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 6, ColumnCode = "Col_06A", ColumnName = "間距\n(SPACING)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 5, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 7, ColumnCode = "Col_07B", ColumnName = "疊構\n(Stack up)", ColumnType = "文字", DataType = "文字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 6, ParentColumnID = 0 });
            StackupColumnList.Add(new StackupColumnModel() { StackupColumnID = 8, ColumnCode = "Col_08B", ColumnName = "板厚\n(Thickness)", ColumnType = "文字", DataType = "數字", DecimalPlaces = 0, MaxLength = 256, OrderNo = 7, ParentColumnID = 0 });

            StackupSampleData = new List<string[]>();
            StackupSampleData.Add(new string[] { "", "", "", "", "", "", "Solder Mask", "" });
            StackupSampleData.Add(new string[] { "L1", "Conductor", "TOP", "T", "", "", "Cu + Plating", "" });
            StackupSampleData.Add(new string[] { "", "", "Dielectric", "", "", "", "", "" });
            StackupSampleData.Add(new string[] { "L1", "Conductor", "BOT", "T", "", "", "Cu + Plating", "" });
            StackupSampleData.Add(new string[] { "", "", "", "", "", "", "Solder Mask", "" });

            stackupDetals = new List<StackupDetalModel>();
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 7, IndexNo = 0, ColumnValue = "Solder Mask", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 1, IndexNo = 1, ColumnValue = "L1", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 2, IndexNo = 1, ColumnValue = "Conductor", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 3, IndexNo = 1, ColumnValue = "TOP", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 4, IndexNo = 1, ColumnValue = "T", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 7, IndexNo = 1, ColumnValue = "Cu + Plating", DataType = "int" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 3, IndexNo = 2, ColumnValue = "Dielectric", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 1, IndexNo = 3, ColumnValue = "L1", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 2, IndexNo = 3, ColumnValue = "Conductor", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 3, IndexNo = 3, ColumnValue = "BOT", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 4, IndexNo = 3, ColumnValue = "T", DataType = "string" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 7, IndexNo = 3, ColumnValue = "Cu + Plating", DataType = "int" });
            stackupDetals.Add(new StackupDetalModel() { StackupColumnID = 7, IndexNo = 4, ColumnValue = "Solder Mask", DataType = "string" });
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

        /// <summary> 解析Excel將資料轉為字典Dic<sheetname,DataTable>
        /// 
        /// </summary>
        /// <param name="Workbook">NPOI的HSSFWorkbook</param>
        /// <returns></returns>
        public List<DataTable> ReadExcelAsTableNPOI(XSSFWorkbook Workbook)
        {
            List<DataTable> AllSheet = new List<DataTable>();

            for (int i = 0; i < Workbook.NumberOfSheets; i++)
            {
                DataTable table = new DataTable();
                var sheet = Workbook.GetSheetAt(i);
                table.TableName = sheet.SheetName;
                //由第一列取標題做為欄位名稱
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = headerRow.FirstCellNum; j < cellCount; j++)
                {
                    if (headerRow.GetCell(j) == null)
                        continue;
                    //以欄位文字為名新增欄位，此處全視為字串型別以求簡化
                    table.Columns.Add(
                        new DataColumn(headerRow.GetCell(j).StringCellValue));
                }
                //略過第零列(標題列)，一直處理至最後一列
                for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);
                    if (row == null) continue;
                    if (row.FirstCellNum < 0) continue;
                    DataRow dataRow = table.NewRow();
                    //依先前取得的欄位數逐一設定欄位內容
                    for (int k = row.FirstCellNum; k < table.Columns.Count; k++)
                        if (row.GetCell(k) != null)
                            //如要針對不同型別做個別處理，可善用.CellType判斷型別
                            //再用.StringCellValue, .DateCellValue, .NumericCellValue...取值
                            //此處只簡單轉成字串
                            dataRow[k] = row.GetCell(k).ToString();
                    table.Rows.Add(dataRow);
                }

                AllSheet.Add(table);
            }

            return AllSheet;
        }

        /// <summary> 欄位設定
        /// 
        /// </summary>
        /// <param name="FontName">字型</param>
        /// <param name="FontHeight">文字大小</param>
        /// <param name="Align">文字排列</param>
        /// <param name="backgroundcolor">背景色(NPOI.HSSF.Util.HSSFColor)</param>
        /// <returns></returns>
        public ICellStyle CreateCellStyle(string FontName, double FontHeight, HorizontalAlignment Align, short backgroundcolor = -1)
        {
            //HSSFWorkbook workbook = new HSSFWorkbook();
            // 建立字型
            IFont font1 = workbook.CreateFont();
            font1.FontName = FontName;
            font1.FontHeightInPoints = FontHeight;

            //  建立樣式
            ICellStyle styleTitle = workbook.CreateCellStyle();
            styleTitle.SetFont(font1);
            styleTitle.Alignment = Align;
            if (backgroundcolor != -1)
                styleTitle.FillForegroundColor = backgroundcolor;


            return styleTitle;
        }

        /// <summary> 匯出Excel範例
        /// 
        /// </summary>
        /// <param name="ExcelSheets"></param>
        /// <param name="headerCtyle"></param>
        /// <param name="DataCtyle"></param>
        /// <returns></returns>
        public MemoryStream ExportExcelSample()
        {

            XSSFWorkbook workbookExport = new XSSFWorkbook(ExportPath);
            MemoryStream ms = new MemoryStream();


            #region == 取得範例樣式style ==
            //轉NPOI類型
            XSSFWorkbook Sample = new XSSFWorkbook(SamplePath);

            var StackupSheet = Sample.GetSheet("Stackup");
            //紀錄範例檔案style
            ICellStyle StackupHeaderStyle = workbookExport.CreateCellStyle();
            StackupHeaderStyle.CloneStyleFrom(StackupSheet.GetRow(4).GetCell(1).CellStyle);
            ICellStyle StackupHeaderStyle2 = workbookExport.CreateCellStyle();
            StackupHeaderStyle2.CloneStyleFrom(StackupSheet.GetRow(4).GetCell(7).CellStyle);
            ICellStyle ThicknessHeaderStyle3 = workbookExport.CreateCellStyle();
            ThicknessHeaderStyle3.CloneStyleFrom(StackupSheet.GetRow(2).GetCell(8).CellStyle);
            ICellStyle DataStyle = workbookExport.CreateCellStyle();
            DataStyle.CloneStyleFrom(StackupSheet.GetRow(6).GetCell(1).CellStyle);
            //取得版厚欄位排序
            Int32 ThicknessNum = StackupColumnList.Where(x => x.ColumnName.Contains("Thickness")).FirstOrDefault().OrderNo;
            //取得版厚欄位
            string ThicknessCol = Char.ConvertFromUtf32(ThicknessNum + 64 + 1);
            #endregion

            // 新增試算表。
            ISheet sheet = workbookExport.GetSheet("Stackup");
            ICell[] Thicknesscell = new ICell[StackupColumnList.Count];
            ICell[] Thicknesscell2 = new ICell[StackupColumnList.Count];

            IRow ThicknessRow1 = sheet.CreateRow(2);
            Thicknesscell[ThicknessNum] = ThicknessRow1.CreateCell(ThicknessNum);
            Thicknesscell[ThicknessNum].CellStyle = ThicknessHeaderStyle3;
            Thicknesscell[ThicknessNum].SetCellValue("總板厚");

            IRow ThicknessRow2 = sheet.CreateRow(3);
            Thicknesscell2[ThicknessNum] = ThicknessRow2.CreateCell(ThicknessNum);
            Thicknesscell2[ThicknessNum].CellStyle = ThicknessHeaderStyle3;
            Thicknesscell2[ThicknessNum].CellFormula = $"ROUND(SUM({ThicknessCol}6:{ThicknessCol}{6 + StackupSampleData.Count - 1}),2)";

            IRow headerRow = sheet.CreateRow(4);

            ICell[] headercell = new ICell[StackupColumnList.Count];
            for (int i = 0; i <= StackupColumnList.Count - 1; i++)
            {
                headercell[i] = headerRow.CreateCell(i);
                headercell[i].CellStyle = StackupColumnList[i].ColumnCode.Contains("B") ? StackupHeaderStyle2 : StackupHeaderStyle;  //  設定標題樣式
                headercell[i].SetCellValue(StackupColumnList[i].ColumnName);
                //設定欄位寬度
                sheet.SetColumnWidth(i, StackupSheet.GetColumnWidth(4));
            }
            sheet.HorizontallyCenter = true;
            //更新有公式的欄位
            sheet.ForceFormulaRecalculation = true;
            //取得有幾筆資料
            int TotalCount = stackupDetals.Select(x => x.IndexNo).Max();

            for (int i = 0; i <= TotalCount; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 5);
                ICell[] Datacell = new ICell[StackupColumnList.Count];

                for (int j = 0; j <= StackupColumnList.Count - 1; j++)
                {
                    Datacell[j] = dataRow.CreateCell(j);
                    Datacell[j].CellStyle = DataStyle;  //  設定資料樣式
                    Int64 StackupColumnID = StackupColumnList.Where(x => x.OrderNo == j).Select(x => x.StackupColumnID).First();

                    if (stackupDetals.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID).Any())
                    {
                        string stackupData = stackupDetals.Where(x => x.IndexNo == i && x.StackupColumnID == StackupColumnID)
                                                          .Select(x => x.ColumnValue)
                                                          .FirstOrDefault();
                        Datacell[j].SetCellValue(stackupData);
                    }
                }
            }

            workbookExport.Write(ms);

            ms.Close();
            ms.Dispose();

            return ms;
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
