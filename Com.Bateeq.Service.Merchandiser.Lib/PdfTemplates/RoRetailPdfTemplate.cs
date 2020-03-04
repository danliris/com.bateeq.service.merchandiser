using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates
{
    public class RoRetailPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(RO_RetailViewModel viewModel)
        {

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 6);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font_8 = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font font_6 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 6);
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
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT. EFRATA RETAILINDO", 10, 820, 0);
            cb.SetFontAndSize(bf_bold, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "RO RETAIL", 10, 805, 0);
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
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.RO}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("ARTICLE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.Article}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("STYLE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.Style.name}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("COUNTER", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.Counter.name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("COLOUR", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Color.name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("DELIVERY DATE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.DeliveryDate.ToString("dd MMMM yyyy")}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("RO QUANTITY", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top_keterangan.Phrase = new Phrase($"{viewModel.Total}", normal_font);
            table_top.AddCell(cell_top_keterangan);
            cell_top.Phrase = new Phrase("RO DESCRIPTION", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top_keterangan.Phrase = new Phrase(viewModel.CostCalculationRetail.Description ?? "", normal_font);
            table_top.AddCell(cell_top_keterangan);
            #endregion

            #region Image

            byte[] imageByte;
            float imageHeight;
            try
            {
                imageByte = Convert.FromBase64String(Base64.GetBase64File(viewModel.CostCalculationRetail.ImageFile));
                Image image = Image.GetInstance(imgb: imageByte);

                if (image.Width > 30)
                {
                    float percentage = 0.0f;
                    percentage = 30 / image.Width;
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
            #endregion

            #region Draw Top
            float row1Y = 800;
            table_top.WriteSelectedRows(0, -1, 10, row1Y, cb);
            #endregion

            #region Fabric Table Title

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
            #endregion

            #region Fabric Table
            PdfPTable table_fabric = new PdfPTable(5);
            table_fabric.TotalWidth = 500f;

            float[] fabric_widths = new float[] { 5f, 5f, 5f, 5f, 5f };
            table_fabric.SetWidths(fabric_widths);

            var fabIndex = 0;

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

            foreach (var materialModel in viewModel.CostCalculationRetail.CostCalculationRetail_Materials)
            {
                if (materialModel.Category.Name == "FAB")
                {
                    cell_fabric_left.Phrase = new Phrase(materialModel.Category.SubCategory != null ? materialModel.Category.SubCategory : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Material.Name != null ? materialModel.Material.Name : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Description != null ? materialModel.Description : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} " + materialModel.UOMQuantity.Name, materialModel.Quantity) : "0", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    fabIndex++;
                }
            }

            if (fabIndex != 0)
            {
                table_fabric_top.WriteSelectedRows(0, -1, 10, rowYTittleFab, cb);
                table_fabric.WriteSelectedRows(0, -1, 10, rowYFab, cb);
            }
            #endregion


            #region Accessoris Table Title

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
            #endregion

            #region Accessoris Table

            PdfPTable table_accessories = new PdfPTable(5);
            table_accessories.TotalWidth = 500f;

            float[] accessories_widths = new float[] { 5f, 5f, 5f, 5f, 5f };
            table_accessories.SetWidths(accessories_widths);

            var accIndex = 0;

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

            foreach (var materialModel in viewModel.CostCalculationRetail.CostCalculationRetail_Materials)
            {
                if (materialModel.Category.Name == "ACC")
                {
                    cell_acc_left.Phrase = new Phrase(materialModel.Category.SubCategory != null ? materialModel.Category.SubCategory : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Material.Name != null ? materialModel.Material.Name : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Description != null ? materialModel.Description : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} " + materialModel.UOMQuantity.Name, materialModel.Quantity) : "0", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);
                    accIndex++;
                }
            }

            if (accIndex != 0)
            {
                table_acc_top.WriteSelectedRows(0, -1, 10, rowYTittleAcc, cb);
                table_accessories.WriteSelectedRows(0, -1, 10, rowYAcc, cb);
            }
            #endregion

            #region Ongkos Table Title

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

            #endregion

            #region Ongkos Table

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

            foreach (var materialModel in viewModel.CostCalculationRetail.CostCalculationRetail_Materials)
            {
                if (materialModel.Category.Name == "ONG")
                {
                    cell_budget_left.Phrase = new Phrase(materialModel.Category.SubCategory != null ? materialModel.Category.SubCategory : "", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Material.Name != null ? materialModel.Material.Name : "", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Description != null ? materialModel.Description : "", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} " + materialModel.UOMQuantity.Name, materialModel.Quantity) : "0", normal_font);
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

            #region Size Breakdown Title

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

            if (ongIndex == 0)
            {
                rowYTittleBreakDown = rowYBudget;
            }

            table_breakdown_top.WriteSelectedRows(0, -1, 5, rowYTittleBreakDown, cb);
            #endregion

            #region == Table Size Breakdown ==
            var tableBreakdownColumn = 3;

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

            List<String> breakdownSizes = new List<string>();

            foreach (var size in viewModel.RO_Retail_SizeBreakdowns)
            {
                var sizes = size.SizeQuantity.Keys;

                foreach (var values in sizes)
                {
                    if (!breakdownSizes.Contains(values))
                    {
                        breakdownSizes.Add(values);
                        tableBreakdownColumn++;
                    }
                }
            }

            PdfPTable table_breakDown = new PdfPTable(tableBreakdownColumn);
            table_breakDown.TotalWidth = 570f;
            List<float> breakdownWidth = new List<float>();
            breakdownWidth.Add(1f);
            breakdownWidth.Add(3f);

            cell_breakDown_center.Phrase = new Phrase("STORE CODE", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("STORE", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            foreach (var size in breakdownSizes)
            {
                breakdownWidth.Add(1f);
                cell_breakDown_center.Phrase = new Phrase(size, bold_font);
                table_breakDown.AddCell(cell_breakDown_center);
            }

            breakdownWidth.Add(1f);
            cell_breakDown_center.Phrase = new Phrase("TOTAL", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            float[] breakdown_width = breakdownWidth.ToArray();
            table_breakDown.SetWidths(breakdown_width);

            foreach (var productRetail in viewModel.RO_Retail_SizeBreakdowns)
            {
                if (productRetail.Total != 0)
                {
                    cell_breakDown_left.Phrase = new Phrase(productRetail.Store.code != null ? productRetail.Store.code : "", normal_font);
                    table_breakDown.AddCell(cell_breakDown_left);

                    cell_breakDown_left.Phrase = new Phrase(productRetail.Store.name != null ? productRetail.Store.name : "", normal_font);
                    table_breakDown.AddCell(cell_breakDown_left);

                    foreach (var size in productRetail.SizeQuantity)
                    {
                        foreach (var sizeHeader in breakdownSizes)
                        {
                            if (size.Key == sizeHeader)
                            {
                                cell_breakDown_left.Phrase = new Phrase(size.Value.ToString() != null ? size.Value.ToString() : "0", normal_font);
                                table_breakDown.AddCell(cell_breakDown_left);
                            }
                        }
                    }

                    cell_breakDown_left.Phrase = new Phrase(productRetail.Total.ToString() != null ? productRetail.Total.ToString() : "0", normal_font);
                    table_breakDown.AddCell(cell_breakDown_left);


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

            }

            cell_breakDown_total.Phrase = new Phrase(" ", bold_font);
            table_breakDown.AddCell(cell_breakDown_total);

            cell_breakDown_total_2.Phrase = new Phrase("TOTAL", bold_font);
            table_breakDown.AddCell(cell_breakDown_total_2);

            foreach (var sizeTotal in breakdownSizes)
            {
                foreach (var sizeHeader in viewModel.SizeQuantityTotal)
                {
                    if (sizeHeader.Key == sizeTotal)
                    {
                        cell_breakDown_left.Phrase = new Phrase(sizeHeader.Value.ToString() != null ? sizeHeader.Value.ToString() : "0", normal_font);
                        table_breakDown.AddCell(cell_breakDown_left);
                    }
                }
            }
            cell_breakDown_left.Phrase = new Phrase(viewModel.Total.ToString() != null ? viewModel.Total.ToString() : "0", normal_font);
            table_breakDown.AddCell(cell_breakDown_left);
            table_breakDown.WriteSelectedRows(0, -1, 10, rowYbreakDown, cb);
            #endregion

            #region Table Instruksi

            PdfPTable table_instruction = new PdfPTable(1);
            float[] instruction_widths = new float[] { 400f };

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

            float rowYInstruction = rowYbreakDown - table_breakDown.TotalHeight - 5;
            float allowedRow2HeightInstruction = rowYInstruction - printedOnHeight - margin;
            var remainingRowToHeightInstruction = rowYInstruction - 5 - printedOnHeight - margin;
            var tableInstructionCurrentHeight = table_instruction.TotalHeight;

            if (remainingRowToHeightInstruction < 0)
            {
                remainingRowToHeightInstruction = remainingRowToHeightInstruction * -1;
            }

            if (allowedRow2HeightInstruction < 0)
            {
                allowedRow2HeightInstruction = allowedRow2HeightInstruction * -1;
            }

            if (tableInstructionCurrentHeight / remainingRowToHeightInstruction > 1)
            {
                if (tableInstructionCurrentHeight / allowedRow2HeightInstruction > 1)
                {
                    PdfPRow headerRow = table_instruction.GetRow(0);
                    PdfPRow lastRow = table_instruction.GetRow(table_instruction.Rows.Count - 1);
                    table_instruction.DeleteLastRow();
                    table_instruction.WriteSelectedRows(0, -1, 10, rowYInstruction, cb);
                    table_instruction.DeleteBodyRows();
                    this.DrawPrintedOn(now, bf, cb);
                    document.NewPage();
                    table_instruction.Rows.Add(headerRow);
                    table_instruction.Rows.Add(lastRow);
                    table_instruction.CalculateHeights();
                    rowYInstruction = startY;
                    remainingRowToHeightInstruction = rowYInstruction - 5 - printedOnHeight - margin;
                    allowedRow2HeightInstruction = remainingRowToHeightInstruction - printedOnHeight - margin;
                }
            }

            table_instruction.WriteSelectedRows(0, -1, 10, rowYInstruction, cb);
            #endregion

            #region RO Image
            var countImageRo = 0;
            byte[] roImage;

            if (viewModel.ImagesFile != null)
            {
                foreach (var index in viewModel.ImagesFile)
                {
                    if (!string.IsNullOrEmpty(index))
                    {
                        countImageRo++;
                    }
                }
            }


            float rowYRoImage = rowYInstruction - table_instruction.TotalHeight - 5;
            float imageRoHeight;
            var remainingRowToHeightRoImage = rowYRoImage - 5 - printedOnHeight - margin;
            float allowedRow2HeightRoImage = rowYRoImage - printedOnHeight - margin;
            PdfPTable table_ro_image = null;

            if (countImageRo != 0)
            {
                table_ro_image = new PdfPTable(8);
                table_ro_image.DefaultCell.Border = Rectangle.NO_BORDER;
                float[] ro_widths = new float[8];

                for (var i = 0; i < 8; i++)
                {
                    ro_widths.SetValue(5f, i);
                }

                table_ro_image.SetWidths(ro_widths);
                table_ro_image.TotalWidth = 570f;


                for (var i = 0; i < viewModel.ImagesFile.Count; i++)
                {
                    try
                    {
                        roImage = Convert.FromBase64String(Base64.GetBase64File(viewModel.ImagesFile[i]));
                    }
                    catch (Exception)
                    {
                        var webClient = new WebClient();
                        roImage = webClient.DownloadData("https://bateeqstorage.blob.core.windows.net/other/no-image.jpg");
                    }

                    Image images = Image.GetInstance(imgb: roImage);
                    var imageName = viewModel.ImagesName[i];

                    if (images.Width > 60)
                    {
                        float percentage = 0.0f;
                        percentage = 60 / images.Width;
                        images.ScalePercent(percentage * 100);
                    }

                    PdfPCell imageCell = new PdfPCell(images);
                    imageCell.Border = 0;
                    imageCell.Padding = 4;

                    PdfPCell nameCell = new PdfPCell();
                    nameCell.Border = 0;
                    nameCell.Padding = 4;

                    nameCell.Phrase = new Phrase(imageName, normal_font);
                    PdfPTable table_ro_name = new PdfPTable(1);
                    table_ro_name.DefaultCell.Border = Rectangle.NO_BORDER;

                    table_ro_name.AddCell(imageCell);
                    table_ro_name.AddCell(nameCell);

                    table_ro_name.CompleteRow();
                    table_ro_image.AddCell(table_ro_name);
                }

                table_ro_image.CompleteRow();

                var tableROImageCurrentHeight = table_ro_image.TotalHeight;

                if (tableROImageCurrentHeight / remainingRowToHeightRoImage > 1)
                {
                    if (tableROImageCurrentHeight / allowedRow2HeightRoImage > 1)
                    {
                        PdfPRow headerRow = table_ro_image.GetRow(0);
                        PdfPRow lastRow = table_ro_image.GetRow(table_ro_image.Rows.Count - 1);
                        table_ro_image.DeleteLastRow();
                        table_ro_image.WriteSelectedRows(0, -1, 10, rowYRoImage, cb);
                        table_ro_image.DeleteBodyRows();
                        this.DrawPrintedOn(now, bf, cb);
                        document.NewPage();
                        table_ro_image.Rows.Add(headerRow);
                        table_ro_image.Rows.Add(lastRow);
                        table_ro_image.CalculateHeights();
                        rowYRoImage = startY;
                        remainingRowToHeightRoImage = rowYRoImage - 5 - printedOnHeight - margin;
                        allowedRow2HeightRoImage = remainingRowToHeightRoImage - printedOnHeight - margin;
                    }
                }

                imageRoHeight = table_ro_image.TotalHeight;
                table_ro_image.WriteSelectedRows(0, -1, 10, rowYRoImage, cb);
            }
            else
            {
                imageRoHeight = 0;
            }

            #endregion

            #region Signature (Bottom, Column 1.2)

            PdfPTable table_signature = new PdfPTable(6);
            table_signature.TotalWidth = 570f;

            float[] signature_widths = new float[] { 1f, 1f, 1f, 1f, 1f, 1f };
            float rowYSignature = rowYRoImage - table_instruction.TotalHeight - 5;
            var remainingRowToHeightSignature = rowYSignature - 5 - printedOnHeight - margin;
            float allowedRow2HeightSignature = rowYSignature - printedOnHeight - margin;
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
                PaddingTop = 15
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

            var tableSignatureCurrentHeight = table_signature.TotalHeight;
            if (tableSignatureCurrentHeight / remainingRowToHeightSignature > 1)
            {
                if (tableSignatureCurrentHeight / allowedRow2HeightSignature > 1)
                {
                    PdfPRow headerRow = table_signature.GetRow(0);
                    PdfPRow lastRow = table_signature.GetRow(table_signature.Rows.Count - 1);
                    table_signature.DeleteLastRow();
                    table_signature.WriteSelectedRows(0, -1, 10, rowYSignature, cb);
                    table_signature.DeleteBodyRows();
                    this.DrawPrintedOn(now, bf, cb);
                    document.NewPage();
                    table_signature.Rows.Add(headerRow);
                    table_signature.Rows.Add(lastRow);
                    table_signature.CalculateHeights();
                    rowYSignature = startY;
                    remainingRowToHeightSignature = rowYSignature - 5 - printedOnHeight - margin;
                    allowedRow2HeightSignature = remainingRowToHeightSignature - printedOnHeight - margin;
                }
            }

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

            float table_signatureY = 0;

            if (table_ro_image != null)
            {
                table_signatureY = rowYRoImage - imageRoHeight - 5;
            }
            else
            {
                table_signatureY = rowYRoImage - 0 - 5;
            }

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
