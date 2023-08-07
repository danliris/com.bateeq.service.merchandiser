using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates
{
    public class CostCalculationGarmentPdfTemplate
    {
        private string GetCurrencyValue(double value, bool isDollar)
        {
            if (isDollar)
            {
                return Number.ToDollar(value);
            }
            else
            {
                return Number.ToRupiah(value);
            }
        }

        public MemoryStream GeneratePdfTemplate(CostCalculationGarmentViewModel viewModel)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font_8 = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            DateTime now = DateTime.Now;

            Document document = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            float margin = 10;
            float printedOnHeight = 10;
            float startY = 840 - margin;

            #region Header
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT. EFRATA GARMINDO UTAMA", 10, 820, 0);
            cb.SetFontAndSize(bf_bold, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "COST CALCULATION PENJUALAN UMUM", 10, 805, 0);
            cb.EndText();
            #endregion

            #region Detail 1 (Top)
            PdfPTable table_detail1 = new PdfPTable(9);
            table_detail1.TotalWidth = 500f;

            float[] detail1_widths = new float[] { 1f, 0.1f, 2f, 1f, 0.1f, 2f, 1f, 0.1f, 2f };
            table_detail1.SetWidths(detail1_widths);

            PdfPCell cell_detail1 = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 1, PaddingBottom = 2, PaddingTop = 2 };
            PdfPCell cell_colon = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE };
            cell_colon.Phrase = new Phrase(":", normal_font);

            cell_detail1.Phrase = new Phrase("RO", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.RO}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("SIZE RANGE", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.SizeRange.Name}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("LEAD TIME", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.LeadTime}", normal_font);
            table_detail1.AddCell(cell_detail1);

            cell_detail1.Phrase = new Phrase("ARTICLE", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.Article}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("SECTION", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.Section}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("FABRIC", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.FabricAllowance}%", normal_font);
            table_detail1.AddCell(cell_detail1);

            cell_detail1.Phrase = new Phrase("DATE", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel._CreatedUtc.ToString("dd MMMM yyyy")}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("COMMODITY", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.Commodity}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("ACC", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.AccessoriesAllowance}%", normal_font);
            table_detail1.AddCell(cell_detail1);

            cell_detail1.Phrase = new Phrase("LINE", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.Line.Name}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_detail1);
            #endregion

            #region Image
            float imageHeight;
            try
            {
                byte[] imageByte = Convert.FromBase64String(Base64.GetBase64File(viewModel.ImageFile));
                Image image = Image.GetInstance(imgb: imageByte);
                if (image.Width > 60)
                {
                    float percentage = 0.0f;
                    percentage = 60 / image.Width;
                    image.ScalePercent(percentage * 100);
                }
                imageHeight = image.ScaledHeight;
                float imageY = 800 - imageHeight;
                image.SetAbsolutePosition(520, imageY);
                cb.AddImage(image, inlineImage: true);
            }
            catch (Exception)
            {
                imageHeight = 0;
            }
            #endregion

            #region Draw Top
            float row1Y = 800;
            table_detail1.WriteSelectedRows(0, -1, 10, row1Y, cb);
            #endregion

            bool isDollar = viewModel.Rate.Id != 0;

            #region Detail 2 (Bottom, Column 1)
            PdfPTable table_detail2 = new PdfPTable(2);
            table_detail2.TotalWidth = 180f;

            float[] detail2_widths = new float[] { 1f, 2f };
            table_detail2.SetWidths(detail2_widths);

            PdfPCell cell_detail2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 2, PaddingBottom = 7, PaddingLeft = 2, PaddingTop = 7 };

            cell_detail2.Phrase = new Phrase("QTY", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Quantity} PCS", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("DESCRIPTION", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Description}", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("CONT/STYLE", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Article}", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("BUYER", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Buyer.Name}", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("DELIVERY", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.DeliveryDate.ToString("dd/MM/yyyy")}", normal_font);
            table_detail2.AddCell(cell_detail2);
            #endregion

            #region Detail 3 (Bottom, Column 2)
            PdfPTable table_detail3 = new PdfPTable(3);
            table_detail3.TotalWidth = 190f;

            float[] detail3_widths = new float[] { 1.7f, 1f, 1.3f };
            table_detail3.SetWidths(detail3_widths);

            PdfPCell cell_detail3 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3 };

            PdfPCell cell_detail3_colspan2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3, Colspan = 2 };

            cell_detail3.Phrase = new Phrase("TOTAL (OL+Material)", normal_font);
            table_detail3.AddCell(cell_detail3);
            double total = 0;
            foreach (CostCalculationGarment_MaterialViewModel item in viewModel.CostCalculationGarment_Materials)
            {
                total += item.Total;
            }
            total += viewModel.ProductionCost;
            cell_detail3_colspan2.Phrase = new Phrase(Number.ToRupiahWithoutSymbol(total), normal_font);
            table_detail3.AddCell(cell_detail3_colspan2);

            cell_detail3.Phrase = new Phrase("OTL 1", normal_font);
            table_detail3.AddCell(cell_detail3);
            double OTL1CalculatedValue = viewModel.OTL1.CalculatedValue ?? 0;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(OTL1CalculatedValue)}", normal_font);
            table_detail3.AddCell(cell_detail3);
            double afterOTL1 = total + OTL1CalculatedValue;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(afterOTL1)}", normal_font);
            table_detail3.AddCell(cell_detail3);

            cell_detail3.Phrase = new Phrase("OTL 2", normal_font);
            table_detail3.AddCell(cell_detail3);
            double OTL2CalculatedValue = viewModel.OTL2.CalculatedValue ?? 0;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(OTL2CalculatedValue)}", normal_font);
            table_detail3.AddCell(cell_detail3);
            double afterOTL2 = afterOTL1 + OTL2CalculatedValue;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(afterOTL2)}", normal_font);
            table_detail3.AddCell(cell_detail3);

            cell_detail3.Phrase = new Phrase("RISK", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Phrase = new Phrase(String.Format("{0:0.00}%", viewModel.Risk), normal_font);
            table_detail3.AddCell(cell_detail3);
            double afterRisk = (100 + viewModel.Risk) * afterOTL2 / 100; ;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(afterRisk)}", normal_font);
            table_detail3.AddCell(cell_detail3);

            cell_detail3.Phrase = new Phrase("BEA ANGKUT", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(viewModel.FreightCost)}", normal_font);
            table_detail3.AddCell(cell_detail3);
            double afterFreightCost = afterRisk + viewModel.FreightCost;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(afterFreightCost)}", normal_font);
            table_detail3.AddCell(cell_detail3);

            cell_detail3.Phrase = new Phrase("SUB TOTAL", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3_colspan2.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(afterFreightCost)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan2);

            cell_detail3.Phrase = new Phrase("NET/FOB (%)", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Phrase = new Phrase(String.Format("{0:0.00}%", viewModel.NETFOBP), normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(viewModel.NETFOB)}", normal_font);
            table_detail3.AddCell(cell_detail3);

            //cell_detail3.Phrase = new Phrase("COMM (%)", normal_font);
            //table_detail3.AddCell(cell_detail3);
            //cell_detail3.Phrase = new Phrase(String.Format("{0:0.00}%", viewModel.CommissionPortion), normal_font);
            //table_detail3.AddCell(cell_detail3);
            //cell_detail3.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(viewModel.CommissionRate)}", normal_font);
            //table_detail3.AddCell(cell_detail3);

            cell_detail3.Phrase = new Phrase("CONFIRM PRICE", normal_font);
            table_detail3.AddCell(cell_detail3); ;
            double confirmPrice = viewModel.ConfirmPrice ?? 0 + viewModel.Rate.Value ?? 0;
            double confirmPriceWithRate = isDollar ? confirmPrice * viewModel.Rate.Value ?? 1 : confirmPrice;
            cell_detail3_colspan2.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(confirmPriceWithRate)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan2);

            ////cell_detail3.Phrase = new Phrase("PROCESS COST" + "".PadRight(5), normal_font);
            cell_detail3.Phrase = new Phrase("OL" + "".PadRight(5), normal_font);
            table_detail3.AddCell(cell_detail3);
            double processCost = viewModel.ProductionCost;
            ////cell_detail3_colspan2.Phrase = new Phrase($"{GetCurrencyValue(processCost, isDollar)}", normal_font);
            cell_detail3_colspan2.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(processCost)}", normal_font);
            //cell_detail3_colspan2.Phrase = new Phrase($"{(processCost)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan2);

            cell_detail3.Phrase = new Phrase("OTL1" + "".PadRight(5), normal_font);
            table_detail3.AddCell(cell_detail3);
            double NOTL1CalculatedValue = viewModel.OTL1.CalculatedValue ?? 0;
            cell_detail3_colspan2.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(NOTL1CalculatedValue)}", normal_font);
            //cell_detail3_colspan2.Phrase = new Phrase($"{(NOTL1CalculatedValue)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan2);

            cell_detail3.Phrase = new Phrase("OTL2" + "".PadRight(5), normal_font);
            table_detail3.AddCell(cell_detail3);
            double NOTL2CalculatedValue = viewModel.OTL2.CalculatedValue ?? 0;
            cell_detail3_colspan2.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(NOTL2CalculatedValue)}", normal_font);
            //cell_detail3_colspan2.Phrase = new Phrase($"{(NOTL2CalculatedValue)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan2);

            cell_detail3.Phrase = new Phrase("Total (OL+OTL1+OTL2)" + "".PadRight(5), normal_font);
            table_detail3.AddCell(cell_detail3);
            double totale = viewModel.ProductionCost + NOTL1CalculatedValue + NOTL2CalculatedValue;
            cell_detail3_colspan2.Phrase = new Phrase($"{Number.ToRupiahWithoutSymbol(totale)}", normal_font);
            //cell_detail3_colspan2.Phrase = new Phrase($"{(totale)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan2);
            #endregion
            #region Detail 4.1 (Bottom, Column 3.1)
            PdfPTable table_detail4_1 = new PdfPTable(2);
            table_detail4_1.TotalWidth = 180f;

            float[] detail4_1_widths = new float[] { 1f, 1f };
            table_detail4_1.SetWidths(detail4_1_widths);

            PdfPCell cell_detail4_1 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3 };

            cell_detail4_1.Phrase = new Phrase("FOB PRICE", bold_font);
            table_detail4_1.AddCell(cell_detail4_1);
            cell_detail4_1.Phrase = new Phrase("CMT PRICE", bold_font);
            table_detail4_1.AddCell(cell_detail4_1);

            double CM_Price = 0;
            foreach (CostCalculationGarment_MaterialViewModel item in viewModel.CostCalculationGarment_Materials)
            {
                CM_Price += item.CM_Price ?? 0;
            }
            double ConfirmPrice = viewModel.ConfirmPrice ?? 0;
            double CMT = CM_Price > 0 ? ConfirmPrice : 0;
            string CMT_Price = this.GetCurrencyValue(CMT, isDollar);
            double FOB = ConfirmPrice + CM_Price;
            string FOB_Price = this.GetCurrencyValue(FOB, isDollar);
            cell_detail4_1.Phrase = new Phrase($"{FOB_Price}", normal_font);
            table_detail4_1.AddCell(cell_detail4_1);
            cell_detail4_1.Phrase = new Phrase($"{CMT_Price}", normal_font);
            table_detail4_1.AddCell(cell_detail4_1);
            #endregion

            #region Detail 4.2 (Bottom, Column 3.2)
            PdfPTable table_detail4_2 = new PdfPTable(2);
            table_detail4_2.TotalWidth = 180f;

            float[] detail4_2_widths = new float[] { 1f, 1f };
            table_detail4_2.SetWidths(detail4_2_widths);

            PdfPCell cell_detail4_2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 3 };

            cell_detail4_2.Phrase = new Phrase("CNF PRICE", bold_font);
            table_detail4_2.AddCell(cell_detail4_2);
            cell_detail4_2.Phrase = new Phrase("CIF PRICE", bold_font);
            table_detail4_2.AddCell(cell_detail4_2);

            string CNF_Price = this.GetCurrencyValue(0, isDollar);
            cell_detail4_2.Phrase = new Phrase($"{CNF_Price}", normal_font);
            table_detail4_2.AddCell(cell_detail4_2);
            string CIF_Price = this.GetCurrencyValue(0, isDollar);
            cell_detail4_2.Phrase = new Phrase($"{CIF_Price}", normal_font);
            table_detail4_2.AddCell(cell_detail4_2);
            #endregion

            #region Detail 5 (Bottom, Column 3.3)
            PdfPTable table_detail5 = new PdfPTable(4);
            table_detail5.TotalWidth = 180f;

            float[] detail5_widths = new float[] { 1f, 1.25f, 1f, 1.25f };
            table_detail5.SetWidths(detail5_widths);
            PdfPCell cell_detail5 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3, Colspan = 2 };

            cell_detail5 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3, Colspan = 2 };
            cell_detail5.Phrase = new Phrase("FREIGHT", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3, Colspan = 2 };
            string freight = this.GetCurrencyValue(viewModel.Freight ?? 0, isDollar);
            cell_detail5.Phrase = new Phrase($"= {freight}", normal_font);
            table_detail5.AddCell(cell_detail5);

            cell_detail5 = new PdfPCell() { Border = Rectangle.LEFT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3, Colspan = 2 };
            cell_detail5.Phrase = new Phrase("INSURANCE", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3, Colspan = 2 };
            string insurance = this.GetCurrencyValue(viewModel.Insurance ?? 0, isDollar);
            cell_detail5.Phrase = new Phrase($"= {insurance}", normal_font);
            table_detail5.AddCell(cell_detail5);

            cell_detail5 = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3, Colspan = 2 };
            cell_detail5.Phrase = new Phrase("CONFIRM PRICE", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3, Colspan = 2 };
            string confirmPriceFOB = this.GetCurrencyValue(viewModel.ConfirmPrice ?? 0, isDollar);
            cell_detail5.Phrase = new Phrase($"= {confirmPriceFOB}", normal_font);
            table_detail5.AddCell(cell_detail5);

            cell_detail5 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase("SMV CUT", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase($"= {viewModel.SMV_Cutting}", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase("SMV SEW", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase($"= {viewModel.SMV_Sewing}", normal_font);
            table_detail5.AddCell(cell_detail5);

            cell_detail5 = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase("SMV FIN", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase($"= {viewModel.SMV_Finishing}", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase("SMV TOT", normal_font);
            table_detail5.AddCell(cell_detail5);
            cell_detail5 = new PdfPCell() { Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 5, PaddingRight = 3, PaddingBottom = 5, PaddingLeft = 3 };
            cell_detail5.Phrase = new Phrase($"= {viewModel.SMV_Total}", normal_font);
            table_detail5.AddCell(cell_detail5);
            #endregion

            #region Signature
            PdfPTable table_signature = new PdfPTable(2);
            table_signature.TotalWidth = 570f;

            float[] signature_widths = new float[] { 1f, 1f };
            table_signature.SetWidths(signature_widths);

            PdfPCell cell_signature = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };

            cell_signature.Phrase = new Phrase("Yg Membuat,", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Menyetujui,", normal_font);
            table_signature.AddCell(cell_signature);

            string signatureArea = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                signatureArea += Environment.NewLine;
            }
            cell_signature.Phrase = new Phrase(signatureArea, normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase(signatureArea, normal_font);
            table_signature.AddCell(cell_signature);

            cell_signature.Phrase = new Phrase("Merchandiser", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Direktur Penjualan", normal_font);
            table_signature.AddCell(cell_signature);
            #endregion

            #region Cost Calculation Material
            PdfPTable table_ccm = new PdfPTable(7);
            table_ccm.TotalWidth = 570f;

            float[] ccm_widths = new float[] { 1.25f, 3.5f, 4f, 9f, 3f, 4f, 4f };
            table_ccm.SetWidths(ccm_widths);

            PdfPCell cell_ccm_center = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
            PdfPCell cell_ccm_left = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
            PdfPCell cell_ccm_right = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };

            cell_ccm_center.Phrase = new Phrase("NO", bold_font);
            table_ccm.AddCell(cell_ccm_center);

            cell_ccm_center.Phrase = new Phrase("CATEGORIES", bold_font);
            table_ccm.AddCell(cell_ccm_center);

            cell_ccm_center.Phrase = new Phrase("MATERIALS", bold_font);
            table_ccm.AddCell(cell_ccm_center);

            cell_ccm_center.Phrase = new Phrase("DESCRIPTION", bold_font);
            table_ccm.AddCell(cell_ccm_center);

            cell_ccm_center.Phrase = new Phrase("QUANTITY", bold_font);
            table_ccm.AddCell(cell_ccm_center);

            cell_ccm_center.Phrase = new Phrase("RP. PTC/PC", bold_font);
            table_ccm.AddCell(cell_ccm_center);

            cell_ccm_center.Phrase = new Phrase("RP. TOTAL", bold_font);
            table_ccm.AddCell(cell_ccm_center);

            double Total = 0;
            float row1Height = imageHeight > table_detail1.TotalHeight ? imageHeight : table_detail1.TotalHeight;
            float row2Y = row1Y - row1Height - 10;
            float[] row3Heights = { table_detail2.TotalHeight, table_detail3.TotalHeight, table_detail4_1.TotalHeight + 10 + table_detail4_2.TotalHeight + 10 + table_detail5.TotalHeight };
            float dollarDetailHeight = 10;
            if (isDollar)
                row3Heights[2] += dollarDetailHeight;
            float row3Height = row3Heights.Max();
            float secondHighestRow3Height = row3Heights[1] > row3Heights[2] ? row3Heights[1] : row3Heights[2];
            bool signatureInsideRow3 = row3Heights.Max() == row3Heights[0] && row3Heights[0] - 10 - secondHighestRow3Height > table_signature.TotalHeight;
            float row2RemainingHeight = row2Y - 10 - row3Height - printedOnHeight - margin;
            float row2AllowedHeight = row2Y - printedOnHeight - margin;

            for (int i = 0; i < viewModel.CostCalculationGarment_Materials.Count; i++)
            {
                cell_ccm_center.Phrase = new Phrase((i + 1).ToString(), normal_font);
                table_ccm.AddCell(cell_ccm_center);

                cell_ccm_left.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].Category.SubCategory != null ? String.Format("{0} - {1}", viewModel.CostCalculationGarment_Materials[i].Category.Name, viewModel.CostCalculationGarment_Materials[i].Category.SubCategory) : viewModel.CostCalculationGarment_Materials[i].Category.Name, normal_font);
                table_ccm.AddCell(cell_ccm_left);

                cell_ccm_left.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].Material.Name, normal_font);
                table_ccm.AddCell(cell_ccm_left);

                cell_ccm_left.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].Description, normal_font);
                table_ccm.AddCell(cell_ccm_left);

                cell_ccm_right.Phrase = new Phrase(String.Format("{0} {1}", viewModel.CostCalculationGarment_Materials[i].Quantity, viewModel.CostCalculationGarment_Materials[i].UOMQuantity.Name), normal_font);
                table_ccm.AddCell(cell_ccm_right);

                cell_ccm_right.Phrase = new Phrase(String.Format("{0}/{1}", Number.ToRupiahWithoutSymbol(viewModel.CostCalculationGarment_Materials[i].Price), viewModel.CostCalculationGarment_Materials[i].UOMPrice.Name), normal_font);
                table_ccm.AddCell(cell_ccm_right);

                cell_ccm_right.Phrase = new Phrase(Number.ToRupiahWithoutSymbol(viewModel.CostCalculationGarment_Materials[i].Total), normal_font);
                table_ccm.AddCell(cell_ccm_right);

                Total += viewModel.CostCalculationGarment_Materials[i].Total;
                float currentHeight = table_ccm.TotalHeight;
                if (currentHeight / row2RemainingHeight > 1)
                {
                    if (currentHeight / row2AllowedHeight > 1)
                    {
                        PdfPRow headerRow = table_ccm.GetRow(0);
                        PdfPRow lastRow = table_ccm.GetRow(table_ccm.Rows.Count - 1);
                        table_ccm.DeleteLastRow();
                        table_ccm.WriteSelectedRows(0, -1, 10, row2Y, cb);
                        table_ccm.DeleteBodyRows();
                        this.DrawPrintedOn(now, bf, cb);
                        document.NewPage();
                        table_ccm.Rows.Add(headerRow);
                        table_ccm.Rows.Add(lastRow);
                        table_ccm.CalculateHeights();
                        row2Y = startY;
                        row2RemainingHeight = row2Y - 10 - row3Height - printedOnHeight - margin;
                        row2AllowedHeight = row2Y - printedOnHeight - margin;
                    }
                }
            }

            cell_ccm_right = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2, Colspan = 6 };
            cell_ccm_right.Phrase = new Phrase("TOTAL", bold_font_8);
            table_ccm.AddCell(cell_ccm_right);

            cell_ccm_right = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
            cell_ccm_right.Phrase = new Phrase(Number.ToRupiah(Total), bold_font_8);
            table_ccm.AddCell(cell_ccm_right);
            #endregion

            #region Draw Middle and Bottom
            table_ccm.WriteSelectedRows(0, -1, 10, row2Y, cb);

            float row3Y = row2Y - table_ccm.TotalHeight - 10;
            float row3RemainingHeight = row3Y - printedOnHeight - margin;
            if (row3RemainingHeight < row3Height)
            {
                this.DrawPrintedOn(now, bf, cb);
                row3Y = startY;
                document.NewPage();
            }

            table_detail2.WriteSelectedRows(0, -1, 10, row3Y, cb);

            table_detail3.WriteSelectedRows(0, -1, 200, row3Y, cb);

            table_detail4_1.WriteSelectedRows(0, -1, 400, row3Y, cb);

            float detail4_2Y = row3Y - table_detail4_1.TotalHeight - 10;
            table_detail4_2.WriteSelectedRows(0, -1, 400, detail4_2Y, cb);

            float noteY = detail4_2Y - table_detail4_2.TotalHeight;
            float table_detail5Y;
            if (isDollar)
            {
                noteY = noteY - 15;
                table_detail5Y = noteY - 5;
                cb.BeginText();
                cb.SetFontAndSize(bf, 7);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $"NOTE: 1 US$ = {Number.ToRupiah(viewModel.Rate.Value)}", 400, noteY, 0);
                cb.EndText();
            }
            else
            {
                table_detail5Y = noteY - 10;
            }
            table_detail5.WriteSelectedRows(0, -1, 400, table_detail5Y, cb);

            float table_signatureX;
            float table_signatureY;
            if (signatureInsideRow3)
            {
                table_signatureX = margin + table_detail2.TotalWidth + 10;
                table_signatureY = row3Y - row3Height + table_signature.TotalHeight;
                table_signature.TotalWidth = 390f;
            }
            else
            {
                table_signatureX = margin;
                table_signatureY = row3Y - row3Height - 10;
                float signatureRemainingHeight = table_signatureY - printedOnHeight - margin;
                if (signatureRemainingHeight < table_signature.TotalHeight)
                {
                    this.DrawPrintedOn(now, bf, cb);
                    table_signatureY = startY;
                    document.NewPage();
                }
            }
            table_signature.WriteSelectedRows(0, -1, table_signatureX, table_signatureY, cb);

            this.DrawPrintedOn(now, bf, cb);
            #endregion

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private void DrawPrintedOn(DateTime now, BaseFont bf, PdfContentByte cb)
        {
            cb.BeginText();
            cb.SetFontAndSize(bf, 6);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Printed on: " + now.ToString("dd/MM/yyyy | HH:mm"), 10, 10, 0);
            cb.EndText();
        }
    }
}
