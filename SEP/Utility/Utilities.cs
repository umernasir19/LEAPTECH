using Azure.Core;
using CsvHelper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using SEP.Model;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Formats.Asn1;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace SEP.Utility
{
    public static class Utilities
    {
        


        public static async Task<List<SEP_CampaignAPData>> SaveAndParseCsvAsync(SEP_CampaignAPFile file, string saveFolder)
        {
            try
            {
                if (file.APFile == null || file.APFile.Length == 0)
                    throw new Exception("Empty file");

                if (!Directory.Exists(saveFolder))
                    Directory.CreateDirectory(saveFolder);

                string savedFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.APFile.FileName);
                string filePath = Path.Combine(saveFolder, savedFileName);
                file.FileName = savedFileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.APFile.CopyToAsync(stream);
                }
                file.FilePath = @"\APFILE\" + savedFileName;
                file.OriginalFileName = file.APFile.FileName;
                // Read file
                var lines = System.IO.File.ReadAllLines(filePath);
                List<SEP_CampaignAPData> APfiledata = new List<SEP_CampaignAPData>();
                foreach (var line in lines)
                {
                    var columns = line.Split(',');
                    var record = new SEP_CampaignAPData
                    {
                        SupplierName = columns[0],
                        SupplierEmail1 = columns[1],
                        ContactInfo = columns[2],
                        SupplierEmail2 = columns[3],
                        SupplierCode = columns[4],
                        SupplierAddress = columns[5],
                        Amount = decimal.TryParse(columns[6], out var amt) ? amt : 0,
                        PaymentMethod = columns[7],
                        PaymentTerms = columns[8],
                        DPO = int.TryParse(columns[9], out var dpo) ? dpo : null,
                        Currency = columns[10],
                        Country = columns[11],
                        NumberOfInvoices = int.TryParse(columns[12], out var inv) ? inv : null,
                        NumberOfTransactions = int.TryParse(columns[13], out var txn) ? txn : null,
                        NumberOfPOs = int.TryParse(columns[14], out var pos) ? pos : null,
                        EarlyPayDiscount = bool.TryParse(columns[15], out var epd) ? epd : null,
                        LatePaymentPenalty = bool.TryParse(columns[16], out var lpp) ? lpp : null,
                        APSTATUS = 0

                    };
                    APfiledata.Add(record);
                }

                return APfiledata;
                // using (var reader = new StreamReader(filePath))
                //// using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                // {
                //     var records = csv.GetRecords<SEP_CampaignAPData>().ToList();
                //     return records;
                // }
            }
            catch (Exception e)
            {
                return new List<SEP_CampaignAPData>();
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, propType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }


        public static int SendEmail(Sep_Email objemail)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp-relay.brevo.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = true;
            string smtpUser = "umernasir19@gmail.com";
            string smtpPass = "fwQ2B1DrEjkzmIAx"; // App password for Gmail/Outlook

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("no-reply@cardify.com");
                mail.To.Add(objemail.To);
                mail.Subject = objemail.Subject;
                mail.Body = objemail.Body;

                SmtpClient smtp = new SmtpClient("smtp-relay.brevo.com", 587);
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtp.EnableSsl = true;

                smtp.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
            return 0;
        }


        public static List<SEP_BuyerAsset> ProcessEndoresement(SEP_Upload_logo objlogo,string baseurl)
        {

            try
            {
                List<SEP_BuyerAsset> objlistbuyerasset = new List<SEP_BuyerAsset>();
                SEP_BuyerAsset objsepbuyerasset = new SEP_BuyerAsset();
                string folderpath = "BuyerAssets";//needs to be changed to webconfig;
                string BuyerPAth = Path.Combine(Directory.GetCurrentDirectory(), folderpath);
                string uniqueFileName = $"logo_{Guid.NewGuid()}{Path.GetExtension(objlogo.Logo.FileName)}";
                string savedPath = Path.Combine(BuyerPAth, uniqueFileName);
                using (var stream = new FileStream(savedPath, FileMode.Create))
                {
                    objlogo.Logo.CopyToAsync(stream);
                }
                //addition to list for logo
                objsepbuyerasset.BuyerID = objlogo.BuyerId;
                objsepbuyerasset.AssetType = "Logo";
                objsepbuyerasset.FilePath = folderpath + @"\" + uniqueFileName;
                objsepbuyerasset.FileName= uniqueFileName;
                objsepbuyerasset.UploadedAt = DateTime.UtcNow;
                objsepbuyerasset.IsActive = true;

                objlistbuyerasset.Add(objsepbuyerasset);
                //end
                
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Supplier_endorsment_letter.html");
                string htmlContent = System.IO.File.ReadAllText(templatePath);

                string relativeImagePath = baseurl+ $"/BuyerAssets/{uniqueFileName}";
                htmlContent = htmlContent.Replace("{{LOGO}}", relativeImagePath);
                string outputfilenameforletter= "BuyerAssets"+ $"endorsement_{DateTime.Now.Ticks}.html";
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(),folderpath, outputfilenameforletter);
                System.IO.File.WriteAllText(outputPath, htmlContent);
                /// adition to list for letter
                objsepbuyerasset = new SEP_BuyerAsset();
                objsepbuyerasset.BuyerID = objlogo.BuyerId;
                objsepbuyerasset.AssetType = "Letter";
                objsepbuyerasset.FilePath = folderpath + @"\" + outputfilenameforletter;
                objsepbuyerasset.FileName = outputfilenameforletter;
                objsepbuyerasset.UploadedAt = DateTime.UtcNow;
                objsepbuyerasset.IsActive = true;
                objlistbuyerasset.Add(objsepbuyerasset);
                return objlistbuyerasset; 


            }
            catch(Exception ex)
            {
                return new List<SEP_BuyerAsset>(); 
            }
        }

    }
}
