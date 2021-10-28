using System.Web.Mvc;
using C1ASPRMS.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;
using System.Text;

namespace C1ASPRMS.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LiveStatus()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
            public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Adminprocessing()
        {
            string status = "";
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            String a = Request["username"].ToString();
            String b = Request["pass"].ToString();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from admin where username=@username and password=@password"))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.AddWithValue("@username", a);
                cmd1.Parameters.AddWithValue("@password", b);
                SqlDataReader sdr1 = cmd1.ExecuteReader();

                if (sdr1.HasRows)
                {
                    sdr1.Read();
                    status = "Login successful.";
                    String email = (String)sdr1["email"];
                    String username = (String)sdr1["username"];
                    Session["email"] = email;
                    Session["username"] = username;

                    return RedirectToActionPermanent("FullBookingHistory", "Home");
                }
                else
                {
                    TempData["adminmsg"] = "INVALID USERNAME/PASSWORD";
                    return RedirectToActionPermanent("AdminLogin", "Home");
                }

            }
        }
        [HttpPost]
        public ActionResult AddContact()
        {
            String status = "";
            String name = Request["name"].ToString();
            String email = Request["email"].ToString();
            String fd = Request["fd"].ToString();
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "INSERT INTO contact(name,email,query) VALUES(@name,@email,@query)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@query", fd);

                    status = (cmd.ExecuteNonQuery() >= 1) ? "Your Query sucessfully registered with us!" : "Technical Failure!";

                    if (status.Equals("Your Query sucessfully registered with us!"))
                    {
                        using (MailMessage mm = new MailMessage("irctc@narender.com", email))
                        {
                            mm.Subject = "IRCTC CUSTOMER CARE";
                            mm.Body = "DEAR IRCTC USER , " + name + " YOU HAVE RECENTLY MADE THE QUERY , WE WILL SOLVE REPORT TO YOU IF NOT REPORT TO US THANK YOU REGRADS FROM NARENDER KESWANI - SYBVOCSD IRCTC MVC 5 ASP.NET";
                            mm.IsBodyHtml = false;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = 587;
                            smtp.Send(mm);

                        }
                    }
                }

            }
            TempData["fd"] = status;
            return RedirectToActionPermanent("ContactUs", "Home");
        }
        public ActionResult ViewContact(ViewContactUs vcu)
        {
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            var model = new List<ViewContactUs>();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from contact"))
            {
                cmd1.Connection = con;
                con.Open();
               
                SqlDataReader rdr = cmd1.ExecuteReader();

                while (rdr.Read())
                {

                    var records = new ViewContactUs();
                    records.name = (String)rdr["name"];
                    records.email = (String)rdr["email"];
                    records.fd = (String)rdr["query"];

                    model.Add(records);
                }
            }
            return View(model);
        }

        public ActionResult FullBookingHistory(FullBookingHistory fbh)
        {
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            var model = new List<FullBookingHistory>();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from trainbooking "))
            {
                cmd1.Connection = con;
                con.Open();
                SqlDataReader rdr = cmd1.ExecuteReader();

                while (rdr.Read())
                {
                    var records = new FullBookingHistory();
                    records.pnr = (Int32)rdr["pnr"];
                    records.trainno = (String)rdr["trainno"];
                    records.doj = (String)rdr["doj"];
                    records.source = (String)rdr["source"];
                    records.destination = (String)rdr["destination"];
                    records.classes = (String)rdr["class"];
                    records.bookingdate = (String)rdr["bookingdate"];
                    records.bs = (String)rdr["bs"];
                    records.pay = (String)rdr["pay"];
                    records.tfare = (Int32)rdr["tfare"];
                    if (!rdr["p1"].Equals("NA"))
                    {
                        records.p1 = (String)rdr["p1"];
                        records.s1 = (String)rdr["s1"];
                        records.q1 = (String)rdr["q1"];
                    }
                    else
                    {
                        records.p1 = null;
                        records.s1 = null;
                        records.q1 = null;
                    }
                    if (!rdr["p2"].Equals("NA"))
                    {
                        records.p2 = (String)rdr["p2"];
                        records.s2 = (String)rdr["s2"];
                        records.q2 = (String)rdr["q2"];
                    }
                    else
                    {
                        records.p2 = null;
                        records.s2 = null;
                        records.q2 = null;
                    }
                    if (!rdr["p3"].Equals("NA"))
                    {
                        records.p3 = (String)rdr["p3"];
                        records.s3 = (String)rdr["s3"];
                        records.q3 = (String)rdr["q3"];
                    }
                    else
                    {
                        records.p3 = null;
                        records.s3 = null;
                        records.q3 = null;
                    }
                    if (!rdr["p4"].Equals("NA"))
                    {
                        records.p4 = (String)rdr["p4"];
                        records.s4 = (String)rdr["s4"];
                        records.q4 = (String)rdr["q4"];
                    }
                    else
                    {
                        records.p4 = null;
                        records.s4 = null;
                        records.q4 = null;
                    }
                    if (!rdr["p5"].Equals("NA"))
                    {
                        records.p5 = (String)rdr["p5"];
                        records.s5 = (String)rdr["s5"];
                        records.q5 = (String)rdr["q5"];
                    }
                    else
                    {
                        records.p5 = null;
                        records.s5 = null;
                        records.q5 = null;
                    }
                    if (!rdr["p6"].Equals("NA"))
                    {
                        records.p6 = (String)rdr["p6"];
                        records.s6 = (String)rdr["s6"];
                        records.q6 = (String)rdr["q6"];
                    }
                    else
                    {
                        records.p6 = null;
                        records.s6 = null;
                        records.q6 = null;
                    }


                    model.Add(records);
                }
            
            }
                return View(model);
            }
        public ActionResult LogOut()
        {
            Session.Abandon();
            String status = "YOU ARE SUCESSFULLY LOGOUT!";
            TempData["logout"] = status;
            return RedirectToActionPermanent("Login", "Home");
        }
        public ActionResult BookingHistory(BookingHistory bh)
        {
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            String un = Session["username"].ToString();
            var model = new List<BookingHistory>();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from trainbooking where username=@u "))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.AddWithValue("@u", un);
                SqlDataReader rdr = cmd1.ExecuteReader();

               while(rdr.Read())
                { 
                    var records = new BookingHistory();
                    records.pnr = (Int32)rdr["pnr"];
                    records.trainno = (String)rdr["trainno"];
                    records.doj = (String)rdr["doj"];
                    records.source = (String)rdr["source"];
                    records.destination = (String)rdr["destination"];
                    records.classes = (String)rdr["class"];
                    records.bookingdate = (String)rdr["bookingdate"];
                    records.bs = (String)rdr["bs"];
                    records.pay = (String)rdr["pay"];
                    records.tfare = (Int32)rdr["tfare"];
                    if (!rdr["p1"].Equals("NA"))
                    {
                        records.p1 = (String)rdr["p1"];
                        records.s1 = (String)rdr["s1"];
                        records.q1 = (String)rdr["q1"];
                    }
                    else
                    {
                        records.p1 = null;
                        records.s1 = null;
                        records.q1 = null;
                    }
                    if (!rdr["p2"].Equals("NA"))
                    {
                        records.p2 = (String)rdr["p2"];
                        records.s2 = (String)rdr["s2"];
                        records.q2 = (String)rdr["q2"];
                    }
                    else
                    {
                        records.p2 = null;
                        records.s2 = null;
                        records.q2 = null;
                    }
                    if (!rdr["p3"].Equals("NA"))
                    {
                        records.p3 = (String)rdr["p3"];
                        records.s3 = (String)rdr["s3"];
                        records.q3 = (String)rdr["q3"];
                    }
                    else
                    {
                        records.p3 = null;
                        records.s3 = null;
                        records.q3 = null;
                    }
                    if (!rdr["p4"].Equals("NA"))
                    {
                        records.p4 = (String)rdr["p4"];
                        records.s4 = (String)rdr["s4"];
                        records.q4 = (String)rdr["q4"];
                    }
                    else
                    {
                        records.p4 = null;
                        records.s4 = null;
                        records.q4 = null;
                    }
                    if (!rdr["p5"].Equals("NA"))
                    {
                        records.p5 = (String)rdr["p5"];
                        records.s5 = (String)rdr["s5"];
                        records.q5 = (String)rdr["q5"];
                    }
                    else
                    {
                        records.p5 = null;
                        records.s5 = null;
                        records.q5 = null;
                    }
                    if (!rdr["p6"].Equals("NA"))
                    {
                        records.p6 = (String)rdr["p6"];
                        records.s6 = (String)rdr["s6"];
                        records.q6 = (String)rdr["q6"];
                    }
                    else
                    {
                        records.p6 = null;
                        records.s6 = null;
                        records.q6 = null;
                    }

                    model.Add(records);

                }
           
                return View(model);

            }
        }
        [HttpPost]
        public ActionResult DeleteTicketProcessing()
        {
            string status = "";
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            String pnr= Request.Form["delt"].ToString();
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "UPDATE trainbooking set bs=@bs where pnr=@pnr";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@bs", "CANCELLED");
                    cmd.Parameters.AddWithValue("@pnr", pnr);

                    status = (cmd.ExecuteNonQuery() >= 1) ? "Ticket DELETED sucessfully and REFUND WILL BE SOON IN BE PROCESS!" : "TECHNICAL ERROR!";
                }
                TempData["del"] = status;
                return RedirectToActionPermanent("DeleteTickets", "Home");
            }
        }
        public ActionResult DeleteTickets(DeleteView dv)
        {
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            String un = Session["username"].ToString();
            var model = new List<DeleteView>();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from trainbooking where username=@u and bs=@bs"))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.AddWithValue("@u", un);
                cmd1.Parameters.AddWithValue("@bs", "BOOKED");
                SqlDataReader rdr = cmd1.ExecuteReader();

                while(rdr.Read())
                {
                    var records = new DeleteView();
                    records.pnr = (Int32)rdr["pnr"];
                    records.trainno = (String)rdr["trainno"];
                    records.doj = (String)rdr["doj"];
                    records.source = (String)rdr["source"];
                    records.destination = (String)rdr["destination"];
                    records.classes = (String)rdr["class"];
                    records.bookingdate = (String)rdr["bookingdate"];
                    records.bs = (String)rdr["bs"];
                    records.pay = (String)rdr["pay"];
                    records.tfare = (Int32)rdr["tfare"];
                    if (!rdr["p1"].Equals("NA"))
                    {
                        records.p1 = (String)rdr["p1"];
                        records.s1 = (String)rdr["s1"];
                        records.q1 = (String)rdr["q1"];
                    }
                    else
                    {
                        records.p1 = null;
                        records.s1 = null;
                        records.q1 = null;
                    }
                    if (!rdr["p2"].Equals("NA"))
                    {
                        records.p2 = (String)rdr["p2"];
                        records.s2 = (String)rdr["s2"];
                        records.q2 = (String)rdr["q2"];
                    }
                    else
                    {
                        records.p2 = null;
                        records.s2 = null;
                        records.q2 = null;
                    }
                    if (!rdr["p3"].Equals("NA"))
                    {
                        records.p3 = (String)rdr["p3"];
                        records.s3 = (String)rdr["s3"];
                        records.q3 = (String)rdr["q3"];
                    }
                    else
                    {
                        records.p3 = null;
                        records.s3 = null;
                        records.q3 = null;
                    }
                    if (!rdr["p4"].Equals("NA"))
                    {
                        records.p4 = (String)rdr["p4"];
                        records.s4 = (String)rdr["s4"];
                        records.q4 = (String)rdr["q4"];
                    }
                    else
                    {
                        records.p4 = null;
                        records.s4 = null;
                        records.q4 = null;
                    }
                    if (!rdr["p5"].Equals("NA"))
                    {
                        records.p5 = (String)rdr["p5"];
                        records.s5 = (String)rdr["s5"];
                        records.q5 = (String)rdr["q5"];
                    }
                    else
                    {
                        records.p5 = null;
                        records.s5 = null;
                        records.q5 = null;
                    }
                    if (!rdr["p6"].Equals("NA"))
                    {
                        records.p6 = (String)rdr["p6"];
                        records.s6 = (String)rdr["s6"];
                        records.q6 = (String)rdr["q6"];
                    }
                    else
                    {
                        records.p6 = null;
                        records.s6 = null;
                        records.q6 = null;
                    }

                    model.Add(records);
                    

                }

                return View(model);
            }
        }
        [HttpPost]
        public ActionResult ChangePwdProcessing()
        {
            String status = "";
            String username = Request["username"].ToString();
            String oldpwd = Request["oldpass"].ToString();
            String newpwd = Request["newpass"].ToString();
            String email = Session["email"].ToString();
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            using (SqlConnection con = new SqlConnection(connection))
            {

                string query = "select * from users where username = @username and password = @oldpwd";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@oldpwd", oldpwd);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        using (SqlConnection con1 = new SqlConnection(connection))
                        {
                            string query2 = "update users set password=@newpwd where username=@username";
                            using (SqlCommand cmd2 = new SqlCommand(query2))
                            {
                                cmd2.Connection = con1;
                                con1.Open();

                                cmd2.Parameters.AddWithValue("@username", username);
                                cmd2.Parameters.AddWithValue("@newpwd", newpwd);

                                status = (cmd2.ExecuteNonQuery() >= 1) ? "Password Changed Successfully!" : "Invalid Username/Password!";
                                

                                if(status.Equals("Password Changed Successfully!"))
                                {
                                    using (MailMessage mm = new MailMessage("irctc@narender.com", email))
                                    {
                                        mm.Subject = "IRCTC FORGOT PASSWORD OTP";
                                        mm.Body = "DEAR IRCTC USER , " + username + " YOU HAVE RECENTLY CHANGE THE PASSWORD , IF NOT REPORT TO US THANK YOU REGRADS FROM NARENDER KESWANI - SYBVOCSD IRCTC MVC 5 ASP.NET";
                                        mm.IsBodyHtml = false;
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.Host = "smtp.gmail.com";
                                        smtp.EnableSsl = true;
                                        NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                                        smtp.UseDefaultCredentials = true;
                                        smtp.Credentials = NetworkCred;
                                        smtp.Port = 587;
                                        smtp.Send(mm);

                                    }
                                }
                            }

                        }
                    }
                }
            }
            TempData["chgpwd"] =status;
            return RedirectToActionPermanent("ChangePassword","Home");
        }
        public ActionResult CancelledTrains()
        {
            return View();
        }
        public ActionResult registerIRCTC()
        {
            return View();
        }
        public ActionResult AddCancelledTrains()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CancelProcessing(AddCancelledTrains act)
        {
            String status = "";
            String tnoo = Request["tnoo"].ToString();
            String cdt = Request["cdt"].ToString();
            String st = Request["st"].ToString();
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "INSERT INTO cancelledtrains(trainno,dt,status) VALUES(@tnno,@cdt,@st)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@tnno", tnoo);
                    cmd.Parameters.AddWithValue("@cdt", cdt);
                    cmd.Parameters.AddWithValue("@st", st);

                    status = (cmd.ExecuteNonQuery() >= 1) ? "Train cancelled Successfully!" : "Train NOT cancelled Successfully!";

                }

            }
            TempData["cp"] = status;
            return RedirectToActionPermanent("CancelledTrains", "Home");
        }
        [HttpPost]
        public ActionResult CheckCancelledTrains(CancelTrain ct)
        {
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            String cdt = Request["cdt"].ToString();
            var model = new List<CancelTrain>();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from cancelledtrains where dt=@cdt and status=@s"))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.AddWithValue("@cdt", cdt);
                cmd1.Parameters.AddWithValue("@s", "CANCELLED");
                SqlDataReader rdr = cmd1.ExecuteReader();

                while (rdr.Read())
                {

                    var records = new CancelTrain();
                    records.thetrain = (String)rdr["trainno"];
                    records.thedate = (String)rdr["dt"];

                    model.Add(records);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckPNRStatus(ViewPNR vp)
        {
        
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            var model = new List<ViewPNR>();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from trainbooking where pnr=@pnr"))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.AddWithValue("@pnr", vp.pnr);
                SqlDataReader rdr = cmd1.ExecuteReader();

                while (rdr.Read())
                {

                    var records = new ViewPNR();
                    records.pnr = (Int32)rdr["pnr"];
                    records.trainno = (String)rdr["trainno"];
                    records.doj = (String)rdr["doj"];
                    records.source = (String)rdr["source"];
                    records.destination = (String)rdr["destination"];
                    records.classes = (String)rdr["class"];
                    records.bookingdate = (String)rdr["bookingdate"];
                    records.bs = (String)rdr["bs"];
                    if (!rdr["p1"].Equals("NA"))
                    {
                        records.p1 = (String)rdr["p1"];
                        records.s1 = (String)rdr["s1"];
                        records.q1 = (String)rdr["q1"];
                    }
                    else
                    {
                        records.p1 = null;
                        records.s1 = null;
                        records.q1 = null;
                    }
                    if (!rdr["p2"].Equals("NA"))
                    {
                        records.p2 = (String)rdr["p2"];
                        records.s2 = (String)rdr["s2"];
                        records.q2 = (String)rdr["q2"];
                    }
                    else
                    {
                        records.p2 = null;
                        records.s2 = null;
                        records.q2 = null;
                    }
                    if (!rdr["p3"].Equals("NA"))
                    {
                        records.p3 = (String)rdr["p3"];
                        records.s3 = (String)rdr["s3"];
                        records.q3 = (String)rdr["q3"];
                    }
                    else
                    {
                        records.p3 = null;
                        records.s3 = null;
                        records.q3 = null;
                    }
                    if (!rdr["p4"].Equals("NA"))
                    {
                        records.p4 = (String)rdr["p4"];
                        records.s4 = (String)rdr["s4"];
                        records.q4 = (String)rdr["q4"];
                    }
                    else
                    {
                        records.p4 = null;
                        records.s4 = null;
                        records.q4 = null;
                    }
                    if (!rdr["p5"].Equals("NA"))
                    {
                        records.p5 = (String)rdr["p5"];
                        records.s5 = (String)rdr["s5"];
                        records.q5 = (String)rdr["q5"];
                    }
                    else
                    {
                        records.p5 = null;
                        records.s5 = null;
                        records.q5 = null;
                    }
                    if (!rdr["p6"].Equals("NA"))
                    {
                        records.p6 = (String)rdr["p6"];
                        records.s6 = (String)rdr["s6"];
                        records.q6 = (String)rdr["q6"];
                    }
                    else
                    {
                        records.p6 = null;
                        records.s6 = null;
                        records.q6 = null;
                    }

                    model.Add(records);
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(USER u)
        {
            string status = "";
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            using (SqlConnection con = new SqlConnection(connection))
            { 
                string query = "INSERT INTO users(username,password,sq,sa,fn,mn,ln,gender,ms,dob,occ,mob,email,nationality,country,address,state,pin) VALUES(@username,@password,@sq,@sa,@fn,@mn,@ln,@gender,@ms,@dob,@occ,@mob,@email,@nationality,@country,@address,@state,@pin)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con; 
                    con.Open();

                    cmd.Parameters.AddWithValue("@username", u.username);
                    cmd.Parameters.AddWithValue("@password", u.password);
                    cmd.Parameters.AddWithValue("@sq", u.sq);
                    cmd.Parameters.AddWithValue("@sa", u.sa);
                    cmd.Parameters.AddWithValue("@fn", u.fn);
                    cmd.Parameters.AddWithValue("@mn", u.mn);
                    cmd.Parameters.AddWithValue("@ln", u.ln);
                    cmd.Parameters.AddWithValue("@gender", u.gender);
                    cmd.Parameters.AddWithValue("@ms", u.ms);
                    cmd.Parameters.AddWithValue("@dob", u.dob);
                    cmd.Parameters.AddWithValue("@occ", u.occ);
                    cmd.Parameters.AddWithValue("@mob", u.mob);
                    cmd.Parameters.AddWithValue("@email", u.email);
                    cmd.Parameters.AddWithValue("@nationality", u.nationality);
                    cmd.Parameters.AddWithValue("@country", u.country);
                    cmd.Parameters.AddWithValue("@address", u.address);
                    cmd.Parameters.AddWithValue("@state", u.state);
                    cmd.Parameters.AddWithValue("@pin", u.pin);
                    status = (cmd.ExecuteNonQuery() >= 1) ? "You are sucessfully registered with us!" : "TECHNICAL ERROR";

                    if (status.Equals("You are sucessfully registered with us!"))
                    {
                        using (MailMessage mm = new MailMessage("irctc@narender.com", u.email))
                        {
                            mm.Subject = "IRCTC New User Confirmation";
                            mm.Body = "WECOME TO IRCTC, " + u.username + "You are sucessfully registered with us REGARDS FROM NARENDER KESWANI SYBVOCSD ASP-NET MVC5";

                            /*if (fuAttachment.HasFile)
                            {
                                string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                                mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                            }*/
                            mm.IsBodyHtml = false;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = 587;
                            smtp.Send(mm);
                           
                        }   
                    }
                }
                TempData["save"] =status;
                return RedirectToActionPermanent("registerIRCTC","Home");
            }
        }
        [HttpPost]
        public ActionResult LoginProcessing(Login l)
        {
            string status = "";
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from users where username=@username and password=@password"))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.AddWithValue("@username", l.username);
                cmd1.Parameters.AddWithValue("@password", l.pass);
                //Session["username"] = l.username;
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                 
                if (sdr1.HasRows)
                {
                    sdr1.Read();
                    status = "Login successful.";
                    String email = (String)sdr1["email"];
                    String username = (String)sdr1["username"];
                    String fn = (String)sdr1["fn"];
                    String mn = (String)sdr1["mn"];
                    String ln = (String)sdr1["ln"];
                    String tn = fn + " " + mn + " " + ln;
                    Session["email"] = email;
                    Session["username"] = username;
                    Session["name"] = tn;
            
                    return RedirectToActionPermanent("BookTicket", "Home");
                }
                else
                {
                    status = "Invalid username or password.";
                    TempData["login"] = status;
                    return RedirectToActionPermanent("Login", "Home");
                }

            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPasswordProcessing()
        {
            String username = Request["usr"].ToString();
            Session["usr"] = username;
            string status = "";
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from users where username=@username"))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.AddWithValue("@username", username);
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                if (sdr1.HasRows)
                {
                    sdr1.Read();
                    String email = (String)sdr1["email"];
                    Random r = new Random();
                    int myotp = r.Next(1456, 6556);
                    Session["myotp"] = myotp;
                    using (MailMessage mm = new MailMessage("irctc@narender.com",email))
                    {
                        mm.Subject = "IRCTC FORGOT PASSWORD OTP";
                        mm.Body = "DEAR IRCTC USER , " + username + " YOUR OTP FOR RESET PASSWORD IS "+myotp;
                        mm.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                        
                    }
                    return RedirectToActionPermanent("OtpVerify", "Home");
                }
                else
                {
                    status = "Invalid Username";
                    TempData["fp"] = status;
                    return RedirectToActionPermanent("ForgotPassword", "Home");
                }

            }

        }
        public ActionResult OtpVerify()
        {
            return View();
        }
        [HttpPost]
      public ActionResult OtpVerfication()
        {
            String status="";
            Int32 otp = Convert.ToInt32(Request["myotp"]);
            Int32 myotp = Convert.ToInt32(Session["myotp"]);
            if(otp.Equals(myotp))
            {
                return RedirectToActionPermanent("NewPassword", "Home");
            }
            else
            {
                status = "Incorect OTP ! Please Try Again!";
                TempData["iotp"] = status;
                return RedirectToActionPermanent("OtpVerify", "Home");
            }
        }
        public ActionResult NewPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewPasswordProcessing()
        {
            String status = "";
            String username = Session["usr"].ToString();
            String newpass = Request["newpass"].ToString();
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            using (SqlConnection con = new SqlConnection(connection))
            {

                string query = "select * from users where username = @username";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        using (SqlConnection con1 = new SqlConnection(connection))
                        {
                            string query2 = "update users set password=@newpwd where username=@username";
                            using (SqlCommand cmd2 = new SqlCommand(query2))
                            {
                                cmd2.Connection = con1;
                                con1.Open();

                                cmd2.Parameters.AddWithValue("@username", username);
                                cmd2.Parameters.AddWithValue("@newpwd", newpass);

                                status = (cmd2.ExecuteNonQuery() >= 1) ? "Password Changed Successfully!" : "Invalid Username/Password!";

                            }

                        }
                    }
                }
            }
            TempData["nwpwd"]=status;
            return RedirectToActionPermanent("NewPassword","Home");

        }
        public ActionResult PNR()
        { 

            return View();
        }
        public ActionResult BookTicket()
        {
            return View();
        }
      
        public ActionResult ViewUsers(ViewUsers vs)
        {
            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            var model = new List<ViewUsers>();
            using (SqlConnection con = new SqlConnection(connection))
            using (SqlCommand cmd1 = new SqlCommand("select * from users"))
            {
                cmd1.Connection = con;
                con.Open();
                SqlDataReader rdr = cmd1.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                {
                    i++;
                    var records = new ViewUsers();
                    records.srno = i;
                    records.address = (String)rdr["address"];
                    records.country = (String)rdr["country"];
                    records.dob = (String)rdr["dob"];
                    records.email = (String)rdr["email"];
                    records.fn = (String)rdr["fn"];
                    records.gender = (String)rdr["gender"];
                    records.ln = (String)rdr["ln"];
                    records.mn = (String)rdr["mn"];
                    records.mob = (Int64)rdr["mob"];
                    records.nationality = (String)rdr["nationality"];
                    records.pin = (String)rdr["pin"];
                    records.state = (String)rdr["state"];
                    records.username = (String)rdr["username"];

                    model.Add(records);
                }
            }
            return View(model);
        }
        public ActionResult TrainRoutes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SeatAvail(SeatAvailability sa)
        {
            String tno = Request["tno"].ToString();
            String dt = Request["dt"].ToString();
            String source = Request["source"].ToString();
            String destination = Request["destination"].ToString();
            String classes = Request["classes"].ToString();

            string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            var model1 = new List<SeatAvailability>();
            string query1 = "select * from seats where trainno=@tno and doj=@dt";
            using (SqlConnection con = new SqlConnection(connection))
     
            using (SqlCommand cmd1 = new SqlCommand(query1))
            {
                cmd1.Connection = con;
                con.Open();
                cmd1.Parameters.Add(new SqlParameter("@tno", sa.tno));
                cmd1.Parameters.Add(new SqlParameter("@dt", sa.dt));
                SqlDataReader rdr = cmd1.ExecuteReader();

                while (rdr.Read())
                {
                    var records = new SeatAvailability();
                    if (classes.Equals("SL"))
                    {
                        records.avail = (Int32)rdr.GetInt32(3);
                    }
                    else if (classes.Equals("3A"))
                    {
                        records.avail = (Int32)rdr.GetInt32(4);
                    }
                    else if (classes.Equals("2A"))
                    {
                        records.avail = (Int32)rdr.GetInt32(5);
                    }
                    else if (classes.Equals("1A"))
                    {
                        records.avail = (Int32)rdr.GetInt32(4);
                    }
                    else if (classes.Equals("2S"))
                    {
                        records.avail = (Int32)rdr.GetInt32(4);
                    }
                    else if (classes.Equals("CC"))
                    {
                        records.avail = (Int32)rdr.GetInt32(4);
                    }
                    records.classes = classes;
                    records.destination = destination;
                    records.dt = dt;
                    records.source = source;
                    records.tno = tno;
                    Session["classes"] = classes;
                    Session["source"] = source;
                    Session["destination"] = destination;
                    Session["tno"] = tno;
                    Session["dt"] = dt;
                    model1.Add(records);
                }
            }
            return View(model1);
        }
        public ActionResult TicketFilling()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TicketProcessing(TicketFilling tf)
        {
            String status = "";
            Random rnd = new Random();
            int pnrgenerator = rnd.Next(111111111, 999999999);
            int faressl = rnd.Next(300, 1150);
            int fares3a = rnd.Next(750, 1950);
            int fares2a = rnd.Next(1150, 2450);
            int fares1a = rnd.Next(1450, 2950);
            int farescc = rnd.Next(575, 1275);
            int fares2s = rnd.Next(250, 550);
            int seats1 = rnd.Next(1, 72);
            int seats2 = rnd.Next(1, 72);
            int seats3 = rnd.Next(1, 72);
            int seats4 = rnd.Next(1, 72);
            int seats5 = rnd.Next(1, 72);
            int seats6 = rnd.Next(1, 72);
            int coach = rnd.Next(1, 9);
            String s1 = "COACH " + coach + " " + seats1;
            String s2 = "COACH " + coach + " " + seats2;
            String s3 = "COACH " + coach + " " + seats3;
            String s4 = "COACH " + coach + " " + seats4;
            String s5 = "COACH " + coach + " " + seats5;
            String s6 = "COACH " + coach + " " + seats6;
            DateTime bookingTime = DateTime.Now;
            String p1 = Request["p1"].ToString();
            String p2 = Request["p2"].ToString();
            String p3 = Request["p3"].ToString();
            String p4 = Request["p4"].ToString();
            String p5 = Request["p5"].ToString();
            String p6 = Request["p6"].ToString();
            String a1 = Request["a1"].ToString();
            String a2 = Request["a2"].ToString();
            String a3 = Request["a3"].ToString();
            String a4 = Request["a4"].ToString();
            String a5 = Request["a5"].ToString();
            String a6 = Request["a6"].ToString();
            String b1 = tf.b1;// Request["b1"].ToString();
            String b2 = tf.b2;// Request["b2"].ToString();
            String b3 = tf.b3;//Request["b3"].ToString();
            String b4 = tf.b4;//Request["b4"].ToString();
            String b5 = tf.b5;//Request["b5"].ToString();
            String b6 = tf.b6;// Request["b6"].ToString();
            String g1 = tf.g1;// Request["g1"].ToString();
            String g2 = tf.g2;// Request["g2"].ToString();
            String g3 = tf.g3;// Request["g3"].ToString();
            String g4 = tf.g4;// Request["g4"].ToString();
            String g5 = tf.g5;// Request["g5"].ToString();
            String g6 = tf.g6;//Request["g6"].ToString();
            String q1 = tf.q1;//Request["q1"].ToString();
            String q2 = tf.q2;//Request["q2"].ToString();
            String q3 = tf.q3;//Request["q3"].ToString();
            String q4 = tf.q4;//Request["q4"].ToString();
            String q5 = tf.q5;//Request["q5"].ToString();
            String q6 = tf.q6;//Request["q6"].ToString();
            String payment = Request["pay"].ToString();
            String emm = tf.emm; //Request["emm"].ToString();
            String mob = tf.mob;
            String classes = Session["classes"].ToString();
            String source = Session["source"].ToString();
            String destination = Session["destination"].ToString();
            String tno = Session["tno"].ToString();
            String dt = Session["dt"].ToString();
            String un = Session["username"].ToString();
            String b = "BOOKED";

            String connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "INSERT INTO trainbooking(pnr,username,email,mob,trainno,doj,source,destination,class,bookingdate,p1,a1,g1,b1,q1,p2,a2,g2,b2,q2,p3,a3,g3,b3,q3,p4,a4,g4,b4,q4,p5,a5,g5,b5,q5,p6,a6,g6,b6,q6,s1,s2,s3,s4,s5,s6,tfare,pay,bs) " +
                    "VALUES(@pnr,@username,@email,@mob,@trainno,@doj,@source,@destination,@class,@bookingdate,@p1,@a1,@g1,@b1,@q1,@p2,@a2,@g2,@b2,@q2,@p3,@a3,@g3,@b3,@q3,@p4,@a4,@g4,@b4,@q4,@p5,@a5,@g5,@b5,@q5,@p6,@a6,@g6,@b6,@q6,@s1,@s2,@s3,@s4,@s5,@s6,@tfare,@pay,@bs)";



                using (SqlCommand cmd = new SqlCommand(query))
                {

                    cmd.Connection = con;
                    con.Open();
                    if (p1 != null && p2 != null && p3 != null && p3 != null && p4 != null && p5 != null && p6 != null
                        && a1 != null && a2 != null && a3 != null && a4 != null && a5 != null && a6 != null
                        && q1 != null && q2 != null && q3 != null && q4 != null && q5 != null && q6 != null
                        && b1 != null && b2 != null && b3 != null && b4 != null && b5 != null && b6 != null
                        && q1 != null && q2 != null && q3 != null && q4 != null && q5 != null && q6 != null)
                    {
                        cmd.Parameters.AddWithValue("@pnr", pnrgenerator);
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@email", emm);
                        cmd.Parameters.AddWithValue("@mob", mob);
                        cmd.Parameters.AddWithValue("@trainno", tno);
                        cmd.Parameters.AddWithValue("@doj", dt);
                        cmd.Parameters.AddWithValue("@source", source);
                        cmd.Parameters.AddWithValue("@destination", destination);
                        cmd.Parameters.AddWithValue("@class", classes);
                        cmd.Parameters.AddWithValue("@bookingdate", bookingTime.ToString());
                        cmd.Parameters.AddWithValue("@p1", tf.p1);
                        cmd.Parameters.AddWithValue("@a1", tf.a1);
                        cmd.Parameters.AddWithValue("@g1", tf.g1);
                        cmd.Parameters.AddWithValue("@b1", tf.b1);
                        cmd.Parameters.AddWithValue("@q1", tf.q1);
                        cmd.Parameters.AddWithValue("@p2", tf.p2);
                        cmd.Parameters.AddWithValue("@a2", tf.a2);
                        cmd.Parameters.AddWithValue("@g2", tf.g2);
                        cmd.Parameters.AddWithValue("@b2", tf.b2);
                        cmd.Parameters.AddWithValue("@q2", tf.q2);
                        cmd.Parameters.AddWithValue("@p3", tf.p3);
                        cmd.Parameters.AddWithValue("@a3", tf.a3);
                        cmd.Parameters.AddWithValue("@g3", tf.g3);
                        cmd.Parameters.AddWithValue("@b3", tf.b3);
                        cmd.Parameters.AddWithValue("@q3", tf.q3);
                        cmd.Parameters.AddWithValue("@p4", tf.p4);
                        cmd.Parameters.AddWithValue("@a4", tf.a4);
                        cmd.Parameters.AddWithValue("@g4", tf.g4);
                        cmd.Parameters.AddWithValue("@b4", tf.b4);
                        cmd.Parameters.AddWithValue("@q4", tf.q4);
                        cmd.Parameters.AddWithValue("@p5", tf.p5);
                        cmd.Parameters.AddWithValue("@a5", tf.a5);
                        cmd.Parameters.AddWithValue("@g5", tf.g5);
                        cmd.Parameters.AddWithValue("@b5", tf.b5);
                        cmd.Parameters.AddWithValue("@q5", tf.q5);
                        cmd.Parameters.AddWithValue("@p6", tf.p6);
                        cmd.Parameters.AddWithValue("@a6", tf.a6);
                        cmd.Parameters.AddWithValue("@g6", tf.g6);
                        cmd.Parameters.AddWithValue("@b6", tf.b6);
                        cmd.Parameters.AddWithValue("@q6", tf.q6);
                        cmd.Parameters.AddWithValue("@s1", s1);
                        cmd.Parameters.AddWithValue("@s2", s2);
                        cmd.Parameters.AddWithValue("@s3", s3);
                        cmd.Parameters.AddWithValue("@s4", s4);
                        cmd.Parameters.AddWithValue("@s5", s5);
                        cmd.Parameters.AddWithValue("@s6", s6);
                        cmd.Parameters.AddWithValue("@pay", tf.pay);
                        int q1f, q2f, q3f, q4f, q5f, q6f;
                        if (classes.Equals("SL"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = faressl / 2;
                            }
                            else
                            {
                                q1f = faressl;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = faressl / 2;
                            }
                            else
                            {
                                q2f = faressl;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = faressl / 2;
                            }
                            else
                            {
                                q3f = faressl;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = faressl / 2;
                            }
                            else
                            {
                                q4f = faressl;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = faressl / 2;
                            }
                            else
                            {
                                q5f = faressl;
                            }
                            if (q6.Equals("SENIOR"))
                            {
                                q6f = faressl / 2;
                            }
                            else
                            {
                                q6f = faressl;
                            }
                            int total = q1f + q2f + q3f + q4f + q5f + q6f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }

                        else if (classes.Equals("3A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares3a / 2;
                            }
                            else
                            {
                                q1f = fares3a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares3a / 2;
                            }
                            else
                            {
                                q2f = fares3a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares3a / 2;
                            }
                            else
                            {
                                q3f = fares3a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares3a / 2;
                            }
                            else
                            {
                                q4f = fares3a;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares3a / 2;
                            }
                            else
                            {
                                q5f = fares3a;
                            }
                            if (q6.Equals("SENIOR"))
                            {
                                q6f = fares3a / 2;
                            }
                            else
                            {
                                q6f = fares3a;
                            }
                            int total = q1f + q2f + q3f + q4f + q5f + q6f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2a / 2;
                            }
                            else
                            {
                                q1f = fares2a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2a / 2;
                            }
                            else
                            {
                                q2f = fares2a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2a / 2;
                            }
                            else
                            {
                                q3f = fares2a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares2a / 2;
                            }
                            else
                            {
                                q4f = fares2a;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares2a / 2;
                            }
                            else
                            {
                                q5f = fares2a;
                            }
                            if (q6.Equals("SENIOR"))
                            {
                                q6f = fares2a / 2;
                            }
                            else
                            {
                                q6f = fares2a;
                            }
                            int total = q1f + q2f + q3f + q4f + q5f + q6f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("1A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares1a / 2;
                            }
                            else
                            {
                                q1f = fares1a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares1a / 2;
                            }
                            else
                            {
                                q2f = fares1a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares1a / 2;
                            }
                            else
                            {
                                q3f = fares1a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares1a / 2;
                            }
                            else
                            {
                                q4f = fares1a;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares1a / 2;
                            }
                            else
                            {
                                q5f = fares1a;
                            }
                            if (q6.Equals("SENIOR"))
                            {
                                q6f = fares1a / 2;
                            }
                            else
                            {
                                q6f = fares1a;
                            }
                            int total = q1f + q2f + q3f + q4f + q5f + q6f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2S"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2s / 2;
                            }
                            else
                            {
                                q1f = fares2s;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2s / 2;
                            }
                            else
                            {
                                q2f = fares2s;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2s / 2;
                            }
                            else
                            {
                                q3f = fares2s;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares2s / 2;
                            }
                            else
                            {
                                q4f = fares2s;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares2s / 2;
                            }
                            else
                            {
                                q5f = fares2s;
                            }
                            if (q6.Equals("SENIOR"))
                            {
                                q6f = fares2s / 2;
                            }
                            else
                            {
                                q6f = fares2s;
                            }
                            int total = q1f + q2f + q3f + q4f + q5f + q6f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("CC"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = farescc / 2;
                            }
                            else
                            {
                                q1f = farescc;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = farescc / 2;
                            }
                            else
                            {
                                q2f = farescc;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = farescc / 2;
                            }
                            else
                            {
                                q3f = farescc;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = farescc / 2;
                            }
                            else
                            {
                                q4f = farescc;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = farescc / 2;
                            }
                            else
                            {
                                q5f = farescc;
                            }
                            if (q6.Equals("SENIOR"))
                            {
                                q6f = farescc / 2;
                            }
                            else
                            {
                                q6f = farescc;
                            }
                            int total = q1f + q2f + q3f + q4f + q5f + q6f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        cmd.Parameters.AddWithValue("@bs", b);
                        status = (cmd.ExecuteNonQuery() >= 1) ? "TICKET BOOKED SUCESSFULLY!" : "Record is not saved";
                        if (status.Equals("TICKET BOOKED SUCESSFULLY!"))
                        {
                            using (MailMessage mm = new MailMessage("irctc@narender.com", emm))
                            {
                                mm.Subject = "IRCTC TICKET CONFIRMATIION";
                                mm.Body = "DEAR IRCTC USER , " + un + " YOUR TICKET WAS BOOKED SUCESSFULLY AND PNR NUMBER IS: " + pnrgenerator;
                                mm.IsBodyHtml = false;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = NetworkCred;
                                smtp.Port = 587;
                                smtp.Send(mm);

                            }

                        }
                    }
                    else if (p1 != null && p2 != null && p3 != null && p3 != null && p4 != null && p5 != null
                        && a1 != null && a2 != null && a3 != null && a4 != null && a5 != null
                        && q1 != null && q2 != null && q3 != null && q4 != null && q5 != null
                        && b1 != null && b2 != null && b3 != null && b4 != null && b5 != null
                        && q1 != null && q2 != null && q3 != null && q4 != null && q5 != null)
                    {
                        cmd.Parameters.AddWithValue("@pnr", pnrgenerator);
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@email", emm);
                        cmd.Parameters.AddWithValue("@mob", mob);
                        cmd.Parameters.AddWithValue("@trainno", tno);
                        cmd.Parameters.AddWithValue("@doj", dt);
                        cmd.Parameters.AddWithValue("@source", source);
                        cmd.Parameters.AddWithValue("@destination", destination);
                        cmd.Parameters.AddWithValue("@class", classes);
                        cmd.Parameters.AddWithValue("@bookingdate", bookingTime.ToString());
                        cmd.Parameters.AddWithValue("@p1", tf.p1);
                        cmd.Parameters.AddWithValue("@a1", tf.a1);
                        cmd.Parameters.AddWithValue("@g1", tf.g1);
                        cmd.Parameters.AddWithValue("@b1", tf.b1);
                        cmd.Parameters.AddWithValue("@q1", tf.q1);
                        cmd.Parameters.AddWithValue("@p2", tf.p2);
                        cmd.Parameters.AddWithValue("@a2", tf.a2);
                        cmd.Parameters.AddWithValue("@g2", tf.g2);
                        cmd.Parameters.AddWithValue("@b2", tf.b2);
                        cmd.Parameters.AddWithValue("@q2", tf.q2);
                        cmd.Parameters.AddWithValue("@p3", tf.p3);
                        cmd.Parameters.AddWithValue("@a3", tf.a3);
                        cmd.Parameters.AddWithValue("@g3", tf.g3);
                        cmd.Parameters.AddWithValue("@b3", tf.b3);
                        cmd.Parameters.AddWithValue("@q3", tf.q3);
                        cmd.Parameters.AddWithValue("@p4", tf.p4);
                        cmd.Parameters.AddWithValue("@a4", tf.a4);
                        cmd.Parameters.AddWithValue("@g4", tf.g4);
                        cmd.Parameters.AddWithValue("@b4", tf.b4);
                        cmd.Parameters.AddWithValue("@q4", tf.q4);
                        cmd.Parameters.AddWithValue("@p5", tf.p5);
                        cmd.Parameters.AddWithValue("@a5", tf.a5);
                        cmd.Parameters.AddWithValue("@g5", tf.g5);
                        cmd.Parameters.AddWithValue("@b5", tf.b5);
                        cmd.Parameters.AddWithValue("@q5", tf.q5);
                        cmd.Parameters.AddWithValue("@p6", "NA");
                        cmd.Parameters.AddWithValue("@a6", "NA");
                        cmd.Parameters.AddWithValue("@g6", "NA");
                        cmd.Parameters.AddWithValue("@b6", "NA");
                        cmd.Parameters.AddWithValue("@q6", "NA");
                        cmd.Parameters.AddWithValue("@s1", s1);
                        cmd.Parameters.AddWithValue("@s2", s2);
                        cmd.Parameters.AddWithValue("@s3", s3);
                        cmd.Parameters.AddWithValue("@s4", s4);
                        cmd.Parameters.AddWithValue("@s5", s5);
                        cmd.Parameters.AddWithValue("@s6", "NA");
                        cmd.Parameters.AddWithValue("@pay", tf.pay);
                        int q1f, q2f, q3f, q4f, q5f, q6f;
                        if (classes.Equals("SL"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = faressl / 2;
                            }
                            else
                            {
                                q1f = faressl;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = faressl / 2;
                            }
                            else
                            {
                                q2f = faressl;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = faressl / 2;
                            }
                            else
                            {
                                q3f = faressl;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = faressl / 2;
                            }
                            else
                            {
                                q4f = faressl;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = faressl / 2;
                            }
                            else
                            {
                                q5f = faressl;
                            }

                            int total = q1f + q2f + q3f + q4f + q5f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("3A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares3a / 2;
                            }
                            else
                            {
                                q1f = fares3a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares3a / 2;
                            }
                            else
                            {
                                q2f = fares3a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares3a / 2;
                            }
                            else
                            {
                                q3f = fares3a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares3a / 2;
                            }
                            else
                            {
                                q4f = fares3a;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares3a / 2;
                            }
                            else
                            {
                                q5f = fares3a;
                            }

                            int total = q1f + q2f + q3f + q4f + q5f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2a / 2;
                            }
                            else
                            {
                                q1f = fares2a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2a / 2;
                            }
                            else
                            {
                                q2f = fares2a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2a / 2;
                            }
                            else
                            {
                                q3f = fares2a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares2a / 2;
                            }
                            else
                            {
                                q4f = fares2a;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares2a / 2;
                            }
                            else
                            {
                                q5f = fares2a;
                            }

                            int total = q1f + q2f + q3f + q4f + q5f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("1A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares1a / 2;
                            }
                            else
                            {
                                q1f = fares1a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares1a / 2;
                            }
                            else
                            {
                                q2f = fares1a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares1a / 2;
                            }
                            else
                            {
                                q3f = fares1a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares1a / 2;
                            }
                            else
                            {
                                q4f = fares1a;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares1a / 2;
                            }
                            else
                            {
                                q5f = fares1a;
                            }

                            int total = q1f + q2f + q3f + q4f + q5f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2S"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2s / 2;
                            }
                            else
                            {
                                q1f = fares2s;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2s / 2;
                            }
                            else
                            {
                                q2f = fares2s;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2s / 2;
                            }
                            else
                            {
                                q3f = fares2s;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares2s / 2;
                            }
                            else
                            {
                                q4f = fares2s;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = fares2s / 2;
                            }
                            else
                            {
                                q5f = fares2s;
                            }

                            int total = q1f + q2f + q3f + q4f + q5f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("CC"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = farescc / 2;
                            }
                            else
                            {
                                q1f = farescc;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = farescc / 2;
                            }
                            else
                            {
                                q2f = farescc;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = farescc / 2;
                            }
                            else
                            {
                                q3f = farescc;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = farescc / 2;
                            }
                            else
                            {
                                q4f = farescc;
                            }
                            if (q5.Equals("SENIOR"))
                            {
                                q5f = farescc / 2;
                            }
                            else
                            {
                                q5f = farescc;
                            }

                            int total = q1f + q2f + q3f + q4f + q5f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        cmd.Parameters.AddWithValue("@bs", b);
                        status = (cmd.ExecuteNonQuery() >= 1) ? "TICKET BOOKED SUCESSFULLY!" : "Record is not saved";
                        if (status.Equals("TICKET BOOKED SUCESSFULLY!"))
                        {
                            using (MailMessage mm = new MailMessage("irctc@narender.com", emm))
                            {
                                mm.Subject = "IRCTC TICKET CONFIRMATIION";
                                mm.Body = "DEAR IRCTC USER , " + un + " YOUR TICKET WAS BOOKED SUCESSFULLY AND PNR NUMBER IS: " + pnrgenerator;
                                mm.IsBodyHtml = false;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = NetworkCred;
                                smtp.Port = 587;
                                smtp.Send(mm);

                            }

                        }
                    }


                    else if (p1 != null && p2 != null && p3 != null && p3 != null && p4 != null
                        && a1 != null && a2 != null && a3 != null && a4 != null
                        && q1 != null && q2 != null && q3 != null && q4 != null
                        && b1 != null && b2 != null && b3 != null && b4 != null
                        && q1 != null && q2 != null && q3 != null && q4 != null)
                    {
                        cmd.Parameters.AddWithValue("@pnr", pnrgenerator);
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@email", emm);
                        cmd.Parameters.AddWithValue("@mob", mob);
                        cmd.Parameters.AddWithValue("@trainno", tno);
                        cmd.Parameters.AddWithValue("@doj", dt);
                        cmd.Parameters.AddWithValue("@source", source);
                        cmd.Parameters.AddWithValue("@destination", destination);
                        cmd.Parameters.AddWithValue("@class", classes);
                        cmd.Parameters.AddWithValue("@bookingdate", bookingTime.ToString());
                        cmd.Parameters.AddWithValue("@p1", tf.p1);
                        cmd.Parameters.AddWithValue("@a1", tf.a1);
                        cmd.Parameters.AddWithValue("@g1", tf.g1);
                        cmd.Parameters.AddWithValue("@b1", tf.b1);
                        cmd.Parameters.AddWithValue("@q1", tf.q1);
                        cmd.Parameters.AddWithValue("@p2", tf.p2);
                        cmd.Parameters.AddWithValue("@a2", tf.a2);
                        cmd.Parameters.AddWithValue("@g2", tf.g2);
                        cmd.Parameters.AddWithValue("@b2", tf.b2);
                        cmd.Parameters.AddWithValue("@q2", tf.q2);
                        cmd.Parameters.AddWithValue("@p3", tf.p3);
                        cmd.Parameters.AddWithValue("@a3", tf.a3);
                        cmd.Parameters.AddWithValue("@g3", tf.g3);
                        cmd.Parameters.AddWithValue("@b3", tf.b3);
                        cmd.Parameters.AddWithValue("@q3", tf.q3);
                        cmd.Parameters.AddWithValue("@p4", tf.p4);
                        cmd.Parameters.AddWithValue("@a4", tf.a4);
                        cmd.Parameters.AddWithValue("@g4", tf.g4);
                        cmd.Parameters.AddWithValue("@b4", tf.b4);
                        cmd.Parameters.AddWithValue("@q4", tf.q4);
                        cmd.Parameters.AddWithValue("@p5", "NA");
                        cmd.Parameters.AddWithValue("@a5", "NA");
                        cmd.Parameters.AddWithValue("@g5", "NA");
                        cmd.Parameters.AddWithValue("@b5", "NA");
                        cmd.Parameters.AddWithValue("@q5", "NA");
                        cmd.Parameters.AddWithValue("@p6", "NA");
                        cmd.Parameters.AddWithValue("@a6", "NA");
                        cmd.Parameters.AddWithValue("@g6", "NA");
                        cmd.Parameters.AddWithValue("@b6", "NA");
                        cmd.Parameters.AddWithValue("@q6", "NA");
                        cmd.Parameters.AddWithValue("@s1", s1);
                        cmd.Parameters.AddWithValue("@s2", s2);
                        cmd.Parameters.AddWithValue("@s3", s3);
                        cmd.Parameters.AddWithValue("@s4", s4);
                        cmd.Parameters.AddWithValue("@s5", "NA");
                        cmd.Parameters.AddWithValue("@s6", "NA");
                        cmd.Parameters.AddWithValue("@pay", tf.pay);
                        int q1f, q2f, q3f, q4f;
                        if (classes.Equals("SL"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = faressl / 2;
                            }
                            else
                            {
                                q1f = faressl;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = faressl / 2;
                            }
                            else
                            {
                                q2f = faressl;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = faressl / 2;
                            }
                            else
                            {
                                q3f = faressl;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = faressl / 2;
                            }
                            else
                            {
                                q4f = faressl;
                            }

                            int total = q1f + q2f + q3f + q4f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("3A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares3a / 2;
                            }
                            else
                            {
                                q1f = fares3a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares3a / 2;
                            }
                            else
                            {
                                q2f = fares3a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares3a / 2;
                            }
                            else
                            {
                                q3f = fares3a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares3a / 2;
                            }
                            else
                            {
                                q4f = fares3a;
                            }

                            int total = q1f + q2f + q3f + q4f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2a / 2;
                            }
                            else
                            {
                                q1f = fares2a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2a / 2;
                            }
                            else
                            {
                                q2f = fares2a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2a / 2;
                            }
                            else
                            {
                                q3f = fares2a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares2a / 2;
                            }
                            else
                            {
                                q4f = fares2a;
                            }

                            int total = q1f + q2f + q3f + q4f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("1A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares1a / 2;
                            }
                            else
                            {
                                q1f = fares1a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares1a / 2;
                            }
                            else
                            {
                                q2f = fares1a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares1a / 2;
                            }
                            else
                            {
                                q3f = fares1a;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares1a / 2;
                            }
                            else
                            {
                                q4f = fares1a;
                            }

                            int total = q1f + q2f + q3f + q4f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2S"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2s / 2;
                            }
                            else
                            {
                                q1f = fares2s;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2s / 2;
                            }
                            else
                            {
                                q2f = fares2s;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2s / 2;
                            }
                            else
                            {
                                q3f = fares2s;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = fares2s / 2;
                            }
                            else
                            {
                                q4f = fares2s;
                            }

                            int total = q1f + q2f + q3f + q4f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("CC"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = farescc / 2;
                            }
                            else
                            {
                                q1f = farescc;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = farescc / 2;
                            }
                            else
                            {
                                q2f = farescc;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = farescc / 2;
                            }
                            else
                            {
                                q3f = farescc;
                            }
                            if (q4.Equals("SENIOR"))
                            {
                                q4f = farescc / 2;
                            }
                            else
                            {
                                q4f = farescc;
                            }

                            int total = q1f + q2f + q3f + q4f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        cmd.Parameters.AddWithValue("@bs", b);
                        status = (cmd.ExecuteNonQuery() >= 1) ? "TICKET BOOKED SUCESSFULLY!" : "Record is not saved";
                        if (status.Equals("TICKET BOOKED SUCESSFULLY!"))
                        {
                            using (MailMessage mm = new MailMessage("irctc@narender.com", emm))
                            {
                                mm.Subject = "IRCTC TICKET CONFIRMATIION";
                                mm.Body = "DEAR IRCTC USER , " + un + " YOUR TICKET WAS BOOKED SUCESSFULLY AND PNR NUMBER IS: " + pnrgenerator;
                                mm.IsBodyHtml = false;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = NetworkCred;
                                smtp.Port = 587;
                                smtp.Send(mm);

                            }

                        }
                    }
                    else if (p1 != null && p2 != null && p3 != null
                        && a1 != null && a2 != null && a3 != null
                        && q1 != null && q2 != null && q3 != null
                        && b1 != null && b2 != null && b3 != null
                        && q1 != null && q2 != null && q3 != null)
                    {
                        cmd.Parameters.AddWithValue("@pnr", pnrgenerator);
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@email", emm);
                        cmd.Parameters.AddWithValue("@mob", mob);
                        cmd.Parameters.AddWithValue("@trainno", tno);
                        cmd.Parameters.AddWithValue("@doj", dt);
                        cmd.Parameters.AddWithValue("@source", source);
                        cmd.Parameters.AddWithValue("@destination", destination);
                        cmd.Parameters.AddWithValue("@class", classes);
                        cmd.Parameters.AddWithValue("@bookingdate", bookingTime.ToString());
                        cmd.Parameters.AddWithValue("@p1", tf.p1);
                        cmd.Parameters.AddWithValue("@a1", tf.a1);
                        cmd.Parameters.AddWithValue("@g1", tf.g1);
                        cmd.Parameters.AddWithValue("@b1", tf.b1);
                        cmd.Parameters.AddWithValue("@q1", tf.q1);
                        cmd.Parameters.AddWithValue("@p2", tf.p2);
                        cmd.Parameters.AddWithValue("@a2", tf.a2);
                        cmd.Parameters.AddWithValue("@g2", tf.g2);
                        cmd.Parameters.AddWithValue("@b2", tf.b2);
                        cmd.Parameters.AddWithValue("@q2", tf.q2);
                        cmd.Parameters.AddWithValue("@p3", tf.p3);
                        cmd.Parameters.AddWithValue("@a3", tf.a3);
                        cmd.Parameters.AddWithValue("@g3", tf.g3);
                        cmd.Parameters.AddWithValue("@b3", tf.b3);
                        cmd.Parameters.AddWithValue("@q3", tf.q3);

                        cmd.Parameters.AddWithValue("@p4", "NA");
                        cmd.Parameters.AddWithValue("@a4", "NA");
                        cmd.Parameters.AddWithValue("@g4", "NA");
                        cmd.Parameters.AddWithValue("@b4", "NA");
                        cmd.Parameters.AddWithValue("@q4", "NA");
                        cmd.Parameters.AddWithValue("@p5", "NA");
                        cmd.Parameters.AddWithValue("@a5", "NA");
                        cmd.Parameters.AddWithValue("@g5", "NA");
                        cmd.Parameters.AddWithValue("@b5", "NA");
                        cmd.Parameters.AddWithValue("@q5", "NA");
                        cmd.Parameters.AddWithValue("@p6", "NA");
                        cmd.Parameters.AddWithValue("@a6", "NA");
                        cmd.Parameters.AddWithValue("@g6", "NA");
                        cmd.Parameters.AddWithValue("@b6", "NA");
                        cmd.Parameters.AddWithValue("@q6", "NA");
                        cmd.Parameters.AddWithValue("@s1", s1);
                        cmd.Parameters.AddWithValue("@s2", s2);
                        cmd.Parameters.AddWithValue("@s3", s3);
                        cmd.Parameters.AddWithValue("@s4", "NA");
                        cmd.Parameters.AddWithValue("@s5", "NA");
                        cmd.Parameters.AddWithValue("@s6", "NA");
                        cmd.Parameters.AddWithValue("@pay", tf.pay);
                        int q1f, q2f, q3f, q4f, q5f, q6f;
                        if (classes.Equals("SL"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = faressl / 2;
                            }
                            else
                            {
                                q1f = faressl;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = faressl / 2;
                            }
                            else
                            {
                                q2f = faressl;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = faressl / 2;
                            }
                            else
                            {
                                q3f = faressl;
                            }

                            int total = q1f + q2f + q3f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("3A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares3a / 2;
                            }
                            else
                            {
                                q1f = fares3a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares3a / 2;
                            }
                            else
                            {
                                q2f = fares3a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares3a / 2;
                            }
                            else
                            {
                                q3f = fares3a;
                            }

                            int total = q1f + q2f + q3f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2a / 2;
                            }
                            else
                            {
                                q1f = fares2a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2a / 2;
                            }
                            else
                            {
                                q2f = fares2a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2a / 2;
                            }
                            else
                            {
                                q3f = fares2a;
                            }

                            if (q6.Equals("SENIOR"))
                            {
                                q6f = fares2a / 2;
                            }
                            else
                            {
                                q6f = fares2a;
                            }
                            int total = q1f + q2f + q3f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("1A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares1a / 2;
                            }
                            else
                            {
                                q1f = fares1a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares1a / 2;
                            }
                            else
                            {
                                q2f = fares1a;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares1a / 2;
                            }
                            else
                            {
                                q3f = fares1a;
                            }

                            int total = q1f + q2f + q3f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2S"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2s / 2;
                            }
                            else
                            {
                                q1f = fares2s;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2s / 2;
                            }
                            else
                            {
                                q2f = fares2s;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = fares2s / 2;
                            }
                            else
                            {
                                q3f = fares2s;
                            }

                            int total = q1f + q2f + q3f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("CC"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = farescc / 2;
                            }
                            else
                            {
                                q1f = farescc;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = farescc / 2;
                            }
                            else
                            {
                                q2f = farescc;
                            }
                            if (q3.Equals("SENIOR"))
                            {
                                q3f = farescc / 2;
                            }
                            else
                            {
                                q3f = farescc;
                            }

                            int total = q1f + q2f + q3f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        cmd.Parameters.AddWithValue("@bs", b);
                        status = (cmd.ExecuteNonQuery() >= 1) ? "TICKET BOOKED SUCESSFULLY!" : "Record is not saved";
                        if (status.Equals("TICKET BOOKED SUCESSFULLY!"))
                        {
                            using (MailMessage mm = new MailMessage("irctc@narender.com", emm))
                            {
                                mm.Subject = "IRCTC TICKET CONFIRMATIION";
                                mm.Body = "DEAR IRCTC USER , " + un + " YOUR TICKET WAS BOOKED SUCESSFULLY AND PNR NUMBER IS: " + pnrgenerator;
                                mm.IsBodyHtml = false;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = NetworkCred;
                                smtp.Port = 587;
                                smtp.Send(mm);

                            }


                        }
                    }
                    else if (p1 != null && p2 != null
                        && a1 != null && a2 != null
                        && q1 != null && q2 != null
                        && b1 != null && b2 != null
                        && q1 != null && q2 != null)
                    {
                        cmd.Parameters.AddWithValue("@pnr", pnrgenerator);
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@email", emm);
                        cmd.Parameters.AddWithValue("@mob", mob);
                        cmd.Parameters.AddWithValue("@trainno", tno);
                        cmd.Parameters.AddWithValue("@doj", dt);
                        cmd.Parameters.AddWithValue("@source", source);
                        cmd.Parameters.AddWithValue("@destination", destination);
                        cmd.Parameters.AddWithValue("@class", classes);
                        cmd.Parameters.AddWithValue("@bookingdate", bookingTime.ToString());
                        cmd.Parameters.AddWithValue("@p1", tf.p1);
                        cmd.Parameters.AddWithValue("@a1", tf.a1);
                        cmd.Parameters.AddWithValue("@g1", tf.g1);
                        cmd.Parameters.AddWithValue("@b1", tf.b1);
                        cmd.Parameters.AddWithValue("@q1", tf.q1);
                        cmd.Parameters.AddWithValue("@p2", tf.p2);
                        cmd.Parameters.AddWithValue("@a2", tf.a2);
                        cmd.Parameters.AddWithValue("@g2", tf.g2);
                        cmd.Parameters.AddWithValue("@b2", tf.b2);
                        cmd.Parameters.AddWithValue("@q2", tf.q2);

                        cmd.Parameters.AddWithValue("@p3", "NA");
                        cmd.Parameters.AddWithValue("@a3", "NA");
                        cmd.Parameters.AddWithValue("@g3", "NA");
                        cmd.Parameters.AddWithValue("@b3", "NA");
                        cmd.Parameters.AddWithValue("@q3", "NA");
                        cmd.Parameters.AddWithValue("@p4", "NA");
                        cmd.Parameters.AddWithValue("@a4", "NA");
                        cmd.Parameters.AddWithValue("@g4", "NA");
                        cmd.Parameters.AddWithValue("@b4", "NA");
                        cmd.Parameters.AddWithValue("@q4", "NA");
                        cmd.Parameters.AddWithValue("@p5", "NA");
                        cmd.Parameters.AddWithValue("@a5", "NA");
                        cmd.Parameters.AddWithValue("@g5", "NA");
                        cmd.Parameters.AddWithValue("@b5", "NA");
                        cmd.Parameters.AddWithValue("@q5", "NA");
                        cmd.Parameters.AddWithValue("@p6", "NA");
                        cmd.Parameters.AddWithValue("@a6", "NA");
                        cmd.Parameters.AddWithValue("@g6", "NA");
                        cmd.Parameters.AddWithValue("@b6", "NA");
                        cmd.Parameters.AddWithValue("@q6", "NA");
                        cmd.Parameters.AddWithValue("@s1", s1);
                        cmd.Parameters.AddWithValue("@s2", s2);
                        cmd.Parameters.AddWithValue("@s3", "NA");
                        cmd.Parameters.AddWithValue("@s4", "NA");
                        cmd.Parameters.AddWithValue("@s5", "NA");
                        cmd.Parameters.AddWithValue("@s6", "NA");
                        cmd.Parameters.AddWithValue("@pay", tf.pay);
                        int q1f, q2f;
                        if (classes.Equals("SL"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = faressl / 2;
                            }
                            else
                            {
                                q1f = faressl;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = faressl / 2;
                            }
                            else
                            {
                                q2f = faressl;
                            }

                            int total = q1f + q2f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("3A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares3a / 2;
                            }
                            else
                            {
                                q1f = fares3a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares3a / 2;
                            }
                            else
                            {
                                q2f = fares3a;
                            }

                            int total = q1f + q2f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2a / 2;
                            }
                            else
                            {
                                q1f = fares2a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2a / 2;
                            }
                            else
                            {
                                q2f = fares2a;
                            }

                            int total = q1f + q2f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("1A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares1a / 2;
                            }
                            else
                            {
                                q1f = fares1a;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares1a / 2;
                            }
                            else
                            {
                                q2f = fares1a;
                            }

                            int total = q1f + q2f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2S"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2s / 2;
                            }
                            else
                            {
                                q1f = fares2s;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = fares2s / 2;
                            }
                            else
                            {
                                q2f = fares2s;
                            }

                            int total = q1f + q2f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("CC"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = farescc / 2;
                            }
                            else
                            {
                                q1f = farescc;
                            }
                            if (q2.Equals("SENIOR"))
                            {
                                q2f = farescc / 2;
                            }
                            else
                            {
                                q2f = farescc;
                            }

                            int total = q1f + q2f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        cmd.Parameters.AddWithValue("@bs", b);
                        status = (cmd.ExecuteNonQuery() >= 1) ? "TICKET BOOKED SUCESSFULLY!" : "Record is not saved";
                        if (status.Equals("TICKET BOOKED SUCESSFULLY!"))
                        {
                            using (MailMessage mm = new MailMessage("irctc@narender.com", emm))
                            {
                                mm.Subject = "IRCTC TICKET CONFIRMATIION";
                                mm.Body = "DEAR IRCTC USER , " + un + " YOUR TICKET WAS BOOKED SUCESSFULLY AND PNR NUMBER IS: " + pnrgenerator;
                                mm.IsBodyHtml = false;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = NetworkCred;
                                smtp.Port = 587;
                                smtp.Send(mm);

                            }

                        }

                    }
                    else if (p1 != null && a1 != null && q1 != null && b1 != null && q1 != null)
                    {
                        cmd.Parameters.AddWithValue("@pnr", pnrgenerator);
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@email", emm);
                        cmd.Parameters.AddWithValue("@mob", mob);
                        cmd.Parameters.AddWithValue("@trainno", tno);
                        cmd.Parameters.AddWithValue("@doj", dt);
                        cmd.Parameters.AddWithValue("@source", source);
                        cmd.Parameters.AddWithValue("@destination", destination);
                        cmd.Parameters.AddWithValue("@class", classes);
                        cmd.Parameters.AddWithValue("@bookingdate", bookingTime.ToString());
                        cmd.Parameters.AddWithValue("@p1", tf.p1);
                        cmd.Parameters.AddWithValue("@a1", tf.a1);
                        cmd.Parameters.AddWithValue("@g1", tf.g1);
                        cmd.Parameters.AddWithValue("@b1", tf.b1);
                        cmd.Parameters.AddWithValue("@q1", tf.q1);

                        cmd.Parameters.AddWithValue("@p2", "NA");
                        cmd.Parameters.AddWithValue("@a2", "NA");
                        cmd.Parameters.AddWithValue("@g2", "NA");
                        cmd.Parameters.AddWithValue("@b2", "NA");
                        cmd.Parameters.AddWithValue("@q2", "NA");
                        cmd.Parameters.AddWithValue("@p3", "NA");
                        cmd.Parameters.AddWithValue("@a3", "NA");
                        cmd.Parameters.AddWithValue("@g3", "NA");
                        cmd.Parameters.AddWithValue("@b3", "NA");
                        cmd.Parameters.AddWithValue("@q3", "NA");
                        cmd.Parameters.AddWithValue("@p4", "NA");
                        cmd.Parameters.AddWithValue("@a4", "NA");
                        cmd.Parameters.AddWithValue("@g4", "NA");
                        cmd.Parameters.AddWithValue("@b4", "NA");
                        cmd.Parameters.AddWithValue("@q4", "NA");
                        cmd.Parameters.AddWithValue("@p5", "NA");
                        cmd.Parameters.AddWithValue("@a5", "NA");
                        cmd.Parameters.AddWithValue("@g5", "NA");
                        cmd.Parameters.AddWithValue("@b5", "NA");
                        cmd.Parameters.AddWithValue("@q5", "NA");

                        cmd.Parameters.AddWithValue("@p6", "NA");
                        cmd.Parameters.AddWithValue("@a6", "NA");
                        cmd.Parameters.AddWithValue("@g6", "NA");
                        cmd.Parameters.AddWithValue("@b6", "NA");
                        cmd.Parameters.AddWithValue("@q6", "NA");
                        cmd.Parameters.AddWithValue("@s1", s1);
                        cmd.Parameters.AddWithValue("@s2", "NA");
                        cmd.Parameters.AddWithValue("@s3", "NA");
                        cmd.Parameters.AddWithValue("@s4", "NA");
                        cmd.Parameters.AddWithValue("@s5", "NA");
                        cmd.Parameters.AddWithValue("@s6", "NA");
                        cmd.Parameters.AddWithValue("@pay", tf.pay);
                        int q1f;
                        if (classes.Equals("SL"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = faressl / 2;
                            }
                            else
                            {
                                q1f = faressl;
                            }

                            int total = q1f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("3A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares3a / 2;
                            }
                            else
                            {
                                q1f = fares3a;
                            }

                            int total = q1f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2a / 2;
                            }
                            else
                            {
                                q1f = fares2a;
                            }

                            int total = q1f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("1A"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares1a / 2;
                            }
                            else
                            {
                                q1f = fares1a;
                            }

                            int total = q1f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("2S"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = fares2s / 2;
                            }
                            else
                            {
                                q1f = fares2s;
                            }

                            int total = q1f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        else if (classes.Equals("CC"))
                        {
                            if (q1.Equals("SENIOR"))
                            {
                                q1f = farescc / 2;
                            }
                            else
                            {
                                q1f = farescc;
                            }

                            int total = q1f;
                            cmd.Parameters.AddWithValue("@tfare", total);
                        }
                        cmd.Parameters.AddWithValue("@bs", b);
                        status = (cmd.ExecuteNonQuery() >= 1) ? "TICKET BOOKED SUCESSFULLY!" : "Record is not saved";
                        if (status.Equals("TICKET BOOKED SUCESSFULLY!"))
                        {
                            using (MailMessage mm = new MailMessage("irctc@narender.com", emm))
                            {
                                mm.Subject = "IRCTC TICKET CONFIRMATIION";
                                mm.Body = "DEAR IRCTC USER , " + un + " YOUR TICKET WAS BOOKED SUCESSFULLY AND PNR NUMBER IS: " + pnrgenerator;
                                mm.IsBodyHtml = false;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential NetworkCred = new NetworkCredential("bhavna.rk76@gmail.com", "10-YgvqazplM");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = NetworkCred;
                                smtp.Port = 587;
                                smtp.Send(mm);

                            }

                        }

                    }
                    else
                    {
                        status = "TECHNICAL ERROR!";
                    }
                }
            }
                TempData["book"] = status;
                return RedirectToActionPermanent("BookingHistory", "Home");
            
        }
        [HttpPost]
        public FileResult Export()
        {
            String pnr = Request["mypnr"].ToString();
            StringBuilder sb = new System.Text.StringBuilder();
          
                string connection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\App_Data\\asprms.mdf;Integrated Security=True";
                //Data Source = "(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\NARENDRA\source\repos\C1ASPRMS\C1ASPRMS\App_Data\asprms.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";

                using (SqlConnection con = new SqlConnection(connection))
                {

                    using (SqlCommand cmd1 = new SqlCommand("select * from trainbooking where pnr=@pnr"))
                    {
                        cmd1.Connection = con;
                        con.Open();

                        cmd1.Parameters.AddWithValue("@pnr", pnr);
                        SqlDataReader rdr = cmd1.ExecuteReader();

                        while (rdr.Read())
                        {

                            sb.Append("<html>");
                      //      sb.Append("html,body{height:100%;}body{margin:0;background:linear-gradient(45deg,#49a09d,#5f2c82);font-family:sans-serif;font-weight:100;}.container{position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);}table{width:800px;border-collapse:collapse;overflow:hidden;box-shadow:0020pxrgba(0,0,0,0.1);}th,td{padding:15px;background-color:rgba(255,255,255,0.2);color:#fff;}th{text-align:left;}thead{th{background-color:#55608f;}}tbody{tr{&:hover{background-color:rgba(255,255,255,0.3);}}td{position:relative;&:hover{&:before{content:'';position:absolute;left:0;right:0;top:-9999px;bottom:-9999px;background-color:rgba(255,255,255,0.2);z-index:-1;}}}}");
                            sb.Append("<body>" +
                                "<center><h3>IRCTCs e-Ticketing Service Electronic Reservation Slip (Personal User)</h3></center><br/><center><p>1.You can travel on e-ticket sent on SMS or take a Virtual Reservation Message (VRM) along with any one of the prescribed ID in original. Please do not print the ERS unless extremely necessary. This Ticket will be valid with an ID proof in original. Please carry original identity proof. If found traveling without original ID proof, passenger will be treated as without ticket and charged as per extent Railway Rules.</p><br/>" +
                                "<p>2.Only confirmed/RAC/Partially confirmed E-ticket is valid for travel.</p><br/>" +
                                " <p>3.Fully Waitlisted E-ticket is invalid for travel if it remains fully waitlisted after preparation of chart and the refund of the booking amount shall be credited to the account used for payment for booking of the ticket. Passengers travelling on a fully waitlisted e-ticket will be treated as Ticketless.</p><br/>" +
                                " <p>4.Valid IDs to be presented during train journey by one of the passenger booked on an e-ticket :- Voter Identity Card / Passport / PAN Card / Driving License / Photo ID card issued by Central / State Govt / Public Sector Undertakings of State / Central Government ,District Administrations , Municipal bodies and Panchayat Administrations which are having serial number / Student Identity Card with photograph issued by recognized School or College for their students / Nationalized Bank Passbook with photograph /Credit Cards issued by Banks with laminated photograph/Unique Identification Card 'Aadhaar', m-Aadhaar, e-Aadhaar. /Passenger showing the Aadhaar/Driving Licence from the 'Issued Document' section by logging into his/her DigiLocker account considered as valid proof of identity. (Documents uploaded by the user i.e. the document in 'Uploaded Document' section will not be considered as a valid proof of identity). 5.Service Accounting Code (SAC) 996411: Local land transport services of passengers by railways for distance upto 150 KMs Service Accounting Code (SAC) 996416: Sightseeing transportation services by railways for Tourist Ticket Service Accounting Code (SAC) 996421: Long distance transport services of passengers through rail network by Railways for distance beyond 150 KMs 6.General rules/ Information for e-ticket passenger have to be studied by the customer for cancellation & refund.</p></center><br/> " +
                                "<p><br/></p><table border='1'><tr>");
                            sb.Append("<th>PNR:</th><td>"+(Int32)rdr["pnr"] + " </ td ></tr>");
                            sb.Append("<tr><th>BOOKING DATE:</th><td>" + (String)rdr["bookingdate"] + "</td></tr>");
                            sb.Append("<tr><th>TRAIN NO:</th><td>" + (String)rdr["trainno"] + "</td></tr>");
                            sb.Append("<tr><th>DATE OF JOURNEY:</th><td>" + (String)rdr["doj"] + "</td></tr>");
                            sb.Append("<tr><th>SOURCE:</th><td>" + (String)rdr["source"] + "</td></tr>");
                            sb.Append("<tr><th>DESTINATION:</th><td>" + (String)rdr["destination"] + "</td></tr>");
                            sb.Append("<tr><th>CLASS:</th><td>" + (String)rdr["class"] + "</td></tr>");
                            sb.Append("<tr><th>BOOKING STATUS:</th><td>" + (String)rdr["bs"] + "</td></tr>");
                            sb.Append("<tr><th>PAYMENT TYPE:</th><td>" + (String)rdr["pay"] + "</td></tr>");
                            sb.Append("<tr><th>TOTAL FARE:</th><td>" + (Int32)rdr["tfare"] + "</td></tr><tr><td></tr></table><br/>");
                            sb.Append("<table border='1'><tr><td colspan='3'><center>PASSENGER'S LIST</center></td></tr><tr>");
                           
                                sb.Append("<tr><td>" + (String)rdr["p1"] + "</td>");
                                sb.Append("<td>" + (String)rdr["s1"] + "</td>");
                                sb.Append("<td>" + (String)rdr["q1"] + "</td></tr>");
                            
                                sb.Append("<tr><td>" + (String)rdr["p2"] + "</td>");
                                sb.Append("<td>" + (String)rdr["s2"] + "</td>");
                                sb.Append("<td>" + (String)rdr["q2"] + "</td></tr>");
                    
                                sb.Append("<tr><td>" + (String)rdr["p3"] + "</td>");
                                sb.Append("<td>" + (String)rdr["s3"] + "</td>");
                                sb.Append("<td>" + (String)rdr["q3"] + "</td></tr>");
                    
                                sb.Append("<tr><td>" + (String)rdr["p4"] + "</td>");
                                sb.Append("<td>" + (String)rdr["s4"] + "</td>");
                                sb.Append("<td>" + (String)rdr["q4"] + "</td></tr>");
                        
                                sb.Append("<tr><td>" + (String)rdr["p5"] + "</td>");
                                sb.Append("<td>" + (String)rdr["s5"] + "</td>");
                                sb.Append("<td>" + (String)rdr["q5"] + "</td></tr>");
                      
                                sb.Append("<tr><td>" + (String)rdr["p6"] + "</td>");
                                sb.Append("<td>" + (String)rdr["s6"] + "</td>");
                                sb.Append("<td>" + (String)rdr["q6"] + "</td></tr>");
                            }
                            sb.Append("</table></body></html>");

                        }

                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    using (MemoryStream stream = new System.IO.MemoryStream())
                    {
                        string imageURL = "C:\\Users\\NARENDRA\\source\\repos\\C1ASPRMS\\C1ASPRMS\\Content\\il.png";

                        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                        jpg.ScaleToFit(240f, 220f);
                        jpg.SpacingBefore = 10f;
                        jpg.SpacingAfter = 1f;
                        jpg.Alignment = Element.ALIGN_CENTER;
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(jpg);
                        htmlparser.Parse(sr);
                        //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();
                       // byte[] bytes = stream.ToArray();
                       // stream.Close();

                        return File(stream.ToArray(), "application/pdf", "IRCTCticket"+pnr+".pdf");

                }
                 }
            }
        }
      }
   