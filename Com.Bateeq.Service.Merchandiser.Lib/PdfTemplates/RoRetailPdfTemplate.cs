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
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font_8 = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font font_9 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
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
            cell_top.Phrase = new Phrase("No RO", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.RO}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("Article", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.Article}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("Style", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.Style.name}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("Content", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.Counter.name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("Colour", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Color.name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("Delivery Date", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.CostCalculationRetail.DeliveryDate.ToString("dd MMMM yyyy")}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("RO quantity", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top_keterangan.Phrase = new Phrase($"{viewModel.Total}", normal_font);
            table_top.AddCell(cell_top_keterangan);
            #endregion

            #region Image

            byte[] imageByte;
            try
            {
                imageByte = Convert.FromBase64String(Base64.GetBase64File(viewModel.CostCalculationRetail.ImageFile));
            }
            catch (Exception)
            {
                var webClient = new WebClient();
                imageByte = webClient.DownloadData("https://bateeqstorage.blob.core.windows.net/other/no-image.jpg");
            }

            Image image = Image.GetInstance(imgb: imageByte);

            if (image.Width > 60)
            {
                float percentage = 0.0f;
                percentage = 60 / image.Width;
                image.ScalePercent(percentage * 100);
            }
            #endregion

            #region Draw Top

            float row1Y = 800;
            float imageY = 800 - image.ScaledHeight;

            table_top.WriteSelectedRows(0, -1, 10, row1Y, cb);
            image.SetAbsolutePosition(520, imageY);
            cb.AddImage(image, inlineImage: true);
            #endregion

            #region Fabric Table Title

            PdfPTable table_fabric_top = new PdfPTable(1);
            table_fabric_top.TotalWidth = 570f;

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

            cell_top_fabric.Phrase = new Phrase("Fabric", bold_font);
            table_fabric_top.AddCell(cell_top_fabric);

            float row1Height = image.ScaledHeight > table_top.TotalHeight ? image.ScaledHeight : table_top.TotalHeight;
            float rowYTittleFab = row1Y - row1Height - 10;
            float allowedRow2Height = rowYTittleFab - printedOnHeight - margin;
            table_fabric_top.WriteSelectedRows(0, -1, 10, rowYTittleFab, cb);
            #endregion

            #region Fabric Table
            PdfPTable table_fabric = new PdfPTable(5);
            table_fabric.TotalWidth = 570f;

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

            cell_fabric_center.Phrase = new Phrase("Accessories", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("Name", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("Description", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("Quantity", bold_font);
            table_fabric.AddCell(cell_fabric_center);

            cell_fabric_center.Phrase = new Phrase("Information", bold_font);
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

                    cell_fabric_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} Meter", materialModel.Quantity) : "0", normal_font);
                    table_fabric.AddCell(cell_fabric_left);

                    cell_fabric_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_fabric.AddCell(cell_fabric_left);
                }
            }

            table_fabric.WriteSelectedRows(0, -1, 10, rowYFab, cb);
            #endregion


            #region Accessoris Table Title

            PdfPTable table_acc_top = new PdfPTable(1);
            table_acc_top.TotalWidth = 570f;

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

            cell_top_acc.Phrase = new Phrase("Accessories", bold_font);
            table_acc_top.AddCell(cell_top_acc);

            float rowYTittleAcc = rowYFab - table_fabric.TotalHeight - 10;
            float allowedRow2HeightTopAcc = rowYTittleFab - printedOnHeight - margin;
            table_acc_top.WriteSelectedRows(0, -1, 10, rowYTittleAcc, cb);
            #endregion

            #region Accessoris Table

            PdfPTable table_accessories = new PdfPTable(5);
            table_accessories.TotalWidth = 570f;

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

            cell_acc_center.Phrase = new Phrase("Accessories", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("Name", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("Description", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("Quantity", bold_font);
            table_accessories.AddCell(cell_acc_center);

            cell_acc_center.Phrase = new Phrase("Information", bold_font);
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

                    cell_acc_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} Meter", materialModel.Quantity) : "0", normal_font);
                    table_accessories.AddCell(cell_acc_left);

                    cell_acc_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_accessories.AddCell(cell_acc_left);
                }
            }

            table_accessories.WriteSelectedRows(0, -1, 10, rowYAcc, cb);
            #endregion

            #region Ongkos Table Title

            PdfPTable table_ong_top = new PdfPTable(1);
            table_ong_top.TotalWidth = 570f;

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

            cell_top_ong.Phrase = new Phrase("Budget", bold_font);
            table_ong_top.AddCell(cell_top_ong);

            float rowYTittleOng = rowYAcc - table_accessories.TotalHeight - 10;
            float allowedRow2HeightTopOng = rowYTittleOng - printedOnHeight - margin;
            table_ong_top.WriteSelectedRows(0, -1, 10, rowYTittleOng, cb);
            #endregion

            #region Budget Table

            PdfPTable table_budget = new PdfPTable(5);
            table_budget.TotalWidth = 570f;

            float[] budget_widths = new float[] { 5f, 5f, 5f, 5f, 5f };
            table_budget.SetWidths(budget_widths);

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

            cell_budget_center.Phrase = new Phrase("Accessories", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("Name", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("Description", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("Quantity", bold_font);
            table_budget.AddCell(cell_budget_center);

            cell_budget_center.Phrase = new Phrase("Information", bold_font);
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

                    cell_budget_left.Phrase = new Phrase(materialModel.Quantity != null ? String.Format("{0} Meter", materialModel.Quantity) : "0", normal_font);
                    table_budget.AddCell(cell_budget_left);

                    cell_budget_left.Phrase = new Phrase(materialModel.Information != null ? materialModel.Information : "", normal_font);
                    table_budget.AddCell(cell_budget_left);
                }
            }

            table_budget.WriteSelectedRows(0, -1, 10, rowYBudget, cb);
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

            cell_top_breakdown.Phrase = new Phrase("Size Breakdown", bold_font);
            table_breakdown_top.AddCell(cell_top_breakdown);

            float rowYTittleBreakDown = rowYBudget - table_budget.TotalHeight - 10;
            float allowedRow2HeightBreakdown = rowYTittleBreakDown - printedOnHeight - margin;
            table_breakdown_top.WriteSelectedRows(0, -1, 10, rowYTittleBreakDown, cb);
            #endregion

            #region == Table Size Breakdown ==

            PdfPTable table_breakDown = new PdfPTable(7);
            table_breakDown.TotalWidth = 570f;

            float[] breakDown_widths = new float[] { 5f, 5f, 5f, 5f, 5f, 5f, 5f };
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

            cell_breakDown_center.Phrase = new Phrase("Kode Toko", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("Toko", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("10", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("12", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("14", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("16", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

            cell_breakDown_center.Phrase = new Phrase("Total", bold_font);
            table_breakDown.AddCell(cell_breakDown_center);

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
                        cell_breakDown_left.Phrase = new Phrase(size.Value.ToString() != null ? size.Value.ToString() : "0", normal_font);
                        table_breakDown.AddCell(cell_breakDown_left);
                    }

                    cell_breakDown_left.Phrase = new Phrase(productRetail.Total.ToString() != null ? productRetail.Total.ToString() : "0", normal_font);
                    table_breakDown.AddCell(cell_breakDown_left);
                }
            }

            cell_breakDown_total.Phrase = new Phrase(" ", bold_font);
            table_breakDown.AddCell(cell_breakDown_total);

            cell_breakDown_total_2.Phrase = new Phrase("Total", bold_font);
            table_breakDown.AddCell(cell_breakDown_total_2);

            foreach (var sizeTotal in viewModel.SizeQuantityTotal)
            {
                cell_breakDown_left.Phrase = new Phrase(sizeTotal.Value.ToString() != null ? sizeTotal.Value.ToString() : "0", normal_font);
                table_breakDown.AddCell(cell_breakDown_left);
            }
            cell_breakDown_left.Phrase = new Phrase(viewModel.Total.ToString() != null ? viewModel.Total.ToString() : "0", normal_font);
            table_breakDown.AddCell(cell_breakDown_left);

            table_breakDown.WriteSelectedRows(0, -1, 10, rowYbreakDown, cb);
            #endregion

            #region Table Instruksi

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

            cell_top_instruction.Phrase = new Phrase("Instruction", normal_font);
            table_instruction.AddCell(cell_top_instruction);
            table_instruction.AddCell(cell_colon_instruction);
            cell_top_keterangan_instruction.Phrase = new Phrase($"{viewModel.Instruction}", normal_font);
            table_instruction.AddCell(cell_top_keterangan_instruction);

            float rowYInstruction = rowYbreakDown - table_breakDown.TotalHeight - 10;
            float allowedRow2HeightInstruction = rowYInstruction - printedOnHeight - margin;

            table_instruction.WriteSelectedRows(0, -1, 10, rowYInstruction, cb);
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
