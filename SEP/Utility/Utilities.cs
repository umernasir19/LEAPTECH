using CsvHelper;
using SEP.Model;
using System.ComponentModel;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;

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

                // Read file
                var lines = System.IO.File.ReadAllLines(filePath);
                List<SEP_CampaignAPData> APfiledata= new List<SEP_CampaignAPData>();
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
                        LatePaymentPenalty = bool.TryParse(columns[16], out var lpp) ? lpp : null
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
            catch(Exception e)
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
    }
}
