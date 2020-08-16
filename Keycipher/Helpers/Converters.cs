namespace Keycipher.Helpers
{
    public static class Converters
    {
        public static string ToStringBoolean(this bool input)
        {
            switch (input)
            {
                case true: return "yes";
                case false: return "no";
            }

            return "";
        }

        public static bool ToBoolean(this string input)
        {
            switch (input)
            {
                case "yes": return true;
                case "no": return false;
                case "1": return true;
                case "0": return false;
                case "true": return true;
                case "false": return false;
                default: return false;
            }
        }
    }
}