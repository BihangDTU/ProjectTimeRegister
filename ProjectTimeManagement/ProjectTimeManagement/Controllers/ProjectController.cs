using iTextSharp.text;
using iTextSharp.text.pdf;
using ProjectTimeManagement.Context;
using ProjectTimeManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;

namespace ProjectTimeManagement.Controllers
{
    public class ProjectController : Controller
    {
        ProjectContext db = new ProjectContext();

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                c.CustomerName = model.CustomerName;
                c.Email = model.Email;
                c.Phone = model.Phone;
                c.Address = model.Address;
                db.Customers.Add(c);
                db.SaveChanges();

                Project p = new Project();
                p.CreatorName = model.CreatorName;
                p.ProjectName = model.ProjectName;
                p.CreatedTime = model.CreatedTime;
                p.CustomerId = c.CustomerId;
                db.Projects.Add(p);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Register(int? id, TimeTable timeTable)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                TimeTable t = new TimeTable();
                t.ProjectId = id ?? default(int);
                t.RegisterTime = timeTable.RegisterTime;
                t.RegisterName = timeTable.RegisterName;
                t.Hours = timeTable.Hours;

                db.TimeTables.Add(t);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeTable);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View((from timeTable in db.TimeTables
                         where timeTable.ProjectId == id
                         select timeTable).ToList());
        }

        // GET: Project/Edit/5
        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, TimeTable collection)
        {

            TimeTable timeTable = (from tT in db.TimeTables
                                    where tT.Id == id
                                    select tT).Single();

            timeTable.RegisterName = collection.RegisterName;
            timeTable.Hours = collection.Hours;
            db.SaveChanges();

            return RedirectToAction("Details/" + timeTable.ProjectId.ToString());
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        string accountNo = "0123456789012";
        string accountName = "John Willion";
        string branch = "Phahon Yothin Branch";
        string bank = "Kasikorn Bank";

        public FileResult CreatePdf(int? id)
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document(PageSize.A4, 70, 70, 70, 70);

            //PdfWriter writer = PdfWriter.GetInstance(doc, workStream);
            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
           // Rectangle pageSize = writer.PageSize;

            //Create PDF Table with 3 columns  
            PdfPTable tableLayout = new PdfPTable(3);
           // doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            #region Top table
            // Create the header table 
            PdfPTable headertable = new PdfPTable(3);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 4, 2, 4 });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            headertable.SpacingAfter = 30;
            PdfPTable nested = new PdfPTable(1);
            nested.DefaultCell.Border = Rectangle.BOX;
            PdfPCell nextPostCell1 = new PdfPCell(new Phrase("ABC Co.,Ltd", bodyFont));
            nextPostCell1.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            nested.AddCell(nextPostCell1);
            PdfPCell nextPostCell2 = new PdfPCell(new Phrase("111/206 Moo 9, Ramkhamheang Road,", bodyFont));
            nextPostCell2.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            nested.AddCell(nextPostCell2);
            PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Nonthaburi 11120", bodyFont));
            nextPostCell3.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            nested.AddCell(nextPostCell3);
            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Rowspan = 4;
            nesthousing.Padding = 0f;
            headertable.AddCell(nesthousing);

            headertable.AddCell("");
            PdfPCell invoiceCell = new PdfPCell(new Phrase("INVOICE", titleFont));
            invoiceCell.HorizontalAlignment = 2;
            invoiceCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(invoiceCell);
            PdfPCell dateCell = new PdfPCell(new Phrase("Date :", bodyFont));
            dateCell.HorizontalAlignment = 2;
            dateCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(dateCell);
            headertable.AddCell(new Phrase(DateTime.Now.ToString("MM/dd/yyyy"), bodyFont));

            int customerId = int.Parse((from p in db.Projects
                                        where p.ProjectId == id
                                        select p.CustomerId).ToList().ElementAt(0).ToString());

            string customerName = (from c in db.Customers
                                   where c.CustomerId == customerId
                                   select c.CustomerName).ToList().ElementAt(0).ToString();

            string address = (from c in db.Customers
                              where c.CustomerId == customerId
                              select c.Address).ToList().ElementAt(0).ToString();

            PdfPCell billCell = new PdfPCell(new Phrase("Bill To :", bodyFont));
            billCell.HorizontalAlignment = 2;
            billCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(billCell);
            headertable.AddCell(new Phrase(customerName + "\n" + address, bodyFont));
            doc.Add(headertable);
            #endregion


            //Add Content to PDF   
            doc.Add(Add_Content_To_PDF(id,tableLayout));

            string totalHours = (from tT in db.TimeTables
                              where tT.ProjectId == id
                              select tT.Hours).ToList().Sum().ToString();

            PdfPTable totalHoursTable = new PdfPTable(2);
            totalHoursTable.HorizontalAlignment = 0;
            totalHoursTable.TotalWidth = 300f;
            totalHoursTable.SetWidths(new int[] { 30, 100});
            totalHoursTable.LockedWidth = true;
            totalHoursTable.SpacingBefore = 20;
            totalHoursTable.DefaultCell.Border = Rectangle.NO_BORDER;
            totalHoursTable.AddCell(new Phrase("Total Hours:", boldTableFont));
            totalHoursTable.AddCell(new Phrase(totalHours, bodyFont));
            doc.Add(totalHoursTable);

            PdfPTable bankInfo = new PdfPTable(1);
            bankInfo.HorizontalAlignment = 0;
            bankInfo.TotalWidth = 300f;
            bankInfo.SetWidths(new int[] {100});
            bankInfo.LockedWidth = true;
            bankInfo.SpacingBefore = 20;
            bankInfo.DefaultCell.Border = Rectangle.NO_BORDER;
            bankInfo.AddCell(new Phrase("Bank Account:", boldTableFont));
            doc.Add(bankInfo);
            doc.Add(Chunk.NEWLINE);

            // Bank Account Info
            PdfPTable bottomTable = new PdfPTable(3);
            bottomTable.HorizontalAlignment = 0;
            bottomTable.TotalWidth = 300f;
            bottomTable.SetWidths(new int[] { 90, 10, 200 });
            bottomTable.LockedWidth = true;
            bottomTable.SpacingBefore = 20;
            bottomTable.DefaultCell.Border = Rectangle.NO_BORDER;
            bottomTable.AddCell(new Phrase("Account No", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountNo, bodyFont));
            bottomTable.AddCell(new Phrase("Account Name", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountName, bodyFont));
            bottomTable.AddCell(new Phrase("Branch", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(branch, bodyFont));
            bottomTable.AddCell(new Phrase("Bank", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(bank, bodyFont));
            doc.Add(bottomTable);


            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(int? id, PdfPTable tableLayout)
        {
             float[] headers = { 40, 40, 40 }; //Header Widths  
             tableLayout.SetWidths(headers); //Set the pdf headers  
             tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage 
             tableLayout.HeaderRows = 1;

            List<TimeTable> timeTable = (from tT in db.TimeTables
                                         where tT.ProjectId == id
                                         select tT).ToList();

            string projectName = (from p in db.Projects
                                  where p.ProjectId == id
                                  select p.ProjectName).ToList().ElementAt(0).ToString();
            tableLayout.AddCell(new PdfPCell(new Phrase("Time spend on " + projectName + " Project", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER
            });
            ////Add header  
            AddCellToHeader(tableLayout, "Register Time");
            AddCellToHeader(tableLayout, "Name");
            AddCellToHeader(tableLayout, "Hours");

            ////Add body  

            foreach (var content in timeTable)
            {
                AddCellToBody(tableLayout, content.RegisterTime);
                AddCellToBody(tableLayout, content.RegisterName);
                AddCellToBody(tableLayout, content.Hours.ToString());
            }

            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, FontFactory.GetFont("Arial", 10, Font.NORMAL)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }

    }
}
