
using Salesforce.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dialer.Models.Salesforce;
using Dialer.Salesforce;
using Dialer.Models;
using Microsoft.AspNet.Identity;

namespace Dialer.Controllers
{
    
    public class LeadsController : Controller
    {
        // Note: the SOQL Field list, and Binding Property list have subtle differences as custom properties may be mapped with the JsonProperty attribute to remove __c
        const string _LeadPostBinding = "Lead ID,LastName,FirstName,Company,City,State,MobilePhone,Phone,Email"; // Lead ID really ID? nothing works
        // GET: Contacts
            [HttpPost, ActionName("send-sms")]

        public ActionResult Index()
        {
            SmsService _smsService = new SmsService();
            SharpGoogleVoice gv = new SharpGoogleVoice("email@gmail.com", "");//enter email
            gv.SendSMS("+14148408420", "test message");//- change number, just showing format prbly put this somewhere else^ - console app logic? 

            //public ActionResult SendSMS(SMS sms)   - this would be for different text service http://emekaemego.blogspot.com/2010/10/sending-sms-with-aspnet-mvc.html
            //{
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        bool isSuccess = false;
            //        string errMsg = null;
            //        string response = _smsService.Send(sms); //Send sms

            //        string code = _smsService.GetResponseMessage(response, out isSuccess, out errMsg);

            //        if (!isSuccess)
            //        {
            //            ModelState.AddModelError("", errMsg);
            //        }
            //        else
            //        {
            //            ViewData["SuccessMsg"] = "Message was successfully sent.";
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("", ex.Message);
            //    }
            //} End text code

            return View(/*sms*/);
        }
        //public ActionResult Index()  - this should show Leads just like Contacts page does/replace above code

        //{
        //IEnumerable<Lead> selectedLeads = Enumerable.Empty<Lead>();
        //try
        //{
        //    selectedLeads = await SalesforceService.MakeAuthenticatedClientRequestAsync(
        //        async (client) =>
        //        {
        //            QueryResult<Lead> lead =
        //                await client.QueryAsync<Lead>("ID, Last Name, First Name, Company, City, State, MobilePhone ,Phone, Email From Lead"); // Leads? all over
        //            return lead.Records;
        //        }
        //        );
        //}
        //catch (Exception e)
        //{
        //    this.ViewBag.OperationName = "query Salesforce Lead"; //Leads?
        //    this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
        //    this.ViewBag.ErrorMessage = e.Message;
        //}
        //if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
        //{
        //    return Redirect(this.ViewBag.AuthorizationUrl);
        //}
        //return View(selectedLeads);


        public async Task<ActionResult> Details(string id)
        {
            IEnumerable<Lead> selectedLeads = Enumerable.Empty<Lead>();
            try
            {
                selectedLeads = await SalesforceService.MakeAuthenticatedClientRequestAsync(
                    async (client) =>
                    {
                        QueryResult<Lead> leads =
                            await client.QueryAsync<Lead>("SELECT ID, Last Name, First Name, Company, City, State, MobilePhone ,Phone, Email From Lead Where Lead Id = '" + id + "'");
                        return leads.Records;
                    }
                    );
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Salesforce Lead Details";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            return View(selectedLeads.FirstOrDefault());
        }

        public async Task<ActionResult> Edit(string id)
        {
            IEnumerable<Lead> selectedLeads = Enumerable.Empty<Lead>();
            try
            {
                selectedLeads = await SalesforceService.MakeAuthenticatedClientRequestAsync(
                    async (client) =>
                    {
                        QueryResult<Lead> lead =
                            await client.QueryAsync<Lead>("SELECT ID, Last Name, First Name, Company, City, State, MobilePhone ,Phone, Email From Lead Where Lead Id= '" + id + "'");
                        return lead.Records;
                    }
                    );
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Edit Salesforce Leads";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            return View(selectedLeads.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = _LeadPostBinding)] Lead lead)
        {
            SuccessResponse success = new SuccessResponse();
            try
            {
                success = await SalesforceService.MakeAuthenticatedClientRequestAsync(
                    async (client) =>
                    {
                        success = await client.UpdateAsync("Lead", lead.Id, lead);
                        return success;
                    }
                    );
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Edit Salesforce Lead";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            if (success.Success == "true")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(lead);
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IEnumerable<Lead> selectedLead = Enumerable.Empty<Lead>();
            try
            {
                selectedLead = await SalesforceService.MakeAuthenticatedClientRequestAsync(
                async (client) =>
                {
                    // Query the properties you'll display for the user to confirm they wish to delete this Contact
                    QueryResult<Lead> lead =
                        await client.QueryAsync<Lead>(string.Format("SELECT Lead ID, Last Name, First Name, Company, City, State, MobilePhone ,Phone, Email From Lead Where Lead Id='{0}'", id));
                    return lead.Records;
                }
                );
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "query Salesforce Leads";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            if (selectedLead.Count() == 0)
            {
                return View();
            }
            else
            {
                return View(selectedLead.FirstOrDefault());
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            bool success = false;
            try
            {
                success = await SalesforceService.MakeAuthenticatedClientRequestAsync(
                    async (client) =>
                    {
                        success = await client.DeleteAsync("Contact", id);
                        return success;
                    }
                    );
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Delete Salesforce Leads";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = _LeadPostBinding)] Lead lead)
        {
            String id = String.Empty;
            try
            {
                id = await SalesforceService.MakeAuthenticatedClientRequestAsync(
                    async (client) =>
                    {
                        return await client.CreateAsync("Lead", lead);
                    }
                    );
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Create Salesforce Lead";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            if (this.ViewBag.ErrorMessage == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(lead);
            }
        }
    }
}