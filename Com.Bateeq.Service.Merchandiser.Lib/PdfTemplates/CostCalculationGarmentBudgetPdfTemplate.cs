using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates
{
    public class CostCalculationGarmentBudgetPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(CostCalculationGarmentViewModel viewModel)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
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
            PdfPCell cell_colon = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Phrase = new Phrase(":", normal_font) };

            #region Header
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT. EFRATA RETAILINDO", 10, 820, 0);
            cb.SetFontAndSize(bf_bold, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "BUDGET PRODUCTION", 10, 805, 0);
            cb.EndText();
            #endregion

            #region Detail 1 (Top)
            PdfPTable table_detail1 = new PdfPTable(9);
            table_detail1.TotalWidth = 570f;

            float[] detail1_widths = new float[] { 1f, 0.1f, 2f, 1f, 0.1f, 2f, 1.5f, 0.1f, 2f };
            table_detail1.SetWidths(detail1_widths);

            PdfPCell cell_detail1 = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 1, PaddingBottom = 2, PaddingTop = 2 };

            cell_detail1.Phrase = new Phrase("RO", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.RO}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("SECTION", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.Section}", normal_font);
            table_detail1.AddCell(cell_detail1);
            cell_detail1.Phrase = new Phrase("CONFIRM ORDER", normal_font);
            table_detail1.AddCell(cell_detail1);
            table_detail1.AddCell(cell_colon);
            cell_detail1.Phrase = new Phrase($"{viewModel.ConfirmDate.ToString("dd/MM/yyyy")}", normal_font);
            table_detail1.AddCell(cell_detail1);
            #endregion

            #region Draw Detail 1
            float row1Y = 800;
            table_detail1.WriteSelectedRows(0, -1, 10, row1Y, cb);
            #endregion

            bool isDollar = viewModel.Rate.Id != 0;

            #region Detail 2 (Bottom, Column 1)
            PdfPTable table_detail2 = new PdfPTable(2);
            table_detail2.TotalWidth = 230f;

            float[] detail2_widths = new float[] { 2f, 5f };
            table_detail2.SetWidths(detail2_widths);

            PdfPCell cell_detail2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 2, PaddingBottom = 7, PaddingLeft = 2, PaddingTop = 7 };

            cell_detail2.Phrase = new Phrase("BUYER", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Buyer.Name}", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("ARTICLE", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Article}", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("DESCRIPTION", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Description}", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("QTY", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.Quantity} PCS", normal_font);
            table_detail2.AddCell(cell_detail2);

            cell_detail2.Phrase = new Phrase("DELIVERY", normal_font);
            table_detail2.AddCell(cell_detail2);
            cell_detail2.Phrase = new Phrase($"{viewModel.DeliveryDate.ToString("dd/MM/yyyy")}", normal_font);
            table_detail2.AddCell(cell_detail2);
            #endregion

            #region Signature
            PdfPTable table_signature = new PdfPTable(5);
            table_signature.TotalWidth = 570f;

            float[] signature_widths = new float[] { 1f, 1f, 1f, 1f, 1f };
            table_signature.SetWidths(signature_widths);

            PdfPCell cell_signature = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };

            cell_signature.Phrase = new Phrase("Membuat,", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Mengetahui,", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Menyetujui,", normal_font);
            table_signature.AddCell(cell_signature);

            string signatureArea = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                signatureArea += Environment.NewLine;
            }

            cell_signature.Phrase = new Phrase(signatureArea, normal_font);
            table_signature.AddCell(cell_signature);
            table_signature.AddCell(cell_signature);
            table_signature.AddCell(cell_signature);
            table_signature.AddCell(cell_signature);
            table_signature.AddCell(cell_signature);

            cell_signature.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature);

            cell_signature.Phrase = new Phrase("Merchandiser", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Ka. Sie IE", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Ka. Sie Merchandiser", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Ka. Sie Pembelian", normal_font);
            table_signature.AddCell(cell_signature);
            
            cell_signature.Phrase = new Phrase("Direktur Marketing", normal_font);
            table_signature.AddCell(cell_signature);
            #endregion

            #region Cost Calculation Material
            PdfPTable table_ccm = new PdfPTable(10);
            table_ccm.TotalWidth = 570f;

            float[] ccm_widths = new float[] { 1f, 3f, 4f, 6f, 2f, 3f, 3f, 2f, 3f, 3f };
            table_ccm.SetWidths(ccm_widths);

            PdfPCell cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };

            cell_ccm.Phrase = new Phrase("NO", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("CATEGORIES", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("MATERIALS", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("DESCRIPTION", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("USAGE", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("PRICE", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("QUANTITY", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("UNIT", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("AMOUNT", bold_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm.Phrase = new Phrase("PO NUMBER", bold_font);
            table_ccm.AddCell(cell_ccm);

            float row2Y = row1Y - table_detail1.TotalHeight - 10;
            float row3Height = table_detail2.TotalHeight;
            float row2RemainingHeight = row2Y - 10 - row3Height - printedOnHeight - margin;
            float row2AllowedHeight = row2Y - printedOnHeight - margin;
            double totalBudget = 0;

            #region Process Cost
            double processCost = viewModel.ProductionCost;
            #endregion

            for (int i = 0; i < viewModel.CostCalculationGarment_Materials.Count; i++)
            {
                cell_ccm.Phrase = new Phrase((i + 1).ToString(), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.HorizontalAlignment = Element.ALIGN_LEFT;

                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].Category.SubCategory != null ? String.Format("{0} - {1}", viewModel.CostCalculationGarment_Materials[i].Category.Name, viewModel.CostCalculationGarment_Materials[i].Category.SubCategory) : viewModel.CostCalculationGarment_Materials[i].Category.Name, normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].Material.Name, normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].Description, normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.HorizontalAlignment = Element.ALIGN_RIGHT;

                double usage = viewModel.CostCalculationGarment_Materials[i].Quantity ?? 0;
                cell_ccm.Phrase = new Phrase(usage.ToString(), normal_font);
                table_ccm.AddCell(cell_ccm);

                double price = viewModel.CostCalculationGarment_Materials[i].Price ?? 0;
                cell_ccm.Phrase = new Phrase(String.Format("{0}/{1}", Number.ToRupiahWithoutSymbol(price), viewModel.CostCalculationGarment_Materials[i].UOMPrice.Name), normal_font);
                table_ccm.AddCell(cell_ccm);

                double factor;
                if (viewModel.CostCalculationGarment_Materials[i].Category.Name == "ACC")
                {
                    factor = viewModel.AccessoriesAllowance ?? 0;
                }
                else
                {
                    factor = viewModel.FabricAllowance ?? 0;
                }
                double totalQuantity = viewModel.Quantity ?? 0;
                double conversion = (double)viewModel.CostCalculationGarment_Materials[i].Conversion;
                double usageConversion = usage / conversion;
                double quantity = (100 + factor) / 100 * usageConversion * totalQuantity;

                quantity = Math.Ceiling(quantity);

                cell_ccm.Phrase = new Phrase(quantity.ToString(), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].UOMQuantity.Name, normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.HorizontalAlignment = Element.ALIGN_RIGHT;
                double amount = quantity * price;
                cell_ccm.Phrase = new Phrase(Number.ToRupiahWithoutSymbol(amount), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationGarment_Materials[i].PO, normal_font);
                table_ccm.AddCell(cell_ccm);

                totalBudget += amount;
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
            #endregion

            #region Detail 3 (Bottom, Column 2)
            PdfPTable table_detail3 = new PdfPTable(8);
            table_detail3.TotalWidth = 330f;

            float[] detail3_widths = new float[] { 3.25f, 4.75f, 1.9f, 0.2f, 1.9f, 1.9f, 0.2f, 1.9f };
            table_detail3.SetWidths(detail3_widths);

            //double budgetCost = isDollar ? viewModel.ConfirmPrice * viewModel.Rate.Value ?? 0 : viewModel.ConfirmPrice ?? 0;
            double totalProcessCost = processCost * (double) viewModel.Quantity;
            double budgetCost = totalBudget / (double) viewModel.Quantity;

            PdfPCell cell_detail3 = new PdfPCell() { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 2, PaddingBottom = 7, PaddingLeft = 2, PaddingTop = 7 };
            PdfPCell cell_detail3_right = new PdfPCell() { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 2, PaddingBottom = 7, PaddingLeft = 2, PaddingTop = 7 };
            PdfPCell cell_detail3_colspan6 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 2, PaddingBottom = 7, PaddingLeft = 2, PaddingTop = 7, Colspan = 6 };
            PdfPCell cell_detail3_colspan8 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 2, PaddingBottom = 7, PaddingLeft = 2, PaddingTop = 7, Colspan = 8 };

            cell_detail3.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER;
            cell_detail3.Phrase = new Phrase("TOTAL BUDGET", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiah(totalBudget)}", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3_colspan6.Phrase = new Phrase("STANDARD HOURS", normal_font);
            table_detail3.AddCell(cell_detail3_colspan6);

            double freightCost = 0;
            foreach (CostCalculationGarment_MaterialViewModel item in viewModel.CostCalculationGarment_Materials)
            {
                freightCost += item.TotalShippingFee;
            }

            cell_detail3.Border = Rectangle.LEFT_BORDER;
            cell_detail3.Phrase = new Phrase("BEA ANGKUT", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Border = Rectangle.RIGHT_BORDER;
            cell_detail3.Phrase = new Phrase($"{Number.ToRupiah(freightCost)}", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Border = Rectangle.LEFT_BORDER;
            cell_detail3.Phrase = new Phrase("SMV. CUT", normal_font);
            table_detail3.AddCell(cell_detail3);
            table_detail3.AddCell(cell_colon);
            cell_detail3.Border = Rectangle.NO_BORDER;
            cell_detail3.Phrase = new Phrase($"{viewModel.SMV_Cutting}", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Border = Rectangle.NO_BORDER;
            cell_detail3.Phrase = new Phrase("SMV. SEW", normal_font);
            table_detail3.AddCell(cell_detail3);
            table_detail3.AddCell(cell_colon);
            cell_detail3.Border = Rectangle.RIGHT_BORDER;
            cell_detail3.Phrase = new Phrase($"{viewModel.SMV_Sewing}", normal_font);
            table_detail3.AddCell(cell_detail3);

            cell_detail3.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            cell_detail3.Phrase = new Phrase("", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
            cell_detail3.Phrase = new Phrase("", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            cell_detail3.Phrase = new Phrase("SMV. FIN", normal_font);
            table_detail3.AddCell(cell_detail3);
            table_detail3.AddCell(cell_colon);
            cell_detail3.Border = Rectangle.BOTTOM_BORDER;
            cell_detail3.Phrase = new Phrase($"{viewModel.SMV_Finishing}", normal_font);
            table_detail3.AddCell(cell_detail3);
            cell_detail3.Border = Rectangle.BOTTOM_BORDER;
            cell_detail3.Phrase = new Phrase("SMV. TOT", normal_font);
            table_detail3.AddCell(cell_detail3);
            table_detail3.AddCell(cell_colon);
            cell_detail3.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
            cell_detail3.Phrase = new Phrase($"{viewModel.SMV_Total}", normal_font);
            table_detail3.AddCell(cell_detail3);


            cell_detail3_colspan8.Phrase = new Phrase("BUDGET COST / PCS" + "".PadRight(5) + $"{Number.ToRupiah(budgetCost)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan8);
            cell_detail3_colspan8.Phrase = new Phrase("PROCESS COST" + "".PadRight(5) + $"{Number.ToRupiah(processCost)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan8);
            cell_detail3_colspan8.Phrase = new Phrase("TOTAL PROCESS COST" + "".PadRight(5) + $"{Number.ToRupiah(totalProcessCost)}", normal_font);
            table_detail3.AddCell(cell_detail3_colspan8);

            double rateValue = (double)viewModel.Rate.Value;
            double confirmPrice = (double)viewModel.ConfirmPrice;

            cell_detail3_colspan8.Phrase = isDollar ? new Phrase($"US$ 1 = {Number.ToRupiah(rateValue)}" + "".PadRight(10) + $"CONFIRM PRICE : {Number.ToDollar(confirmPrice)} / PCS", normal_font) : new Phrase($"CONFIRM PRICE : {Number.ToRupiah(confirmPrice)} / PCS", normal_font);
            table_detail3.AddCell(cell_detail3_colspan8);
            cell_detail3_colspan8.Border = Rectangle.NO_BORDER;
            cell_detail3_colspan8.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_detail3_colspan8.Phrase = new Phrase($"ALLOWANCE >> FAB = {viewModel.FabricAllowance}%, ACC = {viewModel.AccessoriesAllowance}%", normal_font);
            table_detail3.AddCell(cell_detail3_colspan8);
            #endregion

            #region Draw Others
            table_ccm.WriteSelectedRows(0, -1, 10, row2Y, cb);

            float row3Y = row2Y - table_ccm.TotalHeight - 10;
            float row3RemainigHeight = row3Y - printedOnHeight - margin;
            if (row3RemainigHeight < row3Height)
            {
                this.DrawPrintedOn(now, bf, cb);
                row3Y = startY;
                document.NewPage();
            }

            table_detail2.WriteSelectedRows(0, -1, margin, row3Y, cb);

            table_detail3.WriteSelectedRows(0, -1, margin + table_detail2.TotalWidth + 10, row3Y, cb);

            float signatureY = row3Y - row3Height - 10;
            signatureY = signatureY - 50;
            float signatureRemainingHeight = signatureY - printedOnHeight - margin;
            if (signatureRemainingHeight < table_signature.TotalHeight)
            {
                this.DrawPrintedOn(now, bf, cb);
                signatureY = startY;
                document.NewPage();
            }
            table_signature.WriteSelectedRows(0, -1, margin, signatureY, cb);

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
