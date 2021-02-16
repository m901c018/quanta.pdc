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
using cns.ViewModels;
using cns.Models;

namespace cns.Services.Helper
{
    public class ExcelHepler
    {
        public static HSSFWorkbook workbook = new HSSFWorkbook();
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
            ExportPath = _hostingEnvironment.WebRootPath + "\\File\\CNS_ExportSample.xlsx";
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
                for (int i = 5; i <= xSSFSheet.LastRowNum - 1; i++)
                {
                    //判斷資料是否為空
                    bool IsEmpty = true;
                    //每筆有兩列
                    XSSFRow Row = (XSSFRow)xSSFSheet.GetRow(i);
                    DataRow dr = dtExcelRecords.NewRow();
                    for (int j = 0; j <= StackupColumnList.Count - 1; j++) //對工作表每一列 
                    {
                        string cellValue = GetCellValue(Row, j); //獲取i行j列數據 
                        dr[j] = cellValue;
                        if (!string.IsNullOrWhiteSpace(cellValue))
                            IsEmpty = false;
                    }
                    //空資料不加入
                    if (!IsEmpty)
                        dtExcelRecords.Rows.Add(dr);
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
        public void ExcelStackupCheck(DataTable ExcelDt, m_ExcelPartial model)
        {
            model.Errmsg = string.Empty;

            List<string> NameList = new List<string>();
            List<string> GroupNameList = new List<string>();
            //Layer數字檢查結果
            bool LayerNumCheck = false;
            //StackupType檢查結果
            bool StackupTypeCheck = false;
            //Top檢查結果
            bool TopCheck = false;
            //Bot檢查結果
            bool BotCheck = false;
            //VCC檢查結果
            bool VCCCheck = false;
            //GND檢查結果
            bool GNDCheck = false;
            //SVCC檢查結果
            bool SVCCCheck = false;
            //SGND檢查結果
            bool SGNDCheck = false;
            //INx檢查結果
            bool INxCheck = false;
            //INxNum檢查結果
            bool INxNumCheck = false;
            //End檢查結果
            bool EndCheck = false;
            //GroupName檢查結果
            bool GroupNameCheck = false;

            decimal ThicknessTotal = 0;

            int ColIndex = 0;
            //第二行開始才是資料
            for (int i = 1; i <= ExcelDt.Rows.Count - 1; i += 2)
            {
                //當前筆數
                ColIndex += 1;
                //每筆有兩列
                DataRow ColFirst = ExcelDt.Rows[i];

                decimal Thickness1 = 0;
                if(Decimal.TryParse(ColFirst[7].ToString(),out Thickness1))
                {
                    ThicknessTotal += Thickness1;
                }

                //最後一筆疊構(Stack up)結尾是Solder Mask代表結束
                if (ColFirst[6].ToString() == "Solder Mask")
                {
                    if(i == (ExcelDt.Rows.Count - 1))
                        break;
                    else
                    {
                        EndCheck = true;
                        break;
                    }
                }

                DataRow ColSecond = ExcelDt.Rows[i + 1];

                decimal Thickness2 = 0;
                if (Decimal.TryParse(ColSecond[7].ToString(), out Thickness2))
                {
                    ThicknessTotal += Thickness2;
                }

                if (!string.IsNullOrWhiteSpace(ColFirst[2].ToString()))
                    NameList.Add(ColFirst[2].ToString());

                if (!string.IsNullOrWhiteSpace(ColFirst[3].ToString()))
                    GroupNameList.Add(ColFirst[3].ToString());

                int LayerIndex = 0;
                if (Int32.TryParse(System.Text.RegularExpressions.Regex.Replace(ColFirst[0].ToString(), @"[^0-9]+", ""), out LayerIndex))
                {
                    //判斷Layer欄位數字是否
                    if (LayerIndex != ColIndex)
                        LayerNumCheck = true;
                }
                //判斷疊構類別只能有三種選項
                if (!StackupType.Where(x => x.Contains(ColFirst[1].ToString())).Any() || (!StackupType.Where(x => x.Contains(ColSecond[1].ToString())).Any() && ColFirst[2].ToString() != "BOT"))
                    StackupTypeCheck = true;

                //每個『Conductoe』or『Plane』的下一列必須固定帶上『Dielecatric』。
                if (ColFirst[2].ToString() != "BOT" && (ColFirst[1].ToString() == "Conductor" || ColFirst[1].ToString() == "Plane"))
                    ColSecond[1] = "Dielectric";
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
                    //判斷INx數字跟D欄『數字』是否對應
                    if (System.Text.RegularExpressions.Regex.Replace(ColFirst[2].ToString(), @"[^0-9]+", "").ToString() != ColFirst[3].ToString())
                        INxNumCheck = true;

                    if (ColFirst[1].ToString() != "Conductor" || string.IsNullOrWhiteSpace(ColFirst[3].ToString()) || string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                        INxCheck = true;
                }
                //C欄=『VCCx』B欄對應『Conductor』且欄位不可編輯，D欄對應『不填值』且欄位不可編輯， E欄 & F欄必須有值。（VCC, VCC1 …最多到 9），不得重複。
                if (ColFirst[2].ToString().StartsWith("VCC"))
                {
                    if (ColSecond[1].ToString() != "Conductor" || !string.IsNullOrWhiteSpace(ColFirst[3].ToString()) || string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                        VCCCheck = true;
                }
                //C欄=『SVCCx』B欄對應『Plane』且欄位不可編輯，D欄對應『不填值』且欄位不可編輯， E欄 & F欄『不填值』且欄位不可編輯。（SVCC, SVCC1 …最多到 9），不得重複。
                if (ColFirst[2].ToString().StartsWith("SVCC"))
                {
                    if (ColSecond[1].ToString() != "Plane" || !string.IsNullOrWhiteSpace(ColFirst[3].ToString()) || !string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || !string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                        SVCCCheck = true;
                }
                //C欄=『GNDx』B欄對應『Conductor』且欄位不可編輯，D欄對應『不填值』且欄位不可編輯， E欄 & F欄必須有值。（GND, GND1 …最多到 9），不得重複。
                if (ColFirst[2].ToString().StartsWith("GND"))
                {
                    if (ColSecond[1].ToString() != "Conductor" || !string.IsNullOrWhiteSpace(ColFirst[3].ToString()) || string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                        GNDCheck = true;
                }
                //C欄=『SGNDx』B欄對應『Plane』且欄位不可編輯，D欄對應『不填值』且欄位不可編輯， E欄 & F欄『不填值』且欄位不可編輯。（SGND, SGND1, …最多到 9），不得重複。
                if (ColFirst[2].ToString().StartsWith("SGND"))
                {
                    if (ColSecond[1].ToString() != "Plane" || !string.IsNullOrWhiteSpace(ColFirst[3].ToString()) || !string.IsNullOrWhiteSpace(ColFirst[4].ToString()) || !string.IsNullOrWhiteSpace(ColFirst[5].ToString()))
                        SGNDCheck = true;
                }
            }

            //第一筆資料是起始
            if (ExcelDt.Rows[0][6].ToString() != "Solder Mask")
                model.Errmsg += "第一筆起始疊構(Stack up)必需為Solder Mask\n";

            if (LayerNumCheck)
                model.Errmsg += "Layer數字不可跳號\n";

            if (StackupTypeCheck)
                model.Errmsg += "疊構類別：只有 Conductor、Dielectric、Plane三種項目。\n";

            if (TopCheck)
                model.Errmsg += "固定值『TOP』，線寬 & 間距必須有值。\n";

            if (BotCheck)
                model.Errmsg += "固定值『BOT』，線寬 & 間距必須有值。\n";

            if (VCCCheck)
                model.Errmsg += "Name=『VCCx』疊構類別對應『Conductor』且欄位不可編輯，HIGH SPEED GROUP NAME對應『不填值』且欄位不可編輯， 線寬 & 間距必須有值。\n";

            if (NameList.Where(x => x.StartsWith("VCC")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any())
                model.Errmsg += "VCC, VCC1 …最多到 9。\n";

            if (GNDCheck)
                model.Errmsg += "Name=『GNDx』疊構類別對應『Conductor』且欄位不可編輯，HIGH SPEED GROUP NAME對應『不填值』且欄位不可編輯， 線寬 & 間距必須有值。\n";

            if (NameList.Where(x => x.StartsWith("GND")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any())
                model.Errmsg += "GND, GND1 …最多到 9。\n";

            if (SVCCCheck)
                model.Errmsg += "Name=『SVCCx』疊構類別對應『Plane』且欄位不可編輯，HIGH SPEED GROUP NAME對應『不填值』且欄位不可編輯， 線寬 & 間距『不填值』且欄位不可編輯。\n";

            if (NameList.Where(x => x.StartsWith("SVCC")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any()) 
                model.Errmsg += "SVCC, SVCC1 …最多到 9。\n";

            if (SGNDCheck)
                model.Errmsg += "Name=『SGNDx』疊構類別對應『Plane』且欄位不可編輯，HIGH SPEED GROUP NAME對應『不填值』且欄位不可編輯， 線寬 & 間距『不填值』且欄位不可編輯。\n";

            if (NameList.Where(x => x.StartsWith("SGND")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any()) 
                model.Errmsg += "SGND, SGND1, …最多到 9。\n";

            if (INxCheck)
                model.Errmsg += "Name=『INx』疊構類別對應『Conductor』且欄位不可編輯， 線寬 & 間距必須有值。\n";

            if (INxNumCheck)
                model.Errmsg += "Name=『INx』，HIGH SPEED GROUP NAME對應『數字』且欄位不可編輯。\n";

            if (NameList.Where(x => x.StartsWith("IN")).Select(x => System.Text.RegularExpressions.Regex.Replace(x, @"[^0-9]+", "").ToString()).Where(x => x.Length > 1).Any())
                model.Errmsg += "IN1=1, IN2=2, …最多到 9。\n";

            if (GroupNameList[0] != "T")
            {
                GroupNameCheck = true;
            }

            if (GroupNameList[GroupNameList.Count - 1] != "B")
            {
                GroupNameCheck = true;
            }
            //取出數字部分
            var GroupNameNumList = GroupNameList.Where(x => x != "T" && x != "B").ToList();

            int GroupNameNum1 = 0;
            for (int i = 0; i <= GroupNameNumList.Count - 1; i++) 
            {
                if (Int32.TryParse(GroupNameNumList[i], out GroupNameNum1)) 
                {
                    if((i + 1) != GroupNameNum1)
                    {
                        GroupNameCheck = true;
                    }
                    break;
                }
                else
                {
                    GroupNameCheck = true;
                    break;
                }
            }

            if (GroupNameCheck)
                model.Errmsg += "HIGH SPEED GROUP NAME規則 = T + 數字（小至大自動排序）+ B。\n";

            if (EndCheck)
                model.Errmsg += "最後一筆起始疊構(Stack up)必需為Solder Mask。\n";

            model.ExcelSheetDts.Add(ExcelDt);
            model.ThicknessTotal = decimal.Round(ThicknessTotal, 2);
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
            ICellStyle StackupHeaderStyle = workbookExport.CreateCellStyle();
            StackupHeaderStyle.CloneStyleFrom(StackupSheet.GetRow(4).GetCell(1).CellStyle);
            ICellStyle ThicknessHeaderStyle = workbookExport.CreateCellStyle();
            ThicknessHeaderStyle.CloneStyleFrom(StackupSheet.GetRow(4).GetCell(7).CellStyle);
            ICellStyle ThicknessHeaderTotalStyle = workbookExport.CreateCellStyle();
            ThicknessHeaderTotalStyle.CloneStyleFrom(StackupSheet.GetRow(2).GetCell(8).CellStyle);
            //一般欄位樣式
            ICellStyle DataStyle = workbookExport.CreateCellStyle();
            DataStyle.CloneStyleFrom(StackupSheet.GetRow(6).GetCell(1).CellStyle);
            //數字欄位樣式
            ICellStyle NumberStyle = workbookExport.CreateCellStyle();
            NumberStyle.CloneStyleFrom(StackupSheet.GetRow(6).GetCell(1).CellStyle);
            NumberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            //取得版厚欄位排序
            Int32 ThicknessNum = StackupColumnList.Where(x => x.ColumnCode == "Col_08B").FirstOrDefault().OrderNo;
            //取得Excel版厚欄位
            string ThicknessCol = Char.ConvertFromUtf32(ThicknessNum + 64 + 1);
            #endregion

            // 新增試算表。
            ISheet sheet = workbookExport.GetSheet("Stackup");
            ICell[] Thicknesscell = new ICell[StackupColumnList.Count];
            ICell[] Thicknesscell2 = new ICell[StackupColumnList.Count];

            IRow ThicknessRow1 = sheet.CreateRow(2);
            Thicknesscell[ThicknessNum] = ThicknessRow1.CreateCell(ThicknessNum);
            Thicknesscell[ThicknessNum].CellStyle = ThicknessHeaderTotalStyle;
            Thicknesscell[ThicknessNum].SetCellValue("總板厚");

            IRow ThicknessRow2 = sheet.CreateRow(3);
            Thicknesscell2[ThicknessNum] = ThicknessRow2.CreateCell(ThicknessNum);
            Thicknesscell2[ThicknessNum].CellStyle = ThicknessHeaderTotalStyle;
            Thicknesscell2[ThicknessNum].CellFormula = $"ROUND(SUM({ThicknessCol}6:{ThicknessCol}{6 + PDC_StackupDetails.Select(x => x.IndexNo).Max()}),2)";

            IRow headerRow = sheet.CreateRow(4);

            ICell[] headercell = new ICell[StackupColumnList.Count];
            for (int i = 0; i <= StackupColumnList.Count - 1; i++)
            {
                headercell[i] = headerRow.CreateCell(i);
                headercell[i].CellStyle = StackupColumnList[i].ColumnCode.Contains("B") ? ThicknessHeaderStyle : StackupHeaderStyle;  //  設定標題樣式
                headercell[i].SetCellValue(StackupColumnList[i].ColumnName);
                //設定欄位寬度
                sheet.SetColumnWidth(i, StackupSheet.GetColumnWidth(4));
            }
            sheet.HorizontallyCenter = true;
            //更新有公式的欄位
            sheet.ForceFormulaRecalculation = true;
            //取得有幾筆資料
            int TotalCount = PDC_StackupDetails.Select(x => x.IndexNo).Max();

            for (int i = 0; i <= TotalCount; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 5);
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
            //紀錄範例檔案style
            ICellStyle StackupHeaderStyle = workbookExport.CreateCellStyle();
            StackupHeaderStyle.CloneStyleFrom(StackupSheet.GetRow(4).GetCell(1).CellStyle);
            ICellStyle ThicknessHeaderStyle = workbookExport.CreateCellStyle();
            ThicknessHeaderStyle.CloneStyleFrom(StackupSheet.GetRow(4).GetCell(7).CellStyle);
            ICellStyle ThicknessHeaderTotalStyle = workbookExport.CreateCellStyle();
            ThicknessHeaderTotalStyle.CloneStyleFrom(StackupSheet.GetRow(2).GetCell(8).CellStyle);
            //一般欄位樣式
            ICellStyle DataStyle = workbookExport.CreateCellStyle();
            DataStyle.CloneStyleFrom(StackupSheet.GetRow(6).GetCell(1).CellStyle);
            //數字欄位樣式
            ICellStyle NumberStyle = workbookExport.CreateCellStyle();
            NumberStyle.CloneStyleFrom(StackupSheet.GetRow(6).GetCell(1).CellStyle);
            NumberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            //取得版厚欄位排序
            Int32 ThicknessNum = StackupColumnList.Where(x => x.ColumnCode == "Col_08B").FirstOrDefault().OrderNo;
            //取得Excel版厚欄位
            string ThicknessCol = Char.ConvertFromUtf32(ThicknessNum + 64 + 1);
            #endregion

            // 新增試算表。
            ISheet sheet = workbookExport.GetSheet("Stackup");
            ICell[] Thicknesscell = new ICell[StackupColumnList.Count];
            ICell[] Thicknesscell2 = new ICell[StackupColumnList.Count];

            IRow ThicknessRow1 = sheet.CreateRow(2);
            Thicknesscell[ThicknessNum] = ThicknessRow1.CreateCell(ThicknessNum);
            Thicknesscell[ThicknessNum].CellStyle = ThicknessHeaderTotalStyle;
            Thicknesscell[ThicknessNum].SetCellValue("總板厚");

            IRow ThicknessRow2 = sheet.CreateRow(3);
            Thicknesscell2[ThicknessNum] = ThicknessRow2.CreateCell(ThicknessNum);
            Thicknesscell2[ThicknessNum].CellStyle = ThicknessHeaderTotalStyle;
            Thicknesscell2[ThicknessNum].CellFormula = $"ROUND(SUM({ThicknessCol}6:{ThicknessCol}{6 + PDC_StackupDetails.Select(x => x.IndexNo).Max()}),2)";

            IRow headerRow = sheet.CreateRow(4);

            ICell[] headercell = new ICell[StackupColumnList.Count];
            for (int i = 0; i <= StackupColumnList.Count - 1; i++)
            {
                headercell[i] = headerRow.CreateCell(i);
                headercell[i].CellStyle = StackupColumnList[i].ColumnCode.Contains("B") ? ThicknessHeaderStyle : StackupHeaderStyle;  //  設定標題樣式
                headercell[i].SetCellValue(StackupColumnList[i].ColumnName);
                //設定欄位寬度
                sheet.SetColumnWidth(i, StackupSheet.GetColumnWidth(4));
            }
            sheet.HorizontallyCenter = true;
            //更新有公式的欄位
            sheet.ForceFormulaRecalculation = true;
            //取得有幾筆資料
            int TotalCount = PDC_StackupDetails.Select(x => x.IndexNo).Max();

            for (int i = 0; i <= TotalCount; i++)
            {
                IRow dataRow = sheet.CreateRow(i + 5);
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
