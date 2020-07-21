using System;

namespace Cryental.LilacLicensing.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = new Configuration
            {
                ProductID = "", // Your Product ID
                PublicKey = "", // Public Key. Only for Validate.
                PrivateKey = "", // Private Key. All Features without Validate.
                AccessKey = "" // Your Account Access Key
            };

            var licensing = new Licensing(config);

            try
            {
                // Generate License
                var generate = licensing.GenerateLicense(true, 24);
                Console.WriteLine(generate.License);

                // Validate License
                var validate = licensing.Validate(generate.License);
                Console.WriteLine(validate.License);

                // Get All Licenses
                var listLicenses = licensing.GetAllLicenses();

                foreach (var item in listLicenses)
                {
                    Console.WriteLine(item.License);
                }

                // Delete License
                licensing.DeleteLicense(generate.License);

                // Get Current Hardware ID
                Console.WriteLine(licensing.GetHardwareID());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}