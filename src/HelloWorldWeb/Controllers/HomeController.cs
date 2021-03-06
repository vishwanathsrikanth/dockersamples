﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;

namespace HelloWorldWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Submit(string email, string password)
        {
            var userContext = new UserContext();
            userContext.Email = email;
            userContext.Password = password;
            var userContextJson = JsonConvert.SerializeObject(userContext);
            AddToQueue(userContextJson);
            ViewBag.Message = "Registration Completed Successfully, you will recieve a confirmation mail shortly.";
            return View("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContextJson"></param>
        private async void AddToQueue(string userContextJson)
        {
            //Sample storage connection string format: DefaultEndpointsProtocol=https;AccountName=azurestore;AccountKey=KIQ1QGEUvKKYibPFMVPMjfEPoQQCE3HCr71yZp/A1YvSuHrMFMK0ZlvqvSrmAym4OA3DwT05suxBMHH3/zDNWQ==
            var storageAccount = CloudStorageAccount.Parse("[Replace-with-StorageConnectionString");
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queueReference = queueClient.GetQueueReference("messages");
            await queueReference.CreateIfNotExistsAsync();
            await queueReference.AddMessageAsync(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(userContextJson));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
