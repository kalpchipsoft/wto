using System;
using System.Web.Mvc;
using BusinessService.Notification;
using BusinessObjects.Notification;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;

namespace WTO.Controllers.WTO
{
    public class NotificationListController : Controller
    {
        // GET: NotificationList
        public ActionResult Index(Search_Notification obj)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationListBusinessService objBS = new NotificationListBusinessService();
                return View("~/Views/WTO/NotificationList.cshtml", objBS.PageLoad_NotificationsList(obj));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult NotificationExport(string NotificationNumber, int CountryId, DateTime? FinalDateOfComments_From, DateTime? FinalDateOfComments_To, int NotificationType, int StatusId, int ActionId, string ActionStatus, DateTime? MeetingDate, string PendingFrom)
        {
            Search_Notification obj = new Search_Notification();
            obj.NotificationNumber = NotificationNumber;
            obj.CountryId = CountryId;
            obj.FinalDateOfComments_From = FinalDateOfComments_From;
            obj.FinalDateOfComments_To = FinalDateOfComments_To;
            obj.NotificationType = NotificationType;
            obj.StatusId = StatusId;
            obj.ActionId = ActionId;
            obj.ActionStatus = ActionStatus;
            obj.MeetingDate = MeetingDate;
            obj.PendingFrom = PendingFrom;
            NotificationListBusinessService objBS = new NotificationListBusinessService();
            using (DataTable dt = objBS.ExportNotificationList(obj))
            {
                try
                {
                    ExportToExcel(dt, "Notification-List", "Notification-List", "Notification-List");
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
            return View();
        }
        public void ExportToExcel(DataTable dtTemp, string filename, string sheetname, string sheetHeader)
        {
            try
            {
                string Today = DateTime.Now.ToString("d MMM yyyy");
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                string FileName = "";
                if (filename.EndsWith(".xls"))
                    FileName = filename;
                else
                    FileName = filename + ".xls";
                HSSFSheet sheet1 = (NPOI.HSSF.UserModel.HSSFSheet)hssfworkbook.CreateSheet(sheetname);
                sheet1.DisplayGridlines = true;
                sheet1.Footer.Right = "Page " + HSSFFooter.Page;
                sheet1.SetMargin(MarginType.FooterMargin, (double)0.25);

                #region "Print Setup"
                sheet1.SetMargin(MarginType.RightMargin, (double)0.25);
                sheet1.SetMargin(MarginType.TopMargin, (double)0.75);
                sheet1.SetMargin(MarginType.LeftMargin, (double)0.50);
                sheet1.SetMargin(MarginType.BottomMargin, (double)0.75);

                sheet1.PrintSetup.Copies = 1;
                sheet1.PrintSetup.Landscape = false;
                sheet1.PrintSetup.PaperSize = 9;

                sheet1.PrintSetup.Scale = 90;
                sheet1.IsPrintGridlines = true;
                sheet1.Autobreaks = true;
                sheet1.FitToPage = false;

                #endregion
                HSSFRow rowHeader = (NPOI.HSSF.UserModel.HSSFRow)sheet1.CreateRow(0);
                #region "Header"
                for (int k = 0; k < dtTemp.Columns.Count; k++)
                {
                    String columnName = dtTemp.Columns[k].ToString().Replace("_"," ");
                    HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)rowHeader.CreateCell(k);
                    var headerLabelCellStyle = hssfworkbook.CreateCellStyle();
                    headerLabelCellStyle.LeftBorderColor = HSSFColor.Black.Index;
                    headerLabelCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    headerLabelCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    headerLabelCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    headerLabelCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    headerLabelCellStyle.BottomBorderColor = HSSFColor.Grey50Percent.Index;
                    headerLabelCellStyle.TopBorderColor = HSSFColor.Grey50Percent.Index;
                    headerLabelCellStyle.LeftBorderColor = HSSFColor.Grey50Percent.Index;
                    headerLabelCellStyle.RightBorderColor = HSSFColor.Grey50Percent.Index;
                    headerLabelCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                    headerLabelCellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
                    headerLabelCellStyle.ShrinkToFit = true;
                    var formate = hssfworkbook.CreateDataFormat();
                    var headerLabelFont = hssfworkbook.CreateFont();
                    headerLabelFont.FontHeight = 200;
                    headerLabelFont.Boldweight = (short)FontBoldWeight.Bold;
                    var headerDataFormat = hssfworkbook.CreateDataFormat();
                    headerLabelCellStyle.SetFont(headerLabelFont);
                    cell.SetCellValue(columnName);
                    cell.CellStyle = headerLabelCellStyle;
                    rowHeader.Height = 400;
                }
                #endregion
               
                #region "Row"
                int count = 1;


                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    var RowCellStyle = hssfworkbook.CreateCellStyle();
                    HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)sheet1.CreateRow(count);
                    for (int l = 0; l < dtTemp.Columns.Count; l++)
                    {
                        HSSFCell cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(l);
                        String columnName = dtTemp.Columns[l].ToString();
                        cell.CellStyle.WrapText = true;
                        cell.SetCellValue(Convert.ToString(dtTemp.Rows[i][columnName]));
                        cell.CellStyle = RowCellStyle;
                        cell.CellStyle.WrapText = true;
                    }
                    count++;
                }
                #endregion

                #region "Set Columns width"
                for (int d = 0; d < dtTemp.Columns.Count; d++)
                {
                    string columnName = dtTemp.Columns[d].ToString();
                    if (columnName == "Notification")
                    {
                        sheet1.SetColumnWidth(d, 35 * 300);
                    }
                    else if (columnName == "Processing_Status")
                    {
                        sheet1.SetColumnWidth(d, 35 * 300);
                    }
                    else if (columnName == "Action_Status")
                    {
                        sheet1.SetColumnWidth(d, 35 * 300);
                    }
                    else
                    {
                        sheet1.AutoSizeColumn(d);
                    }
                }
                #endregion

                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", FileName));
                Response.Clear();
                Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
        {
            MemoryStream file1 = new MemoryStream();
            hssfworkbook.Write(file1);
            return file1;
        }
    }
}