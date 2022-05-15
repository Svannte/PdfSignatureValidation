using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Signatures;


namespace Pdf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string path = @"C:\Users\kptw1\source\repos\Pdf\Pdf\Documents\";
            //string path2 = @"C:\Users\kptw1\source\repos\Pdf\Pdf\Documents\document.pdf";
            int i = 0;
            string[] dirs = Directory.GetFiles(path);
            string[] filesnames = Directory.GetFiles(path).Select(file => Path.GetFileName(file)).ToArray();
            foreach (var file in dirs)
            {
                PdfReader pdf = new PdfReader(file);
                PdfDocument pdfDocument = new PdfDocument(pdf);
                var signatures = (List<string>)new SignatureUtil(pdfDocument).GetSignatureNames();
                PdfDictionary catalog = pdfDocument.GetTrailer();
                PdfDictionary map = catalog.GetAsDictionary(PdfName.Info);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine(" | /File Name: " + filesnames[i] + " | ");
                foreach (KeyValuePair<PdfName, PdfObject>entry in map.EntrySet())
                {
                    if(entry.Key.ToString() == "/CreationDate" | entry.Key.ToString() == "/ModDate")
                    {
                        var datePdf = PdfDate.Decode(entry.Value.ToString());
                        Console.WriteLine(" | " + entry.Key + " - " + datePdf + " | ");
                    }
                    else
                    {
                        Console.WriteLine(" | " + entry.Key + " - " + entry.Value + " | ");
                    }
                }
                Console.WriteLine(" | /Number of Signatures: " + signatures.Count() + " | ");
                if (signatures.Count != 0)
                {
                    foreach (string signature in signatures)
                    {
                        var context = new SignatureUtil(pdfDocument).GetSignature(signature);
                        if (context != null)
                        {
                            Console.WriteLine(" ///////////////////////////////// ");
                            Console.WriteLine(" / Digital Signature Information / ");
                            //Console.WriteLine(" | /File Name: "+ filesnames[i]+ " | "); 
                            var date = PdfDate.Decode(context.GetDate().ToString());
                            //string w3cDate = PdfDate.GetW3CDate(context.GetDate().ToString());  
                            Console.WriteLine(" / Name: " + context.GetName().ToString() + " / ");
                            Console.WriteLine(" / Date: " + date + " / "); // Day/Month/Year
                            Console.WriteLine(" / Location: " + context.GetLocation() + " / ");
                            Console.WriteLine(" / Reason: " + context.GetReason() + " / ");
                            //PdfSignatureAppearance a = new PdfSignatureAppearance();
                            Console.WriteLine(" ///////////////////////////////// ");

                        }
                        Console.WriteLine("----------------------------------------");
                    }
                    
                }
                else
                {
                    Console.WriteLine("! Signature does not exist");
                }
                i++;

            } 


            
        }
    }
}
