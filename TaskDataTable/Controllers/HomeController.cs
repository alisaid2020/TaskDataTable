using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskDataTable.Models;
using System.Linq.Dynamic;
using System.Globalization;
using System.IO;
using Rotativa;
using Microsoft.Reporting.WebForms;
using iTextSharp.text.pdf;
using System.Data.Entity;
using TaskDataTable.Hubs;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TaskDataTable.Controllers
{
    public class HomeController : Controller
    {
        Context Db;
        public HomeController()
        {
            Db = new Context();
        }
        public ActionResult Index()
        {

            //using (DbContextTransaction dbtran = Db.Database.BeginTransaction())
            //{
            //    try
            //    {

            //    }
            //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            //    {
            //        dbtran.Rollback();
            //        throw;
            //    }
            //}
            return View(Db.StudentDegrees_kw.Where(s => s.IS_Deleted == false).ToList());
        }

        public ActionResult ShowGrid()
        {

            var Schools = Db.StudentDegrees_kw.Select(s => new { SchoolID = s.SchoolID, SchoolName = s.SchoolName }).Distinct().ToList();
            ViewBag.schools = new SelectList(Schools, "SchoolID", "SchoolName");
            return View();
        }

        public ActionResult LoadData(string filter_data,string RecomendStatues)
        {
            try
            {
                
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                string Searchschool = Request.Form.GetValues("columns[1][search][value]")[0];

                string SearchDate = Request.Form.GetValues("columns[1][search][value]")[0];

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data
                var customerData = (from Std in Db.StudentDegrees_kw
                                    select Std);

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDir);
                }

                //Search
                //

                decimal number;
                if (decimal.TryParse(searchValue, out number))
                {
                    var val = decimal.Parse(searchValue);
                    customerData = customerData.Where(m => m.Percentage == val || m.Percentage > val);
                }
                else
                {
                    customerData = customerData.Where(m => m.StudentName.StartsWith(searchValue));
                }

                if (!string.IsNullOrEmpty(Searchschool))
                {
                    var schoolid = int.Parse(Searchschool);
                    customerData = customerData.Where(m => m.SchoolID == schoolid);
                    //customerData = customerData.Where(m => m.StudentName.Contains(searchValue));
                    //customerData = customerData.Where(m => m.SchoolName.StartsWith(empName));
                }

                //if (filter_data != "" )
                //{
                //    customerData= customerData.Where(m =>  DbFunctions.TruncateTime(m.CreatedAt) > DateTimeOffset.Parse(filter_data));
                //}

                //if (!string.IsNullOrEmpty(SearchDate))
                //{
                //    DateTime SrchDate = Convert.ToDateTime(SearchDate);
                //    customerData = customerData.Where(m => m.CreatedAt <= SrchDate).OrderBy(m => m.CreatedAt);

                //}

                //if (!string.IsNullOrEmpty(SearchDate))
                //{
                //    var SrchDate = Convert.ToDateTime(SearchDate);
                //    customerData = customerData.Where(m => DbFunctions.TruncateTime(m.CreatedAt) > SrchDate).OrderBy(m => m.CreatedAt);

                //}
                //if (RecomendStatues == "recomendation")
                //{
                //    customerData = customerData.Where(a => a.ID ==100);
                //}
                //total number of rows count     
                recordsTotal = customerData.Count();
                //Paging     
                    var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            try
            {

                var Std = Db.StudentDegrees_kw.Where(a => a.ID == ID).FirstOrDefault();

                var Schools = Db.StudentDegrees_kw.Select(s => new { SchoolID = s.SchoolID, SchoolName = s.SchoolName }).Distinct().ToList();

                ViewBag.school = new SelectList(Schools, "SchoolID", "SchoolName");
                return View(Std);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult Edit(StudentDegrees_kw Std)
        {
            if (ModelState.IsValid)
            {

                var school = Db.StudentDegrees_kw.Select(a =>
                new { SchoolID = a.SchoolID, SchoolName = a.SchoolName }).Where(a => a.SchoolID == Std.SchoolID).FirstOrDefault();

                Std.SchoolName = school.SchoolName;
                Db.Entry(Std).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChangesAsync();
                return RedirectToAction("ShowGrid", "Home");
            }
            else
            {
                var Schools = Db.StudentDegrees_kw.Select(s => new { SchoolID = s.SchoolID, SchoolName = s.SchoolName }).Distinct().ToList();
                ViewBag.school = new SelectList(Schools, "SchoolID", "SchoolName");
                return View(Std);
            }


        }

        [HttpPost]
        public JsonResult DeleteStudent(int? ID)
        {

            var customer = Db.StudentDegrees_kw.Find(ID);

           
            //using (DbContextTransaction dbtran = Db.Database.BeginTransaction())
            //{
            //    try
            //    {

            //        var c1 = Db.StudentDegrees_kw.Where(ww => ww.ID == 20701).FirstOrDefault();
            //        var c2 = Db.StudentDegrees_kw.Where(ww => ww.ID == 20702).FirstOrDefault();
            //        var c = Db.StudentDegrees_kw.Where(ww => ww.ID == 20703).FirstOrDefault();


            //        Db.StudentDegrees_kw.Remove(c1);
            //        Db.StudentDegrees_kw.Remove(c2);
            //        Db.StudentDegrees_kw.Remove(c);
            //        Db.SaveChanges();

            //        dbtran.Commit();
            //    }
            //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            //    {
            //        dbtran.Rollback();
            //        throw;
            //    }
            //}


            if (ID == null)
                return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
            Db.StudentDegrees_kw.Remove(customer);
            Db.SaveChanges();

            return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var Schools = Db.StudentDegrees_kw.Select(s => new { SchoolID = s.SchoolID, SchoolName = s.SchoolName }).Distinct().ToList();
            ViewBag.school = new SelectList(Schools, "SchoolID", "SchoolName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(StudentDegrees_kw std)
        {
            if (ModelState.IsValid)
            {
                var school = Db.StudentDegrees_kw.Select(a =>
         new { SchoolID = a.SchoolID, SchoolName = a.SchoolName }).Where(a => a.SchoolID == std.SchoolID).FirstOrDefault();

                std.SchoolName = school.SchoolName;
                Db.StudentDegrees_kw.Add(std);
                Db.SaveChanges();

                return RedirectToAction("ShowGrid", "Home");

            }
            else
            {
                var Schools = Db.StudentDegrees_kw.Select(s => new { SchoolID = s.SchoolID, SchoolName = s.SchoolName }).Distinct().ToList();
                ViewBag.school = new SelectList(Schools, "SchoolID", "SchoolName");
                return View();
            }


        }
        [HttpGet]
        public ActionResult add()
        {
            return View();
        }

        public JsonResult Get()
        {
         
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Context"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [Id],[CusId],[CusName]
                    FROM [dbo].[CustomerInfo] WHERE [Status] <> 0", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    var listCus = reader.Cast<IDataRecord>()
                            .Select(x => new
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CusId = reader["CusId"].ToString(),
                                CusName = reader["CusName"].ToString(),
                                //notification = reader["notification"].ToString(),
                            }).ToList();

                    return Json(new { listCus = listCus }, JsonRequestBehavior.AllowGet);

                }
            }
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            MyHub.trysignalR();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ChckEmail(string Email, int? ID)
        {
            if (ID == null)
            {
                return Json(!Db.StudentDegrees_kw.Any(s => s.Email.ToLower() == Email.ToLower()), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var EditStd = Json(Db.StudentDegrees_kw.Any(s => s.Email.ToLower() == Email.ToLower()), JsonRequestBehavior.AllowGet);
                return Json(Db.StudentDegrees_kw.Any(s => s.Email.ToLower() == Email.ToLower()), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsAlreadyExistEmail(string Email, int? ID)
        {
            bool status;

            var ChckEmail = Db.StudentDegrees_kw.Where(a => a.Email.ToLower() == Email.ToLower()).FirstOrDefault();
            var Std = Db.StudentDegrees_kw.Where(s => s.ID == ID).FirstOrDefault();
            if (Std != null)
            {

                if (Std.Email == Email)
                {
                    //Case Modify Edit Student   
                    status = true;
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }

            if (ChckEmail != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult pdf(string Reporttype)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/FirsrReport.rdlc");

            ReportParameter parameter = new ReportParameter("RP1", "عنوان التقرير ");
            localReport.SetParameters(new ReportParameter[] { parameter });

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DsStudents";
            reportDataSource.Value = Db.StudentDegrees_kw.Take(10);
            localReport.DataSources.Add(reportDataSource);

            string reporttype = Reporttype;
            string mimeType;
            string encoding;
            string filenameExtension;
            if (Reporttype == "pdf")
            {
                filenameExtension = "pdf";
            }
            else
            {
                filenameExtension = "jpg";

            }

            string[] streams;
            Warning[] warnings;
            byte[] renderBytes;
            renderBytes = localReport.Render(reporttype, "", out mimeType, out encoding, out filenameExtension, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment;filename =student_report." + filenameExtension);
            return File(renderBytes, mimeType);
            //return View();
        }
        public ActionResult PrintPdf(string Reporttype)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/FirsrReport.rdlc");
            ReportParameter parameter = new ReportParameter("RP1", "عنوان التقرير ");
            localReport.SetParameters(new ReportParameter[] { parameter });

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension = "pdf";

            //The DeviceInfo settings should be changed based on the reportType

            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx


            string deviceInfo ="";
            Warning[] warnings;

            string[] streams;

            byte[] renderedBytes;


            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DsStudents";
            reportDataSource.Value = Db.StudentDegrees_kw.Take(10);
            localReport.DataSources.Add(reportDataSource);
            //Render the report

            renderedBytes = localReport.Render(reportType,deviceInfo,out mimeType,out encoding, out fileNameExtension, out streams,out warnings);

            //var doc = new Document();
            var reader = new PdfReader(renderedBytes); 

            using (FileStream fs = new FileStream(Server.MapPath(@"~/Reports/Summary.pdf"), FileMode.Create))
            {
                PdfStamper stamper = new PdfStamper(reader, fs);
                string Printer = "";
                if (Printer == null || Printer == "")
                {
                    stamper.JavaScript = "var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = getPrintParams().printerName;print(pp);\r";
                    stamper.Close();
                }
                else
                {
                    stamper.JavaScript = "var pp = getPrintParams();pp.interactive = pp.constants.interactionLevel.automatic;pp.printerName = " + Printer + ";print(pp);\r";
                    stamper.Close();
                }
            }
            reader.Close();

            FileStream fss = new FileStream(Server.MapPath("~/Reports/Summary.pdf"), FileMode.Open);
            byte[] bytes = new byte[fss.Length];
            fss.Read(bytes, 0, Convert.ToInt32(fss.Length));
            fss.Close();
            System.IO.File.Delete(Server.MapPath("~/Reports/Summary.pdf"));
            return File(bytes, "application/pdf");
        }


    }
}
