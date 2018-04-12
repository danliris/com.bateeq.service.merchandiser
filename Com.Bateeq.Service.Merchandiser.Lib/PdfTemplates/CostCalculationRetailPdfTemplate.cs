using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Net;

namespace Com.Bateeq.Service.Merchandiser.Lib.PdfTemplates
{
    public class CostCalculationRetailPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(CostCalculationRetailViewModel viewModel)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font font_10 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            Font font_12 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
            DateTime now = DateTime.Now;

            Document document = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            #region Header
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT. EFRATA RETAILINDO", 10, 820, 0);
            cb.SetFontAndSize(bf_bold, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "COST CALCULATION", 10, 805, 0);
            cb.EndText();
            #endregion

            #region Top
            PdfPTable table_top = new PdfPTable(9);
            table_top.TotalWidth = 500f;

            float[] top_widths = new float[] { 1f, 0.1f, 2f, 1f, 0.1f, 2f, 1f, 0.1f, 2f };
            table_top.SetWidths(top_widths);

            PdfPCell cell_top = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 1, PaddingBottom = 5, PaddingTop = 5 };
            PdfPCell cell_colon = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE };
            cell_colon.Phrase = new Phrase(":", normal_font);

            cell_top.Phrase = new Phrase("RO", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.RO}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("PEMBELI", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Buyer.Name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("TARIF OL", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            double OLValue = viewModel.OL.Value ?? 0;
            cell_top.Phrase = new Phrase($"{OLValue}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("ARTIKEL", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Article}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("DELIV DATE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.DeliveryDate.ToString("dd MMMM yyyy")}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("TARIF OTL 1", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            double OTL1Value = viewModel.OTL1.Value ?? 0;
            cell_top.Phrase = new Phrase($"{OTL1Value}%", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("STYLE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Style.name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("SIZE RANGE", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.SizeRange.Name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("TARIF OTL 2", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            double OTL2Value = viewModel.OTL2.Value ?? 0;
            cell_top.Phrase = new Phrase($"{OTL2Value}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("MUSIM", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Season.name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("EFISIENSI", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Efficiency.Value}%", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("TARIF OTL 3", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            double OTL3Value = viewModel.OTL3.Value ?? 0;
            cell_top.Phrase = new Phrase($"{OTL3Value}", normal_font);
            table_top.AddCell(cell_top);

            cell_top.Phrase = new Phrase("KONTER", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Counter.name}", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("RESIKO", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            cell_top.Phrase = new Phrase($"{viewModel.Risk}%", normal_font);
            table_top.AddCell(cell_top);
            cell_top.Phrase = new Phrase("STD HOUR", normal_font);
            table_top.AddCell(cell_top);
            table_top.AddCell(cell_colon);
            double STD_Hour = OLValue + OTL1Value + OTL2Value + OTL3Value;
            cell_top.Phrase = new Phrase($"{STD_Hour}", normal_font);
            table_top.AddCell(cell_top);
            #endregion

            #region Image
            byte[] imageByte;
            try
            {
                imageByte = Convert.FromBase64String(Base64.GetBase64File(viewModel.ImageFile));
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

            #region Detail (Bottom, Column 1.1)
            PdfPTable table_detail = new PdfPTable(2);
            table_detail.TotalWidth = 280f;

            float[] detail_widths = new float[] { 1f, 1f };
            table_detail.SetWidths(detail_widths);

            PdfPCell cell_detail;

            cell_detail = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5, Rowspan = 4 };

            double total = Convert.ToDouble(viewModel.OTL1.CalculatedValue + viewModel.OTL2.CalculatedValue + viewModel.OTL3.CalculatedValue);
            cell_detail.Phrase = new Phrase(
                "OL".PadRight(22) + ": " + viewModel.OL.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "OTL 1".PadRight(20) + ": " + viewModel.OTL1.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "OTL 2".PadRight(20) + ": " + viewModel.OTL2.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "OTL 3".PadRight(20) + ": " + viewModel.OTL3.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "Total".PadRight(22) + ": " + total + Environment.NewLine
                , normal_font);
            table_detail.AddCell(cell_detail);

            cell_detail = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_detail.Phrase = new Phrase("HPP", normal_font);
            table_detail.AddCell(cell_detail);
            cell_detail = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            cell_detail.Phrase = new Phrase(Number.ToRupiah(viewModel.HPP), font_12);
            table_detail.AddCell(cell_detail);
            cell_detail = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_detail.Phrase = new Phrase("Wholesale Price: HPP X 2.20", normal_font);
            table_detail.AddCell(cell_detail);
            cell_detail = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            cell_detail.Phrase = new Phrase(Number.ToRupiah(viewModel.WholesalePrice), font_12);
            table_detail.AddCell(cell_detail);
            #endregion

            #region Keterangan (Bottom, Column 1.2)
            PdfPTable table_keterangan = new PdfPTable(2);
            table_keterangan.TotalWidth = 280f;

            float[] keterangan_widths = new float[] { 1f, 3f };
            table_keterangan.SetWidths(keterangan_widths);

            PdfPCell cell_keterangan = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingRight = 5, PaddingBottom = 5, PaddingTop = 5 };
            cell_keterangan.Phrase = new Phrase("KETERANGAN", normal_font);
            table_keterangan.AddCell(cell_keterangan);
            cell_keterangan.Phrase = new Phrase(": " + viewModel.Description, normal_font);
            table_keterangan.AddCell(cell_keterangan);
            #endregion

            #region Price (Bottom, Column 2)
            PdfPTable table_price = new PdfPTable(5);
            table_price.TotalWidth = 280f;

            float[] price_widths = new float[] { 1.6f, 3f, 3f, 4f, 1f };
            table_price.SetWidths(price_widths);

            PdfPCell cell_price;
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };

            cell_price.Phrase = new Phrase("KET (X)", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("HARGA", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("PEMBULATAN HARGA", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("KETERANGAN", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", bold_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed20), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding20), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding20") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.1", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed21), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding21), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding21") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.2", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed22), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding22), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding22") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.3", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed23), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding23), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding23") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.4", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed24), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding24), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding24") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.5", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed25), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding25), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding25") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.6", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed26), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding26), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding26") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.7", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed27), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding27), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding27") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.8", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed28), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding28), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding28") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.9", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed29), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding29), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding29") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("3.0", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Proposed30), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(Number.ToRupiah(viewModel.Rounding30), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding30") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("Others", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(viewModel.RoundingOthers > 0 ? Number.ToRupiah(viewModel.RoundingOthers) : "", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("RoundingOthers") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            #endregion

            #region Signature
            PdfPTable table_signature = new PdfPTable(3);
            table_signature.TotalWidth = 570f;

            float[] signature_widths = new float[] { 1f, 1f, 1f };
            table_signature.SetWidths(signature_widths);

            PdfPCell cell_signature = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };

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
            cell_signature.Phrase = new Phrase(signatureArea, normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase(signatureArea, normal_font);
            table_signature.AddCell(cell_signature);

            cell_signature.Phrase = new Phrase("Ka. Sie Pembelian", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Direktur Operasional", normal_font);
            table_signature.AddCell(cell_signature);
            cell_signature.Phrase = new Phrase("Wakil Direktur Utama", normal_font);
            table_signature.AddCell(cell_signature);
            #endregion

            #region Draw Top
            float row1Y = 800;
            table_top.WriteSelectedRows(0, -1, 10, row1Y, cb);

            float imageY = 800 - image.ScaledHeight;
            image.SetAbsolutePosition(520, imageY);
            cb.AddImage(image, inlineImage: true);
            #endregion

            #region Cost Calculation Material
            PdfPTable table_ccm = new PdfPTable(8);
            table_ccm.TotalWidth = 570f;

            float[] ccm_widths = new float[] { 1.25f, 3.5f, 4f, 6f, 3f, 4f, 3f, 4f };
            table_ccm.SetWidths(ccm_widths);

            PdfPCell cell_ccm;
            cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };

            cell_ccm.Phrase = new Phrase("NO", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("KATEGORI", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("MATERIAL", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("DESKRIPSI", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("KUANTITAS", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("HARGA PER SATUAN", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("KONVERSI", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("TOTAL", bold_font);
            table_ccm.AddCell(cell_ccm);

            double Total = 0;
            float row1EndY = image.ScaledHeight > table_top.TotalHeight ? row1Y - image.ScaledHeight : row1Y - table_top.TotalHeight;
            float row2Y = row1EndY - 10;
            float allowedRow2Height = row2Y - 20 - 10;
            float remainingRow2Height = row1EndY - table_price.TotalHeight - table_signature.TotalHeight - 30;
            for (int i = 0; i < viewModel.CostCalculationRetail_Materials.Count; i++)
            {
                cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
                cell_ccm.Phrase = new Phrase((i + 1).ToString(), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationRetail_Materials[i].Category.SubCategory != null ? String.Format("{0} - {1}", viewModel.CostCalculationRetail_Materials[i].Category.Name, viewModel.CostCalculationRetail_Materials[i].Category.SubCategory) : viewModel.CostCalculationRetail_Materials[i].Category.Name, normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationRetail_Materials[i].Material.Name, normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationRetail_Materials[i].Description, normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
                cell_ccm.Phrase = new Phrase(String.Format("{0} {1}", viewModel.CostCalculationRetail_Materials[i].Quantity, viewModel.CostCalculationRetail_Materials[i].UOMQuantity.Name), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(String.Format("{0}/{1}", Number.ToRupiahWithoutSymbol(viewModel.CostCalculationRetail_Materials[i].Price), viewModel.CostCalculationRetail_Materials[i].UOMPrice.Name), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationRetail_Materials[i].Conversion.ToString(), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(Number.ToRupiah(viewModel.CostCalculationRetail_Materials[i].Total), normal_font);
                table_ccm.AddCell(cell_ccm);
                Total += viewModel.CostCalculationRetail_Materials[i].Total;

                float currentHeight = table_ccm.TotalHeight;
                if (currentHeight / remainingRow2Height > 1)
                {
                    if (currentHeight / allowedRow2Height > 1)
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
                        row2Y = 830;
                        allowedRow2Height = row2Y - 20 - 10;
                        remainingRow2Height = 840 - 10 - 10 - table_price.TotalHeight - 10 - table_signature.TotalHeight - 20 - 10;
                    }
                }
            }

            cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5, Colspan = 7 };
            cell_ccm.Phrase = new Phrase("TOTAL", normal_font);
            table_ccm.AddCell(cell_ccm);
            cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_ccm.Phrase = new Phrase(Number.ToRupiah(Total), normal_font);
            table_ccm.AddCell(cell_ccm);
            #endregion

            #region Draw Middle and Bottom
            table_ccm.WriteSelectedRows(0, -1, 10, row2Y, cb);

            float row3Y = row2Y - table_ccm.TotalHeight - 10;
            float row3LeftHeight = 20 + table_detail.TotalHeight + 5 + table_keterangan.TotalHeight;
            float row3Height = row3LeftHeight > table_price.TotalHeight ? row3LeftHeight + 10 + table_signature.TotalHeight : table_price.TotalHeight + 10 + table_signature.TotalHeight;
            float remainingRow3Height = row3Y - 20 - 10;
            if (remainingRow3Height < row3Height)
            {
                this.DrawPrintedOn(now, bf, cb);
                row3Y = 830;
                document.NewPage();
            }

            #region Calculated HPP
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "KALKULASI HPP: Total Cost + (Total Cost * Risk)", 10, row3Y - 10, 0);
            #endregion

            float table_detailY = row3Y - 20;
            table_detail.WriteSelectedRows(0, -1, 10, table_detailY, cb);

            float table_keteranganY = table_detailY - table_detail.TotalHeight - 5;
            table_keterangan.WriteSelectedRows(0, -1, 10, table_keteranganY, cb);

            table_price.WriteSelectedRows(0, -1, 300, row3Y, cb);

            float table_signatureY = 30 + table_signature.TotalHeight;
            table_signature.WriteSelectedRows(0, -1, 10, table_signatureY, cb);

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
