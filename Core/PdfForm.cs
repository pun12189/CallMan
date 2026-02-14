using BahiKitab.Models;
using BahiKitab.Services;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using NumericWordsConversion;
using System.Diagnostics;

namespace BahiKitab.Core
{
    public class PdfForm
    {
        /// <summary>
        /// The MigraDoc document that represents the invoice.
        /// </summary>
        Document _document;

        /// <summary>
        /// The MigraDoc document that represents the invoice.
        /// </summary>
        private CurrencyWordsConverter converter;

        /// <summary>
        /// The MigraDoc document that represents the invoice.
        /// </summary>
        private readonly ImagesDataService imagesDataService;

        /// <summary>
        /// The text frame of the MigraDoc document that contains the address.
        /// </summary>
        TextFrame _addressFrame;

        /// <summary>
        /// The table of the MigraDoc document that contains the invoice items.
        /// </summary>
        Table _table;

        /// <summary>
        /// The Represents the orientation od MigraDoc document.
        /// </summary>
        PageSetup pageSetup;

        /// <summary>
        /// The Represents the orientation od MigraDoc document.
        /// </summary>
        Section section;

        public PdfForm()
        {
            converter = new CurrencyWordsConverter(new CurrencyWordsConversionOptions
            {
                Culture = Culture.Nepali,
                OutputFormat = OutputFormat.English,
                CurrencyUnitSeparator = "and",
                CurrencyUnit = "rupee",
                SubCurrencyUnit = "paisa"
            });
            imagesDataService = new ImagesDataService();
        }

        public async void CreateFreePdf(LeadOrderModel data)
        {
            Document document = new Document();
            document.Info.Title = "Quotation Order";
            document.Info.Subject = "Details of Customer's Order";
            document.Info.Author = "Lucky Furniture | Zirakpur";            

            // 1. Styles Definition
            Style style = document.Styles["Normal"];
            style.Font.Name = "Segoe UI";
            style.Font.Size = 12;

            Section section = document.AddSection();            
            section.PageSetup.Orientation = Orientation.Landscape;
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.LeftMargin = "1cm";
            section.PageSetup.RightMargin = "1cm";
            section.PageSetup.TopMargin = "1cm";
            section.PageSetup.BottomMargin = "1cm";

            // --- HEADER: Store Identity ---
            Paragraph title = section.AddParagraph("QUOTATION"); // 
            title.Format.Font.Size = 20;
            title.Format.Font.Bold = true;
            title.Format.Alignment = ParagraphAlignment.Center;

            // Seller Info Table
            Table logotable = section.AddTable();
            logotable.AddColumn("7cm");
            logotable.AddColumn("20cm");
            Row lRow = logotable.AddRow();

            var logoPath = @"C:\Users\Puneet Aggrwal\Pictures\logo.png";

            if (System.IO.File.Exists(logoPath))
            {
                var logo = lRow.Cells[0].AddImage(logoPath);
                logo.Width = "5cm";
            }

            var logoinfo = lRow.Cells[1];
            logoinfo.Format.Alignment = ParagraphAlignment.Right;
            logoinfo.Format.Font.Bold = true;
            logoinfo.AddParagraph("ID: " + data.OrderId); // 
            logoinfo.AddParagraph("Best Furniture Store in Zirakpur");

            // Seller Info Table
            Table sellerTable = section.AddTable();
            sellerTable.AddColumn("13cm");
            sellerTable.AddColumn("14cm");
            Row sRow = sellerTable.AddRow();

            var sellerInfo = sRow.Cells[0];
            sellerInfo.AddParagraph();
            sellerInfo.AddParagraph("LUCKY FURNITURE").Format.Font.Bold = true; // [cite: 46, 47]
            sellerInfo.AddParagraph("BASEMENT, GROUND, FIRST AND SECOND, \nSCO 2 & 3, Patiala Road, Yes Bank,\nUtrathiya, Zirakpur, SAS Nagar,\nPunjab, 140603"); // [cite: 48]
            sellerInfo.AddParagraph("Mobile: 9815760634 | Email: design.luckyfurniture@gmail.com"); // [cite: 49, 51]
            sellerInfo.AddParagraph("GSTIN: 03AAZPG4515F1ZS"); // [cite: 50]
            sellerInfo.AddParagraph();

            var invMeta = sRow.Cells[1];
            invMeta.Format.Alignment = ParagraphAlignment.Right;
            invMeta.AddParagraph();
            //invMeta.AddParagraph("Invoice No.: 2"); // 
            invMeta.AddParagraph("Date: " + DateTime.Now.ToString("dd/MM/yyyy, hh:mm tt")); // [cite: 59]
            //invMeta.AddParagraph("PAN: AAZPG4515F"); // [cite: 58]

            // --- BILL TO / SHIP TO ---
            Table addressTable = section.AddTable();
            addressTable.Borders.Width = 0.5;
            addressTable.AddColumn("13cm");
            addressTable.AddColumn("14cm");
            Row addrRow = addressTable.AddRow();
            addrRow.HeadingFormat = true;
            addrRow.Shading.Color = Colors.LightBlue; // हेडर का बैकग्राउंड
            addrRow.Format.Font.Color = Colors.Black; // हेडर का टेक्स्ट कलर
            addrRow.Format.Font.Bold = true;
            addrRow.Cells[0].AddParagraph("BILL TO").Format.Font.Bold = true; // [cite: 53]
            addrRow.Cells[1].AddParagraph("SHIP TO").Format.Font.Bold = true; // [cite: 56]

            Row addrData = addressTable.AddRow();
            string client = "SBP\nSingla Builders & Promoters\nLalru Site"; // [cite: 54, 55]
            addrData.Cells[0].AddParagraph(client);
            addrData.Cells[1].AddParagraph(client); // [cite: 57, 60]

            // --- ITEMS TABLE ---
            Table itemsTable = section.AddTable();
            itemsTable.Format.Alignment = ParagraphAlignment.Center;
            itemsTable.Borders.Width = 0.5;
            itemsTable.AddColumn("2cm");  // sno
            itemsTable.AddColumn("6cm"); // name
            itemsTable.AddColumn("8cm");//image
            itemsTable.AddColumn("2cm");// qty
            itemsTable.AddColumn("4cm"); // rate
            itemsTable.AddColumn("2cm"); // gst
            itemsTable.AddColumn("3cm");// Amount

            Row header = itemsTable.AddRow();
            header.HeadingFormat = true;
            header.Shading.Color = Colors.LightBlue; // हेडर का बैकग्राउंड
            header.Format.Font.Color = Colors.Black; // हेडर का टेक्स्ट कलर
            header.Format.Font.Bold = true;

            string[] cols = { "S.NO", "ITEMS", "IMAGE", "QTY", "RATE", "TAX", "AMOUNT" }; // [cite: 61]
            for (int i = 0; i < cols.Length; i++) header.Cells[i].AddParagraph(cols[i]).Format.Font.Bold = true;

            var trate = 0.0;
            var tgst = 0.0;
            var count = 0;
            foreach(var item in data.OrderedProducts)
            {
                Row r1 = itemsTable.AddRow();
                r1.Cells[0].AddParagraph((++count).ToString()); // [cite: 61]
                r1.Cells[1].AddParagraph(item.Name);

                if (item.ImageIds.Count > 0)
                {
                    var image = await imagesDataService.GetImage(item.ImageIds[0]);
                    var l = r1.Cells[2].AddImage(Helper.Helper.SaveBitmapSourceToTempFile(image.ImageSource));
                    l.Width = "5cm";
                    l.LockAspectRatio = true;
                    l.Left = ShapePosition.Center;
                }

                if (item.Name == "Carriage")
                {
                    r1.Cells[3].AddParagraph();
                }
                else
                {                    
                    r1.Cells[3].AddParagraph(item.ProductCost.Quantity + "pcs");
                }

                //r1.Cells[1].AddParagraph("94036000");

                
                r1.Cells[4].AddParagraph(item.ProductCost.Rate + "₹");
                r1.Cells[5].AddParagraph(item.ProductCost.GST + "%");
                r1.Cells[6].AddParagraph(item.ProductCost.TotalPrice + "₹");
                                

                trate += (item.ProductCost.Rate * item.ProductCost.Quantity);
                tgst += (item.ProductCost.TotalPrice - (item.ProductCost.Rate * item.ProductCost.Quantity));
            }            

            // Repeat for other items as per source 61...

            // --- SUMMARY & BANK DETAILS ---
            Table footerTable = section.AddTable();
            footerTable.AddColumn("13cm"); // Bank Details
            footerTable.AddColumn("14cm");  // Totals
            Row fRow = footerTable.AddRow();

            //[cite_start]// Bank Details [cite: 62, 64]
            var bank = fRow.Cells[0];
            bank.AddParagraph();
            bank.AddParagraph("BANK DETAILS").Format.Font.Bold = true;
            bank.AddParagraph("Name: Lucky Furniture");
            bank.AddParagraph("IFSC: HDFC0000154");
            bank.AddParagraph("A/c No: 50200044148165");

            //[cite_start]// Totals Table [cite: 63]
            var totals = fRow.Cells[1];
            totals.AddParagraph();
            totals.AddParagraph("Taxable Amount: " + trate + "₹").Format.Alignment = ParagraphAlignment.Right; ;
            totals.AddParagraph("CGST @9%: " + tgst/2 + "₹").Format.Alignment = ParagraphAlignment.Right; ;
            totals.AddParagraph("SGST @9%: " + tgst/2 + "₹").Format.Alignment = ParagraphAlignment.Right; ;
            var p = totals.AddParagraph("Total Amount: " + data.OrderAmount + "₹");
            p.Format.Alignment = ParagraphAlignment.Right;
            p.Format.Font.Bold = true;
            totals.AddParagraph("Received: " + data.ReceivedAmount + "₹" + " | Balance: " + data.Balance + "₹").Format.Alignment = ParagraphAlignment.Right; ;

            string words = converter.ToWords(Convert.ToDecimal(data.OrderAmount));
            // Total in words
            section.AddParagraph("\nTotal Amount (in words): " + words).Format.Font.Bold = true; // [cite: 71, 72]

            // --- QR CODE & TERMS ---
            /*Table qrTable = section.AddTable();
            qrTable.AddColumn("4cm"); // QR Placeholder
            qrTable.AddColumn("15cm"); // Terms
            Row qRow = qrTable.AddRow();

            qRow.Cells[0].AddParagraph("UPI: 331011358788228@cnrb");*/ // [cite: 65, 66]

            section.AddParagraph();
            section.AddParagraph("TERMS AND CONDITIONS").Format.Font.Bold = true; // [cite: 68]
            section.AddParagraph("1. Goods once sold will not be taken back.\n2. Disputes subject to Zirakpur jurisdiction.\n"); // [cite: 69, 70]

            // Signatures
            section.AddParagraph("Authorised Signatory").Format.Alignment = ParagraphAlignment.Right; // [cite: 74, 76]
            section.AddParagraph(); // [cite: 74, 76]
            section.AddParagraph("Receiver's Signature").Format.Alignment = ParagraphAlignment.Right; // [cite: 74, 76]            

            // Render PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer() { Document = document };
            renderer.RenderDocument();
            var filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + "LKY" + Math.Abs(DateTime.Now.GetHashCode()) + ".pdf";
            renderer.PdfDocument.Save(filename);
            Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
        }

    }
}
