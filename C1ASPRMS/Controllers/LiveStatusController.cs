
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using C1ASPRMS.Models.LiveStatusModel;
using Newtonsoft.Json;

namespace C1ASPRMS.Controllers.LiveStatus
{

    public class LiveStatusController : Controller
    {
       
    [HttpPost]
    public async Task<ActionResult> Live()
        {
        List<LiveStatusModel> ls = new List<LiveStatusModel>();
        String tno = Request["tno"].ToString();
        String dt = Request["dt"].ToString();
        String stn = Request["stn"].ToString();
        String apikey = "oy6qifktqz";

            // GET: LiveStatus
            string Baseurl = " https://api.railwayapi.com/v2/live/train/";
            String Completeurl = Baseurl + tno + "/station/" + stn + "/date/" + dt + "/apikey/" + apikey + "/";
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Completeurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("response");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                   ls = JsonConvert.DeserializeObject<List<LiveStatusModel>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(ls);
            }
        }
    }
}