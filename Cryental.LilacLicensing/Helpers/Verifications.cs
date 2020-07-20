using System;
using System.Text.RegularExpressions;

namespace Cryental.LilacLicensing.Helpers
{
    public static class Verifications
    {
        public static bool IsCorrectLicenseFormat(string license)
        {
            return Regex.IsMatch(license, @"[A-Z0-9]{6}-[A-Z0-9]{6}-[A-Z0-9]{6}-[A-Z0-9]{6}-[A-Z0-9]{6}-[A-Z0-9]{6}", RegexOptions.Compiled);
        }

        public static bool IsCorrectAccessFormat(string guid)
        {
            try
            {
                return Guid.TryParse(guid, out _);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsCorrectKey(string key)
        {
            return key.Length == 80;
        }
    }
}