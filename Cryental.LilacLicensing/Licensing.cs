using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Cryental.LilacLicensing.Helpers;
using Cryental.LilacLicensing.Models;
using DeviceId;
using DeviceId.Encoders;
using DeviceId.Formatters;
using Newtonsoft.Json;
using RestSharp;

namespace Cryental.LilacLicensing
{
    public class Licensing
    {
        internal Configuration Config = new Configuration();
        internal string HardwareID;

        public Licensing(Configuration config)
        {
            if (config.ProductID.Length > 5)
            {
                throw new Exception("Product ID length must be less than or equal to 5.");
            }

            if (string.IsNullOrEmpty(config.AccessKey))
            {
                throw new Exception("You must provide an access key to use this library.");
            }

            HardwareID = new DeviceIdBuilder().AddProcessorId().AddMotherboardSerialNumber().UseFormatter(new HashDeviceIdFormatter(SHA256.Create, new Base64UrlByteArrayEncoder())).ToString().ToUpper();
            Config = config;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public string GetHardwareID()
        {
            return HardwareID;
        }

        public Licenses.Object Validate(string license)
        {
            if (string.IsNullOrEmpty(Config.PublicKey))
            {
                throw new Exception("You must provide a public key to use this function.");
            }

            if (!Verifications.IsCorrectAccessFormat(Config.AccessKey))
            {
                throw new Exception("You must provide a correct access key to use this function.");
            }

            if (!Verifications.IsCorrectKey(Config.PublicKey))
            {
                throw new Exception("You must provide a correct public key to use this function.");
            }

            if (!Verifications.IsCorrectLicenseFormat(license))
            {
                throw new Exception("Please provide a correct license key.");
            }

            var client = new RestClient(Globals.EndPoint);
            var request = new RestRequest("validate", Method.POST);

            request.AddHeader("X-API-KEY", Config.PublicKey);
            request.AddHeader("X-PRODUCT-ID", Config.ProductID);
            request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

            request.AddParameter("license", license, ParameterType.GetOrPost);
            request.AddParameter("hwid", HardwareID, ParameterType.GetOrPost);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new WebException("Can't connect to the server at this moment.");
            }

            var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

            if (parsedResponse.Result)
            {
                return parsedResponse.Object;
            }

            throw new Exception(parsedResponse.Message);
        }

        public List<Licenses.Object> GetAllLicenses()
        {
            if (string.IsNullOrEmpty(Config.PrivateKey))
            {
                throw new Exception("You must provide a private key to use this function.");
            }

            if (!Verifications.IsCorrectAccessFormat(Config.AccessKey))
            {
                throw new Exception("You must provide a correct access key to use this function.");
            }

            if (!Verifications.IsCorrectKey(Config.PrivateKey))
            {
                throw new Exception("You must provide a correct private key to use this function.");
            }

            var client = new RestClient(Globals.EndPoint);
            var request = new RestRequest("/", Method.GET);

            request.AddHeader("X-API-KEY", Config.PrivateKey);
            request.AddHeader("X-PRODUCT-ID", Config.ProductID);
            request.AddHeader("X-ACCESS-KEY", Config.AccessKey);


            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new WebException("Can't connect to the server at this moment.");
            }

            var parsedResponse = JsonConvert.DeserializeObject<Licenses.RawInput>(response.Content);

            if (!parsedResponse.Result)
            {
                throw new Exception(parsedResponse.Message);
            }

            var tempList = new List<Licenses.Object>();

            foreach (var item in parsedResponse.Objects)
            {
                item.Config = Config;
                item.HardwareID = HardwareID;

                tempList.Add(item);
            }

            return tempList;
        }

        public void DeleteLicense(string license)
        {
            if (string.IsNullOrEmpty(Config.PrivateKey))
            {
                throw new Exception("You must provide a private key to use this function.");
            }

            if (!Verifications.IsCorrectAccessFormat(Config.AccessKey))
            {
                throw new Exception("You must provide a correct access key to use this function.");
            }

            if (!Verifications.IsCorrectKey(Config.PrivateKey))
            {
                throw new Exception("You must provide a correct private key to use this function.");
            }

            var client = new RestClient(Globals.EndPoint);
            var request = new RestRequest("/" + license, Method.DELETE);

            request.AddHeader("X-API-KEY", Config.PrivateKey);
            request.AddHeader("X-PRODUCT-ID", Config.ProductID);
            request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new WebException("Can't connect to the server at this moment.");
            }

            var parsedResponse = JsonConvert.DeserializeObject<CommonResponse>(response.Content);

            if (parsedResponse.Result)
            {
                return;
            }

            throw new Exception(parsedResponse.Message);
        }

        public Licenses.Object GenerateLicense(bool hardwareLock, int expireHour, bool lifetime = false, string customData = "", string comment = "")
        {
            if (string.IsNullOrEmpty(Config.PrivateKey))
            {
                throw new Exception("You must provide a private key to use this function.");
            }

            if (!Verifications.IsCorrectAccessFormat(Config.AccessKey))
            {
                throw new Exception("You must provide a correct access key to use this function.");
            }

            if (!Verifications.IsCorrectKey(Config.PrivateKey))
            {
                throw new Exception("You must provide a correct private key to use this function.");
            }

            if (expireHour <= 0)
            {
                throw new Exception("You must provide a correct expire hour to use this function.");
            }

            var client = new RestClient(Globals.EndPoint);
            var request = new RestRequest("/", Method.POST);

            request.AddHeader("X-API-KEY", Config.PrivateKey);
            request.AddHeader("X-PRODUCT-ID", Config.ProductID);
            request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

            request.AddParameter("expireHour", expireHour.ToString());
            request.AddParameter("hwidStatus", hardwareLock.ToStringBoolean());
            request.AddParameter("lifetime", lifetime.ToStringBoolean());
            request.AddParameter("customData", customData);
            request.AddParameter("comment", comment);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new WebException("Can't connect to the server at this moment.");
            }

            var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

            if (parsedResponse.Result)
            {
                parsedResponse.Object.Config = Config;
                parsedResponse.Object.HardwareID = HardwareID;

                return parsedResponse.Object;
            }

            throw new Exception(parsedResponse.Message);
        }
    }
}