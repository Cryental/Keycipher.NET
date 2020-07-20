using System;
using System.Collections.Generic;
using System.Net;
using Cryental.LilacLicensing.Helpers;
using Newtonsoft.Json;
using RestSharp;

namespace Cryental.LilacLicensing.Models
{
    public class Licenses
    {
        public class Subscription
        {
            [JsonProperty("starts")]
            public string Starts { get; set; }

            [JsonProperty("expires")]
            public string Expires { get; set; }
        }

        public class Extra
        {
            [JsonProperty("lockedTo")]
            public string LockedTo { get; set; }

            [JsonProperty("custom_data")]
            public string CustomData { get; set; }

            [JsonProperty("comment")]
            public string Comment { get; set; }
        }

        public class Object
        {
            internal Configuration Config = new Configuration();
            internal string HardwareID;

            [JsonProperty("license")]
            public string License { get; set; }

            [JsonProperty("isActivated")]
            public bool IsActivated { get; set; }

            [JsonProperty("isExpired")]
            public bool IsExpired { get; set; }

            [JsonProperty("isLifetime")]
            public bool IsLifetime { get; set; }

            [JsonProperty("isBanned")]
            public bool IsBanned { get; set; }

            [JsonProperty("subscription")]
            public Subscription Subscription { get; set; }

            [JsonProperty("extra")]
            public Extra Extra { get; set; }

            public void Ban()
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
                var request = new RestRequest("banStatus", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("isBanned", true.ToStringBoolean());

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void RefreshData()
            {
                if (string.IsNullOrEmpty(Config.PrivateKey))
                {
                    throw new Exception("You must provide a private key to use this function.");
                }

                if (!Verifications.IsCorrectAccessFormat(Config.AccessKey))
                {
                    throw new Exception("You must provide a correct access key to use this function.");
                }

                if (!Verifications.IsCorrectKey(Config.PublicKey))
                {
                    throw new Exception("You must provide a correct public key to use this function.");
                }

                if (!Verifications.IsCorrectLicenseFormat(License))
                {
                    throw new Exception("Please provide a correct license key.");
                }

                var client = new RestClient(Globals.EndPoint);
                var request = new RestRequest("/" + License, Method.GET);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (!parsedResponse.Result)
                {
                    throw new Exception(parsedResponse.Message);
                }

                License = parsedResponse.Object.License;
                IsActivated = parsedResponse.Object.IsActivated;
                IsExpired = parsedResponse.Object.IsExpired;
                IsLifetime = parsedResponse.Object.IsLifetime;
                IsBanned = parsedResponse.Object.IsBanned;
                if (parsedResponse.Object.Subscription != null)
                {
                    Subscription = new Subscription
                    {
                        Starts = parsedResponse.Object.Subscription.Starts,
                        Expires = parsedResponse.Object.Subscription.Expires
                    };
                }

                Extra = new Extra
                {
                    LockedTo = parsedResponse.Object.Extra.LockedTo,
                    CustomData = parsedResponse.Object.Extra.CustomData,
                    Comment = parsedResponse.Object.Extra.Comment
                };
            }

            public void Unban()
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
                var request = new RestRequest("banStatus", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("isBanned", false.ToStringBoolean());

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void ModifyComment(string comment)
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
                var request = new RestRequest("", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("comment", comment);

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void ModifyCustomData(string customData)
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
                var request = new RestRequest("", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("customData", customData);

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void ReleaseHardwareID()
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
                var request = new RestRequest("", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("hwid", "");

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void SetHardwareID(string newHardwareID)
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

                if (newHardwareID.Length != 43)
                {
                    throw new Exception("Please enter the correct hardware ID.");
                }

                var client = new RestClient(Globals.EndPoint);
                var request = new RestRequest("", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("hwid", newHardwareID);

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void ExtendHours(int hours)
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

                if (hours <= 0)
                {
                    throw new Exception("Please provide 1 hour or more for a parameter.");
                }

                var client = new RestClient(Globals.EndPoint);
                var request = new RestRequest("", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("addHour", hours.ToString());

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void SetHardwareLockStatus(bool status)
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
                var request = new RestRequest("", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("hwidStatus", status.ToStringBoolean());

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void ModifyHours(int hours)
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

                if (hours <= 0)
                {
                    throw new Exception("Please provide 1 hour or more for a parameter.");
                }

                var client = new RestClient(Globals.EndPoint);
                var request = new RestRequest("", Method.PATCH);

                request.AddHeader("X-API-KEY", Config.PrivateKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AlwaysMultipartFormData = true;

                request.AddParameter("license", License);
                request.AddParameter("expireHour", hours.ToString());

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }

            public void Activate()
            {
                if (IsActivated)
                {
                    throw new Exception("This license is already activated.");
                }

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

                if (!Verifications.IsCorrectLicenseFormat(License))
                {
                    throw new Exception("Please provide a correct license key.");
                }

                var client = new RestClient(Globals.EndPoint);
                var request = new RestRequest("validate", Method.POST);

                request.AddHeader("X-API-KEY", Config.PublicKey);
                request.AddHeader("X-PRODUCT-ID", Config.ProductID);
                request.AddHeader("X-ACCESS-KEY", Config.AccessKey);

                request.AddParameter("license", License, ParameterType.GetOrPost);

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    throw new WebException("Can't connect to the server at this moment.");
                }

                var parsedResponse = JsonConvert.DeserializeObject<License.RawInput>(response.Content);

                if (parsedResponse.Result)
                {
                    RefreshData();
                    return;
                }

                throw new Exception(parsedResponse.Message);
            }
        }

        public class RawInput
        {
            [JsonProperty("result")]
            public bool Result { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("objects")]
            public List<Object> Objects { get; set; }
        }
    }
}