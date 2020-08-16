using System;
using System.Linq;

namespace Keycipher.Test
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
                // Generate 1 License
                var generate = licensing.GenerateLicense(1, true, 24);
                foreach (var obj in generate)
                {
                    Console.WriteLine(obj.License);
                }

                // Validate License
                var validate = licensing.Validate(generate[0].License);
                Console.WriteLine(validate.License);

                // Get All Licenses
                var listLicenses = licensing.GetAllLicenses();

                foreach (var item in listLicenses)
                {
                    Console.WriteLine(item.License);
                }

                // Delete License
                licensing.DeleteLicense(generate.FirstOrDefault()?.License);

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