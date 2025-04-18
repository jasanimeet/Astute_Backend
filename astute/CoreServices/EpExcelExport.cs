using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace astute.CoreServices
{
    public class EpExcelExport
    {
        public const String ImageURL = "/Files/CategoryValueIcon/";
        public const String VideoURL = "/Files/CategoryValueIcon/";
        public const String CertiURL = "/Files/CategoryValueIcon/";
        public static void removingGreenTagWarning(ExcelWorksheet template1, string address)
        {
            var xdoc = template1.WorksheetXml;
            //Create the import nodes (note the plural vs singular
            var ignoredErrors = xdoc.CreateNode(System.Xml.XmlNodeType.Element, "ignoredErrors", xdoc.DocumentElement.NamespaceURI);
            var ignoredError = xdoc.CreateNode(System.Xml.XmlNodeType.Element, "ignoredError", xdoc.DocumentElement.NamespaceURI);
            ignoredErrors.AppendChild(ignoredError);

            //Attributes for the INNER node
            var sqrefAtt = xdoc.CreateAttribute("sqref");
            sqrefAtt.Value = address;// Or whatever range is needed....

            var flagAtt = xdoc.CreateAttribute("numberStoredAsText");
            flagAtt.Value = "1";

            ignoredError.Attributes.Append(sqrefAtt);
            ignoredError.Attributes.Append(flagAtt);

            //Now put the OUTER node into the worksheet xml
            xdoc.LastChild.AppendChild(ignoredErrors);
        }
        public static void CreateExcel(DataTable dt, string _strFolderPath, string _strFilePath, string colorType = "")
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int inStartIndex = 7;
                    int inwrkrow = 7;
                    int inEndCounter = dt.Rows.Count + inStartIndex;
                    int TotalRow = dt.Rows.Count;
                    int i;
                    string values_1, S_Detail, cut, status, ForCust_Hold;
                    Int64 number_1 = 0;
                    bool success1 = false;

                    Color colFromHex_Pointer = System.Drawing.ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = System.Drawing.ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = System.Drawing.ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = System.Drawing.ColorTranslator.FromHtml("#fff2cc");

                    #region Add header details
                    ep.Workbook.Properties.Author = "SUNRISE DIAMOND";
                    ep.Workbook.Properties.Title = "SUNRISE DIAMOND PVT. LTD.";
                    ep.Workbook.Worksheets.Add("SearchStock");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    var pic = worksheet.Drawings.AddPicture("Landscape", new FileInfo("D:\\Projects\\astute\\astute\\Files\\CategoryValueIcon\\MILKY_30062023060725.jpg"));
                    pic.SetPosition(6, 0, 50, 0);
                    pic.Effect.SetPresetShadow(ePresetExcelShadowType.OuterBottomRight);
                    pic.Effect.OuterShadow.Distance = 10;
                    pic.Effect.SetPresetSoftEdges(ePresetExcelSoftEdgesType.SoftEdge5Pt);

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";
                    worksheet.Cells[1, 3, 3, 12].Style.Font.Bold = true;

                    worksheet.Cells[1, 6].Value = "SUNRISE DIAMONDS INVENTORY FOR THE DATE " + " " + DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells[1, 6].Style.Font.Size = 24;
                    worksheet.Cells[1, 6].Style.Font.Bold = true;

                    Color colFromHex_H1 = System.Drawing.ColorTranslator.FromHtml("#8497b0");
                    worksheet.Cells[1, 6].Style.Font.Color.SetColor(colFromHex_H1);
                    Color col_color_Red = System.Drawing.ColorTranslator.FromHtml("#ff0000");

                    worksheet.Row(5).Height = 40;
                    worksheet.Row(6).Height = 40;
                    worksheet.Row(6).Style.WrapText = true;

                    worksheet.Cells[2, 2].Value = "All Prices are final Selling Cash Price";
                    worksheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 2].Style.Font.Size = 11;
                    worksheet.Cells[2, 2].Style.Font.Bold = true;
                    worksheet.Cells[2, 2].Style.Font.Color.SetColor(col_color_Red);

                    worksheet.Cells[2, 6].Value = "UNIT 1, 14/F, PENINSULA SQUARE, EAST WING, 18 SUNG ON STREET, HUNG HOM, KOWLOON, HONG KONG TEL : +852 - 27235100    FAX : +852 - 2314 9100";
                    worksheet.Cells[2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 6].Style.Font.Size = 11;
                    worksheet.Cells[2, 6].Style.Font.Bold = true;
                    worksheet.Cells[2, 6].Style.Font.Color.SetColor(colFromHex_H1);

                    worksheet.Cells[3, 6].Value = "Email Id : sales@sunrisediam.com    Web : www.sunrisediamonds.com.hk . Download Apps on Android, IOS and Windows";
                    worksheet.Cells[3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[3, 6].Style.Font.Size = 11;
                    worksheet.Cells[3, 6].Style.Font.Bold = true;
                    worksheet.Cells[3, 6].Style.Font.Color.SetColor(colFromHex_H1);

                    worksheet.Cells[5, 1].Value = "Total";
                    worksheet.Cells[5, 1, 5, 49].Style.Font.Bold = true;
                    worksheet.Cells[5, 1, 5, 49].Style.Font.Size = 11;
                    worksheet.Cells[5, 1, 5, 49].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[5, 1, 5, 49].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[5, 1, 5, 49].Style.Font.Size = 11;

                    worksheet.Cells[6, 1, 6, 49].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[6, 1, 6, 49].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[6, 1, 6, 49].Style.Font.Size = 10;
                    worksheet.Cells[6, 1, 6, 49].Style.Font.Bold = true;

                    worksheet.Cells[6, 1, 6, 49].AutoFilter = true;
                    worksheet.Cells[1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    var cellBackgroundColor1 = worksheet.Cells[6, 1, 6, 49].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);
                    #endregion

                    #region Header Column Name Declaration
                    worksheet.Cells[6, 1].Value = "Sr. No";
                    worksheet.Cells[6, 2].Value = "DNA";
                    worksheet.Cells[6, 3].Value = "View Image";
                    worksheet.Cells[6, 4].Value = "HD Movie";
                    worksheet.Cells[6, 5].Value = "Stock Id";
                    worksheet.Cells[6, 6].Value = "Location";
                    worksheet.Cells[6, 7].Value = "Status";

                    worksheet.Cells[6, 8].Value = "Shape";
                    worksheet.Cells[6, 9].Value = "Pointer";
                    worksheet.Cells[6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[6, 9].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                    worksheet.Cells[6, 10].Value = "Lab";
                    worksheet.Cells[6, 11].Value = "Certi Type";
                    worksheet.Cells[6, 12].Value = "Certi No.";
                    worksheet.Cells[6, 13].Value = "BGM";
                    worksheet.Cells[6, 14].Value = "Color";
                    worksheet.Cells[6, 15].Value = "Clarity";
                    worksheet.Cells[6, 16].Value = "Cts";

                    worksheet.Cells[6, 17].Value = "Rap Price($)";
                    worksheet.Cells[6, 18].Value = "Rap Amt($)";
                    worksheet.Cells[6, 19].Value = "Offer Disc.(%)"; // "Disc(%)";
                    worksheet.Cells[6, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[6, 19].Style.Fill.BackgroundColor.SetColor(colFromHex_Dis);

                    worksheet.Cells[6, 20].Value = "Offer Value($)"; // "Net Amt($)";
                    worksheet.Cells[6, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[6, 20].Style.Fill.BackgroundColor.SetColor(colFromHex_Dis);
                    worksheet.Cells[6, 21].Value = "Price/Cts";

                    worksheet.Cells[6, 22].Value = "Cut";
                    worksheet.Cells[6, 23].Value = "Polish";
                    worksheet.Cells[6, 24].Value = "Symm";
                    worksheet.Cells[6, 25].Value = "Fls";
                    worksheet.Cells[6, 26].Value = "Ratio";
                    worksheet.Cells[6, 27].Value = "Length";
                    worksheet.Cells[6, 28].Value = "Width";
                    worksheet.Cells[6, 29].Value = "Depth";
                    worksheet.Cells[6, 30].Value = "Depth(%)";
                    worksheet.Cells[6, 31].Value = "Table(%)";
                    worksheet.Cells[6, 32].Value = "Key To Symbol";
                    worksheet.Cells[6, 33].Value = "GIA Comment";
                    worksheet.Cells[6, 34].Value = "Culet";
                    worksheet.Cells[6, 35].Value = "Table Black";
                    worksheet.Cells[6, 36].Value = "Crown Black";
                    worksheet.Cells[6, 37].Value = "Table White";
                    worksheet.Cells[6, 38].Value = "Crown White";
                    worksheet.Cells[6, 39].Value = "Cr Ang";
                    worksheet.Cells[6, 40].Value = "Cr Ht";
                    worksheet.Cells[6, 41].Value = "Pav Ang";
                    worksheet.Cells[6, 42].Value = "Pav Ht";
                    worksheet.Cells[6, 43].Value = "Table Open";
                    worksheet.Cells[6, 44].Value = "Crown Open";
                    worksheet.Cells[6, 45].Value = "Pav Open";
                    worksheet.Cells[6, 46].Value = "Girdle Open";
                    worksheet.Cells[6, 47].Value = "Girdle(%)";   //41
                    worksheet.Cells[6, 48].Value = "Girdle Type"; //42
                    worksheet.Cells[6, 49].Value = "Laser Insc";  //43

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[6, 1, 6, 49].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format

                    worksheet.View.FreezePanes(7, 1);

                    worksheet.Cells[6, 1].AutoFitColumns(5.43);
                    worksheet.Cells[6, 2].AutoFitColumns(9);
                    worksheet.Cells[6, 3].AutoFitColumns(9);
                    worksheet.Cells[6, 4].AutoFitColumns(9);
                    worksheet.Cells[6, 5].AutoFitColumns(12);
                    worksheet.Cells[6, 6].AutoFitColumns(10.14);
                    worksheet.Cells[6, 7].AutoFitColumns(10.5);
                    worksheet.Cells[6, 8].AutoFitColumns(9.57);
                    worksheet.Cells[6, 9].AutoFitColumns(9);
                    worksheet.Cells[6, 10].AutoFitColumns(6.70);//8.14
                    worksheet.Cells[6, 11].AutoFitColumns(9.50);//8.14
                    worksheet.Cells[6, 12].AutoFitColumns(13.5);
                    worksheet.Cells[6, 13].AutoFitColumns(8.43);
                    if (colorType == "Fancy")
                    {
                        worksheet.Cells[6, 14].AutoFitColumns(26);
                    }
                    else
                    {
                        worksheet.Cells[6, 14].AutoFitColumns(9.29);
                    }
                    worksheet.Cells[6, 15].AutoFitColumns(8.43);
                    worksheet.Cells[6, 16].AutoFitColumns(8.43);
                    worksheet.Cells[6, 17].AutoFitColumns(13);
                    worksheet.Cells[6, 18].AutoFitColumns(13);
                    worksheet.Cells[6, 19].AutoFitColumns(10.50);
                    worksheet.Cells[6, 20].AutoFitColumns(13);
                    worksheet.Cells[6, 21].AutoFitColumns(9);
                    worksheet.Cells[6, 22].AutoFitColumns(7.86);
                    worksheet.Cells[6, 23].AutoFitColumns(7.86);
                    worksheet.Cells[6, 24].AutoFitColumns(7.86);
                    worksheet.Cells[6, 25].AutoFitColumns(7.86);
                    worksheet.Cells[6, 26].AutoFitColumns(7.86);
                    worksheet.Cells[6, 27].AutoFitColumns(7.86);
                    worksheet.Cells[6, 28].AutoFitColumns(7.86);
                    worksheet.Cells[6, 29].AutoFitColumns(7.86);
                    worksheet.Cells[6, 30].AutoFitColumns(9);
                    worksheet.Cells[6, 31].AutoFitColumns(7.9);
                    worksheet.Cells[6, 32].AutoFitColumns(35.29);
                    worksheet.Cells[6, 33].AutoFitColumns(38);
                    worksheet.Cells[6, 34].AutoFitColumns(7.86);
                    worksheet.Cells[6, 35].AutoFitColumns(7.86);
                    worksheet.Cells[6, 36].AutoFitColumns(7.86);
                    worksheet.Cells[6, 37].AutoFitColumns(7.86);
                    worksheet.Cells[6, 38].AutoFitColumns(7.86);
                    worksheet.Cells[6, 39].AutoFitColumns(7.86);
                    worksheet.Cells[6, 40].AutoFitColumns(7.86);
                    worksheet.Cells[6, 41].AutoFitColumns(7.86);
                    worksheet.Cells[6, 42].AutoFitColumns(7.86);
                    worksheet.Cells[6, 43].AutoFitColumns(7.86);
                    worksheet.Cells[6, 44].AutoFitColumns(7.86);
                    worksheet.Cells[6, 45].AutoFitColumns(7.86);
                    worksheet.Cells[6, 46].AutoFitColumns(7.86);
                    worksheet.Cells[6, 47].AutoFitColumns(7.86); //41
                    worksheet.Cells[6, 48].AutoFitColumns(7.86); //42
                    worksheet.Cells[6, 49].AutoFitColumns(7.86); //43

                    //Set Cell Faoat value with Alignment
                    worksheet.Cells[inStartIndex, 1, inEndCounter, 49].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    int pairNo, tempPairNo = 0;
                    bool PairLastColumn = false;
                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell

                        if (_strFilePath.Contains("PairSearch"))
                        {
                            pairNo = ((dt.Rows[i - inStartIndex]["pair_no"] != null) ?
                                (dt.Rows[i - inStartIndex]["pair_no"].GetType().Name != "DBNull" ?
                                Convert.ToInt32(dt.Rows[i - inStartIndex]["pair_no"]) : 0) : 0);

                            if (tempPairNo > 0)
                            {
                                if (pairNo != tempPairNo)
                                {
                                    tempPairNo = pairNo;
                                    PairLastColumn = true;
                                }
                                else
                                {
                                    PairLastColumn = false;
                                }
                            }
                            else
                            {
                                tempPairNo = pairNo;
                                PairLastColumn = false;
                            }
                        }
                        else
                        {
                            PairLastColumn = false;
                        }

                        if (PairLastColumn)
                        {
                            worksheet.Cells[(inwrkrow - 1), 1, (inwrkrow - 1), 49].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }

                        string Table_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Table_Open"]);
                        string Crown_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Open"]);
                        string Pav_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Pav_Open"]);
                        string Girdle_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Girdle_Open"]);

                        if ((Table_Open != "" && Table_Open != "NN") || (Crown_Open != "" && Crown_Open != "NN") ||
                            (Pav_Open != "" && Pav_Open != "NN") || (Girdle_Open != "" && Girdle_Open != "NN"))
                        {
                            var tcpg = worksheet.Cells[inwrkrow, 12].Style.Fill;
                            tcpg.PatternType = ExcelFillStyle.Solid;
                            tcpg.BackgroundColor.SetColor(tcpg_bg_clr);
                        }

                        worksheet.Cells[inwrkrow, 1].Value = Convert.ToInt32(dt.Rows[i - inStartIndex]["sr"]);

                        worksheet.Cells[inwrkrow, 2].Value = Convert.ToString(dt.Rows[i - inStartIndex]["view_dna"] == null ? "" : dt.Rows[i - inStartIndex]["view_dna"]);

                        worksheet.Cells[inwrkrow, 4].Value = Convert.ToString(dt.Rows[i - inStartIndex]["movie_url"] == null || dt.Rows[i - inStartIndex]["movie_url"].ToString() == "" ? "" : VideoURL + dt.Rows[i - inStartIndex]["stone_ref_no"]);

                        S_Detail = Convert.ToString(dt.Rows[i - inStartIndex]["view_dna"]);
                        if (S_Detail != "")
                        {
                            worksheet.Cells[inwrkrow, 2].Formula = "=HYPERLINK(\"" + S_Detail + "\",\" DNA \")";
                            worksheet.Cells[inwrkrow, 2].Style.Font.UnderLine = true;
                            worksheet.Cells[inwrkrow, 2].Style.Font.Color.SetColor(Color.Blue);
                        }

                        var img = false;
                        if (dt.Rows[i - inStartIndex]["image_url"].ToString() != "")
                            img = true;
                        if (dt.Rows[i - inStartIndex]["image_url1"].ToString() != "")
                            img = true;
                        if (dt.Rows[i - inStartIndex]["image_url2"].ToString() != "")
                            img = true;
                        if (dt.Rows[i - inStartIndex]["image_url3"].ToString() != "")
                            img = true;

                        if (img == true)
                        {
                            worksheet.Cells[inwrkrow, 3].Formula = "=HYPERLINK(\"" + Convert.ToString(ImageURL + dt.Rows[i - inStartIndex]["stone_ref_no"]) + "\",\" Image \")";
                            worksheet.Cells[inwrkrow, 3].Style.Font.UnderLine = true;
                            worksheet.Cells[inwrkrow, 3].Style.Font.Color.SetColor(Color.Blue);
                        }


                        if (dt.Rows[i - inStartIndex]["movie_url"] != null)
                        {
                            var Video = Convert.ToString(dt.Rows[i - inStartIndex]["movie_url"]);
                            if (Video != "")
                            {
                                worksheet.Cells[inwrkrow, 4].Formula = "=HYPERLINK(\"" + Convert.ToString(VideoURL + dt.Rows[i - inStartIndex]["stone_ref_no"]) + "\",\" Video \")";
                                worksheet.Cells[inwrkrow, 4].Style.Font.UnderLine = true;
                                worksheet.Cells[inwrkrow, 4].Style.Font.Color.SetColor(Color.Blue);
                            }
                        }

                        values_1 = dt.Rows[i - inStartIndex]["stone_ref_no"].ToString();
                        success1 = Int64.TryParse(values_1, out number_1);
                        if (success1)
                        {
                            worksheet.Cells[inwrkrow, 5].Value = Convert.ToInt64(dt.Rows[i - inStartIndex]["stone_ref_no"]);
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 5].Value = values_1;
                        }
                        worksheet.Cells[inwrkrow, 6].Value = asTitleCase.ToTitleCase(Convert.ToString(dt.Rows[i - inStartIndex]["Location"]).ToLower());

                        status = Convert.ToString(dt.Rows[i - inStartIndex]["status"]).ToLower();
                        ForCust_Hold = dt.Rows[i - inStartIndex]["ForCust_Hold"].ToString();

                        if (status == "available offer")
                            status = "Offer";
                        else if (status == "buss. process" && ForCust_Hold == "1")
                            status = "Busy";
                        else if (status == "buss. process")
                            status = "Busy";

                        worksheet.Cells[inwrkrow, 7].Value = asTitleCase.ToTitleCase(status);

                        worksheet.Cells[inwrkrow, 8].Value = Convert.ToString(dt.Rows[i - inStartIndex]["shape"]);
                        worksheet.Cells[inwrkrow, 9].Value = Convert.ToString(dt.Rows[i - inStartIndex]["pointer"]);

                        string certi_type = (Convert.ToString(dt.Rows[i - inStartIndex]["certi_type"]) != "" ? " " + Convert.ToString(dt.Rows[i - inStartIndex]["certi_type"]) : "");

                        worksheet.Cells[inwrkrow, 10].Value = Convert.ToString(dt.Rows[i - inStartIndex]["lab"]);
                        worksheet.Cells[inwrkrow, 10].Style.Font.Color.SetColor(Color.Blue);

                        if (dt.Rows[i - inStartIndex]["view_certi_url"] != null)
                        {
                            var Certificate = Convert.ToString(dt.Rows[i - inStartIndex]["view_certi_url"]);
                            if (Certificate != "")
                            {
                                worksheet.Cells[inwrkrow, 10].Formula = "=HYPERLINK(\"" + Convert.ToString(CertiURL + dt.Rows[i - inStartIndex]["stone_ref_no"]) + "\",\" " + dt.Rows[i - inStartIndex]["lab"] + " \")";
                                worksheet.Cells[inwrkrow, 10].Style.Font.UnderLine = true;
                                worksheet.Cells[inwrkrow, 10].Style.Font.Color.SetColor(Color.Blue);
                            }
                        }

                        worksheet.Cells[inwrkrow, 11].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Certi_Type"]);

                        if (Convert.ToString(dt.Rows[i - inStartIndex]["CertiTypeLink"]) != "" && Convert.ToString(dt.Rows[i - inStartIndex]["Certi_Type"]) != "")
                        {
                            var CertiTypeLink = "http://115.124.127.225:8124/" + dt.Rows[i - inStartIndex]["stone_ref_no"];
                            var Certi_Type = Convert.ToString(dt.Rows[i - inStartIndex]["Certi_Type"]);

                            worksheet.Cells[inwrkrow, 11].Formula = "=HYPERLINK(\"" + CertiTypeLink + "\",\" " + Certi_Type + " \")";
                            worksheet.Cells[inwrkrow, 11].Style.Font.UnderLine = true;
                            worksheet.Cells[inwrkrow, 11].Style.Font.Color.SetColor(Color.Blue);
                        }

                        values_1 = dt.Rows[i - inStartIndex]["certi_no"].ToString();
                        success1 = Int64.TryParse(values_1, out number_1);
                        if (success1)
                        {
                            worksheet.Cells[inwrkrow, 12].Value = Convert.ToInt64(dt.Rows[i - inStartIndex]["certi_no"]).ToString();
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 12].Value = values_1;
                        }

                        worksheet.Cells[inwrkrow, 13].Value = Convert.ToString(dt.Rows[i - inStartIndex]["BGM"]);

                        worksheet.Cells[inwrkrow, 14].Value = Convert.ToString(dt.Rows[i - inStartIndex]["color"]);
                        worksheet.Cells[inwrkrow, 15].Value = Convert.ToString(dt.Rows[i - inStartIndex]["clarity"]);
                        worksheet.Cells[inwrkrow, 16].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["cts"]);

                        string cur_rap_rate = dt.Rows[i - inStartIndex]["cur_rap_rate"].ToString();

                        //16 Rap Price($)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 17].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["cur_rap_rate"]);
                        }

                        //17 Rap Amt($)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 18].Formula = "=Q" + inwrkrow + "*P" + inwrkrow;
                        }

                        //18 Offer Disc.(%)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 19].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["sales_disc_per"]);
                        }

                        //19 Offer Value($)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 20].Formula = "=R" + inwrkrow + "+(" + "R" + inwrkrow + "*S" + inwrkrow + "/100" + ")";
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 20].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["net_amount"]);
                        }

                        //20 Price/Cts
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 21].Formula = "=T" + inwrkrow + "/P" + inwrkrow;
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 21].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["price_per_cts"]);
                        }

                        cut = Convert.ToString(dt.Rows[i - inStartIndex]["cut"]);
                        worksheet.Cells[inwrkrow, 22].Value = (cut == "FR" ? "F" : cut);
                        worksheet.Cells[inwrkrow, 23].Value = Convert.ToString(dt.Rows[i - inStartIndex]["polish"]);
                        worksheet.Cells[inwrkrow, 24].Value = Convert.ToString(dt.Rows[i - inStartIndex]["symm"]);

                        if (Convert.ToString(dt.Rows[i - inStartIndex]["cut"]) == "3EX")
                        {
                            worksheet.Cells[inwrkrow, 22].Style.Font.Bold = true;
                            worksheet.Cells[inwrkrow, 23].Style.Font.Bold = true;
                            worksheet.Cells[inwrkrow, 24].Style.Font.Bold = true;
                        }

                        worksheet.Cells[inwrkrow, 25].Value = Convert.ToString(dt.Rows[i - inStartIndex]["fls"]);
                        worksheet.Cells[inwrkrow, 26].Value = ((dt.Rows[i - inStartIndex]["RATIO"] != null) ?
                               (dt.Rows[i - inStartIndex]["RATIO"].GetType().Name != "DBNull" ?
                               Convert.ToDouble(dt.Rows[i - inStartIndex]["RATIO"]) : ((Double?)null)) : null);

                        worksheet.Cells[inwrkrow, 27].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["length"]);
                        worksheet.Cells[inwrkrow, 28].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["width"]);
                        worksheet.Cells[inwrkrow, 29].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["depth"]);

                        worksheet.Cells[inwrkrow, 30].Value = ((dt.Rows[i - inStartIndex]["depth_per"] != null) ?
                               (dt.Rows[i - inStartIndex]["depth_per"].GetType().Name != "DBNull" ?
                               Convert.ToDouble(dt.Rows[i - inStartIndex]["depth_per"]) : ((Double?)null)) : null);

                        worksheet.Cells[inwrkrow, 31].Value = ((dt.Rows[i - inStartIndex]["table_per"] != null) ?
                               (dt.Rows[i - inStartIndex]["table_per"].GetType().Name != "DBNull" ?
                               Convert.ToDouble(dt.Rows[i - inStartIndex]["table_per"]) : ((Double?)null)) : null);

                        worksheet.Cells[inwrkrow, 32].Value = Convert.ToString(dt.Rows[i - inStartIndex]["symbol"] == null ? "" : dt.Rows[i - inStartIndex]["symbol"].ToString());
                        worksheet.Cells[inwrkrow, 33].Value = Convert.ToString(dt.Rows[i - inStartIndex]["sComments"]);
                        worksheet.Cells[inwrkrow, 34].Value = asTitleCase.ToTitleCase(Convert.ToString(dt.Rows[i - inStartIndex]["sculet"]).ToLower());
                        worksheet.Cells[inwrkrow, 35].Value = Convert.ToString(dt.Rows[i - inStartIndex]["table_natts"] == null ? "" : dt.Rows[i - inStartIndex]["table_natts"]);
                        worksheet.Cells[inwrkrow, 36].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Natts"] == null ? "" : dt.Rows[i - inStartIndex]["Crown_Natts"]);
                        worksheet.Cells[inwrkrow, 37].Value = Convert.ToString(dt.Rows[i - inStartIndex]["inclusion"]);
                        worksheet.Cells[inwrkrow, 38].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Inclusion"] == null ? "" : dt.Rows[i - inStartIndex]["Crown_Inclusion"]);

                        worksheet.Cells[inwrkrow, 39].Value = dt.Rows[i - inStartIndex]["crown_angle"] == null ? 0 : dt.Rows[i - inStartIndex]["crown_angle"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["crown_angle"];
                        worksheet.Cells[inwrkrow, 40].Value = dt.Rows[i - inStartIndex]["crown_height"] == null ? 0 : dt.Rows[i - inStartIndex]["crown_height"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["crown_height"];
                        worksheet.Cells[inwrkrow, 41].Value = dt.Rows[i - inStartIndex]["pav_angle"] == null ? 0 : dt.Rows[i - inStartIndex]["pav_angle"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["pav_angle"];
                        worksheet.Cells[inwrkrow, 42].Value = dt.Rows[i - inStartIndex]["pav_height"] == null ? 0 : dt.Rows[i - inStartIndex]["pav_height"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["pav_height"];

                        worksheet.Cells[inwrkrow, 43].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Table_Open"]);
                        worksheet.Cells[inwrkrow, 44].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Open"]);
                        worksheet.Cells[inwrkrow, 45].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Pav_Open"]);
                        worksheet.Cells[inwrkrow, 46].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Girdle_Open"]);
                        worksheet.Cells[inwrkrow, 47].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["girdle_per"]);

                        worksheet.Cells[inwrkrow, 48].Value = asTitleCase.ToTitleCase(Convert.ToString(dt.Rows[i - inStartIndex]["girdle_type"] == null ? "" : dt.Rows[i - inStartIndex]["girdle_type"]).ToLower());

                        worksheet.Cells[inwrkrow, 49].Value = Convert.ToString(dt.Rows[i - inStartIndex]["sInscription"] == null ? "" : dt.Rows[i - inStartIndex]["sInscription"]);

                        inwrkrow++;

                        #endregion
                    }

                    worksheet.Cells[inStartIndex, 1, (inwrkrow - 1), 49].Style.Font.Size = 9;
                    worksheet.Cells[inStartIndex, 16, (inwrkrow - 1), 21].Style.Numberformat.Format = "#,##0.00";

                    worksheet.Cells[inStartIndex, 9, (inwrkrow - 1), 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[inStartIndex, 9, (inwrkrow - 1), 9].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);

                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Font.Bold = true;

                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Fill.BackgroundColor.SetColor(colFromHex_Dis);
                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Font.Color.SetColor(System.Drawing.Color.Red);

                    worksheet.Cells[inStartIndex, 27, (inwrkrow - 1), 31].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[inStartIndex, 39, (inwrkrow - 1), 42].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[inStartIndex, 47, (inwrkrow - 1), 47].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[5, 5].Formula = "ROUND(SUBTOTAL(102,A" + inStartIndex + ":A" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 5].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 5].Style.Numberformat.Format = "#,##";

                    ExcelStyle cellStyleHeader_Total = worksheet.Cells[5, 5].Style;
                    cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                            = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    worksheet.Cells[5, 16].Formula = "ROUND(SUBTOTAL(109,P" + inStartIndex + ":P" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 16].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 16].Style.Numberformat.Format = "#,##0.00";

                    ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[5, 16].Style;
                    cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                            = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    worksheet.Cells[5, 18].Formula = "ROUND(SUBTOTAL(109,R" + inStartIndex + ":R" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 18].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 18].Style.Numberformat.Format = "#,##0";

                    ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[5, 18].Style;
                    cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                            = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    //=IF(SUBTOTAL(109,Q7: Q1020)=0,0,ROUND((1-(SUBTOTAL(109,S7:S12347)/SUBTOTAL(109,Q7:Q12347)))*(-100),2))
                    worksheet.Cells[5, 19].Formula = "IF(SUBTOTAL(109,R" + inStartIndex + ": R" + (inwrkrow - 1) + ")=0,0,ROUND((1-(SUBTOTAL(109,T" + inStartIndex + ":T" + (inwrkrow - 1) + ")/SUBTOTAL(109,R" + inStartIndex + ":R" + (inwrkrow - 1) + ")))*(-100),2))";
                    worksheet.Cells[5, 19].Style.Numberformat.Format = "#,##0.00";

                    ExcelStyle cellStyleHeader_TotalDis = worksheet.Cells[5, 19].Style;
                    cellStyleHeader_TotalDis.Border.Left.Style = cellStyleHeader_TotalDis.Border.Right.Style
                            = cellStyleHeader_TotalDis.Border.Top.Style = cellStyleHeader_TotalDis.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    worksheet.Cells[5, 20].Formula = "ROUND(SUBTOTAL(109,T" + inStartIndex + ":T" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 20].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 20].Style.Numberformat.Format = "#,##0";

                    ExcelStyle cellStyleHeader_TotalNet = worksheet.Cells[5, 20].Style;
                    cellStyleHeader_TotalNet.Border.Left.Style = cellStyleHeader_TotalNet.Border.Right.Style
                            = cellStyleHeader_TotalNet.Border.Top.Style = cellStyleHeader_TotalNet.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    System.IO.File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static void CreateCategoryExcel(DataTable dt, string _strFolderPath, string _strFilePath, string colorType = "")
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int inStartIndex = 7;
                    int inwrkrow = 7;
                    int inEndCounter = dt.Rows.Count + inStartIndex;
                    int TotalRow = dt.Rows.Count;
                    int i;
                    string values_1, S_Detail, cut, status, ForCust_Hold;
                    Int64 number_1 = 0;
                    bool success1 = false;

                    Color colFromHex_Pointer = System.Drawing.ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = System.Drawing.ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = System.Drawing.ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = System.Drawing.ColorTranslator.FromHtml("#fff2cc");

                    #region Add header details
                    ep.Workbook.Properties.Author = "SUNRISE DIAMOND";
                    ep.Workbook.Properties.Title = "SUNRISE DIAMOND PVT. LTD.";
                    ep.Workbook.Worksheets.Add("SearchStock");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    var pic = worksheet.Drawings.AddPicture("Landscape", new FileInfo("D:\\Projects\\astute\\astute\\Files\\CategoryValueIcon\\MILKY_30062023060725.jpg"));
                    pic.SetPosition(6, 0, 50, 0);
                    pic.Effect.SetPresetShadow(ePresetExcelShadowType.OuterBottomRight);
                    pic.Effect.OuterShadow.Distance = 10;
                    pic.Effect.SetPresetSoftEdges(ePresetExcelSoftEdgesType.SoftEdge5Pt);

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";
                    worksheet.Cells[1, 3, 3, 12].Style.Font.Bold = true;

                    worksheet.Cells[1, 6].Value = "SUNRISE DIAMONDS INVENTORY FOR THE DATE " + " " + DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells[1, 6].Style.Font.Size = 24;
                    worksheet.Cells[1, 6].Style.Font.Bold = true;

                    Color colFromHex_H1 = System.Drawing.ColorTranslator.FromHtml("#8497b0");
                    worksheet.Cells[1, 6].Style.Font.Color.SetColor(colFromHex_H1);
                    Color col_color_Red = System.Drawing.ColorTranslator.FromHtml("#ff0000");

                    worksheet.Row(5).Height = 40;
                    worksheet.Row(6).Height = 40;
                    worksheet.Row(6).Style.WrapText = true;

                    worksheet.Cells[2, 2].Value = "All Prices are final Selling Cash Price";
                    worksheet.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 2].Style.Font.Size = 11;
                    worksheet.Cells[2, 2].Style.Font.Bold = true;
                    worksheet.Cells[2, 2].Style.Font.Color.SetColor(col_color_Red);

                    worksheet.Cells[2, 6].Value = "UNIT 1, 14/F, PENINSULA SQUARE, EAST WING, 18 SUNG ON STREET, HUNG HOM, KOWLOON, HONG KONG TEL : +852 - 27235100    FAX : +852 - 2314 9100";
                    worksheet.Cells[2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 6].Style.Font.Size = 11;
                    worksheet.Cells[2, 6].Style.Font.Bold = true;
                    worksheet.Cells[2, 6].Style.Font.Color.SetColor(colFromHex_H1);

                    worksheet.Cells[3, 6].Value = "Email Id : sales@sunrisediam.com    Web : www.sunrisediamonds.com.hk . Download Apps on Android, IOS and Windows";
                    worksheet.Cells[3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[3, 6].Style.Font.Size = 11;
                    worksheet.Cells[3, 6].Style.Font.Bold = true;
                    worksheet.Cells[3, 6].Style.Font.Color.SetColor(colFromHex_H1);

                    worksheet.Cells[5, 1].Value = "Total";
                    worksheet.Cells[5, 1, 5, 49].Style.Font.Bold = true;
                    worksheet.Cells[5, 1, 5, 49].Style.Font.Size = 11;
                    worksheet.Cells[5, 1, 5, 49].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[5, 1, 5, 49].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[5, 1, 5, 49].Style.Font.Size = 11;

                    worksheet.Cells[6, 1, 6, 49].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[6, 1, 6, 49].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[6, 1, 6, 49].Style.Font.Size = 10;
                    worksheet.Cells[6, 1, 6, 49].Style.Font.Bold = true;

                    worksheet.Cells[6, 1, 6, 49].AutoFilter = true;
                    worksheet.Cells[1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    var cellBackgroundColor1 = worksheet.Cells[6, 1, 6, 49].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);
                    #endregion

                    #region Header Column Name Declaration
                    worksheet.Cells[6, 1].Value = "Sr. No";
                    worksheet.Cells[6, 2].Value = "DNA";
                    worksheet.Cells[6, 3].Value = "View Image";
                    worksheet.Cells[6, 4].Value = "HD Movie";
                    worksheet.Cells[6, 5].Value = "Stock Id";
                    worksheet.Cells[6, 6].Value = "Location";
                    worksheet.Cells[6, 7].Value = "Status";

                    worksheet.Cells[6, 8].Value = "Shape";
                    worksheet.Cells[6, 9].Value = "Pointer";
                    worksheet.Cells[6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[6, 9].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                    worksheet.Cells[6, 10].Value = "Lab";
                    worksheet.Cells[6, 11].Value = "Certi Type";
                    worksheet.Cells[6, 12].Value = "Certi No.";
                    worksheet.Cells[6, 13].Value = "BGM";
                    worksheet.Cells[6, 14].Value = "Color";
                    worksheet.Cells[6, 15].Value = "Clarity";
                    worksheet.Cells[6, 16].Value = "Cts";

                    worksheet.Cells[6, 17].Value = "Rap Price($)";
                    worksheet.Cells[6, 18].Value = "Rap Amt($)";
                    worksheet.Cells[6, 19].Value = "Offer Disc.(%)"; // "Disc(%)";
                    worksheet.Cells[6, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[6, 19].Style.Fill.BackgroundColor.SetColor(colFromHex_Dis);

                    worksheet.Cells[6, 20].Value = "Offer Value($)"; // "Net Amt($)";
                    worksheet.Cells[6, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[6, 20].Style.Fill.BackgroundColor.SetColor(colFromHex_Dis);
                    worksheet.Cells[6, 21].Value = "Price/Cts";

                    worksheet.Cells[6, 22].Value = "Cut";
                    worksheet.Cells[6, 23].Value = "Polish";
                    worksheet.Cells[6, 24].Value = "Symm";
                    worksheet.Cells[6, 25].Value = "Fls";
                    worksheet.Cells[6, 26].Value = "Ratio";
                    worksheet.Cells[6, 27].Value = "Length";
                    worksheet.Cells[6, 28].Value = "Width";
                    worksheet.Cells[6, 29].Value = "Depth";
                    worksheet.Cells[6, 30].Value = "Depth(%)";
                    worksheet.Cells[6, 31].Value = "Table(%)";
                    worksheet.Cells[6, 32].Value = "Key To Symbol";
                    worksheet.Cells[6, 33].Value = "GIA Comment";
                    worksheet.Cells[6, 34].Value = "Culet";
                    worksheet.Cells[6, 35].Value = "Table Black";
                    worksheet.Cells[6, 36].Value = "Crown Black";
                    worksheet.Cells[6, 37].Value = "Table White";
                    worksheet.Cells[6, 38].Value = "Crown White";
                    worksheet.Cells[6, 39].Value = "Cr Ang";
                    worksheet.Cells[6, 40].Value = "Cr Ht";
                    worksheet.Cells[6, 41].Value = "Pav Ang";
                    worksheet.Cells[6, 42].Value = "Pav Ht";
                    worksheet.Cells[6, 43].Value = "Table Open";
                    worksheet.Cells[6, 44].Value = "Crown Open";
                    worksheet.Cells[6, 45].Value = "Pav Open";
                    worksheet.Cells[6, 46].Value = "Girdle Open";
                    worksheet.Cells[6, 47].Value = "Girdle(%)";   //41
                    worksheet.Cells[6, 48].Value = "Girdle Type"; //42
                    worksheet.Cells[6, 49].Value = "Laser Insc";  //43

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[6, 1, 6, 49].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format

                    worksheet.View.FreezePanes(7, 1);

                    worksheet.Cells[6, 1].AutoFitColumns(5.43);
                    worksheet.Cells[6, 2].AutoFitColumns(9);
                    worksheet.Cells[6, 3].AutoFitColumns(9);
                    worksheet.Cells[6, 4].AutoFitColumns(9);
                    worksheet.Cells[6, 5].AutoFitColumns(12);
                    worksheet.Cells[6, 6].AutoFitColumns(10.14);
                    worksheet.Cells[6, 7].AutoFitColumns(10.5);
                    worksheet.Cells[6, 8].AutoFitColumns(9.57);
                    worksheet.Cells[6, 9].AutoFitColumns(9);
                    worksheet.Cells[6, 10].AutoFitColumns(6.70);//8.14
                    worksheet.Cells[6, 11].AutoFitColumns(9.50);//8.14
                    worksheet.Cells[6, 12].AutoFitColumns(13.5);
                    worksheet.Cells[6, 13].AutoFitColumns(8.43);
                    if (colorType == "Fancy")
                    {
                        worksheet.Cells[6, 14].AutoFitColumns(26);
                    }
                    else
                    {
                        worksheet.Cells[6, 14].AutoFitColumns(9.29);
                    }
                    worksheet.Cells[6, 15].AutoFitColumns(8.43);
                    worksheet.Cells[6, 16].AutoFitColumns(8.43);
                    worksheet.Cells[6, 17].AutoFitColumns(13);
                    worksheet.Cells[6, 18].AutoFitColumns(13);
                    worksheet.Cells[6, 19].AutoFitColumns(10.50);
                    worksheet.Cells[6, 20].AutoFitColumns(13);
                    worksheet.Cells[6, 21].AutoFitColumns(9);
                    worksheet.Cells[6, 22].AutoFitColumns(7.86);
                    worksheet.Cells[6, 23].AutoFitColumns(7.86);
                    worksheet.Cells[6, 24].AutoFitColumns(7.86);
                    worksheet.Cells[6, 25].AutoFitColumns(7.86);
                    worksheet.Cells[6, 26].AutoFitColumns(7.86);
                    worksheet.Cells[6, 27].AutoFitColumns(7.86);
                    worksheet.Cells[6, 28].AutoFitColumns(7.86);
                    worksheet.Cells[6, 29].AutoFitColumns(7.86);
                    worksheet.Cells[6, 30].AutoFitColumns(9);
                    worksheet.Cells[6, 31].AutoFitColumns(7.9);
                    worksheet.Cells[6, 32].AutoFitColumns(35.29);
                    worksheet.Cells[6, 33].AutoFitColumns(38);
                    worksheet.Cells[6, 34].AutoFitColumns(7.86);
                    worksheet.Cells[6, 35].AutoFitColumns(7.86);
                    worksheet.Cells[6, 36].AutoFitColumns(7.86);
                    worksheet.Cells[6, 37].AutoFitColumns(7.86);
                    worksheet.Cells[6, 38].AutoFitColumns(7.86);
                    worksheet.Cells[6, 39].AutoFitColumns(7.86);
                    worksheet.Cells[6, 40].AutoFitColumns(7.86);
                    worksheet.Cells[6, 41].AutoFitColumns(7.86);
                    worksheet.Cells[6, 42].AutoFitColumns(7.86);
                    worksheet.Cells[6, 43].AutoFitColumns(7.86);
                    worksheet.Cells[6, 44].AutoFitColumns(7.86);
                    worksheet.Cells[6, 45].AutoFitColumns(7.86);
                    worksheet.Cells[6, 46].AutoFitColumns(7.86);
                    worksheet.Cells[6, 47].AutoFitColumns(7.86); //41
                    worksheet.Cells[6, 48].AutoFitColumns(7.86); //42
                    worksheet.Cells[6, 49].AutoFitColumns(7.86); //43

                    //Set Cell Faoat value with Alignment
                    worksheet.Cells[inStartIndex, 1, inEndCounter, 49].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    int pairNo, tempPairNo = 0;
                    bool PairLastColumn = false;
                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell

                        if (_strFilePath.Contains("PairSearch"))
                        {
                            pairNo = ((dt.Rows[i - inStartIndex]["pair_no"] != null) ?
                                (dt.Rows[i - inStartIndex]["pair_no"].GetType().Name != "DBNull" ?
                                Convert.ToInt32(dt.Rows[i - inStartIndex]["pair_no"]) : 0) : 0);

                            if (tempPairNo > 0)
                            {
                                if (pairNo != tempPairNo)
                                {
                                    tempPairNo = pairNo;
                                    PairLastColumn = true;
                                }
                                else
                                {
                                    PairLastColumn = false;
                                }
                            }
                            else
                            {
                                tempPairNo = pairNo;
                                PairLastColumn = false;
                            }
                        }
                        else
                        {
                            PairLastColumn = false;
                        }

                        if (PairLastColumn)
                        {
                            worksheet.Cells[(inwrkrow - 1), 1, (inwrkrow - 1), 49].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }

                        string Table_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Table_Open"]);
                        string Crown_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Open"]);
                        string Pav_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Pav_Open"]);
                        string Girdle_Open = Convert.ToString(dt.Rows[i - inStartIndex]["Girdle_Open"]);

                        if ((Table_Open != "" && Table_Open != "NN") || (Crown_Open != "" && Crown_Open != "NN") ||
                            (Pav_Open != "" && Pav_Open != "NN") || (Girdle_Open != "" && Girdle_Open != "NN"))
                        {
                            var tcpg = worksheet.Cells[inwrkrow, 12].Style.Fill;
                            tcpg.PatternType = ExcelFillStyle.Solid;
                            tcpg.BackgroundColor.SetColor(tcpg_bg_clr);
                        }

                        worksheet.Cells[inwrkrow, 1].Value = Convert.ToInt32(dt.Rows[i - inStartIndex]["sr"]);

                        worksheet.Cells[inwrkrow, 2].Value = Convert.ToString(dt.Rows[i - inStartIndex]["view_dna"] == null ? "" : dt.Rows[i - inStartIndex]["view_dna"]);

                        worksheet.Cells[inwrkrow, 4].Value = Convert.ToString(dt.Rows[i - inStartIndex]["movie_url"] == null || dt.Rows[i - inStartIndex]["movie_url"].ToString() == "" ? "" : VideoURL + dt.Rows[i - inStartIndex]["stone_ref_no"]);

                        S_Detail = Convert.ToString(dt.Rows[i - inStartIndex]["view_dna"]);
                        if (S_Detail != "")
                        {
                            worksheet.Cells[inwrkrow, 2].Formula = "=HYPERLINK(\"" + S_Detail + "\",\" DNA \")";
                            worksheet.Cells[inwrkrow, 2].Style.Font.UnderLine = true;
                            worksheet.Cells[inwrkrow, 2].Style.Font.Color.SetColor(Color.Blue);
                        }

                        var img = false;
                        if (dt.Rows[i - inStartIndex]["image_url"].ToString() != "")
                            img = true;
                        if (dt.Rows[i - inStartIndex]["image_url1"].ToString() != "")
                            img = true;
                        if (dt.Rows[i - inStartIndex]["image_url2"].ToString() != "")
                            img = true;
                        if (dt.Rows[i - inStartIndex]["image_url3"].ToString() != "")
                            img = true;

                        if (img == true)
                        {
                            worksheet.Cells[inwrkrow, 3].Formula = "=HYPERLINK(\"" + Convert.ToString(ImageURL + dt.Rows[i - inStartIndex]["stone_ref_no"]) + "\",\" Image \")";
                            worksheet.Cells[inwrkrow, 3].Style.Font.UnderLine = true;
                            worksheet.Cells[inwrkrow, 3].Style.Font.Color.SetColor(Color.Blue);
                        }


                        if (dt.Rows[i - inStartIndex]["movie_url"] != null)
                        {
                            var Video = Convert.ToString(dt.Rows[i - inStartIndex]["movie_url"]);
                            if (Video != "")
                            {
                                worksheet.Cells[inwrkrow, 4].Formula = "=HYPERLINK(\"" + Convert.ToString(VideoURL + dt.Rows[i - inStartIndex]["stone_ref_no"]) + "\",\" Video \")";
                                worksheet.Cells[inwrkrow, 4].Style.Font.UnderLine = true;
                                worksheet.Cells[inwrkrow, 4].Style.Font.Color.SetColor(Color.Blue);
                            }
                        }

                        values_1 = dt.Rows[i - inStartIndex]["stone_ref_no"].ToString();
                        success1 = Int64.TryParse(values_1, out number_1);
                        if (success1)
                        {
                            worksheet.Cells[inwrkrow, 5].Value = Convert.ToInt64(dt.Rows[i - inStartIndex]["stone_ref_no"]);
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 5].Value = values_1;
                        }
                        worksheet.Cells[inwrkrow, 6].Value = asTitleCase.ToTitleCase(Convert.ToString(dt.Rows[i - inStartIndex]["Location"]).ToLower());

                        status = Convert.ToString(dt.Rows[i - inStartIndex]["status"]).ToLower();
                        ForCust_Hold = dt.Rows[i - inStartIndex]["ForCust_Hold"].ToString();

                        if (status == "available offer")
                            status = "Offer";
                        else if (status == "buss. process" && ForCust_Hold == "1")
                            status = "Busy";
                        else if (status == "buss. process")
                            status = "Busy";

                        worksheet.Cells[inwrkrow, 7].Value = asTitleCase.ToTitleCase(status);

                        worksheet.Cells[inwrkrow, 8].Value = Convert.ToString(dt.Rows[i - inStartIndex]["shape"]);
                        worksheet.Cells[inwrkrow, 9].Value = Convert.ToString(dt.Rows[i - inStartIndex]["pointer"]);

                        string certi_type = (Convert.ToString(dt.Rows[i - inStartIndex]["certi_type"]) != "" ? " " + Convert.ToString(dt.Rows[i - inStartIndex]["certi_type"]) : "");

                        worksheet.Cells[inwrkrow, 10].Value = Convert.ToString(dt.Rows[i - inStartIndex]["lab"]);
                        worksheet.Cells[inwrkrow, 10].Style.Font.Color.SetColor(Color.Blue);

                        if (dt.Rows[i - inStartIndex]["view_certi_url"] != null)
                        {
                            var Certificate = Convert.ToString(dt.Rows[i - inStartIndex]["view_certi_url"]);
                            if (Certificate != "")
                            {
                                worksheet.Cells[inwrkrow, 10].Formula = "=HYPERLINK(\"" + Convert.ToString(CertiURL + dt.Rows[i - inStartIndex]["stone_ref_no"]) + "\",\" " + dt.Rows[i - inStartIndex]["lab"] + " \")";
                                worksheet.Cells[inwrkrow, 10].Style.Font.UnderLine = true;
                                worksheet.Cells[inwrkrow, 10].Style.Font.Color.SetColor(Color.Blue);
                            }
                        }

                        worksheet.Cells[inwrkrow, 11].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Certi_Type"]);

                        if (Convert.ToString(dt.Rows[i - inStartIndex]["CertiTypeLink"]) != "" && Convert.ToString(dt.Rows[i - inStartIndex]["Certi_Type"]) != "")
                        {
                            var CertiTypeLink = "http://115.124.127.225:8124/" + dt.Rows[i - inStartIndex]["stone_ref_no"];
                            var Certi_Type = Convert.ToString(dt.Rows[i - inStartIndex]["Certi_Type"]);

                            worksheet.Cells[inwrkrow, 11].Formula = "=HYPERLINK(\"" + CertiTypeLink + "\",\" " + Certi_Type + " \")";
                            worksheet.Cells[inwrkrow, 11].Style.Font.UnderLine = true;
                            worksheet.Cells[inwrkrow, 11].Style.Font.Color.SetColor(Color.Blue);
                        }

                        values_1 = dt.Rows[i - inStartIndex]["certi_no"].ToString();
                        success1 = Int64.TryParse(values_1, out number_1);
                        if (success1)
                        {
                            worksheet.Cells[inwrkrow, 12].Value = Convert.ToInt64(dt.Rows[i - inStartIndex]["certi_no"]).ToString();
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 12].Value = values_1;
                        }

                        worksheet.Cells[inwrkrow, 13].Value = Convert.ToString(dt.Rows[i - inStartIndex]["BGM"]);

                        worksheet.Cells[inwrkrow, 14].Value = Convert.ToString(dt.Rows[i - inStartIndex]["color"]);
                        worksheet.Cells[inwrkrow, 15].Value = Convert.ToString(dt.Rows[i - inStartIndex]["clarity"]);
                        worksheet.Cells[inwrkrow, 16].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["cts"]);

                        string cur_rap_rate = dt.Rows[i - inStartIndex]["cur_rap_rate"].ToString();

                        //16 Rap Price($)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 17].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["cur_rap_rate"]);
                        }

                        //17 Rap Amt($)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 18].Formula = "=Q" + inwrkrow + "*P" + inwrkrow;
                        }

                        //18 Offer Disc.(%)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 19].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["sales_disc_per"]);
                        }

                        //19 Offer Value($)
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 20].Formula = "=R" + inwrkrow + "+(" + "R" + inwrkrow + "*S" + inwrkrow + "/100" + ")";
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 20].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["net_amount"]);
                        }

                        //20 Price/Cts
                        if (cur_rap_rate != "" && cur_rap_rate != null)
                        {
                            worksheet.Cells[inwrkrow, 21].Formula = "=T" + inwrkrow + "/P" + inwrkrow;
                        }
                        else
                        {
                            worksheet.Cells[inwrkrow, 21].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["price_per_cts"]);
                        }

                        cut = Convert.ToString(dt.Rows[i - inStartIndex]["cut"]);
                        worksheet.Cells[inwrkrow, 22].Value = (cut == "FR" ? "F" : cut);
                        worksheet.Cells[inwrkrow, 23].Value = Convert.ToString(dt.Rows[i - inStartIndex]["polish"]);
                        worksheet.Cells[inwrkrow, 24].Value = Convert.ToString(dt.Rows[i - inStartIndex]["symm"]);

                        if (Convert.ToString(dt.Rows[i - inStartIndex]["cut"]) == "3EX")
                        {
                            worksheet.Cells[inwrkrow, 22].Style.Font.Bold = true;
                            worksheet.Cells[inwrkrow, 23].Style.Font.Bold = true;
                            worksheet.Cells[inwrkrow, 24].Style.Font.Bold = true;
                        }

                        worksheet.Cells[inwrkrow, 25].Value = Convert.ToString(dt.Rows[i - inStartIndex]["fls"]);
                        worksheet.Cells[inwrkrow, 26].Value = ((dt.Rows[i - inStartIndex]["RATIO"] != null) ?
                               (dt.Rows[i - inStartIndex]["RATIO"].GetType().Name != "DBNull" ?
                               Convert.ToDouble(dt.Rows[i - inStartIndex]["RATIO"]) : ((Double?)null)) : null);

                        worksheet.Cells[inwrkrow, 27].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["length"]);
                        worksheet.Cells[inwrkrow, 28].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["width"]);
                        worksheet.Cells[inwrkrow, 29].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["depth"]);

                        worksheet.Cells[inwrkrow, 30].Value = ((dt.Rows[i - inStartIndex]["depth_per"] != null) ?
                               (dt.Rows[i - inStartIndex]["depth_per"].GetType().Name != "DBNull" ?
                               Convert.ToDouble(dt.Rows[i - inStartIndex]["depth_per"]) : ((Double?)null)) : null);

                        worksheet.Cells[inwrkrow, 31].Value = ((dt.Rows[i - inStartIndex]["table_per"] != null) ?
                               (dt.Rows[i - inStartIndex]["table_per"].GetType().Name != "DBNull" ?
                               Convert.ToDouble(dt.Rows[i - inStartIndex]["table_per"]) : ((Double?)null)) : null);

                        worksheet.Cells[inwrkrow, 32].Value = Convert.ToString(dt.Rows[i - inStartIndex]["symbol"] == null ? "" : dt.Rows[i - inStartIndex]["symbol"].ToString());
                        worksheet.Cells[inwrkrow, 33].Value = Convert.ToString(dt.Rows[i - inStartIndex]["sComments"]);
                        worksheet.Cells[inwrkrow, 34].Value = asTitleCase.ToTitleCase(Convert.ToString(dt.Rows[i - inStartIndex]["sculet"]).ToLower());
                        worksheet.Cells[inwrkrow, 35].Value = Convert.ToString(dt.Rows[i - inStartIndex]["table_natts"] == null ? "" : dt.Rows[i - inStartIndex]["table_natts"]);
                        worksheet.Cells[inwrkrow, 36].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Natts"] == null ? "" : dt.Rows[i - inStartIndex]["Crown_Natts"]);
                        worksheet.Cells[inwrkrow, 37].Value = Convert.ToString(dt.Rows[i - inStartIndex]["inclusion"]);
                        worksheet.Cells[inwrkrow, 38].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Inclusion"] == null ? "" : dt.Rows[i - inStartIndex]["Crown_Inclusion"]);

                        worksheet.Cells[inwrkrow, 39].Value = dt.Rows[i - inStartIndex]["crown_angle"] == null ? 0 : dt.Rows[i - inStartIndex]["crown_angle"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["crown_angle"];
                        worksheet.Cells[inwrkrow, 40].Value = dt.Rows[i - inStartIndex]["crown_height"] == null ? 0 : dt.Rows[i - inStartIndex]["crown_height"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["crown_height"];
                        worksheet.Cells[inwrkrow, 41].Value = dt.Rows[i - inStartIndex]["pav_angle"] == null ? 0 : dt.Rows[i - inStartIndex]["pav_angle"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["pav_angle"];
                        worksheet.Cells[inwrkrow, 42].Value = dt.Rows[i - inStartIndex]["pav_height"] == null ? 0 : dt.Rows[i - inStartIndex]["pav_height"].ToString() == "" ? 0 : dt.Rows[i - inStartIndex]["pav_height"];

                        worksheet.Cells[inwrkrow, 43].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Table_Open"]);
                        worksheet.Cells[inwrkrow, 44].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Crown_Open"]);
                        worksheet.Cells[inwrkrow, 45].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Pav_Open"]);
                        worksheet.Cells[inwrkrow, 46].Value = Convert.ToString(dt.Rows[i - inStartIndex]["Girdle_Open"]);
                        worksheet.Cells[inwrkrow, 47].Value = Convert.ToDouble(dt.Rows[i - inStartIndex]["girdle_per"]);

                        worksheet.Cells[inwrkrow, 48].Value = asTitleCase.ToTitleCase(Convert.ToString(dt.Rows[i - inStartIndex]["girdle_type"] == null ? "" : dt.Rows[i - inStartIndex]["girdle_type"]).ToLower());

                        worksheet.Cells[inwrkrow, 49].Value = Convert.ToString(dt.Rows[i - inStartIndex]["sInscription"] == null ? "" : dt.Rows[i - inStartIndex]["sInscription"]);

                        inwrkrow++;

                        #endregion
                    }

                    worksheet.Cells[inStartIndex, 1, (inwrkrow - 1), 49].Style.Font.Size = 9;
                    worksheet.Cells[inStartIndex, 16, (inwrkrow - 1), 21].Style.Numberformat.Format = "#,##0.00";

                    worksheet.Cells[inStartIndex, 9, (inwrkrow - 1), 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[inStartIndex, 9, (inwrkrow - 1), 9].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);

                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Font.Bold = true;

                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Fill.BackgroundColor.SetColor(colFromHex_Dis);
                    worksheet.Cells[inStartIndex, 19, (inwrkrow - 1), 20].Style.Font.Color.SetColor(System.Drawing.Color.Red);

                    worksheet.Cells[inStartIndex, 27, (inwrkrow - 1), 31].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[inStartIndex, 39, (inwrkrow - 1), 42].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[inStartIndex, 47, (inwrkrow - 1), 47].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[5, 5].Formula = "ROUND(SUBTOTAL(102,A" + inStartIndex + ":A" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 5].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 5].Style.Numberformat.Format = "#,##";

                    ExcelStyle cellStyleHeader_Total = worksheet.Cells[5, 5].Style;
                    cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                            = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    worksheet.Cells[5, 16].Formula = "ROUND(SUBTOTAL(109,P" + inStartIndex + ":P" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 16].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 16].Style.Numberformat.Format = "#,##0.00";

                    ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[5, 16].Style;
                    cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                            = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    worksheet.Cells[5, 18].Formula = "ROUND(SUBTOTAL(109,R" + inStartIndex + ":R" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 18].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 18].Style.Numberformat.Format = "#,##0";

                    ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[5, 18].Style;
                    cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                            = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    //=IF(SUBTOTAL(109,Q7: Q1020)=0,0,ROUND((1-(SUBTOTAL(109,S7:S12347)/SUBTOTAL(109,Q7:Q12347)))*(-100),2))
                    worksheet.Cells[5, 19].Formula = "IF(SUBTOTAL(109,R" + inStartIndex + ": R" + (inwrkrow - 1) + ")=0,0,ROUND((1-(SUBTOTAL(109,T" + inStartIndex + ":T" + (inwrkrow - 1) + ")/SUBTOTAL(109,R" + inStartIndex + ":R" + (inwrkrow - 1) + ")))*(-100),2))";
                    worksheet.Cells[5, 19].Style.Numberformat.Format = "#,##0.00";

                    ExcelStyle cellStyleHeader_TotalDis = worksheet.Cells[5, 19].Style;
                    cellStyleHeader_TotalDis.Border.Left.Style = cellStyleHeader_TotalDis.Border.Right.Style
                            = cellStyleHeader_TotalDis.Border.Top.Style = cellStyleHeader_TotalDis.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    worksheet.Cells[5, 20].Formula = "ROUND(SUBTOTAL(109,T" + inStartIndex + ":T" + (inwrkrow - 1) + "),2)";
                    worksheet.Cells[5, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[5, 20].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                    worksheet.Cells[5, 20].Style.Numberformat.Format = "#,##0";

                    ExcelStyle cellStyleHeader_TotalNet = worksheet.Cells[5, 20].Style;
                    cellStyleHeader_TotalNet.Border.Left.Style = cellStyleHeader_TotalNet.Border.Right.Style
                            = cellStyleHeader_TotalNet.Border.Top.Style = cellStyleHeader_TotalNet.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    System.IO.File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static void Create_Customer_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Customer Stock");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "Image")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "Video")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "Pointer")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        #region Assigns Value to Cell
                        int kk = 0;
                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                            
                            if (Column_Name == "Image")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Image"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "Video")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Video"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;
                                worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                if (Column_Name == "Lab")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Certificate_URL"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Amount($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Final Disc(%)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Disc(%)"]);
                                    //worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Disc(%)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(14) + i + "/" + GetExcelColumnLetter(12) + i + ")-100,0)";

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "Final Amt US($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "Price / Cts")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Price / Cts"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Price / Cts"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(ppc_bg);
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Polish"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Symm"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(ratio_bg);
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["Length"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Length"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["Width"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Width"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth(%)")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table(%)")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Table(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Table(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Girdle(%)")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Girdle(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Girdle(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Angle")
                                {
                                    string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Angle"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Angle"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Height")
                                {
                                    string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Height"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Height"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pav Angle")
                                {
                                    string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Angle"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Angle"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pav Height")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Height"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Height"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "Stock Id")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CTS")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Rap Amount($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Final Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(14) + "" + inStartIndex + ":" + GetExcelColumnLetter(14) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(12) + "" + inStartIndex + ":" + GetExcelColumnLetter(12) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Final Amt US($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 2)
                    {
                        worksheet.DeleteColumn(totalColumns - 1, 2);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        
        public static void Create_Customer_Stock_Availability_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count-2;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Customer Stock");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name != "Cost Disc(%)" && Column_Name != "Cost Amt US($)" && Column_Name != "Certificate_URL" && Column_Name != "Certificate_No")
                        {
                            if (Column_Name == "Image")
                            {
                                k += 1;
                                worksheet.Cells[2, k].Value = "Image";
                                worksheet.Cells[2, k].AutoFitColumns(7);
                            }
                            else if (Column_Name == "Video")
                            {
                                k += 1;
                                worksheet.Cells[2, k].Value = "Video";
                                worksheet.Cells[2, k].AutoFitColumns(7);
                            }
                            else
                            {
                                k += 1;
                                worksheet.Cells[2, k].Value = Column_Name;
                                worksheet.Cells[2, k].AutoFitColumns(10);

                                if (Column_Name == "Pointer")
                                {
                                    worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                                }
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        #region Assigns Value to Cell
                        int kk = 0;
                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                            if (Column_Name != "Cost Disc(%)" && Column_Name != "Cost Amt US($)" && Column_Name != "Certificate_URL" && Column_Name != "Certificate_No")
                            {
                                if (Column_Name == "Image")
                                {
                                    kk += 1;

                                    string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Image"]);
                                    if (!string.IsNullOrEmpty(Image_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Video")
                                {
                                    kk += 1;

                                    string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Video"]);
                                    if (!string.IsNullOrEmpty(Video_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else
                                {

                                    kk += 1;
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    if (Column_Name == "Lab")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]);
                                        string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Certificate_URL"]);

                                        if (!string.IsNullOrEmpty(Certificate_URL))
                                        {
                                            worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]) + " \")";
                                            worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                        }
                                    }
                                    if (Column_Name == "CTS")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (Column_Name == "Rap Rate($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (Column_Name == "Rap Amount($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (Column_Name == "Final Disc(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Disc(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Disc(%)"]) : 0;
                                        //worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(14) + i + "/" + GetExcelColumnLetter(12) + i + ")-100,0)";

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                    }
                                    else if (Column_Name == "Final Amt US($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                    }
                                    else if (Column_Name == "Price / Cts")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Price / Cts"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Price / Cts"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(ppc_bg);
                                    }
                                    else if (Column_Name == "Cut")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                        if (cutValue == "3EX")
                                        {
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                        }
                                    }
                                    else if (Column_Name == "Polish")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Polish"]);

                                        if (cutValue == "3EX")
                                        {
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                        }
                                    }
                                    else if (Column_Name == "Symm")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Symm"]);

                                        if (cutValue == "3EX")
                                        {
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                        }
                                    }
                                    else if (Column_Name == "RATIO")
                                    {
                                        string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(ratio_bg);
                                    }
                                    else if (Column_Name == "Length")
                                    {
                                        string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["Length"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Length"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Width")
                                    {
                                        string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["Width"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Width"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Depth")
                                    {
                                        string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Depth(%)")
                                    {
                                        string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Table(%)")
                                    {
                                        string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Table(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Table(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Girdle(%)")
                                    {
                                        string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Girdle(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Girdle(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Crown Angle")
                                    {
                                        string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Angle"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Angle"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Crown Height")
                                    {
                                        string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Height"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Height"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Pav Angle")
                                    {
                                        string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Angle"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Angle"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Pav Height")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Height"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Height"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name != "Cost Disc(%)" && Column_Name != "Cost Amt US($)" && Column_Name != "Certificate_URL" && Column_Name != "Certificate_No")
                        {
                            if (Column_Name == "Image")
                            {
                                kkk += 1;
                            }
                            else if (Column_Name == "Video")
                            {
                                kkk += 1;
                            }
                            else
                            {
                                kkk += 1;
                                if (Column_Name == "Stock Id")
                                {
                                    worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                    worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                    worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                    ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                    cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                            = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                            = ExcelBorderStyle.Medium;
                                }
                                else if (Column_Name == "CTS")
                                {
                                    worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                    worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                    worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                    ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                    cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                            = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                            = ExcelBorderStyle.Medium;
                                }
                                else if (Column_Name == "Rap Amount($)")
                                {
                                    worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                    worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                    worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                    ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                    cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                            = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                            = ExcelBorderStyle.Medium;
                                }
                                else if (Column_Name == "Final Disc(%)")
                                {
                                    worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(101," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                    //worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(14) + "" + inStartIndex + ":" + GetExcelColumnLetter(14) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(12) + "" + inStartIndex + ":" + GetExcelColumnLetter(12) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                    worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Final Amt US($)")
                                {
                                    worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                    worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                    worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                    ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                    cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                            = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                            = ExcelBorderStyle.Medium;
                                }
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 2)
                    {
                        worksheet.DeleteColumn(totalColumns - 1, 2);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Buyer_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");
                    Color supp_cost_clr = ColorTranslator.FromHtml("#FF99CC");
                    Color supp_off_bg = ColorTranslator.FromHtml("#C0C0C0");
                    Color supp_final_bg = ColorTranslator.FromHtml("#FFFF00");
                    Color max_slab_bg = ColorTranslator.FromHtml("#FF99FF");
                    Color rank_bg = ColorTranslator.FromHtml("#F2DC13");
                    Color buyer_bg = ColorTranslator.FromHtml("#93C5F7");
                    Color status_bg = ColorTranslator.FromHtml("#C4BD97");
                    Color bid_bg = ColorTranslator.FromHtml("#CCECFF");
                    Color avg_stock_bg = ColorTranslator.FromHtml("#FCD5B4");
                    Color avg_pur_bg = ColorTranslator.FromHtml("#66FFCC");
                    Color avg_sales_bg = ColorTranslator.FromHtml("#E4DFEC");
                    Color kts_bg = ColorTranslator.FromHtml("#DAEEF3");
                    Color comm_grade_bg = ColorTranslator.FromHtml("#99CC00");
                    Color para_grade_bg = ColorTranslator.FromHtml("#00FFFF");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Buyer Stock");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        k += 1;
                        worksheet.Cells[2, k].Value = Column_Name;

                        if (Column_Name == "DNA" || Column_Name == "Image" || Column_Name == "Video" || Column_Name == "Status")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(7.0);
                        }
                        else if (Column_Name == "Lab" || Column_Name == "Supplier Status" || Column_Name == "Polish" || Column_Name == "Symm" || Column_Name == "Fls")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(5.0);
                        }
                        else if (Column_Name == "Supplier Name" || Column_Name == "CTS" || Column_Name == "Shade")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(10.0);
                        }
                        else if (Column_Name == "Rank" || Column_Name == "Cut")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(4.7);
                        }
                        else if (Column_Name == "Buyer Name")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(6.5);
                        }
                        else if (Column_Name == "Supplier No" || Column_Name == "Clarity")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(8.0);
                        }
                        else if (Column_Name == "Shape" || Column_Name == "Sub Pointer" || Column_Name == "Supplier Final Disc. With Max Slab(%)" || Column_Name == "Certi Date" || Column_Name == "Country of Origin")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(9.0);
                        }
                        else if (Column_Name == "Color")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(6.0);
                        }
                        else if (Column_Name == "Cert No" || Column_Name == "Pointer" || Column_Name == "Rap Rate($)" || Column_Name == "Rap Amount($)" || Column_Name == "Supplier Base Offer(%)" || Column_Name == "Supplier Base Offer Value($)" || Column_Name == "Supplier Final Disc(%)" || Column_Name == "Supplier Final Value($)" || Column_Name == "Bid Disc(%)" || Column_Name == "Bid Amt" || Column_Name == "Bid/Ct" || Column_Name == "Depth")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(0.0);

                            if (Column_Name == "Pointer")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(ppc_bg);
                            }
                        }
                        else if (Column_Name == "Supplier Final Value With Max Slab($)")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(10.5);
                        }
                        else if (Column_Name == "Avg. Stock Disc(%)" || Column_Name == "Avg. Stock Pcs" || Column_Name == "Avg. Pur. Disc(%)" || Column_Name == "Avg. Pur. Pcs" || Column_Name == "Avg. Sales Disc(%)" || Column_Name == "Sales Pcs")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(5.3);
                        }
                        else if (Column_Name == "KTS Grade" || Column_Name == "Comm. Grade" || Column_Name == "Zone" || Column_Name == "Sub Zone" || Column_Name == "Para. Grade")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(3.6);
                        }
                        else if (Column_Name == "Key To Symbol" || Column_Name == "Girdle")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(15.0);
                        }
                        else if (Column_Name == "RATIO" || Column_Name == "Length" || Column_Name == "Width" || Column_Name == "Depth(%)" || Column_Name == "Table(%)" || Column_Name == "Crown Angle" || Column_Name == "Crown Height" || Column_Name == "Pav Angle" || Column_Name == "Pav Height" || Column_Name == "Girdle(%)" || Column_Name == "Star Length" || Column_Name == "Lower HF" || Column_Name == "Luster" || Column_Name == "Cert Type" || Column_Name == "Table White" || Column_Name == "Side White" || Column_Name == "Table Black" || Column_Name == "Side Black" || Column_Name == "Culet" || Column_Name == "Milky")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(4.8);
                        }
                        else if (Column_Name == "Comment" || Column_Name == "Supplier Comment")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(25.0);
                        }
                        else if (Column_Name == "Table Open" || Column_Name == "Crown Open" || Column_Name == "Pav Open" || Column_Name == "Girdle Open" || Column_Name == "Table EF" || Column_Name == "Crown EF" || Column_Name == "Pav EF")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(4.9);
                        }
                        else if (Column_Name == "Girdle Type")
                        {
                            worksheet.Cells[2, k].AutoFitColumns(5.4);
                        }
                        else
                        {
                            worksheet.Cells[2, k].AutoFitColumns(0.0);
                        }
                    }
                    
                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        #region Assigns Value to Cell
                        int kk = 0;
                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                            try
                            {
                                if (Column_Name == "Image")
                                {
                                    kk += 1;

                                    string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Image"]);
                                    if (!string.IsNullOrEmpty(Image_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Video")
                                {
                                    kk += 1;

                                    string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Video"]);
                                    if (!string.IsNullOrEmpty(Video_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else
                                {
                                    kk += 1;
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);

                                    if (Column_Name == "DNA")
                                    {
                                        string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                        if (!string.IsNullOrEmpty(DNA_URL))
                                        {
                                            worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                            worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                        }
                                        else
                                        {
                                            worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                        }
                                    }
                                    else if (Column_Name == "Lab")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]);
                                        string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Certificate_URL"]);

                                        if (!string.IsNullOrEmpty(Certificate_URL))
                                        {
                                            worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]) + " \")";
                                            worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                        }
                                    }
                                    else if (Column_Name == "Rank")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rank"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(rank_bg);
                                    }
                                    else if (Column_Name == "Buyer Name")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Buyer Name"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(buyer_bg);
                                    }
                                    else if (Column_Name == "Status")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Status"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(status_bg);
                                    }
                                    else if (Column_Name == "Bid Disc(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Bid Disc(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Bid Disc(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(bid_bg);
                                    }
                                    else if (Column_Name == "Bid Amt")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Bid Amt"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Bid Amt"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(bid_bg);
                                    }
                                    else if (Column_Name == "Bid/Ct")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Bid/Ct"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Bid/Ct"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(bid_bg);
                                    }
                                    else if (Column_Name == "Avg. Stock Disc(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Avg. Stock Disc(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Avg. Stock Disc(%)"]) : DBNull.Value;

                                        worksheet.Cells[1, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(avg_stock_bg);
                                    }
                                    else if (Column_Name == "Avg. Stock Pcs")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Avg. Stock Pcs"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(avg_stock_bg);
                                    }
                                    else if (Column_Name == "Avg. Pur. Disc(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Avg. Pur. Disc(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Avg. Pur. Disc(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(avg_pur_bg);
                                    }
                                    else if (Column_Name == "Avg. Pur. Pcs")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Avg. Pur. Pcs"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(avg_pur_bg);
                                    }
                                    else if (Column_Name == "Avg. Sales Disc(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Avg. Sales Disc(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Avg. Sales Disc(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(avg_sales_bg);
                                    }
                                    else if (Column_Name == "Sales Pcs")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Sales Pcs"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(avg_sales_bg);
                                    }
                                    else if (Column_Name == "KTS Grade")
                                    {
                                        string kts_grade = Convert.ToString(dtStock.Rows[i - inStartIndex]["KTS Grade"]);
                                        worksheet.Cells[inwrkrow, kk].Value = kts_grade;

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(kts_bg);

                                        if(kts_grade == "K3") 
                                        {
                                            for (int col = 1; col <= column_dt.Rows.Count; col++)
                                            {
                                                worksheet.Cells[inwrkrow, col].Style.Font.Color.SetColor(Color.Red);
                                            }
                                        }
                                    }
                                    else if (Column_Name == "Comm. Grade")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Comm. Grade"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(comm_grade_bg);
                                    }
                                    else if (Column_Name == "Zone")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Zone"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(rank_bg);
                                    }
                                    else if (Column_Name == "Para. Grade")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Para. Grade"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(para_grade_bg);
                                    }
                                    else if (Column_Name == "Comment")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Comment"]);

                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(kts_bg);
                                    }
                                    else if (Column_Name == "CTS")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (Column_Name == "Rap Rate($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (Column_Name == "Rap Amount($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    }
                                    else if (Column_Name == "Supplier Base Offer(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Base Offer(%)"]);
                                        //worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Base Offer(%)"]) : 0;
                                        worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(22) + i + "/" + GetExcelColumnLetter(20) + i + ")-100,0)";

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_off_bg);
                                    }
                                    else if (Column_Name == "Supplier Base Offer Value($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Base Offer Value($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Base Offer Value($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_off_bg);
                                    }
                                    else if (Column_Name == "Supplier Final Disc(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Final Disc(%)"]);
                                        //worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Final Disc(%)"]) : 0;
                                        worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(24) + i + "/" + GetExcelColumnLetter(20) + i + ")-100,0)";
                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_final_bg);
                                    }
                                    else if (Column_Name == "Supplier Final Value($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Final Value($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Final Value($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_final_bg);
                                    }
                                    else if (Column_Name == "Supplier Final Disc. With Max Slab(%)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Final Disc. With Max Slab(%)"]);
                                        //worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Final Disc. With Max Slab(%)"]) : 0;
                                        worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(26) + i + "/" + GetExcelColumnLetter(20) + i + ")-100,0)";
                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(max_slab_bg);
                                    }
                                    else if (Column_Name == "Supplier Final Value With Max Slab($)")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Final Value With Max Slab($)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Final Value With Max Slab($)"]) : 0;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(max_slab_bg);
                                    }
                                    else if (Column_Name == "Cut")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                        if (cutValue == "3EX")
                                        {
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                        }
                                    }
                                    else if (Column_Name == "Polish")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Polish"]);
                                        
                                        if (cutValue == "3EX")
                                        {
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                        }
                                    }
                                    else if (Column_Name == "Symm")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Symm"]);

                                        if (cutValue == "3EX")
                                        {
                                            worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                        }
                                    }
                                    else if (Column_Name == "RATIO")
                                    {
                                        string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Length")
                                    {
                                        string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["Length"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Length"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Width")
                                    {
                                        string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["Width"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Width"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Depth")
                                    {
                                        string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Depth(%)")
                                    {
                                        string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Table(%)")
                                    {
                                        string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Table(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Table(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Girdle(%)")
                                    {
                                        string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Girdle(%)"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Girdle(%)"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Crown Angle")
                                    {
                                        string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Angle"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Angle"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Crown Height")
                                    {
                                        string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Height"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Height"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Pav Angle")
                                    {
                                        string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Angle"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Angle"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Pav Height")
                                    {
                                        string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Height"]);
                                        worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Height"]) : DBNull.Value;

                                        worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    }
                                    else if (Column_Name == "Cert Type")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cert Type"]);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    try
                    {
                        worksheet.Cells[inStartIndex, 1, (inwrkrow - 1), Row_Count].Style.Font.Size = 9;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }


                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "Supplier No")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CTS")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Rap Amount($)")
                            {

                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";

                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Supplier Base Offer(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(22) + "" + inStartIndex + ":" + GetExcelColumnLetter(22) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(20) + "" + inStartIndex + ":" + GetExcelColumnLetter(20) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Base Offer Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Supplier Final Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(24) + "" + inStartIndex + ":" + GetExcelColumnLetter(24) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(20) + "" + inStartIndex + ":" + GetExcelColumnLetter(20) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Final Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Supplier Final Disc. With Max Slab(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(26) + "" + inStartIndex + ":" + GetExcelColumnLetter(26) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(20) + "" + inStartIndex + ":" + GetExcelColumnLetter(20) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Final Value With Max Slab($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    //if (totalColumns >= 2)
                    //{
                    //    worksheet.DeleteColumn(totalColumns - 1, 3);
                    //}

                    if (totalColumns > 1)
                    {
                        worksheet.DeleteColumn(totalColumns, totalColumns);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static void Create_Supplier_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");
                    Color supp_cost_clr = ColorTranslator.FromHtml("#FF99CC");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Supplier Stock");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "Image")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "Video")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "Pointer")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(ppc_bg);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        #region Assigns Value to Cell
                        int kk = 0;
                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "Image")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Image"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "Video")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Video"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;
                                worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                if (Column_Name == "Lab")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Certificate_URL"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Amount($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supplier Cost Disc(%)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Cost Disc(%)"]);
                                    //worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Cost Disc(%)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(17) + i + "/" + GetExcelColumnLetter(15) + i + ")-100,0)";

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_cost_clr);
                                }
                                else if (Column_Name == "Supplier Cost Value($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Cost Value($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Cost Value($)"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_cost_clr);
                                }
                                else if (Column_Name == "Final Disc(%)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Disc(%)"]);
                                    //worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Disc(%)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(19) + i + "/" + GetExcelColumnLetter(15) + i + ")-100,0)";

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "Final Amt US($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "Supplier Base Offer(%)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Base Offer(%)"]);
                                    //worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Base Offer(%)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(21) + i + "/" + GetExcelColumnLetter(15) + i + ")-100,0)";

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supplier Base Offer Value($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Base Offer Value($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Base Offer Value($)"]) : 0;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Polish"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Symm"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(ratio_bg);
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["Length"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Length"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["Width"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Width"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth(%)")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table(%)")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Table(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Table(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Girdle(%)")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Girdle(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Girdle(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Angle")
                                {
                                    string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Angle"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Angle"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Height")
                                {
                                    string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Crown Height"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Crown Height"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pav Angle")
                                {
                                    string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Angle"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Angle"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pav Height")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Pav Height"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Pav Height"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, (inwrkrow - 1), Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "Cert No")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CTS")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Rap Amount($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Supplier Cost Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(17) + "" + inStartIndex + ":" + GetExcelColumnLetter(17) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(15) + "" + inStartIndex + ":" + GetExcelColumnLetter(15) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Cost Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Final Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(19) + "" + inStartIndex + ":" + GetExcelColumnLetter(19) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(15) + "" + inStartIndex + ":" + GetExcelColumnLetter(15) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Final Amt US($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Supplier Base Offer(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(21) + "" + inStartIndex + ":" + GetExcelColumnLetter(21) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(15) + "" + inStartIndex + ":" + GetExcelColumnLetter(15) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Base Offer Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    //if (totalColumns >= 2)
                    //{
                    //    worksheet.DeleteColumn(totalColumns - 1, 2);
                    //}
                    if (totalColumns > 1)
                    {
                        worksheet.DeleteColumn(totalColumns, totalColumns);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static void Create_Cart_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color yellow = ColorTranslator.FromHtml("#e5dc2b");
                    Color pink = ColorTranslator.FromHtml("#febdfe");
                    Color cyan = ColorTranslator.FromHtml("#24dbcd");
                    Color purple = ColorTranslator.FromHtml("#b5c7f7");
                    
                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Cart");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "POINTER")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;
                        
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;
                                //worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                if (Column_Name == "DNA")
                                {
                                    string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    if (!string.IsNullOrEmpty(DNA_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                    else
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    }
                                }
                                else if (Column_Name == "LAB")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "COMPANY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["COMPANY"]);
                                }
                                else if (Column_Name == "RANK")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["RANK"]);
                                }
                                else if (Column_Name == "BUYER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["BUYER"]);
                                }
                                else if (Column_Name == "STOCK ID")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["STOCK ID"]);
                                }
                                else if (Column_Name == "SUNRISE STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SUNRISE STATUS"]);
                                }
                                else if (Column_Name == "STOCK TYPE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["STOCK TYPE"]);
                                }
                                else if (Column_Name == "SUPPLIER NO")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SUPPLIER NO"]);
                                }
                                else if (Column_Name == "CERTIFICATE NO")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE NO"]);
                                }
                                else if (Column_Name == "SHAPE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SHAPE"]);
                                }
                                else if (Column_Name == "POINTER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["POINTER"]);
                                }
                                else if (Column_Name == "SUB POINTER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SUB POINTER"]);
                                }
                                else if (Column_Name == "COLOR")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["COLOR"]);
                                }
                                else if (Column_Name == "CLARITY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["CLARITY"]);
                                }
                                else if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP RATE")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP RATE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP RATE"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART STATUS"]);
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "CART BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART BASE DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(21) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "CART BASE AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART BASE AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CART BASE AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "CART FINAL DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(23) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "CART FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CART FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "CART MAX SLAB FINAL DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART MAX SLAB FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(25) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "CART MAX SLAB FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART MAX SLAB FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CART MAX SLAB FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "CART OFFER DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART OFFER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(27) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(cyan);
                                }
                                else if (Column_Name == "CART OFFER AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART OFFER AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CART OFFER AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(cyan);
                                }
                                else if (Column_Name == "STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["STATUS"]);
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(30) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "COST DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(32) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "COST AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "MAX SLAB BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["MAX SLAB BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(34) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "MAX SLAB BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["MAX SLAB BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["MAX SLAB BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "OFFER DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["OFFER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(36) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(cyan);
                                }
                                else if (Column_Name == "OFFER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(cyan);
                                }
                                else if (Column_Name == "BUYER DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BUYER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(38) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(purple);
                                }
                                else if (Column_Name == "BUYER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BUYER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["BUYER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(purple);
                                }
                                else if (Column_Name == "EXPECTED FINAL DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXPECTED FINAL DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) ? "IFERROR((100*" + GetExcelColumnLetter(40) + i + "/" + GetExcelColumnLetter(18) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXPECTED FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXP PROFIT AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXP PROFIT AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["EXP PROFIT AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXP PROFIT PER")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXP PROFIT PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["EXP PROFIT PER"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "AVG STOCK DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["AVG STOCK DISC"]);
                                }
                                else if (Column_Name == "AVG STOCK PCS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["AVG STOCK PCS"]);
                                }
                                else if (Column_Name == "AVG PURCHASE DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["AVG PURCHASE DISC"]);
                                }
                                else if (Column_Name == "AVG PURCHASE PCS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["AVG PURCHASE PCS"]);
                                }
                                else if (Column_Name == "AVG SALE DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["AVG SALE DISC"]);
                                }
                                else if (Column_Name == "AVG SALE PCS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["AVG SALE PCS"]);
                                }
                                else if (Column_Name == "KTS GRADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["KTS GRADE"]);
                                }
                                else if (Column_Name == "COMMENT GRADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["COMMENT GRADE"]);
                                }
                                else if (Column_Name == "ZONE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["ZONE"]);
                                }
                                else if (Column_Name == "PARAMETER GRADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["PARAMETER GRADE"]);
                                }
                                else if (Column_Name == "CUT")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "POLISH")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["POLISH"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "SYMM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SYMM"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "FLS INTENSITY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["FLS INTENSITY"]);
                                }
                                else if (Column_Name == "KEY TO SYMBOL")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["KEY TO SYMBOL"]);
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LENGTH")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["LENGTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["LENGTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "WIDTH")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["WIDTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["WIDTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH PER")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "TABLE PER")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["TABLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["TABLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN ANGLE")
                                {
                                    string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN HEIGHT")
                                {
                                    string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION ANGLE")
                                {
                                    string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION HEIGHT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "GIRDLE PER")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LUSTER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["LUSTER"]);
                                }
                                else if (Column_Name == "CERT TYPE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERT TYPE"]);
                                }
                                else if (Column_Name == "TABLE BLACK")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["TABLE BLACK"]);
                                }
                                else if (Column_Name == "SIDE BLACK")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SIDE BLACK"]);
                                }
                                else if (Column_Name == "TABLE WHITE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["TABLE WHITE"]);
                                }
                                else if (Column_Name == "SIDE WHITE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SIDE WHITE"]);
                                }
                                else if (Column_Name == "CULET")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["CULET"]);
                                }
                                else if (Column_Name == "LAB COMMENTS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB COMMENTS"]);
                                }
                                else if (Column_Name == "SUPPLIER COMMENTS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SUPPLIER COMMENTS"]);
                                }
                                else if (Column_Name == "TABLE OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["TABLE OPEN"]);
                                }
                                else if (Column_Name == "CROWN OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN OPEN"]);
                                }
                                else if (Column_Name == "PAVILION OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION OPEN"]);
                                }
                                else if (Column_Name == "GIRDLE OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["GIRDLE OPEN"]);
                                }
                                else if (Column_Name == "EXTRA FACET TABLE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXTRA FACET TABLE"]);
                                }
                                else if (Column_Name == "EXTRA FACET CROWN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXTRA FACET CROWN"]);
                                }
                                else if (Column_Name == "EXTRA FACET PAVILION")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXTRA FACET PAVILION"]);
                                }
                                else if (Column_Name == "BGM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["BGM"]);
                                }
                                else if (Column_Name == "SHADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SHADE"]);
                                }
                                else if (Column_Name == "MILKY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["MILKY"]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "SUPPLIER NO")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CTS")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "RAP AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CART BASE DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(21) + "" + inStartIndex + ":" + GetExcelColumnLetter(21) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "CART BASE AMT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CART FINAL DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(23) + "" + inStartIndex + ":" + GetExcelColumnLetter(23) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "CART FINAL AMT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CART MAX SLAB FINAL DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(25) + "" + inStartIndex + ":" + GetExcelColumnLetter(25) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "CART MAX SLAB FINAL AMT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CART OFFER DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(27) + "" + inStartIndex + ":" + GetExcelColumnLetter(27) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "CART OFFER AMT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "BASE DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(30) + "" + inStartIndex + ":" + GetExcelColumnLetter(30) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "BASE AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "COST DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(32) + "" + inStartIndex + ":" + GetExcelColumnLetter(32) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "COST AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "MAX SLAB BASE DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(34) + "" + inStartIndex + ":" + GetExcelColumnLetter(34) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "MAX SLAB BASE AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "OFFER DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(36) + "" + inStartIndex + ":" + GetExcelColumnLetter(36) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "OFFER AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "BUYER DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(38) + "" + inStartIndex + ":" + GetExcelColumnLetter(38) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "BUYER AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns > 1)
                    {
                        worksheet.DeleteColumn(totalColumns);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Approval_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Cart");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "POINTER")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;

                                if (Column_Name == "DNA")
                                {
                                    string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    if (!string.IsNullOrEmpty(DNA_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                    else
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    }
                                }
                                else if (Column_Name == "LAB")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP RATE")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP RATE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP RATE"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXPECTED FINAL DISC")
                                {

                                    int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                    int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) && pav_Height != "0.00" ? "IFERROR((100*" + GetExcelColumnLetter(columnIndex) + i + "/" + GetExcelColumnLetter(rapIndex) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXPECTED FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE DISC")
                                {
                                    int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                    int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) && pav_Height != "0.00" ? "IFERROR((100*" + GetExcelColumnLetter(columnIndex) + i + "/" + GetExcelColumnLetter(rapIndex) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST DISC")
                                {
                                    int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                    int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) && pav_Height != "0.00" ? "IFERROR((100*" + GetExcelColumnLetter(columnIndex) + i + "/" + GetExcelColumnLetter(rapIndex) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER DISC")
                                {
                                    int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                    int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BUYER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Formula = !string.IsNullOrEmpty(pav_Height) && pav_Height != "0.00" ? "IFERROR((100*" + GetExcelColumnLetter(columnIndex) + i + "/" + GetExcelColumnLetter(rapIndex) + i + ")-100,0)" : "0";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BUYER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["BUYER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CUT")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "POLISH")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["POLISH"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "SYMM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SYMM"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LENGTH")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["LENGTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["LENGTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "WIDTH")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["WIDTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["WIDTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH PER")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "TABLE PER")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["TABLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["TABLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN ANGLE")
                                {
                                    string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN HEIGHT")
                                {
                                    string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION ANGLE")
                                {
                                    string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION HEIGHT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "GIRDLE PER")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else 
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "SUPPLIER NO")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CTS")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "RAP AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "EXPECTED FINAL DISC")
                            {
                                int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(columnIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(columnIndex) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(rapIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(rapIndex) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "EXPECTED FINAL AMT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "BASE DISC")
                            {
                                int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(columnIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(columnIndex) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(rapIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(rapIndex) + "" + (inwrkrow - 1) + "))-100),2),0)";

                                //worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(25) + "" + inStartIndex + ":" + GetExcelColumnLetter(26) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(21) + "" + inStartIndex + ":" + GetExcelColumnLetter(21) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "BASE AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "COST DISC")
                            {
                                int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(columnIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(columnIndex) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(rapIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(rapIndex) + "" + (inwrkrow - 1) + "))-100),2),0)";

                                //worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(27) + "" + inStartIndex + ":" + GetExcelColumnLetter(28) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(21) + "" + inStartIndex + ":" + GetExcelColumnLetter(21) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "COST AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            //else if (Column_Name == "MAX SLAB BASE DISC")
                            //{
                            //    worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(29) + "" + inStartIndex + ":" + GetExcelColumnLetter(30) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(21) + "" + inStartIndex + ":" + GetExcelColumnLetter(21) + "" + (inwrkrow - 1) + "))-100),2),0)";
                            //    worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            //}
                            //else if (Column_Name == "MAX SLAB BASE AMOUNT")
                            //{
                            //    worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            //    worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            //    worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            //    worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            //    ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            //    cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                            //            = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                            //            = ExcelBorderStyle.Medium;
                            //}
                            else if (Column_Name == "BUYER DISC")
                            {
                                int columnIndex = GetColumnIndexByName(dtStock, Column_Name) + 1;
                                int rapIndex = GetColumnIndexByName(dtStock, "RAP AMOUNT");
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(columnIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(columnIndex) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(rapIndex) + "" + inStartIndex + ":" + GetExcelColumnLetter(rapIndex) + "" + (inwrkrow - 1) + "))-100),2),0)";

                                //worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(29) + "" + inStartIndex + ":" + GetExcelColumnLetter(29) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(21) + "" + inStartIndex + ":" + GetExcelColumnLetter(21) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "BUYER AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 2)
                    {
                        worksheet.DeleteColumn(totalColumns - 1, 2);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Order_Processing_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Cart");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "POINTER")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;

                                if (Column_Name == "DNA")
                                {
                                    string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    if (!string.IsNullOrEmpty(DNA_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                    else
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    }
                                }
                                else if (Column_Name == "LAB")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP RATE")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP RATE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP RATE"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(24) + i + "/" + GetExcelColumnLetter(22) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(26) + i + "/" + GetExcelColumnLetter(22) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BUYER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["BUYER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(28) + i + "/" + GetExcelColumnLetter(22) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "OFFER DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(31) + i + "/" + GetExcelColumnLetter(22) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "OFFER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CUT")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "POLISH")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["POLISH"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "SYMM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SYMM"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LENGTH")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["LENGTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["LENGTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "WIDTH")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["WIDTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["WIDTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH PER")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "TABLE PER")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["TABLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["TABLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN ANGLE")
                                {
                                    string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN HEIGHT")
                                {
                                    string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION ANGLE")
                                {
                                    string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION HEIGHT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "GIRDLE PER")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "MANAGEMENT STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else 
                                { 
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "SUPPLIER NO")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CTS")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "RAP AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "BASE DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(24) + "" + inStartIndex + ":" + GetExcelColumnLetter(24) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(22) + "" + inStartIndex + ":" + GetExcelColumnLetter(22) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "BASE AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "COST DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(26) + "" + inStartIndex + ":" + GetExcelColumnLetter(26) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(22) + "" + inStartIndex + ":" + GetExcelColumnLetter(22) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "COST AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "BUYER DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(28) + "" + inStartIndex + ":" + GetExcelColumnLetter(28) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(22) + "" + inStartIndex + ":" + GetExcelColumnLetter(22) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "BUYER AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }

                            else if (Column_Name == "OFFER DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(31) + "" + inStartIndex + ":" + GetExcelColumnLetter(31) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(22) + "" + inStartIndex + ":" + GetExcelColumnLetter(22) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "OFFER AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    //if (totalColumns >= 2)
                    //{
                    //    worksheet.DeleteColumn(totalColumns - 1, 2);
                    //}

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Stock_Availability_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");
                    Color supp_cost_clr = ColorTranslator.FromHtml("#FF99CC");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Cart");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "POINTER")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;

                                if (Column_Name == "LAB")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP RATE")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP RATE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP RATE"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RAP AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(18) + i + "/" + GetExcelColumnLetter(16) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_cost_clr);
                                }
                                else if (Column_Name == "COST AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["COST AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(supp_cost_clr);
                                }
                                else if (Column_Name == "OFFER DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(20) + i + "/" + GetExcelColumnLetter(16) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "OFFER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "BASE DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(22) + i + "/" + GetExcelColumnLetter(16) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CUT")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "POLISH")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["POLISH"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "SYMM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["SYMM"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(dtStock.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LENGTH")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["LENGTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["LENGTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "WIDTH")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["WIDTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["WIDTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH PER")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["DEPTH PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["DEPTH PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "TABLE PER")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["TABLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["TABLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN ANGLE")
                                {
                                    string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN HEIGHT")
                                {
                                    string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CROWN HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION ANGLE")
                                {
                                    string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION HEIGHT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["PAVILION HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "GIRDLE PER")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["GIRDLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else 
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "CERTIFICATE NO")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "CTS")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "RAP AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "COST DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(16) + "" + inStartIndex + ":" + GetExcelColumnLetter(16) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "COST AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "OFFER DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(20) + "" + inStartIndex + ":" + GetExcelColumnLetter(20) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(16) + "" + inStartIndex + ":" + GetExcelColumnLetter(16) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "OFFER AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "BASE DISC")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(22) + "" + inStartIndex + ":" + GetExcelColumnLetter(22) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(16) + "" + inStartIndex + ":" + GetExcelColumnLetter(16) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "BASE AMOUNT")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 2)
                    {
                        worksheet.DeleteColumn(totalColumns, 1);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Approval_Column_Wise_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                DataTable s_dt = new DataTable();
                foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                {
                    DataColumn existingColumn = dtStock.Columns[columnName];
                    s_dt.Columns.Add(columnName, existingColumn.DataType);

                }
                foreach (DataRow row in dtStock.AsEnumerable())
                {
                    DataRow newRow = s_dt.Rows.Add();
                    foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                    {
                        newRow[columnName] = row[columnName];
                    }
                }
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = s_dt.Rows.Count + inStartIndex;
                    int TotalRow = s_dt.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Cart");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "POINTER")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;
                        
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;

                                if (Column_Name == "DNA")
                                {
                                    string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    if (!string.IsNullOrEmpty(DNA_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                    else
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    }
                                }
                                else if (Column_Name == "LAB")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP RATE")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["RAP RATE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RAP RATE"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["RAP AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RAP AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXPECTED FINAL DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXPECTED FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BASE DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BASE DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["COST DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["COST DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["COST AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "MAX SLAB BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "MAX SLAB BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BUYER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BUYER DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BUYER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BUYER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CUT")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "POLISH")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["POLISH"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "SYMM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SYMM"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(s_dt.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LENGTH")
                                {
                                    string Length = Convert.ToString(s_dt.Rows[i - inStartIndex]["LENGTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["LENGTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "WIDTH")
                                {
                                    string Width = Convert.ToString(s_dt.Rows[i - inStartIndex]["WIDTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["WIDTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH")
                                {
                                    string Depth = Convert.ToString(s_dt.Rows[i - inStartIndex]["DEPTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["DEPTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH PER")
                                {
                                    string pepth_per = Convert.ToString(s_dt.Rows[i - inStartIndex]["DEPTH PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["DEPTH PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "TABLE PER")
                                {
                                    string table_Per = Convert.ToString(s_dt.Rows[i - inStartIndex]["TABLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["TABLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN ANGLE")
                                {
                                    string crown_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex]["CROWN ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CROWN ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN HEIGHT")
                                {
                                    string crown_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CROWN HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CROWN HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION ANGLE")
                                {
                                    string pav_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex]["PAVILION ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["PAVILION ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION HEIGHT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["PAVILION HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["PAVILION HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "GIRDLE PER")
                                {
                                    string girdle_Per = Convert.ToString(s_dt.Rows[i - inStartIndex]["GIRDLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["GIRDLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CART OFFER DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART OFFER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART OFFER DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART OFFER AMT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART OFFER AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART OFFER AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else 
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "SUPPLIER NO")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 1)
                    {
                        worksheet.DeleteColumn(totalColumns, 1);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Cart_Column_Wise_Excel(DataTable dtStock, DataTable column_dt, List<Dictionary<string, object>> color_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                DataTable s_dt = new DataTable();
                foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                {
                    DataColumn existingColumn = dtStock.Columns[columnName];
                    s_dt.Columns.Add(columnName, existingColumn.DataType);

                }
                foreach (DataRow row in dtStock.AsEnumerable())
                {
                    DataRow newRow = s_dt.Rows.Add();
                    foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                    {
                        newRow[columnName] = row[columnName];
                    }
                }
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = s_dt.Rows.Count + inStartIndex;
                    int TotalRow = s_dt.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Cart");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40;
                    worksheet.Row(2).Height = 40;
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "POINTER")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true;

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["CUT"]);
                        string statusValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["STATUS"]);
                        string cartStatusValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART STATUS"]);
                        string ktsGradeValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["KTS GRADE"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            kk += 1;
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                string Image_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                string Video_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                if (Column_Name == "DNA")
                                {
                                    string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    if (!string.IsNullOrEmpty(DNA_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                    else
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    }
                                }
                                else if (Column_Name == "LAB")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "COMPANY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["COMPANY"]);
                                }
                                else if (Column_Name == "RANK")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["RANK"]);
                                }
                                else if (Column_Name == "BUYER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["BUYER"]);
                                }
                                else if (Column_Name == "STOCK ID")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["STOCK ID"]);
                                }
                                else if (Column_Name == "STOCK TYPE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["STOCK TYPE"]);
                                }
                                else if (Column_Name == "SUPPLIER NO")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SUPPLIER NO"]);
                                }
                                else if (Column_Name == "SUNRISE STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SUNRISE STATUS"]);
                                }
                                else if (Column_Name == "CERTIFICATE NO")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["CERTIFICATE NO"]);
                                }
                                else if (Column_Name == "SHAPE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SHAPE"]);
                                }
                                else if (Column_Name == "POINTER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["POINTER"]);
                                }
                                else if (Column_Name == "SUB POINTER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SUB POINTER"]);
                                }
                                else if (Column_Name == "VALIDITY DATE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["VALIDITY DATE"]);
                                }
                                else if (Column_Name == "COLOR")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["COLOR"]);
                                }
                                else if (Column_Name == "CLARITY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["CLARITY"]);
                                }
                                else if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP RATE")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["RAP RATE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RAP RATE"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["RAP AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RAP AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART STATUS"]);
                                }
                                else if (Column_Name == "CART BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART BASE DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART BASE DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART BASE AMT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART BASE AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART BASE AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART FINAL DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART FINAL DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART FINAL DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART MAX SLAB FINAL DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART MAX SLAB FINAL DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART MAX SLAB FINAL DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART MAX SLAB FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CART MAX SLAB FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART MAX SLAB FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART OFFER DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART OFFER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CART OFFER DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CART OFFER AMT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["CART OFFER AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["CART OFFER AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["STATUS"]);
                                }
                                else if (Column_Name == "BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BASE DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BASE DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["COST DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["COST DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["COST AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "MAX SLAB BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "MAX SLAB BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["MAX SLAB BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "OFFER DISC")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["OFFER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["OFFER DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "OFFER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["OFFER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BUYER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BUYER DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BUYER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BUYER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXPECTED FINAL DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXPECTED FINAL AMT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["EXPECTED FINAL AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXP PROFIT AMT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXP PROFIT AMT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["EXP PROFIT AMT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "EXP PROFIT PER")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXP PROFIT PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["EXP PROFIT PER"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "AVG STOCK DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["AVG STOCK DISC"]);
                                }
                                else if (Column_Name == "AVG STOCK PCS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["AVG STOCK PCS"]);
                                }
                                else if (Column_Name == "AVG PURCHASE DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["AVG PURCHASE DISC"]);
                                }
                                else if (Column_Name == "AVG PURCHASE PCS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["AVG PURCHASE PCS"]);
                                }
                                else if (Column_Name == "AVG SALE DISC")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["AVG SALE DISC"]);
                                }
                                else if (Column_Name == "AVG SALE PCS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["AVG SALE PCS"]);
                                }
                                else if (Column_Name == "KTS GRADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["KTS GRADE"]);
                                }
                                else if (Column_Name == "COMMENT GRADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["COMMENT GRADE"]);
                                }
                                else if (Column_Name == "ZONE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["ZONE"]);
                                }
                                else if (Column_Name == "PARAMETER GRADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["PARAMETER GRADE"]);
                                }
                                else if (Column_Name == "CUT")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "POLISH")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["POLISH"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "SYMM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SYMM"]);
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "FLS INTENSITY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["FLS INTENSITY"]);
                                }
                                else if (Column_Name == "KEY TO SYMBOL")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["KEY TO SYMBOL"]);
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(s_dt.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LENGTH")
                                {
                                    string Length = Convert.ToString(s_dt.Rows[i - inStartIndex]["LENGTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["LENGTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "WIDTH")
                                {
                                    string Width = Convert.ToString(s_dt.Rows[i - inStartIndex]["WIDTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["WIDTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH")
                                {
                                    string Depth = Convert.ToString(s_dt.Rows[i - inStartIndex]["DEPTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["DEPTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH PER")
                                {
                                    string pepth_per = Convert.ToString(s_dt.Rows[i - inStartIndex]["DEPTH PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["DEPTH PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "TABLE PER")
                                {
                                    string table_Per = Convert.ToString(s_dt.Rows[i - inStartIndex]["TABLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["TABLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN ANGLE")
                                {
                                    string crown_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex]["CROWN ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CROWN ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN HEIGHT")
                                {
                                    string crown_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CROWN HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CROWN HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION ANGLE")
                                {
                                    string pav_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex]["PAVILION ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["PAVILION ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION HEIGHT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["PAVILION HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["PAVILION HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "GIRDLE PER")
                                {
                                    string girdle_Per = Convert.ToString(s_dt.Rows[i - inStartIndex]["GIRDLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["GIRDLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CERT TYPE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["CERT TYPE"]);
                                }
                                else if (Column_Name == "LUSTER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["LUSTER"]);
                                }
                                else if (Column_Name == "TABLE BLACK")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["TABLE BLACK"]);
                                }
                                else if (Column_Name == "SIDE BLACK")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SIDE BLACK"]);
                                }
                                else if (Column_Name == "TABLE WHITE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["TABLE WHITE"]);
                                }
                                else if (Column_Name == "SIDE WHITE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SIDE WHITE"]);
                                }
                                else if (Column_Name == "CULET")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["CULET"]);
                                }
                                else if (Column_Name == "LAB COMMENTS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB COMMENTS"]);
                                }
                                else if (Column_Name == "SUPPLIER COMMENTS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SUPPLIER COMMENTS"]);
                                }
                                else if (Column_Name == "TABLE OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["TABLE OPEN"]);
                                }
                                else if (Column_Name == "CROWN OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["CROWN OPEN"]);
                                }
                                else if (Column_Name == "PAVILION OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["PAVILION OPEN"]);
                                }
                                else if (Column_Name == "GIRDLE OPEN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["GIRDLE OPEN"]);
                                }
                                else if (Column_Name == "EXTRA FACET TABLE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXTRA FACET TABLE"]);
                                }
                                else if (Column_Name == "EXTRA FACET CROWN")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXTRA FACET CROWN"]);
                                }
                                else if (Column_Name == "EXTRA FACET PAVILION")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["EXTRA FACET PAVILION"]);
                                }
                                else if (Column_Name == "BGM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["BGM"]);
                                }
                                else if (Column_Name == "SHADE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SHADE"]);
                                }
                                else if (Column_Name == "MILKY")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["MILKY"]);
                                }
                                else if (Column_Name == "CUSTOMER")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["CUSTOMER"]);
                                }
                                else if (Column_Name == "REMARKS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["REMARKS"]);
                                }
                                else if (Column_Name == "VALIDITY DAYS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["VALIDITY DAYS"]);
                                }
                                else if (Column_Name == "REMARKS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["REMARKS"]);
                                }
                                else if (Column_Name == "INSERTED DATE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["INSERTED DATE"]);
                                }
                                else if (Column_Name == "UPDATED DATE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["UPDATED DATE"]);
                                }
                                else if (Column_Name == "REQUESTED FOR")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["REQUESTED FOR"]);
                                }
                                else if (Column_Name == "MANAGEMENT REMARKS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["MANAGEMENT REMARKS"]);
                                }
                                else if (Column_Name == "SUNRISE STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SUNRISE STATUS"]);
                                }
                                else if (Column_Name == "SUNRISE REMARKS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SUNRISE REMARKS"]);
                                }
                                else if (Column_Name == "SUB ZONE")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SUB ZONE"]);
                                }
                            }

                            var matchingDict = color_dt.Find(dict => dict.ContainsKey("Display_Name") && dict["Display_Name"].ToString() == Column_Name);

                            if (matchingDict != null)
                            {
                                if (matchingDict.ContainsKey("Back_Colour") && !string.IsNullOrWhiteSpace(matchingDict["Back_Colour"]?.ToString()))
                                {
                                    string backColour = matchingDict["Back_Colour"].ToString();

                                    Color color = ColorTranslator.FromHtml(backColour);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(color);
                                }

                                if (matchingDict.ContainsKey("Fore_Colour") && !string.IsNullOrWhiteSpace(matchingDict["Fore_Colour"]?.ToString()))
                                {
                                    string foreColour = matchingDict["Fore_Colour"].ToString();

                                    Color color = ColorTranslator.FromHtml(foreColour);

                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(color);
                                }
                            }

                            if (ktsGradeValue == "K3" && statusValue != "SOLD" && cartStatusValue != "Rejected")
                            {
                                for (int col = 1; col <= column_dt.Rows.Count; col++)
                                {
                                    worksheet.Cells[inwrkrow, col].Style.Font.Color.SetColor(Color.Red);
                                }
                            }
                            
                            if (statusValue == "SOLD")
                            {
                                worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                Color soldColor = ColorTranslator.FromHtml("#FFD4B5");
                                worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(soldColor);
                            }
                            
                            if (cartStatusValue == "Rejected")
                            {
                                worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                Color rejectedColor = ColorTranslator.FromHtml("#EE9C9C");
                                worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(rejectedColor);
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;
                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        kkk += 1;
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "SUPPLIER NO")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                            ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                    = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "CTS")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                    = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "RAP AMOUNT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                    = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "CART BASE DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) +""+ inStartIndex + ":" + GetExcelColumnLetter(kkk) +""+ (inwrkrow - 1) + ")/SUBTOTAL(103,A" +""+ inStartIndex +""+ ":A" +""+ (inwrkrow - 1) +"), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "CART BASE AMT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "CART FINAL DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "CART FINAL AMT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "CART MAX SLAB FINAL DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "CART MAX SLAB FINAL AMT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "CART OFFER DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "CART OFFER AMT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "BASE DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "BASE AMOUNT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "COST DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "COST AMOUNT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "MAX SLAB BASE DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "MAX SLAB BASE AMOUNT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "OFFER DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "OFFER AMOUNT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "BUYER DISC")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + ")/SUBTOTAL(103,A" + "" + inStartIndex + "" + ":A" + "" + (inwrkrow - 1) + "), 2)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "BUYER AMOUNT")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                    }
                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 1)
                    {
                        worksheet.DeleteColumn(totalColumns, 1);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Order_Processing_Column_Wise_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                DataTable s_dt = new DataTable();
                foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                {
                    DataColumn existingColumn = dtStock.Columns[columnName];
                    s_dt.Columns.Add(columnName, existingColumn.DataType);

                }
                foreach (DataRow row in dtStock.AsEnumerable())
                {
                    DataRow newRow = s_dt.Rows.Add();
                    foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                    {
                        newRow[columnName] = row[columnName];
                    }
                }
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Cart");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);

                            if (Column_Name == "POINTER")
                            {
                                worksheet.Cells[2, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[2, k].Style.Fill.BackgroundColor.SetColor(colFromHex_Pointer);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;
                        
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;

                                if (Column_Name == "DNA")
                                {
                                    string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    if (!string.IsNullOrEmpty(DNA_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                    else
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    }
                                }
                                else if (Column_Name == "LAB")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "CTS")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CTS"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CTS"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP RATE")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["RAP RATE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RAP RATE"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "RAP AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["RAP AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RAP AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BASE DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BASE DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BASE AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BASE AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BASE AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BUYER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BUYER DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "BUYER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["BUYER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["BUYER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["COST DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["COST DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "COST AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["COST AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["COST AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "OFFER DISC")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["OFFER DISC"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["OFFER DISC"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "OFFER AMOUNT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["OFFER AMOUNT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["OFFER AMOUNT"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "CUT")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "POLISH")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["POLISH"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "SYMM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["SYMM"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "RATIO")
                                {
                                    string ratio = Convert.ToString(s_dt.Rows[i - inStartIndex]["RATIO"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["RATIO"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "LENGTH")
                                {
                                    string Length = Convert.ToString(s_dt.Rows[i - inStartIndex]["LENGTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["LENGTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "WIDTH")
                                {
                                    string Width = Convert.ToString(s_dt.Rows[i - inStartIndex]["WIDTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["WIDTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH")
                                {
                                    string Depth = Convert.ToString(s_dt.Rows[i - inStartIndex]["DEPTH"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["DEPTH"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "DEPTH PER")
                                {
                                    string pepth_per = Convert.ToString(s_dt.Rows[i - inStartIndex]["DEPTH PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["DEPTH PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "TABLE PER")
                                {
                                    string table_Per = Convert.ToString(s_dt.Rows[i - inStartIndex]["TABLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["TABLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN ANGLE")
                                {
                                    string crown_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex]["CROWN ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CROWN ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "CROWN HEIGHT")
                                {
                                    string crown_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["CROWN HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["CROWN HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION ANGLE")
                                {
                                    string pav_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex]["PAVILION ANGLE"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["PAVILION ANGLE"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "PAVILION HEIGHT")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex]["PAVILION HEIGHT"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["PAVILION HEIGHT"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "GIRDLE PER")
                                {
                                    string girdle_Per = Convert.ToString(s_dt.Rows[i - inStartIndex]["GIRDLE PER"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex]["GIRDLE PER"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "MANAGEMENT STATUS")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else 
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "SUPPLIER NO")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 1)
                    {
                        worksheet.DeleteColumn(totalColumns, 1);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Order_Processing_Excel_Admin(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");

                    Color yellow = ColorTranslator.FromHtml("#F2DC13");
                    Color pink = ColorTranslator.FromHtml("#FF99CC");
                    Color blue = ColorTranslator.FromHtml("#93C5F7");
                    Color red = ColorTranslator.FromHtml("#FFABAB");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Order");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "Image")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "Video")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            if (Column_Name == "EXPECTED PROFIT AMT")
                            {
                                //k += 1;
                            }
                            else { 
                                k += 1;
                                worksheet.Cells[2, k].Value = Column_Name;
                                worksheet.Cells[2, k].AutoFitColumns(10);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "Image")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Image"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "Video")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Video"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "EXPECTED PROFIT AMT")
                            {
                                string EXPECTED_PROFIT_AMT = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXPECTED PROFIT AMT"]);
                                if (EXPECTED_PROFIT_AMT.Contains("-")) 
                                {
                                    for (int col = 1; col <= column_dt.Rows.Count; col++)
                                    {
                                        worksheet.Cells[inwrkrow, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, col].Style.Fill.BackgroundColor.SetColor(red);
                                    }
                                }
                            }
                            else
                            {
                                kk += 1;

                                if (Column_Name == "Lab")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cts"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Cts"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Amount($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Current Cost Disc(%)")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(15) + i + "/" + GetExcelColumnLetter(13) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Current Cost Value($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Current Cost Value($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Current Cost Value($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Supplier Cost Disc(%)")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(17) + i + "/" + GetExcelColumnLetter(13) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Supplier Cost Value($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Cost Value($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Cost Value($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Final Disc(%)")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(19) + i + "/" + GetExcelColumnLetter(13) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(blue);
                                }
                                else if (Column_Name == "Final Amt US($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(blue);
                                }
                                else if (Column_Name == "Supplier Base Offer(%)")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(21) + i + "/" + GetExcelColumnLetter(13) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supplier Base Offer Value($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Base Offer Value($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Base Offer Value($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Polish"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Symm"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["Length"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Length"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["Width"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Width"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth (%)")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth (%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth (%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table (%)")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Table (%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Table (%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Girdle(%)")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Girdle(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Girdle(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Status")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Status"]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Remarks")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Remarks"]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }

                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "Ref No")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Cts")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Rap Amount($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Current Cost Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(15) + "" + inStartIndex + ":" + GetExcelColumnLetter(15) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(13) + "" + inStartIndex + ":" + GetExcelColumnLetter(13) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Current Cost Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Supplier Cost Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(17) + "" + inStartIndex + ":" + GetExcelColumnLetter(17) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(13) + "" + inStartIndex + ":" + GetExcelColumnLetter(13) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Cost Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Final Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(19) + "" + inStartIndex + ":" + GetExcelColumnLetter(19) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(13) + "" + inStartIndex + ":" + GetExcelColumnLetter(13) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Final Amt US($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }

                            else if (Column_Name == "Supplier Base Offer(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(21) + "" + inStartIndex + ":" + GetExcelColumnLetter(21) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(13) + "" + inStartIndex + ":" + GetExcelColumnLetter(13) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Base Offer Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns > 1)
                    {
                        worksheet.DeleteColumn(totalColumns, totalColumns);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Order_Processing_Excel_Mazal(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color yellow = ColorTranslator.FromHtml("#F2DC13");
                    Color blue = ColorTranslator.FromHtml("#93C5F7");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Mazal");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "Image")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "Video")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "Image")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Image"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "Video")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Video"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                kk += 1;

                                if (Column_Name == "Lab")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cts"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Cts"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Amount($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Final Disc(%)")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(16) + i + "/" + GetExcelColumnLetter(14) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(blue);
                                }
                                else if (Column_Name == "Final Amt US($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(blue);
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Polish"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Symm"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["Length"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Length"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["Width"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Width"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth (%)")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth (%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth (%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table (%)")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Table (%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Table (%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Girdle(%)")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Girdle(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Girdle(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Status")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Status"]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Remarks")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Remarks"]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        kkk += 1;
                        if (Column_Name == "Ref No")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                            ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                    = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Cts")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                    = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Rap Amount($)")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                    = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Final Disc(%)")
                        {
                            worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(16) + "" + inStartIndex + ":" + GetExcelColumnLetter(16) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(14) + "" + inStartIndex + ":" + GetExcelColumnLetter(14) + "" + (inwrkrow - 1) + "))-100),2),0)";
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                        }
                        else if (Column_Name == "Final Amt US($)")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns > 1)
                    {
                        worksheet.DeleteColumn(totalColumns, totalColumns);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Order_Processing_Excel_User(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHex_Dis = ColorTranslator.FromHtml("#ccffff");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color tcpg_bg_clr = ColorTranslator.FromHtml("#fff2cc");
                    Color cellBg = ColorTranslator.FromHtml("#ccffff");
                    Color ppc_bg = ColorTranslator.FromHtml("#c6e0b4");
                    Color ratio_bg = ColorTranslator.FromHtml("#bdd7ee");
                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                    Color colFromHex_H1 = ColorTranslator.FromHtml("#8497b0");
                    Color col_color_Red = ColorTranslator.FromHtml("#ff0000");
                    Color yellow = ColorTranslator.FromHtml("#F2DC13");
                    Color pink = ColorTranslator.FromHtml("#FF99CC");
                    Color blue = ColorTranslator.FromHtml("#93C5F7");
                    Color red = ColorTranslator.FromHtml("#FFABAB");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Order");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    worksheet.Row(2).Height = 40; // Set row height
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "Image")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "Video")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            if (Column_Name == "EXPECTED PROFIT AMT")
                            {
                                //k += 1;
                            }
                            else
                            {
                                k += 1;
                                worksheet.Cells[2, k].Value = Column_Name;
                                worksheet.Cells[2, k].AutoFitColumns(10);
                            }
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;
                        
                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "Image")
                            {
                                kk += 1;

                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Image"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "Video")
                            {
                                kk += 1;

                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["Video"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "EXPECTED PROFIT AMT")
                            {
                                string EXPECTED_PROFIT_AMT = Convert.ToString(dtStock.Rows[i - inStartIndex]["EXPECTED PROFIT AMT"]);
                                if (EXPECTED_PROFIT_AMT.Contains("-"))
                                {
                                    for (int col = 1; col <= column_dt.Rows.Count; col++)
                                    {
                                        worksheet.Cells[inwrkrow, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        worksheet.Cells[inwrkrow, col].Style.Fill.BackgroundColor.SetColor(red);
                                    }
                                }
                            }
                            else
                            {
                                kk += 1;

                                //worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                if (Column_Name == "Lab")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]);
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["Lab"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cts"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Cts"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Rate($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Amount($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Rap Amount($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supplier Cost Disc(%)")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(18) + i + "/" + GetExcelColumnLetter(16) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Supplier Cost Value($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Supplier Cost Value($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Supplier Cost Value($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Final Disc(%)")
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "IFERROR((100*" + GetExcelColumnLetter(20) + i + "/" + GetExcelColumnLetter(16) + i + ")-100,0)";
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(blue);
                                }
                                else if (Column_Name == "Final Amt US($)")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Final Amt US($)"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(blue);
                                }
                                else if (Column_Name == "Price / Cts")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex]["Price / Cts"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Price / Cts"]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Polish"]);
                                    
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Symm"]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex]["Length"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Length"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex]["Width"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Width"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth (%)")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Depth (%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Depth (%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table (%)")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Table (%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Table (%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Girdle(%)")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex]["Girdle(%)"]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex]["Girdle(%)"]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Status")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Status"]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Remarks")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["Remarks"]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Image")
                        {
                            kkk += 1;
                        }
                        else if (Column_Name == "Video")
                        {
                            kkk += 1;
                        }
                        else
                        {
                            kkk += 1;
                            if (Column_Name == "Ref No")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                                ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                        = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Cts")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                        = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Rap Amount($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                        = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Supplier Cost Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(18) + "" + inStartIndex + ":" + GetExcelColumnLetter(18) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(16) + "" + inStartIndex + ":" + GetExcelColumnLetter(16) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Supplier Cost Value($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                            else if (Column_Name == "Final Disc(%)")
                            {
                                worksheet.Cells[1, kkk].Formula = "IFERROR(ROUND(((SUBTOTAL(109," + GetExcelColumnLetter(20) + "" + inStartIndex + ":" + GetExcelColumnLetter(20) + "" + (inwrkrow - 1) + ")*100/SUBTOTAL(109," + GetExcelColumnLetter(16) + "" + inStartIndex + ":" + GetExcelColumnLetter(16) + "" + (inwrkrow - 1) + "))-100),2),0)";
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";
                            }
                            else if (Column_Name == "Final Amt US($)")
                            {
                                worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                        = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns > 1)
                    {
                        worksheet.DeleteColumn(totalColumns, totalColumns);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Account_Group_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 2;
                    int inwrkrow = 2;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Account Group");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        k += 1;
                        worksheet.Cells[1, k].Value = Column_Name;
                        worksheet.Cells[1, k].AutoFitColumns(10);
                    }

                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[1, 1, 1, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[1, 1, 1, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[1, 1, 1, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;
                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            kk += 1;
                            worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        public static void Create_Account_Master_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 2;
                    int inwrkrow = 2;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Account Master");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40; // Set row height
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        k += 1;
                        worksheet.Cells[1, k].Value = Column_Name;
                        worksheet.Cells[1, k].AutoFitColumns(10);
                    }

                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[1, 1, 1, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[1, 1, 1, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[1, 1, 1, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;
                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            kk += 1;
                            worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        static string GetExcelColumnLetter(int columnNumber)
        {
            string columnLetter = "";

            while (columnNumber > 0)
            {
                int remainder = (columnNumber - 1) % 26;
                char letter = (char)('A' + remainder);
                columnLetter = letter + columnLetter;
                columnNumber = (columnNumber - 1) / 26;
            }

            return columnLetter;
        }
        static int GetColumnIndexByName(DataTable table, string columnName)
        {
            DataColumn column = table.Columns[columnName];
            if (column != null)
            {
                return column.Ordinal - 1;
            }
            return -1;
        }

        public static void Create_Latest_Supplier_Stock_Excel(DataTable dtStock, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    var worksheet = ep.Workbook.Worksheets.Add(DateTime.Now.ToString("dd-MM-yyyy"));

                    worksheet.Cells["A1"].LoadFromDataTable(dtStock, true);

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;

                    var headerCells = worksheet.Cells[1, 1, 1, dtStock.Columns.Count];
                    headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    headerCells.Style.Font.Size = 10;
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, ep.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void Create_Supplier_Stock_Upload_Status_Excel(DataTable dtSupplier, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (var ep = new ExcelPackage())
                {
                    var worksheet = ep.Workbook.Worksheets.Add(DateTime.Now.ToString("dd-MM-yyyy"));

                    worksheet.Cells["A1"].LoadFromDataTable(dtSupplier, true);

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;

                    var headerCells = worksheet.Cells[1, 1, 1, dtSupplier.Columns.Count];
                    headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    headerCells.Style.Font.Size = 10;
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, ep.GetAsByteArray());
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public static void Create_Order_Processing_Excel_Pre_Post(DataTable dtSupplier, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (var ep = new ExcelPackage())
                {
                    var worksheet = ep.Workbook.Worksheets.Add(DateTime.Now.ToString("dd-MM-yyyy"));

                    worksheet.Cells["A1"].LoadFromDataTable(dtSupplier, true);

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;

                    var headerCells = worksheet.Cells[1, 1, 1, dtSupplier.Columns.Count];
                    headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    headerCells.Style.Font.Size = 10;
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, ep.GetAsByteArray());
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public static void Create_Lab_Entry_Report_Column_Wise_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                DataTable s_dt = new DataTable();
                foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                {
                    DataColumn existingColumn = dtStock.Columns[columnName];
                    s_dt.Columns.Add(columnName, existingColumn.DataType);

                }
                foreach (DataRow row in dtStock.AsEnumerable())
                {
                    DataRow newRow = s_dt.Rows.Add();
                    foreach (var columnName in column_dt.AsEnumerable().Select(row => Convert.ToString(row["Column_Name"])))
                    {
                        newRow[columnName] = row[columnName];
                    }
                }
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = s_dt.Rows.Count + inStartIndex;
                    int TotalRow = s_dt.Rows.Count;
                    int i;

                    Color colFromHex_Pointer = ColorTranslator.FromHtml("#c6e0b4");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");

                    Color yellow = ColorTranslator.FromHtml("#F2DC13");
                    Color Supp_Cost = ColorTranslator.FromHtml("#FFB8C5");
                    Color Net_Cost = ColorTranslator.FromHtml("#E7FFFF");
                    Color Profit = ColorTranslator.FromHtml("#D1FFD1");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Lab Entry Report");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40;
                    worksheet.Row(2).Height = 40;
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true;

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["CUT"]);
                        string statusValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["STATUS"]);
                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            kk += 1;
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            if (Column_Name == "IMAGE LINK")
                            {
                                string Image_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                string Video_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                if (Column_Name == "DNA")
                                {
                                    string DNA_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    if (!string.IsNullOrEmpty(DNA_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(DNA_URL) + "\",\" DNA \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                    else
                                    {
                                        worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex]["DNA"]);
                                    }
                                }
                                else if (Column_Name == "Lab")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex]["LAB"]);
                                    string Certificate_URL = Convert.ToString(s_dt.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + Convert.ToString(dtStock.Rows[i - inStartIndex]["LAB"]) + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Stone Status")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Column(kk).Width = 20;
                                }
                                else if (Column_Name == "Type")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Status")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);

                                    Color common_bg = ColorTranslator.FromHtml("#ffffcc");
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);

                                    if (string.Equals(statusValue, "SOLD", StringComparison.OrdinalIgnoreCase) || string.Equals(statusValue, "QC REJECT", StringComparison.OrdinalIgnoreCase) || string.Equals(statusValue, "CANCEL", StringComparison.OrdinalIgnoreCase))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Red);
                                    }
                                    if (string.Equals(statusValue, "CHECKING AVAILABILITY", StringComparison.OrdinalIgnoreCase))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Green);
                                    }
                                    
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                }
                                else if (Column_Name == "Shipment Type")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Order No")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Order Date")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Days")
                                {
                                    int days = Convert.ToInt32(dtStock.Rows[i - inStartIndex][Column_Name].ToString());

                                    worksheet.Cells[inwrkrow, kk].Value = days.ToString();

                                    if (days > 7)
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Red);
                                    }
                                }
                                else if (Column_Name == "Purchase Doc No")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Expected Delivery Date")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Supplier Ref No")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Cert No")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Supplier Name")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Supplier Short Name")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Assist By")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Company")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Stock Id")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Shape")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Pointer")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Sub Pointer")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "BGM")
                                {
                                    string BGM = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name].ToString());

                                    worksheet.Cells[inwrkrow, kk].Value = BGM;

                                    if (!string.Equals(BGM, "NO BGM", StringComparison.OrdinalIgnoreCase))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Red);
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Color")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Pre Sold")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Clarity")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supp Base Disc %")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supp Base Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supp Cost Disc %")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(Supp_Cost);
                                }
                                else if (Column_Name == "Supp Cost Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(Supp_Cost);
                                }
                                else if (Column_Name == "Offer Disc %")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Offer Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Web Disc %")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Web Disc Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Final Disc %")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(Net_Cost);
                                }
                                else if (Column_Name == "Final Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(Net_Cost);
                                }
                                else if (Column_Name == "Profit %")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(Profit);

                                    if (pav_Height.Contains("-"))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Red);
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Profit")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(Profit);

                                    if (pav_Height.Contains("-"))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Red);
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Shipment Min Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                }
                                else if (Column_Name == "Shipment Total")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Fls")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Ratio")
                                {
                                    string ratio = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(ratio) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                    Color common_bg = ColorTranslator.FromHtml("#bdd7ee");
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth %")
                                {
                                    string pepth_per = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table %")
                                {
                                    string table_Per = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Key To Symbol")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Comment")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Girdle %")
                                {
                                    string girdle_Per = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Angle")
                                {
                                    string crown_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Height")
                                {
                                    string crown_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pavilion Angle")
                                {
                                    string pav_Angle = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pavilion Height")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table White")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Crown White")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Table Black")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Crown Black")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Culet")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Table Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Crown Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Pavilion Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Girdle Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Sunrise Stock ID")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Sunrise Status")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Requested Remarks")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Order Process Remarks")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);

                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "RFID No")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Fortune Source Company")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Supp Verified Disc%")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Supp Verified Amount")
                                {
                                    string pav_Height = Convert.ToString(s_dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(s_dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Delivered")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                            }
                        }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;
                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        kkk += 1;
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                        if (Column_Name == "Stock Id")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex+  ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                            ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                    = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Cts")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                    = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Rap Amount")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                    = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Supp Base Disc %")
                        {
                            int finalColumnIndex = GetColumnIndexByName(worksheet, "Supp Base Amount");
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Amount");

                            if (finalColumnIndex > 0 && rapColumnIndex > 0)
                            {
                                worksheet.Cells[1, kkk].Formula =
                                                                    "=IFERROR(ROUND((-100+(SUBTOTAL(109, " + GetExcelColumnLetter(finalColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(finalColumnIndex) + (inwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (inwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Supp Base Amount")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Offer Disc %")
                        {
                            int finalColumnIndex = GetColumnIndexByName(worksheet, "Offer Amount");
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Amount");

                            if (finalColumnIndex > 0 && rapColumnIndex > 0)
                            {
                                worksheet.Cells[1, kkk].Formula =
                                                                    "=IFERROR(ROUND((-100+(SUBTOTAL(109, " + GetExcelColumnLetter(finalColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(finalColumnIndex) + (inwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (inwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Offer Amount")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Supp Cost Disc %")
                        {
                            int finalColumnIndex = GetColumnIndexByName(worksheet, "Supp Cost Amount");
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Amount");

                            if (finalColumnIndex > 0 && rapColumnIndex > 0)
                            {
                                worksheet.Cells[1, kkk].Formula =
                                                                    "=IFERROR(ROUND((-100+(SUBTOTAL(109, " + GetExcelColumnLetter(finalColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(finalColumnIndex) + (inwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (inwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Supp Cost Amount")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                    }
                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns >= 1)
                    {
                        worksheet.DeleteColumn(totalColumns, 1);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public static void Create_Lab_Entry_Report_Excel(DataTable dtStock, DataTable column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dtStock.Rows.Count + inStartIndex;
                    int TotalRow = dtStock.Rows.Count;
                    int i;

                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color yellow = ColorTranslator.FromHtml("#F2DC13");
                    Color blue = ColorTranslator.FromHtml("#93C5F7");

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Lab Entry");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = DateTime.Now.ToString("dd-MM-yyyy");
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40;
                    worksheet.Row(2).Height = 40;
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        if (Column_Name == "IMAGE LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Image";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else if (Column_Name == "VIDEO LINK")
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = "Video";
                            worksheet.Cells[2, k].AutoFitColumns(7);
                        }
                        else
                        {
                            k += 1;
                            worksheet.Cells[2, k].Value = Column_Name;
                            worksheet.Cells[2, k].AutoFitColumns(10);
                        }
                    }

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["Cut"]);
                        string statusValue = Convert.ToString(dtStock.Rows[i - inStartIndex]["STATUS"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            kk += 1;
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);
                            if (Column_Name == "IMAGE LINK")
                            {
                                string Image_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["IMAGE LINK"]);
                                if (!string.IsNullOrEmpty(Image_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else if (Column_Name == "VIDEO LINK")
                            {
                                string Video_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["VIDEO LINK"]);
                                if (!string.IsNullOrEmpty(Video_URL))
                                {
                                    worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Video \")";
                                    worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                    worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                }
                            }
                            else
                            {
                                if (Column_Name == "Stock Id")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Lab")
                                {
                                    string lab = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = lab;
                                    string Certificate_URL = Convert.ToString(dtStock.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(Certificate_URL))
                                    {
                                        worksheet.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + Convert.ToString(Certificate_URL) + "\",\" " + lab + " \")";
                                        worksheet.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Cert No")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Status")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);

                                    Color common_bg = ColorTranslator.FromHtml("#ffffcc");
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);

                                    if (statusValue == "SOLD" || statusValue == "QC REJECT")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Red);
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                }

                                    if (statusValue == "BUSY")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Shape")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Pointer")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "BGM")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Color")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Clarity")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Amount")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Final Disc%")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "Final Amount")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    Color common_bg = ColorTranslator.FromHtml("#CCFFFF");
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(common_bg);
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Fls")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth%")
                                {
                                    string pepth_per = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table%")
                                {
                                    string table_Per = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Key To Symbol")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Comment")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Girdle%")
                                {
                                    string girdle_Per = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Angle")
                                {
                                    string crown_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Crown Height")
                                {
                                    string crown_Height = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(crown_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pavilion Angle")
                                {
                                    string pav_Angle = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Angle) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Pavilion Height")
                                {
                                    string pav_Height = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                    worksheet.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dtStock.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table White")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Crown White")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Table Black")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Crown Black")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Culet")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Table Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Crown Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Pavilion Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                                else if (Column_Name == "Girdle Open")
                                {
                                    worksheet.Cells[inwrkrow, kk].Value = Convert.ToString(dtStock.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        kkk += 1;
                        if (Column_Name == "Stock Id")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103,A" + inStartIndex + ":A" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                            ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                    = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Cts")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Rap Amount")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Final Disc%")
                        {
                            int finalColumnIndex = GetColumnIndexByName(worksheet, "Final Amount");
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Amount");

                            if (finalColumnIndex > 0 && rapColumnIndex > 0)
                            {
                                worksheet.Cells[1, kkk].Formula =
                                                                    "=IFERROR(ROUND((-100 + (SUBTOTAL(109, " + GetExcelColumnLetter(finalColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(finalColumnIndex) + (inwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + inStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (inwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, kkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Final Amount")
                        {
                            worksheet.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_TotalAmt = worksheet.Cells[1, kkk].Style;
                            cellStyleHeader_TotalAmt.Border.Left.Style = cellStyleHeader_TotalAmt.Border.Right.Style
                                    = cellStyleHeader_TotalAmt.Border.Top.Style = cellStyleHeader_TotalAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                    }

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet.Dimension.End.Column;

                    if (totalColumns > 1)
                    {
                        worksheet.DeleteColumn(totalColumns, totalColumns);
                    }

                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public static void Create_Lab_Entry_Auto_Order_Not_Placed_Overseas(DataTable dtSupplier, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (var ep = new ExcelPackage())
                {
                    var worksheet = ep.Workbook.Worksheets.Add(DateTime.Now.ToString("dd-MM-yyyy"));

                    worksheet.Cells["A1"].LoadFromDataTable(dtSupplier, true);

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;

                    var headerCells = worksheet.Cells[1, 1, 1, dtSupplier.Columns.Count];
                    headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    headerCells.Style.Font.Size = 10;
                    headerCells.Style.Font.Bold = true;
                    headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    int rowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, rowEnd, 100].Address);

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, ep.GetAsByteArray());
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public static void Create_Purchase_Detail_Excel(DataTable dt, DataTable column_dt, DataTable pricing_dt, DataTable pricing_column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    Color yellow = ColorTranslator.FromHtml("#FFFF00");
                    Color pink = ColorTranslator.FromHtml("#FFCCFF");
                    Color mustard_yellow = ColorTranslator.FromHtml("#FFD700");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color light_yellow = ColorTranslator.FromHtml("#FFF2E0");
                    Color sky_blue = ColorTranslator.FromHtml("#E0FFFF");

                    int pRow_Count = pricing_column_dt.Rows.Count;
                    int pinStartIndex = 3;
                    int pinwrkrow = 3;
                    int pinEndCounter = pricing_dt.Rows.Count + pinStartIndex;
                    int pTotalRow = pricing_dt.Rows.Count;
                    int pi;

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Sheet1");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = "Pricing Layout";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40;
                    worksheet.Row(2).Height = 40;
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int pk = 0;
                    for (int j = 0; j < pricing_column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(pricing_column_dt.Rows[j]["Column_Name"]);

                        pk += 1;
                        worksheet.Cells[2, pk].Value = Column_Name;
                        worksheet.Cells[2, pk].AutoFitColumns(10);

                    }

                    worksheet.Cells[1, 1, 1, pRow_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, pRow_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, pRow_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, pRow_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, pRow_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, pRow_Count].AutoFilter = true; // Set Filter to header

                    var pcellBackgroundColor1 = worksheet.Cells[2, 1, 2, pRow_Count].Style.Fill;
                    pcellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    pcellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle pcellStyleHeader1 = worksheet.Cells[2, 1, 2, pRow_Count].Style;
                    pcellStyleHeader1.Border.Left.Style = pcellStyleHeader1.Border.Right.Style
                            = pcellStyleHeader1.Border.Top.Style = pcellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[pinStartIndex, 1, pinEndCounter, pRow_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var pasTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (pi = pinStartIndex; pi < pinEndCounter; pi++)
                    {
                        #region Assigns Value to Cell
                        int pkk = 0;

                        string cutValue = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["Cut"]);

                        for (int j = 0; j < pricing_column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(pricing_column_dt.Rows[j]["Column_Name"]);
                            {
                                pkk += 1;

                                if (Column_Name == "Lab")
                                {
                                    string labValue = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["Lab"]);
                                    string certificateURL = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["CERTI"]);

                                    if (!string.IsNullOrEmpty(certificateURL) && !string.IsNullOrEmpty(labValue))
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Value = labValue;
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "=HYPERLINK(\"" + certificateURL + "\",\"" + labValue + "\")";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.UnderLine = true;
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Image")
                                {
                                    string Image_URL = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["Image"]);

                                    if (!string.IsNullOrEmpty(Image_URL))
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.UnderLine = true;
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Color.SetColor(Color.Blue);
                                    }

                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Video")
                                {
                                    string Video_URL = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["VIDEO"]);

                                    if (!string.IsNullOrEmpty(Video_URL))
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Movie \")";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.UnderLine = true;
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate" || Column_Name == "Rap Value")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                    
                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Final Disc")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(yellow);

                                    worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                }
                                else if (Column_Name == "Final Value")
                                {
                                    int FinalColumnIndex = GetColumnIndexByName(worksheet, "Final Disc");
                                    int RapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");

                                    if (FinalColumnIndex > 0 && RapColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR(((100 - " + GetExcelColumnLetter(FinalColumnIndex) + pi + ") * " + GetExcelColumnLetter(RapColumnIndex) + pi + ")/100,0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(yellow);

                                    worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                }
                                else if (Column_Name == "Profit (%)")
                                {
                                    int ProfitColumnIndex = GetColumnIndexByName(worksheet, "Profit Value");
                                    int SupplierColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                                    if (ProfitColumnIndex > 0 && SupplierColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR((" + GetExcelColumnLetter(ProfitColumnIndex) + pi + " / " + GetExcelColumnLetter(SupplierColumnIndex) + pi +")*100,0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(light_yellow);
                                }
                                else if (Column_Name == "Profit Value")
                                {
                                    int FinalColumnIndex = GetColumnIndexByName(worksheet, "Final Value");
                                    int SupplierColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                                    if (FinalColumnIndex > 0 && SupplierColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR((" + GetExcelColumnLetter(FinalColumnIndex) + pi + " - " + GetExcelColumnLetter(SupplierColumnIndex) + pi + "),0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(light_yellow);
                                }
                                else if (Column_Name == "Supplier Final Disc(%)")
                                {
                                    int SupplierColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");
                                    int RapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");

                                    if (RapColumnIndex > 0 && SupplierColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR((1 - (" + GetExcelColumnLetter(SupplierColumnIndex) + pi + " / " + GetExcelColumnLetter(RapColumnIndex) + pi + ")) * 100,0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Supplier Final Value")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "KTS Grade" || Column_Name == "Com" || Column_Name == "Zone" || Column_Name == "Sub Zone" || Column_Name == "P.Grade" || Column_Name == "FMS" || Column_Name == "Bullet" || Column_Name == "Stock Disc (%)" || Column_Name == "Stock Pcs" || Column_Name == "Avg Pur Disc (%)" || Column_Name == "Avg Pur Pcs" || Column_Name == "Sales Disc (%)" || Column_Name == "Sales Pcs" || Column_Name == "Raptop" || Column_Name == "Rapnet Days")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                             
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(sky_blue);
                                }
                                else if (Column_Name == "Sunrise Grade")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                             
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(sky_blue);
                                    
                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Source Party")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Stone Clarity")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(mustard_yellow);
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish" || Column_Name == "Symm")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Length" || Column_Name == "Width" || Column_Name == "Depth" || Column_Name == "Depth (%)" || Column_Name == "Table (%)" || Column_Name == "Girdle (%)" || Column_Name == "Crown Angle" || Column_Name == "Crown Height" || Column_Name == "Pav Angle" || Column_Name == "Pav Height")
                                {
                                    string value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(value) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : DBNull.Value;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "0.00";
                                }
                                else
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                }
                            }
                        }

                        pinwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[pinStartIndex, 1, pinwrkrow, pRow_Count].Style.Font.Size = 9;

                    int pkkk = 0;
                    for (int j = 0; j < pricing_column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(pricing_column_dt.Rows[j]["Column_Name"]);

                        pkkk += 1;
                        if (Column_Name == "Supplier Stock ID")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##";

                            ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                    = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Cts")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                    = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Rap Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                    = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Final Disc")
                        {
                            int finalColumnIndex = GetColumnIndexByName(worksheet, "Final Value");
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");

                            if (finalColumnIndex > 0 && rapColumnIndex > 0)
                            {
                                worksheet.Cells[1, pkkk].Formula =
                                                                    "=IFERROR(ROUND((100-(SUBTOTAL(109, " + GetExcelColumnLetter(finalColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(finalColumnIndex) + (pinwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (pinwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, pkkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Final Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_FinalValue = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_FinalValue.Border.Left.Style = cellStyleHeader_FinalValue.Border.Right.Style
                                    = cellStyleHeader_FinalValue.Border.Top.Style = cellStyleHeader_FinalValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Profit (%)")
                        {
                            int profitColumnIndex = GetColumnIndexByName(worksheet, "Profit Value");
                            int supplierFinalValueColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                            if (profitColumnIndex > 0 && supplierFinalValueColumnIndex > 0)
                            {
                                worksheet.Cells[1, pkkk].Formula =
                                                                    "=IFERROR(ROUND((SUBTOTAL(109, " + GetExcelColumnLetter(profitColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(profitColumnIndex) + (pinwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(supplierFinalValueColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(supplierFinalValueColumnIndex) + (pinwrkrow - 1) +
                                                                    ")) * 100, 2), 0.00)";

                                worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, pkkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Profit Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_ProfitValue = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_ProfitValue.Border.Left.Style = cellStyleHeader_ProfitValue.Border.Right.Style
                                    = cellStyleHeader_ProfitValue.Border.Top.Style = cellStyleHeader_ProfitValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Supplier Final Disc(%)")
                        {
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");
                            int supplierFinalValueColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                            if (rapColumnIndex > 0 && supplierFinalValueColumnIndex > 0)
                            {
                                worksheet.Cells[1, pkkk].Formula =
                                                                    "=IFERROR(ROUND((100-(SUBTOTAL(109, " + GetExcelColumnLetter(supplierFinalValueColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(supplierFinalValueColumnIndex) + (pinwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (pinwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, pkkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Supplier Final Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_SupplierFinalValue = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_SupplierFinalValue.Border.Left.Style = cellStyleHeader_SupplierFinalValue.Border.Right.Style
                                    = cellStyleHeader_SupplierFinalValue.Border.Top.Style = cellStyleHeader_SupplierFinalValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                    }

                    int prowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, prowEnd, 100].Address);

                    int ptotalColumns = worksheet.Dimension.End.Column;

                    if (ptotalColumns > 1)
                    {
                        worksheet.DeleteColumn(ptotalColumns, ptotalColumns);
                    }
                    
                    int Row_Count = column_dt.Rows.Count;
                    int inStartIndex = 3;
                    int inwrkrow = 3;
                    int inEndCounter = dt.Rows.Count + inStartIndex;
                    int TotalRow = dt.Rows.Count;
                    int i;

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Sheet2");

                    ExcelWorksheet worksheet1 = ep.Workbook.Worksheets[1];

                    worksheet1.Name = "Sheet1";
                    worksheet1.Cells.Style.Font.Size = 11;
                    worksheet1.Cells.Style.Font.Name = "Calibri";

                    worksheet1.Row(1).Height = 40; // Set row height
                    worksheet1.Row(2).Height = 40; // Set row height
                    worksheet1.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int k = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        k += 1;
                        worksheet1.Cells[2, k].Value = Column_Name;
                        worksheet1.Cells[2, k].AutoFitColumns(10);

                    }
                    
                    worksheet1.Cells[1, 1, 1, Row_Count].Style.Font.Bold = true;
                    worksheet1.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;
                    worksheet1.Cells[1, 1, 1, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells[1, 1, 1, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet1.Cells[1, 1, 1, Row_Count].Style.Font.Size = 11;

                    worksheet1.Cells[2, 1, 2, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet1.Cells[2, 1, 2, Row_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet1.Cells[2, 1, 2, Row_Count].Style.Font.Size = 10;
                    worksheet1.Cells[2, 1, 2, Row_Count].Style.Font.Bold = true;

                    worksheet1.Cells[2, 1, 2, Row_Count].AutoFilter = true; // Set Filter to header

                    var cellBackgroundColor1 = worksheet1.Cells[2, 1, 2, Row_Count].Style.Fill;
                    cellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    cellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle cellStyleHeader1 = worksheet1.Cells[2, 1, 2, Row_Count].Style;
                    cellStyleHeader1.Border.Left.Style = cellStyleHeader1.Border.Right.Style
                            = cellStyleHeader1.Border.Top.Style = cellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet1.View.FreezePanes(3, 1);
                    worksheet1.Cells[inStartIndex, 1, inEndCounter, Row_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var asTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (i = inStartIndex; i < inEndCounter; i++)
                    {
                        #region Assigns Value to Cell
                        int kk = 0;

                        string cutValue = Convert.ToString(dt.Rows[i - inStartIndex]["Cut"]);

                        for (int j = 0; j < column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                            {
                                kk += 1;

                                if (Column_Name == "Lab")
                                {
                                    string labValue = Convert.ToString(dt.Rows[i - inStartIndex]["Lab"]);
                                    string certificateURL = Convert.ToString(dt.Rows[i - inStartIndex]["CERTIFICATE LINK"]);

                                    if (!string.IsNullOrEmpty(certificateURL) && !string.IsNullOrEmpty(labValue))
                                    {
                                        worksheet1.Cells[inwrkrow, kk].Value = labValue;
                                        worksheet1.Cells[inwrkrow, kk].Formula = "=HYPERLINK(\"" + certificateURL + "\",\"" + labValue + "\")";
                                        worksheet1.Cells[inwrkrow, kk].Style.Font.UnderLine = true;
                                        worksheet1.Cells[inwrkrow, kk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Value")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Final Disc")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Supplier Final Disc(%)")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Final Value")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(yellow);
                                }
                                else if (Column_Name == "Supplier Final Value")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Supplier Per Carat")
                                {
                                    string pav_Height = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : 0;
                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Stone Clarity")
                                {
                                    worksheet1.Cells[inwrkrow, kk].Value = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);

                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet1.Cells[inwrkrow, kk].Style.Fill.BackgroundColor.SetColor(mustard_yellow);
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet1.Cells[inwrkrow, kk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet1.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish")
                                {
                                    worksheet1.Cells[inwrkrow, kk].Value = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet1.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Symm")
                                {
                                    worksheet1.Cells[inwrkrow, kk].Value = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet1.Cells[inwrkrow, kk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Length")
                                {
                                    string Length = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Length) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Width")
                                {
                                    string Width = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Width) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth")
                                {
                                    string Depth = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(Depth) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Depth (%)")
                                {
                                    string pepth_per = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(pepth_per) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Table (%)")
                                {
                                    string table_Per = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(table_Per) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else if (Column_Name == "Girdle (%)")
                                {
                                    string girdle_Per = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                    worksheet1.Cells[inwrkrow, kk].Value = !string.IsNullOrEmpty(girdle_Per) ? Convert.ToDouble(dt.Rows[i - inStartIndex][Column_Name]) : DBNull.Value;

                                    worksheet1.Cells[inwrkrow, kk].Style.Numberformat.Format = "0.00";
                                }
                                else
                                {
                                    worksheet1.Cells[inwrkrow, kk].Value = Convert.ToString(dt.Rows[i - inStartIndex][Column_Name]);
                                }
                            }
                        }

                        inwrkrow++;
                        #endregion
                    }
                    worksheet1.Cells[inStartIndex, 1, inwrkrow, Row_Count].Style.Font.Size = 9;

                    int kkk = 0;
                    for (int j = 0; j < column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(column_dt.Rows[j]["Column_Name"]);

                        kkk += 1;
                        if (Column_Name == "Supplier Stock ID")
                        {
                            worksheet1.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet1.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet1.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet1.Cells[1, kkk].Style.Numberformat.Format = "#,##";

                            ExcelStyle cellStyleHeader_Total = worksheet1.Cells[1, kkk].Style;
                            cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                    = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Cts")
                        {
                            worksheet1.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet1.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet1.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet1.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_Totalcarat = worksheet1.Cells[1, kkk].Style;
                            cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                    = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Rap Value")
                        {
                            worksheet1.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet1.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet1.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet1.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_RapAmt = worksheet1.Cells[1, kkk].Style;
                            cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                    = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Final Value")
                        {
                            worksheet1.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet1.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet1.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet1.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_FinalValue = worksheet1.Cells[1, kkk].Style;
                            cellStyleHeader_FinalValue.Border.Left.Style = cellStyleHeader_FinalValue.Border.Right.Style
                                    = cellStyleHeader_FinalValue.Border.Top.Style = cellStyleHeader_FinalValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Supplier Final Value")
                        {
                            worksheet1.Cells[1, kkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(kkk) + "" + inStartIndex + ":" + GetExcelColumnLetter(kkk) + "" + (inwrkrow - 1) + "),2)";
                            worksheet1.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet1.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet1.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_SupplierFinalValue = worksheet1.Cells[1, kkk].Style;
                            cellStyleHeader_SupplierFinalValue.Border.Left.Style = cellStyleHeader_SupplierFinalValue.Border.Right.Style
                                    = cellStyleHeader_SupplierFinalValue.Border.Top.Style = cellStyleHeader_SupplierFinalValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Supplier Per Carat")
                        {
                            int ctsColumnIndex = GetColumnIndexByName(worksheet1, "Cts");
                            int supplierFinalValueColumnIndex = GetColumnIndexByName(worksheet1, "Supplier Final Value");

                            if (ctsColumnIndex > 0 && supplierFinalValueColumnIndex > 0)
                            {
                                worksheet1.Cells[1, kkk].Formula = "=" + GetExcelColumnLetter(supplierFinalValueColumnIndex) + "1/" + GetExcelColumnLetter(ctsColumnIndex) + "1";

                                worksheet1.Cells[1, kkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet1.Cells[1, kkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet1.Cells[1, kkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_SupplierPerCarat = worksheet1.Cells[1, kkk].Style;
                                cellStyleHeader_SupplierPerCarat.Border.Left.Style = cellStyleHeader_SupplierPerCarat.Border.Right.Style
                                        = cellStyleHeader_SupplierPerCarat.Border.Top.Style = cellStyleHeader_SupplierPerCarat.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                    }

                    int rowEnd = worksheet1.Dimension.End.Row;
                    removingGreenTagWarning(worksheet1, worksheet1.Cells[1, 1, rowEnd, 100].Address);

                    int totalColumns = worksheet1.Dimension.End.Column;

                    if (totalColumns > 1)
                    {
                        worksheet1.DeleteColumn(totalColumns, totalColumns);
                    }
                    
                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public static void Create_Purchase_Detail_Pricing_Excel(DataTable pricing_dt, DataTable pricing_column_dt, string _strFolderPath, string _strFilePath)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    Color colFromHex = ColorTranslator.FromHtml("#d3d3d3");
                    Color yellow = ColorTranslator.FromHtml("#FFFF00");
                    Color pink = ColorTranslator.FromHtml("#FFCCFF");
                    Color mustard_yellow = ColorTranslator.FromHtml("#FFD700");
                    Color colFromHexTotal = ColorTranslator.FromHtml("#d9e1f2");
                    Color light_yellow = ColorTranslator.FromHtml("#FFF2E0");
                    Color sky_blue = ColorTranslator.FromHtml("#E0FFFF");

                    int pRow_Count = pricing_column_dt.Rows.Count;
                    int pinStartIndex = 3;
                    int pinwrkrow = 3;
                    int pinEndCounter = pricing_dt.Rows.Count + pinStartIndex;
                    int pTotalRow = pricing_dt.Rows.Count;
                    int pi;

                    #region Company Detail on Header
                    ep.Workbook.Worksheets.Add("Sheet1");

                    ExcelWorksheet worksheet = ep.Workbook.Worksheets[0];

                    worksheet.Name = "Pricing Layout";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.Font.Name = "Calibri";

                    worksheet.Row(1).Height = 40;
                    worksheet.Row(2).Height = 40;
                    worksheet.Row(2).Style.WrapText = true;
                    #endregion

                    #region Header Name Declaration
                    int pk = 0;
                    for (int j = 0; j < pricing_column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(pricing_column_dt.Rows[j]["Column_Name"]);

                        pk += 1;
                        worksheet.Cells[2, pk].Value = Column_Name;
                        worksheet.Cells[2, pk].AutoFitColumns(10);

                    }

                    worksheet.Cells[1, 1, 1, pRow_Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.Font.Size = 11;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, pRow_Count].Style.Font.Size = 11;

                    worksheet.Cells[2, 1, 2, pRow_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1, 2, pRow_Count].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    worksheet.Cells[2, 1, 2, pRow_Count].Style.Font.Size = 10;
                    worksheet.Cells[2, 1, 2, pRow_Count].Style.Font.Bold = true;

                    worksheet.Cells[2, 1, 2, pRow_Count].AutoFilter = true;

                    var pcellBackgroundColor1 = worksheet.Cells[2, 1, 2, pRow_Count].Style.Fill;
                    pcellBackgroundColor1.PatternType = ExcelFillStyle.Solid;
                    pcellBackgroundColor1.BackgroundColor.SetColor(colFromHex);

                    ExcelStyle pcellStyleHeader1 = worksheet.Cells[2, 1, 2, pRow_Count].Style;
                    pcellStyleHeader1.Border.Left.Style = pcellStyleHeader1.Border.Right.Style
                            = pcellStyleHeader1.Border.Top.Style = pcellStyleHeader1.Border.Bottom.Style
                            = ExcelBorderStyle.Medium;
                    #endregion

                    #region Set AutoFit and Decimal Number Format
                    worksheet.View.FreezePanes(3, 1);
                    worksheet.Cells[pinStartIndex, 1, pinEndCounter, pRow_Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    var pasTitleCase = Thread.CurrentThread.CurrentCulture.TextInfo;

                    for (pi = pinStartIndex; pi < pinEndCounter; pi++)
                    {
                        #region Assigns Value to Cell
                        int pkk = 0;

                        string cutValue = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["Cut"]);

                        for (int j = 0; j < pricing_column_dt.Rows.Count; j++)
                        {
                            string Column_Name = Convert.ToString(pricing_column_dt.Rows[j]["Column_Name"]);
                            {
                                pkk += 1;

                                if (Column_Name == "Lab")
                                {
                                    string labValue = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["Lab"]);
                                    string certificateURL = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["CERTI"]);

                                    if (!string.IsNullOrEmpty(certificateURL) && !string.IsNullOrEmpty(labValue))
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Value = labValue;
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "=HYPERLINK(\"" + certificateURL + "\",\"" + labValue + "\")";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.UnderLine = true;
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Image")
                                {
                                    string Image_URL = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["Image"]);

                                    if (!string.IsNullOrEmpty(Image_URL))
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "=HYPERLINK(\"" + Image_URL + "\",\" Image \")";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.UnderLine = true;
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Color.SetColor(Color.Blue);
                                    }

                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Video")
                                {
                                    string Video_URL = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex]["VIDEO"]);

                                    if (!string.IsNullOrEmpty(Video_URL))
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "=HYPERLINK(\"" + Video_URL + "\",\" Movie \")";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.UnderLine = true;
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Color.SetColor(Color.Blue);
                                    }
                                }
                                else if (Column_Name == "Cts")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                }
                                else if (Column_Name == "Rap Rate" || Column_Name == "Rap Value")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                    
                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Final Disc")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(yellow);

                                    worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                }
                                else if (Column_Name == "Final Value")
                                {
                                    int FinalColumnIndex = GetColumnIndexByName(worksheet, "Final Disc");
                                    int RapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");

                                    if (FinalColumnIndex > 0 && RapColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR(((100 - " + GetExcelColumnLetter(FinalColumnIndex) + pi + ") * " + GetExcelColumnLetter(RapColumnIndex) + pi + ")/100,0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(yellow);

                                    worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                }
                                else if (Column_Name == "Profit (%)")
                                {
                                    int ProfitColumnIndex = GetColumnIndexByName(worksheet, "Profit Value");
                                    int SupplierColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                                    if (ProfitColumnIndex > 0 && SupplierColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR((" + GetExcelColumnLetter(ProfitColumnIndex) + pi + " / " + GetExcelColumnLetter(SupplierColumnIndex) + pi + ")*100,0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(light_yellow);
                                }
                                else if (Column_Name == "Profit Value")
                                {
                                    int FinalColumnIndex = GetColumnIndexByName(worksheet, "Final Value");
                                    int SupplierColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                                    if (FinalColumnIndex > 0 && SupplierColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR((" + GetExcelColumnLetter(FinalColumnIndex) + pi + " - " + GetExcelColumnLetter(SupplierColumnIndex) + pi + "),0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(light_yellow);
                                }
                                else if (Column_Name == "Supplier Final Disc(%)")
                                {
                                    int SupplierColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");
                                    int RapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");

                                    if (RapColumnIndex > 0 && SupplierColumnIndex > 0)
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Formula = "IFERROR((1 - (" + GetExcelColumnLetter(SupplierColumnIndex) + pi + " / " + GetExcelColumnLetter(RapColumnIndex) + pi + ")) * 100,0)";
                                        worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";
                                    }

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "Supplier Final Value")
                                {
                                    string pav_Height = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(pav_Height) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : 0;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "#,##0.00";

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(pink);
                                }
                                else if (Column_Name == "KTS Grade" || Column_Name == "Com" || Column_Name == "Zone" || Column_Name == "Sub Zone" || Column_Name == "P.Grade" || Column_Name == "FMS" || Column_Name == "Bullet" || Column_Name == "Stock Disc (%)" || Column_Name == "Stock Pcs" || Column_Name == "Avg Pur Disc (%)" || Column_Name == "Avg Pur Pcs" || Column_Name == "Sales Disc (%)" || Column_Name == "Sales Pcs" || Column_Name == "Raptop" || Column_Name == "Rapnet Days")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                             
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(sky_blue);
                                }
                                else if (Column_Name == "Sunrise Grade")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                             
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(sky_blue);
                                    
                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Source Party")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                    worksheet.Column(pkk).Hidden = true;
                                }
                                else if (Column_Name == "Stone Clarity")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Fill.BackgroundColor.SetColor(mustard_yellow);
                                }
                                else if (Column_Name == "Cut")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = cutValue;

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Polish" || Column_Name == "Symm")
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    if (cutValue == "3EX")
                                    {
                                        worksheet.Cells[pinwrkrow, pkk].Style.Font.Bold = true;
                                    }
                                }
                                else if (Column_Name == "Length" || Column_Name == "Width" || Column_Name == "Depth" || Column_Name == "Depth (%)" || Column_Name == "Table (%)" || Column_Name == "Girdle (%)" || Column_Name == "Crown Angle" || Column_Name == "Crown Height" || Column_Name == "Pav Angle" || Column_Name == "Pav Height")
                                {
                                    string value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);

                                    worksheet.Cells[pinwrkrow, pkk].Value = !string.IsNullOrEmpty(value) ? Convert.ToDouble(pricing_dt.Rows[pi - pinStartIndex][Column_Name]) : DBNull.Value;
                                    worksheet.Cells[pinwrkrow, pkk].Style.Numberformat.Format = "0.00";
                                }
                                else
                                {
                                    worksheet.Cells[pinwrkrow, pkk].Value = Convert.ToString(pricing_dt.Rows[pi - pinStartIndex][Column_Name]);
                                }
                            }
                        }

                        pinwrkrow++;
                        #endregion
                    }
                    worksheet.Cells[pinStartIndex, 1, pinwrkrow, pRow_Count].Style.Font.Size = 9;

                    int pkkk = 0;
                    for (int j = 0; j < pricing_column_dt.Rows.Count; j++)
                    {
                        string Column_Name = Convert.ToString(pricing_column_dt.Rows[j]["Column_Name"]);

                        pkkk += 1;
                        if (Column_Name == "Supplier Stock ID")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(103," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##";

                            ExcelStyle cellStyleHeader_Total = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_Total.Border.Left.Style = cellStyleHeader_Total.Border.Right.Style
                                    = cellStyleHeader_Total.Border.Top.Style = cellStyleHeader_Total.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Cts")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_Totalcarat = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_Totalcarat.Border.Left.Style = cellStyleHeader_Totalcarat.Border.Right.Style
                                    = cellStyleHeader_Totalcarat.Border.Top.Style = cellStyleHeader_Totalcarat.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Rap Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_RapAmt = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_RapAmt.Border.Left.Style = cellStyleHeader_RapAmt.Border.Right.Style
                                    = cellStyleHeader_RapAmt.Border.Top.Style = cellStyleHeader_RapAmt.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Final Disc")
                        {
                            int finalColumnIndex = GetColumnIndexByName(worksheet, "Final Value");
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");

                            if (finalColumnIndex > 0 && rapColumnIndex > 0)
                            {
                                worksheet.Cells[1, pkkk].Formula =
                                                                    "=IFERROR(ROUND((100-(SUBTOTAL(109, " + GetExcelColumnLetter(finalColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(finalColumnIndex) + (pinwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (pinwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, pkkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Final Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_FinalValue = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_FinalValue.Border.Left.Style = cellStyleHeader_FinalValue.Border.Right.Style
                                    = cellStyleHeader_FinalValue.Border.Top.Style = cellStyleHeader_FinalValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Profit (%)")
                        {
                            int profitColumnIndex = GetColumnIndexByName(worksheet, "Profit Value");
                            int supplierFinalValueColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                            if (profitColumnIndex > 0 && supplierFinalValueColumnIndex > 0)
                            {
                                worksheet.Cells[1, pkkk].Formula =
                                                                    "=IFERROR(ROUND((SUBTOTAL(109, " + GetExcelColumnLetter(profitColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(profitColumnIndex) + (pinwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(supplierFinalValueColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(supplierFinalValueColumnIndex) + (pinwrkrow - 1) +
                                                                    ")) * 100, 2), 0.00)";

                                worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, pkkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Profit Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_ProfitValue = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_ProfitValue.Border.Left.Style = cellStyleHeader_ProfitValue.Border.Right.Style
                                    = cellStyleHeader_ProfitValue.Border.Top.Style = cellStyleHeader_ProfitValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                        else if (Column_Name == "Supplier Final Disc(%)")
                        {
                            int rapColumnIndex = GetColumnIndexByName(worksheet, "Rap Value");
                            int supplierFinalValueColumnIndex = GetColumnIndexByName(worksheet, "Supplier Final Value");

                            if (rapColumnIndex > 0 && supplierFinalValueColumnIndex > 0)
                            {
                                worksheet.Cells[1, pkkk].Formula =
                                                                    "=IFERROR(ROUND((100-(SUBTOTAL(109, " + GetExcelColumnLetter(supplierFinalValueColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(supplierFinalValueColumnIndex) + (pinwrkrow - 1) +
                                                                    ") / SUBTOTAL(109, " + GetExcelColumnLetter(rapColumnIndex) + pinStartIndex +
                                                                    ":" + GetExcelColumnLetter(rapColumnIndex) + (pinwrkrow - 1) +
                                                                    ")) * 100), 2), 0.00)";

                                worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                                worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                                ExcelStyle cellStyleHeader_Profit = worksheet.Cells[1, pkkk].Style;
                                cellStyleHeader_Profit.Border.Left.Style = cellStyleHeader_Profit.Border.Right.Style
                                        = cellStyleHeader_Profit.Border.Top.Style = cellStyleHeader_Profit.Border.Bottom.Style
                                        = ExcelBorderStyle.Medium;
                            }
                        }
                        else if (Column_Name == "Supplier Final Value")
                        {
                            worksheet.Cells[1, pkkk].Formula = "ROUND(SUBTOTAL(109," + GetExcelColumnLetter(pkkk) + "" + pinStartIndex + ":" + GetExcelColumnLetter(pkkk) + "" + (pinwrkrow - 1) + "),2)";
                            worksheet.Cells[1, pkkk].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, pkkk].Style.Fill.BackgroundColor.SetColor(colFromHexTotal);
                            worksheet.Cells[1, pkkk].Style.Numberformat.Format = "#,##0.00";

                            ExcelStyle cellStyleHeader_SupplierFinalValue = worksheet.Cells[1, pkkk].Style;
                            cellStyleHeader_SupplierFinalValue.Border.Left.Style = cellStyleHeader_SupplierFinalValue.Border.Right.Style
                                    = cellStyleHeader_SupplierFinalValue.Border.Top.Style = cellStyleHeader_SupplierFinalValue.Border.Bottom.Style
                                    = ExcelBorderStyle.Medium;
                        }
                    }

                    int prowEnd = worksheet.Dimension.End.Row;
                    removingGreenTagWarning(worksheet, worksheet.Cells[1, 1, prowEnd, 100].Address);

                    int ptotalColumns = worksheet.Dimension.End.Column;

                    if (ptotalColumns > 1)
                    {
                        worksheet.DeleteColumn(ptotalColumns, ptotalColumns);
                    }
                    
                    Byte[] bin = ep.GetAsByteArray();

                    if (!Directory.Exists(_strFolderPath))
                    {
                        Directory.CreateDirectory(_strFolderPath);
                    }

                    File.WriteAllBytes(_strFilePath, bin);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private static int GetColumnIndexByName(ExcelWorksheet worksheet, string columnName)
        {
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                if (worksheet.Cells[2, col].Text.Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return col;
                }
            }
            return -1;
        }
    }
}