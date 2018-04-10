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
            var normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            var bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            var font_10 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
            var font_12 = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 12);
            DateTime now = DateTime.Now;

            Document document = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            #region Row1
            cb.BeginText();

            // Top
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT. EFRATA RETAILINDO", 10, 820, 0);
            cb.SetFontAndSize(bf_bold, 12);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "COST CALCULATION", 10, 805, 0);
            cb.SetFontAndSize(bf, 8);
            // Col 1
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "RO", 10, 790, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.RO}", 60, 790, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Artikel", 10, 775, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.Article}", 60, 775, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Style", 10, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.Style.name}", 60, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Musim", 10, 745, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.Season.name}", 60, 745, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Konter", 10, 730, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.Counter.name}", 60, 730, 0);
            // Col 2
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Pembeli", 190, 790, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.Buyer.Name}", 240, 790, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Delivery Date", 190, 775, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.DeliveryDate.ToString("dd MMMM yyyy")}", 240, 775, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Size Range", 190, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.SizeRange.Name}", 240, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Efisiensi", 190, 745, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.Efficiency.Value}%", 240, 745, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Resiko", 190, 730, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.Risk}%", 240, 730, 0);
            // Col 3
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tarif OL", 370, 790, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.OL.Value}", 420, 790, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tarif OTL 1", 370, 775, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.OTL1.Value}", 420, 775, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tarif OTL 2", 370, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.OTL2.Value}", 420, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Tarif OTL 3", 370, 745, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {viewModel.OTL3.Value}", 420, 745, 0);
            var STD_Hour = viewModel.OL.Value + viewModel.OTL1.Value + viewModel.OTL2.Value + viewModel.OTL3.Value;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "STD Hour", 370, 730, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, $": {STD_Hour}", 420, 730, 0);

            cb.EndText();

            // Col 4
            byte[] imageByte;
            try
            {
                imageByte = Convert.FromBase64String(Base64Converter.GetBase64File(viewModel.ImageFile));
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
            int row1Heigth = 800 - Convert.ToInt32(image.ScaledHeight);
            image.SetAbsolutePosition(520, row1Heigth);
            cb.AddImage(image, inlineImage: true);
            #endregion

            #region Row3

            #region Left

            #region Detail Table
            PdfPTable table_detail = new PdfPTable(2);
            table_detail.TotalWidth = 280f;

            float[] detail_widths = new float[] { 1f, 1f };
            table_detail.SetWidths(detail_widths);

            PdfPCell cell_detail;

            cell_detail = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5, Rowspan = 4 };

            double total = Convert.ToDouble(viewModel.OTL1.CalculatedValue + viewModel.OTL2.CalculatedValue + viewModel.OTL3.CalculatedValue);
            cell_detail.Phrase = new Phrase(
                "OL".PadRight(24) + ": " + viewModel.OL.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "OTL 1".PadRight(22) + ": " + viewModel.OTL1.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "OTL 2".PadRight(22) + ": " + viewModel.OTL2.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "OTL 3".PadRight(22) + ": " + viewModel.OTL3.CalculatedValue + Environment.NewLine + Environment.NewLine +
                "Total".PadRight(24) + ": " + total + Environment.NewLine
                , normal_font);
            table_detail.AddCell(cell_detail);

            cell_detail = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_detail.Phrase = new Phrase("HPP", normal_font);
            table_detail.AddCell(cell_detail);
            cell_detail = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            cell_detail.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.HPP), font_12);
            table_detail.AddCell(cell_detail);
            cell_detail = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_detail.Phrase = new Phrase("Wholesale Price: HPP x 2.20", normal_font);
            table_detail.AddCell(cell_detail);
            cell_detail = new PdfPCell() { Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_TOP, Padding = 5 };
            cell_detail.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.WholesalePrice), font_12);
            table_detail.AddCell(cell_detail);
            #endregion

            #region Keterangan
            PdfPTable table_keterangan = new PdfPTable(2);
            table_keterangan.TotalWidth = 280f;

            float[] keterangan_widths = new float[] { 1f, 3f };
            table_keterangan.SetWidths(keterangan_widths);

            PdfPCell cell_keterangan = new PdfPCell() { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_keterangan.Phrase = new Phrase("Keterangan", normal_font);
            table_keterangan.AddCell(cell_keterangan);
            cell_keterangan.Phrase = new Phrase(": " + viewModel.Description, normal_font);
            table_keterangan.AddCell(cell_keterangan);
            #endregion

            #endregion

            #region Right
            PdfPTable table_price = new PdfPTable(5);
            table_price.TotalWidth = 280f;

            float[] price_widths = new float[] { 1.5f, 2.5f, 2.5f, 4f, 1f };
            table_price.SetWidths(price_widths);

            PdfPCell cell_price;
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };

            cell_price.Phrase = new Phrase("Ket (x)", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("Harga", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("Pembulatan Harga", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("Keterangan", bold_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", bold_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed20), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding20), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding20") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.1", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed21), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding21), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding21") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.2", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed22), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding22), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding22") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.3", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed23), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding23), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding23") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.4", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed24), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding24), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding24") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.5", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed25), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding25), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding25") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.6", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed26), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding26), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding26") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.7", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed27), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding27), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding27") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.8", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed28), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding28), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding28") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("2.9", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed29), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding29), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("Rounding29") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase("3.0", normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Proposed30), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.Rounding30), normal_font);
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
            cell_price.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.RoundingOthers), normal_font);
            table_price.AddCell(cell_price);
            cell_price.Phrase = new Phrase("", normal_font);
            table_price.AddCell(cell_price);
            cell_price = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_price.Phrase = new Phrase(viewModel.SelectedRounding.ToString().Equals("RoundingOthers") ? "*" : "", normal_font);
            table_price.AddCell(cell_price);

            #endregion

            #endregion

            #region Footer

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

            #endregion

            #region Row2
            PdfPTable table_ccm = new PdfPTable(7);
            table_ccm.TotalWidth = 570f;

            float[] ccm_widths = new float[] { 1f, 3f, 4f, 5f, 2f, 3f, 3f };
            table_ccm.SetWidths(ccm_widths);

            PdfPCell cell_ccm;
            cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };

            cell_ccm.Phrase = new Phrase("NO", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("CATEGORIES", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("MATERIALS", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("DESCRIPTION", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("QUANTITY", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("CONVERSION", bold_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm.Phrase = new Phrase("TOTAL", bold_font);
            table_ccm.AddCell(cell_ccm);

            double Total = 0;
            int row2Y = (int)row1Heigth - 10;
            int allowedRow2Height = row2Y - 20 - 10;
            int remainingRow2Height = (int)(row1Heigth - table_price.TotalHeight - table_signature.TotalHeight - 30);
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

                cell_ccm.Phrase = new Phrase(viewModel.CostCalculationRetail_Materials[i].Conversion.ToString(), normal_font);
                table_ccm.AddCell(cell_ccm);

                cell_ccm.Phrase = new Phrase(NumberConverter.ToRupiah(viewModel.CostCalculationRetail_Materials[i].Total), normal_font);
                table_ccm.AddCell(cell_ccm);
                Total += viewModel.CostCalculationRetail_Materials[i].Total;

                int currentHeight = (int)table_ccm.TotalHeight;
                if (currentHeight / remainingRow2Height > 0)
                {
                    if (currentHeight / allowedRow2Height > 0)
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
                        table_ccm.CalculateHeights(); // required to update TotalHeight after row added, else TotalHeight = 0
                        row2Y = 830;
                        allowedRow2Height = row2Y - 20 - 10;
                        remainingRow2Height = (int)(840 - 10 - 10 - table_price.TotalHeight - 10 - table_signature.TotalHeight - 20 - 10);
                    }
                }
            }

            // Footer
            cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5, Colspan = 6 };
            cell_ccm.Phrase = new Phrase("Total", normal_font);
            table_ccm.AddCell(cell_ccm);

            cell_ccm = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell_ccm.Phrase = new Phrase(NumberConverter.ToRupiah(Total), normal_font);
            table_ccm.AddCell(cell_ccm);

            // Insert last table
            table_ccm.WriteSelectedRows(0, -1, 10, row2Y, cb);
            
            #endregion

            #region Drawing
            int row3Y = (int)(row2Y - table_ccm.TotalHeight - 10);
            int row3Height = (int)(table_price.TotalHeight + 10 + table_signature.TotalHeight);
            int remainingRow3Height = (int)(row3Y - 20 - 10);
            if (remainingRow3Height < row3Height)
            {
                this.DrawPrintedOn(now, bf, cb);
                row3Y = 830;
                document.NewPage();
            }

            #region Calculated HPP
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "KALKULASI HPP: Total Cost + (Total Cost * Risk)", 10, row3Y - 10, 0);
            cb.EndText();
            #endregion

            float table_detailY = row3Y - 20;
            table_detail.WriteSelectedRows(0, -1, 10, table_detailY, cb);

            float table_keteranganY = table_detailY - table_detail.TotalHeight - 10;
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
