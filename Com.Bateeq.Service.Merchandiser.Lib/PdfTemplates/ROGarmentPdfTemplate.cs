using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Net;

namespace Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates
{
    public class ROGarmentPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(RO_GarmentViewModel viewModel)
        {
            //set pdf stream
            MemoryStream stream = new MemoryStream();
            Document document = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            //set content configuration
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font_8 = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font font_9 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
            DateTime now = DateTime.Now;
            float margin = 10;
            float printedOnHeight = 10;
            float startY = 840 - margin;

            #region Header
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT. EFRATA RETAILINDO", 10, 820, 0);
            cb.SetFontAndSize(bf_bold, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "RO PENJUALAN UMUM", 10, 805, 0);
            cb.EndText();
            #endregion

            #region Top
            PdfPTable table_top = new PdfPTable(9);
            float[] top_widths = new float[] { 1f, 0.1f, 2f, 1f, 0.1f, 2f, 1f, 0.1f, 2f };

            table_top.TotalWidth = 500f;
            table_top.SetWidths(top_widths);

            PdfPCell cell_top = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2
            };

            PdfPCell cell_colon = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            PdfPCell cell_top_keterangan = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2,
                Colspan = 7
            };

            cell_colon.Phrase = new Phrase(":", normal_font);
            cell_top.Phrase = new Phrase("NO RO", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.RO}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("SECTION", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.Section}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("DATE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.ConfirmDate.ToString("dd MMMM yyyy")}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("LINE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.Line.Name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("BUYER", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.Buyer.Name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("DELIVERY DATE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.DeliveryDate.ToString("dd MMMM yyyy")}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("DESC", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.Description}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("QUANTITY", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Total.ToString()}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("SIZE RANGE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationGarment.SizeRange.Name}", normal_font);
            table_top.AddCell(cell_top);

            byte[] imageByte;
            float imageHeight;
            try
            {
                imageByte = Convert.FromBase64String(Base64.GetBase64File(viewModel.CostCalculationGarment.ImageFile));
                Image image = Image.GetInstance(imgb: imageByte);

                if (image.Width > 60)
                {
                    float percentage = 0.0f;
                    percentage = 60 / image.Width;
                    image.ScalePercent(percentage * 100);
                }

                float imageY = 800 - image.ScaledHeight;
                imageHeight = image.ScaledHeight;
                image.SetAbsolutePosition(520, imageY);
                cb.AddImage(image, inlineImage: true);
            }
            catch (Exception)
            {
                imageHeight = 0;
            }


            float row1Y = 800;

            table_top.WriteSelectedRows(0, -1, 10, row1Y, cb);
            #endregion

            #region Table Fabric
            //Fabric title
            PdfPTable table_fabric_top = new PdfPTable(1);
            table_fabric_top.TotalWidth = 500f;

            float[] fabric_widths_top = new float[] { 5f };
            table_fabric_top.SetWidths(fabric_widths_top);

            PdfPCell cell_top_fabric = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2
            };

            cell_top_fabric.Phrase = new Phrase("FABRIC", bold_font);
            table_fabric_top.AddCell(cell_top_fabric);

            float row1Height = imageHeight > table_top.TotalHeight ? imageHeight : table_top.TotalHeight;
            float rowYTittleFab = row1Y - row1Height - 10;
            float allowedRow2Height = rowYTittleFab - printedOnHeight - margin;
            table_fabric_top.WriteSelectedRows(0, -1, 10, rowYTittleFab, cb);

            //Main fabric table
            PdfPTable table_fabric = new PdfPTable(5);
            table_fabric.TotalWidth = 500f;

            float[] fabric_widths = new float[] { 5f, 5f, 5f, 5f, 5f };
            table_fabric.SetWidths(fabric_widths);

            PdfPCell cell_fabric_center = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            PdfPCell cell_fabric_left = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            float rowYFab = rowYTittleFab - table_fabric_top.TotalHeight - 5;
            float allowedRow2HeightFab = rowYFab - printedOnHeight - margin;

            cell_fabric_center.Phrase = new Phrase("FABRIC", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("NAME", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("DESCRIPTION", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("QUANTITY", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("REMARK", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            foreach (var materialModel in viewModel.CostCalculationGarment.CostCalculationGarment_Materials)
            {
                if (materialModel.Category.Name == "FAB")
                {
                    cell_fabric_left.Phrase = new Phrase(materialModel.Category.SubCategory != null ? materialModel.Category.SubCategory : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Material.Name != null ? materialModel.Material.Name : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Description != null ? materialModel.Description : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Quantity.ToString() != null ? String.Format("{0} " + materialModel.UOMQuantity.Name, materialModel.Quantity.ToString()) : "0", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);
                }
            }
            table_fabric.WriteSelectedRows(0, -1, 10, rowYFab, cb);
            #endregion

            #region Table Accessories
            //Accessories Title
            PdfPTable table_acc_top = new PdfPTable(1);
            table_acc_top.TotalWidth = 500f;

            float[] acc_width_top = new float[] { 5f };
            table_acc_top.SetWidths(acc_width_top);

            PdfPCell cell_top_acc = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2
            };

            cell_top_acc.Phrase = new Phrase("ACCESSORIES", bold_font);
            table_acc_top.AddCell(cell_top_acc);

            float rowYTittleAcc = rowYFab - table_fabric.TotalHeight - 10;
            float allowedRow2HeightTopAcc = rowYTittleFab - printedOnHeight - margin;
            table_acc_top.WriteSelectedRows(0, -1, 10, rowYTittleAcc, cb);

            //Main Accessories Table
            PdfPTable table_accessories = new PdfPTable(5);
            table_accessories.TotalWidth = 500f;

            float[] accessories_widths = new float[] { 5f, 5f, 5f, 5f, 5f };
            table_accessories.SetWidths(accessories_widths);

            PdfPCell cell_acc_center = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            PdfPCell cell_acc_left = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            float rowYAcc = rowYTittleAcc - table_fabric_top.TotalHeight - 5;
            float allowedRow2HeightAcc = rowYAcc - printedOnHeight - margin;

            cell_acc_center.Phrase = new Phrase("ACCESSORIES", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("NAME", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("DESCRIPTION", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("QUANTITY", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("REMARK", bold_font);
            table_accessories.AddCell(cell_acc_center);

            foreach (var materialModel in viewModel.CostCalculationGarment.CostCalculationGarment_Materials)
            {
                if (materialModel.Category.Name == "ACC")
                {
                    cell_acc_left.Phrase = new Phrase(materialModel.Category.SubCategory != null ? materialModel.Category.SubCategory : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Material.Name != null ? materialModel.Material.Name : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Description != null ? materialModel.Description : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} " + materialModel.UOMQuantity.Name, materialModel.Quantity.ToString()) : "0", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);
                }
            }
            table_accessories.WriteSelectedRows(0, -1, 10, rowYAcc, cb);
            #endregion

            #region Table Ongkos
            //Ongkos Title
            PdfPTable table_ong_top = new PdfPTable(1);
            table_ong_top.TotalWidth = 500f;

            float[] ong_width_top = new float[] { 5f };
            table_ong_top.SetWidths(ong_width_top);

            PdfPCell cell_top_ong = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2
            };

            cell_top_ong.Phrase = new Phrase("ONGKOS", bold_font);
            table_ong_top.AddCell(cell_top_ong);

            float rowYTittleOng = rowYAcc - table_accessories.TotalHeight - 10;
            float allowedRow2HeightTopOng = rowYTittleOng - printedOnHeight - margin;


            //Main Table Ongkos
            PdfPTable table_budget = new PdfPTable(5);
            table_budget.TotalWidth = 500f;

            float[] budget_widths = new float[] { 5f, 5f, 5f, 5f, 5f };
            table_budget.SetWidths(budget_widths);
            var ongIndex = 0;

            PdfPCell cell_budget_center = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            PdfPCell cell_budget_left = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            float rowYBudget = rowYTittleOng - table_ong_top.TotalHeight - 5;
            float allowedRow2HeightBudget = rowYBudget - printedOnHeight - margin;

            cell_budget_center.Phrase = new Phrase("ONGKOS", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("NAME", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("DESCRIPTION", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("QUANTITY", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("REMARK", bold_font);
            table_budget.AddCell(cell_budget_center);

            foreach (var materialModel in viewModel.CostCalculationGarment.CostCalculationGarment_Materials)
            {
                if (materialModel.Category.Name == "ONG")
                {
                    cell_budget_left.Phrase = new Phrase(materialModel.Category.SubCategory != null ? materialModel.Category.SubCategory : "", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Material.Name != null ? materialModel.Material.Name : "", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Description != null ? materialModel.Description : "", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} " + materialModel.UOMQuantity.Name, materialModel.Quantity.ToString()) : "0", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    ongIndex++;
                }
            }

            if (ongIndex != 0)
            {
                table_budget.WriteSelectedRows(0, -1, 10, rowYBudget, cb);
                table_ong_top.WriteSelectedRows(0, -1, 10, rowYTittleOng, cb);
            }
            #endregion

            #region Table Size Breakdown
            //Title
            PdfPTable table_breakdown_top = new PdfPTable(1);
            table_breakdown_top.TotalWidth = 570f;

            float[] breakdown_width_top = new float[] { 5f };
            table_breakdown_top.SetWidths(breakdown_width_top);

            PdfPCell cell_top_breakdown = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2
            };

            cell_top_breakdown.Phrase = new Phrase("SIZE BREAKDOWN", bold_font);
            table_breakdown_top.AddCell(cell_top_breakdown);

            float rowYTittleBreakDown = rowYBudget - table_budget.TotalHeight - 10;
            float allowedRow2HeightBreakdown = rowYTittleBreakDown - printedOnHeight - margin;
            table_breakdown_top.WriteSelectedRows(0, -1, 10, rowYTittleBreakDown, cb);

            //Main Table Size Breakdown
            PdfPTable table_breakDown = new PdfPTable(2);
            table_breakDown.TotalWidth = 570f;

            float[] breakDown_widths = new float[] { 5f, 10f };
            table_breakDown.SetWidths(breakDown_widths);

            PdfPCell cell_breakDown_center = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            PdfPCell cell_breakDown_left = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            PdfPCell cell_breakDown_total = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            PdfPCell cell_breakDown_total_2 = new PdfPCell()
            {
                Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2
            };

            float rowYbreakDown = rowYTittleBreakDown - table_breakdown_top.TotalHeight - 5;
            float allowedRow2HeightBreakDown = rowYbreakDown - printedOnHeight - margin;
            var remainingRowToHeightBrekdown = rowYbreakDown - 5 - printedOnHeight - margin;

            cell_breakDown_center.Phrase = new Phrase("WARNA", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("SIZE RANGE", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            foreach (var productRetail in viewModel.RO_Garment_SizeBreakdowns)
            {
                if (productRetail.Total != 0)
                {
                    cell_breakDown_left.Phrase = new Phrase(productRetail.Color.name != null ? productRetail.Color.name : "", normal_font);
                    table_breakDown.AddCell(cell_breakDown_left);

                    PdfPTable table_breakDown_child = new PdfPTable(3);
                    table_breakDown_child.TotalWidth = 300f;

                    float[] breakDown_child_widths = new float[] { 5f, 5f, 5f };
                    table_breakDown_child.SetWidths(breakDown_child_widths);

                    PdfPCell cell_breakDown_child_center = new PdfPCell()
                    {
                        Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 2
                    };

                    PdfPCell cell_breakDown_child_left = new PdfPCell()
                    {
                        Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 2
                    };

                    cell_breakDown_child_center.Phrase = new Phrase("KETERANGAN", bold_font);
                    table_breakDown_child.AddCell(cell_breakDown_child_center);

                    cell_breakDown_child_center.Phrase = new Phrase("SIZE", bold_font);
                    table_breakDown_child.AddCell(cell_breakDown_child_center);

                    cell_breakDown_child_center.Phrase = new Phrase("QUANTITY", bold_font);
                    table_breakDown_child.AddCell(cell_breakDown_child_center);

                    foreach (var size in productRetail.RO_Garment_SizeBreakdown_Details)
                    {

                        cell_breakDown_child_left.Phrase = new Phrase(size.Information != null ? size.Information : "", normal_font);
                        table_breakDown_child.AddCell(cell_breakDown_child_left);

                        cell_breakDown_child_left.Phrase = new Phrase(size.Size.Name != null ? size.Size.Name : "", normal_font);
                        table_breakDown_child.AddCell(cell_breakDown_child_left);

                        cell_breakDown_child_left.Phrase = new Phrase(size.Quantity.ToString() != null ? size.Quantity.ToString() : "0", normal_font);
                        table_breakDown_child.AddCell(cell_breakDown_child_left);
                    }

                    cell_breakDown_child_left.Phrase = new Phrase(" ", bold_font);
                    table_breakDown_child.AddCell(cell_breakDown_child_left);

                    cell_breakDown_child_left.Phrase = new Phrase("TOTAL", bold_font);
                    table_breakDown_child.AddCell(cell_breakDown_child_left);

                    cell_breakDown_child_left.Phrase = new Phrase(productRetail.Total.ToString() != null ? productRetail.Total.ToString() : "0", normal_font);
                    table_breakDown_child.AddCell(cell_breakDown_child_left);

                    table_breakDown_child.WriteSelectedRows(0, -1, 10, 0, cb);

                    table_breakDown.AddCell(table_breakDown_child);
                }

                var tableBreakdownCurrentHeight = table_breakDown.TotalHeight;

                if (tableBreakdownCurrentHeight / remainingRowToHeightBrekdown > 1)
                {
                    if (tableBreakdownCurrentHeight / allowedRow2HeightBreakDown > 1)
                    {
                        PdfPRow headerRow = table_breakDown.GetRow(0);
                        PdfPRow lastRow = table_breakDown.GetRow(table_breakDown.Rows.Count - 1);
                        table_breakDown.DeleteLastRow();
                        table_breakDown.WriteSelectedRows(0, -1, 10, rowYbreakDown, cb);
                        table_breakDown.DeleteBodyRows();
                        this.DrawPrintedOn(now, bf, cb);
                        document.NewPage();
                        table_breakDown.Rows.Add(headerRow);
                        table_breakDown.Rows.Add(lastRow);
                        table_breakDown.CalculateHeights();
                        rowYbreakDown = startY;
                        remainingRowToHeightBrekdown = rowYbreakDown - 5 - printedOnHeight - margin;
                        allowedRow2HeightBreakDown = remainingRowToHeightBrekdown - printedOnHeight - margin;
                    }
                }
            }

            cell_breakDown_total_2.Phrase = new Phrase("TOTAL", bold_font);
            table_breakDown.AddCell(cell_breakDown_total_2);

            cell_breakDown_total_2.Phrase = new Phrase(viewModel.Total.ToString(), bold_font);
            table_breakDown.AddCell(cell_breakDown_total_2);

            table_breakDown.WriteSelectedRows(0, -1, 10, rowYbreakDown, cb);
            #endregion

            #region Table Instruksi
            //Title
            PdfPTable table_instruction = new PdfPTable(1);
            float[] instruction_widths = new float[] { 5f };

            table_instruction.TotalWidth = 500f;
            table_instruction.SetWidths(instruction_widths);

            PdfPCell cell_top_instruction = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2
            };

            PdfPCell cell_colon_instruction = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            PdfPCell cell_top_keterangan_instruction = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                PaddingRight = 1,
                PaddingBottom = 2,
                PaddingTop = 2,
                Colspan = 7
            };

            cell_top_instruction.Phrase = new Phrase("INSTRUCTION", normal_font);
            table_instruction.AddCell(cell_top_instruction);
            table_instruction.AddCell(cell_colon_instruction);
            cell_top_keterangan_instruction.Phrase = new Phrase($"{viewModel.Instruction}", normal_font);
            table_instruction.AddCell(cell_top_keterangan_instruction);

            float rowYInstruction = rowYbreakDown - table_breakDown.TotalHeight - 10;
            float allowedRow2HeightInstruction = rowYInstruction - printedOnHeight - margin;

            table_instruction.WriteSelectedRows(0, -1, 10, rowYInstruction, cb);
            #endregion

            #region RO Image
            var countImageRo = 0;
            byte[] roImage;

            foreach (var index in viewModel.ImagesFile)
            {
                countImageRo++;
            }

            
            float rowYRoImage = rowYInstruction - table_instruction.TotalHeight - 10;
            float imageRoHeight;

            if (countImageRo != 0)
            {
                if (countImageRo > 5)
                {
                    countImageRo = 5;
                }

                PdfPTable table_ro_image = new PdfPTable(countImageRo);
                float[] ro_widths = new float[countImageRo];

                for (var i = 0; i < countImageRo; i++)
                {
                    ro_widths.SetValue(5f, i);
                }

                if (countImageRo != 0)
                {
                    table_ro_image.SetWidths(ro_widths);
                }

                table_ro_image.TotalWidth = 570f;

                foreach (var imageFromRo in viewModel.ImagesFile)
                {
                    try
                    {
                        roImage = Convert.FromBase64String(Base64.GetBase64File(imageFromRo));
                    }
                    catch (Exception)
                    {
                        var webClient = new WebClient();
                        roImage = webClient.DownloadData("https://bateeqstorage.blob.core.windows.net/other/no-image.jpg");
                    }

                    Image images = Image.GetInstance(imgb: roImage);

                    if (images.Width > 60)
                    {
                        float percentage = 0.0f;
                        percentage = 60 / images.Width;
                        images.ScalePercent(percentage * 100);
                    }

                    PdfPCell imageCell = new PdfPCell(images);
                    imageCell.Border = 0;
                    table_ro_image.AddCell(imageCell);
                }

                PdfPCell cell_image = new PdfPCell()
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Padding = 2,
                };

                foreach (var name in viewModel.ImagesName)
                {
                    cell_image.Phrase = new Phrase(name, normal_font);
                    table_ro_image.AddCell(cell_image);
                }

                imageRoHeight = table_ro_image.TotalHeight;
                table_ro_image.WriteSelectedRows(0, -1, 10, rowYRoImage, cb);
            } else
            {
                imageRoHeight = 0;
            }
            #endregion

            #region Signature
            PdfPTable table_signature = new PdfPTable(6);
            table_signature.TotalWidth = 570f;

            float[] signature_widths = new float[] { 1f, 1f, 1f, 1f, 1f, 1f };
            table_signature.SetWidths(signature_widths);

            PdfPCell cell_signature = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2,
            };

            PdfPCell cell_signature_noted = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 2,
                PaddingTop = 50
            };

            cell_signature.Phrase = new Phrase("Dibuat", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Kasie Merchandiser", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("R & D", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Ka Produksi", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Mengetahui", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Menyetujui", normal_font);
            table_signature.AddCell(cell_signature);

            cell_signature_noted.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature_noted);
            cell_signature_noted.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature_noted);
            cell_signature_noted.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature_noted);
            cell_signature_noted.Phrase = new Phrase("(                           )", normal_font);
            table_signature.AddCell(cell_signature_noted);
            cell_signature_noted.Phrase = new Phrase("(Haenis Gunarto)", normal_font);
            table_signature.AddCell(cell_signature_noted);
            cell_signature_noted.Phrase = new Phrase("(Michelle Tjokrosaputro)", normal_font);
            table_signature.AddCell(cell_signature_noted);

            float table_signatureY = rowYRoImage - imageRoHeight - 10;
            table_signature.WriteSelectedRows(0, -1, 10, table_signatureY, cb);
            #endregion

            this.DrawPrintedOn(now, bf, cb);
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
