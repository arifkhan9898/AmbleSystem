﻿﻿using System;
using System.Data;
using System.IO;
using System.Text;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Windows.Forms;
namespace AmbleClient.ExcelHelper
{

    public class ExcelHelper
    {
        /// <summary>   
        /// DataTable导出到Excel文件   
        /// </summary>   
        /// <param name="dtSource">源DataTable</param>   
        /// <param name="strHeaderText">表头文本</param>   
        /// <param name="strFileName">保存位置</param>
        /// <param name="strSheetName">工作表名称</param>
  
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName, string strSheetName, string[] oldColumnNames, string[] newColumnNames)
        {
            if (strSheetName == "")
            {
                strSheetName = "Sheet";
            }
            using (MemoryStream ms = Export(dtSource, strHeaderText, strSheetName, oldColumnNames, newColumnNames))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>   
        /// DataTable导出到Excel的MemoryStream   
        /// </summary>   
        /// <param name="dtSource">源DataTable</param>   
        /// <param name="strHeaderText">表头文本</param>   
        /// <param name="strSheetName">工作表名称</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText, string strSheetName, string[] oldColumnNames, string[] newColumnNames)
        {
            if (oldColumnNames.Length != newColumnNames.Length)
            {
                return new MemoryStream();
            }
            HSSFWorkbook workbook = new HSSFWorkbook();
            //HSSFSheet sheet = workbook.CreateSheet();// workbook.CreateSheet();
            ISheet sheet = workbook.CreateSheet(strSheetName);

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "http://....../";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                                 //填加xls文件作者信息   
                si.ApplicationName = "Amble Info";            //填加xls文件创建程序信息   
                si.LastAuthor = "Amble";           //填加xls文件最后保存者信息   
                si.Comments = "Generated by Amble Electronics";      //填加xls文件作者信息   
                si.Title = strHeaderText;               //填加xls文件标题信息   
                si.Subject = strHeaderText;              //填加文件主题信息   
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

           
            #region 取得列宽
            int[] arrColWidth = new int[oldColumnNames.Length];
            for (int i = 0; i < oldColumnNames.Length; i++)
            {
                arrColWidth[i] = Encoding.GetEncoding(936).GetBytes(newColumnNames[i]).Length;
            }


            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < oldColumnNames.Length; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][oldColumnNames[j]].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }

            }
            #endregion
            
            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet(strSheetName + ((int)rowIndex / 65535).ToString());
                    }

                    #region 表头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                    }
                    #endregion


                    #region 列头及样式
                    {
                        //HSSFRow headerRow = sheet.CreateRow(1);
                        IRow headerRow = sheet.CreateRow(1);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                    
                      
                        for (int i = 0; i < oldColumnNames.Length; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(newColumnNames[i]);
                            headerRow.GetCell(i).CellStyle = headStyle;
                            //设置列宽
                            sheet.SetColumnWidth(i, (arrColWidth[i] + 1) * 256);
                        }

                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion


                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                //foreach (DataColumn column in dtSource.Columns)
                for (int i = 0; i < oldColumnNames.Length; i++)
                {
                    ICell newCell = dataRow.CreateCell(i);

                    string drValue = row[oldColumnNames[i]].ToString();

                    switch (dtSource.Columns[oldColumnNames[i]].DataType.ToString())
                    {
                        case "System.String"://字符串类型   
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型   
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示   
                            break;
                        case "System.Boolean"://布尔型   
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型   
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型   
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理   
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }


            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                sheet = null;
                workbook = null;
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet   
                return ms;
            }
        }


        /// <summary>读取excel   
        /// 默认第一行为表头，导入第一个工作表
        /// </summary>   
        /// <param name="strFileName">excel文档路径</param>   
        /// <returns></returns>   
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            try
            {
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The file " + strFileName + " is used by another program, please close the program and try again.");
                return dt;
            
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, string SheetName, int HeaderRowIndex)
        {
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new HSSFWorkbook(file);
                return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
            }
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="SheetIndex">要获取数据的工作表序号(从0开始)</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, int SheetIndex, int HeaderRowIndex)
        {
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new HSSFWorkbook(file);
                string SheetName = workbook.GetSheetName(SheetIndex);
                return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
            }
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件流</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            IWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ExcelFileStream.Close();
            return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="ExcelFileStream">Excel文件流</param>
        /// <param name="SheetIndex">要获取数据的工作表序号(从0开始)</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            IWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ExcelFileStream.Close();
            string SheetName = workbook.GetSheetName(SheetIndex);
            return RenderDataTableFromExcel(workbook, SheetName, HeaderRowIndex);
        }

        /// <summary>
        /// 从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="workbook">要处理的工作薄</param>
        /// <param name="SheetName">要获取数据的工作表名称</param>
        /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(IWorkbook workbook, string SheetName, int HeaderRowIndex)
        {
            ISheet sheet = workbook.GetSheet(SheetName);
            DataTable table = new DataTable();
            try
            {
                IRow headerRow = sheet.GetRow(HeaderRowIndex);
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;

                #region 循环各行各列,写入数据到DataTable
                for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dataRow[j] = null;
                        }
                        else
                        {
                            //dataRow[j] = cell.ToString();
                            switch (cell.CellType)
                            {
                                case CellType.BLANK:
                                    dataRow[j] = null;
                                    break;
                                case CellType.BOOLEAN:
                                    dataRow[j] = cell.BooleanCellValue;
                                    break;
                                case CellType.NUMERIC:
                                    dataRow[j] = cell.ToString();
                                    break;
                                case CellType.STRING:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                                case CellType.ERROR:
                                    dataRow[j] = cell.ErrorCellValue;
                                    break;
                                case CellType.FORMULA:
                                default:
                                    dataRow[j] = "=" + cell.CellFormula;
                                    break;
                            }
                        }
                    }
                    table.Rows.Add(dataRow);
                    //dataRow[j] = row.GetCell(j).ToString();
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                table.Clear();
                table.Columns.Clear();
                table.Columns.Add("出错了");
                DataRow dr = table.NewRow();
                dr[0] = ex.Message;
                table.Rows.Add(dr);
                return table;
            }
            finally
            {
                //sheet.Dispose();
                workbook = null;
                sheet = null;
            }
            #region 清除最后的空行
            for (int i = table.Rows.Count - 1; i > 0; i--)
            {
                bool isnull = true;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j] != null)
                    {
                        if (table.Rows[i][j].ToString() != "")
                        {
                            isnull = false;
                            break;
                        }
                    }
                }
                if (isnull)
                {
                    table.Rows[i].Delete();
                }
            }
            #endregion
            return table;
        }
    }
}