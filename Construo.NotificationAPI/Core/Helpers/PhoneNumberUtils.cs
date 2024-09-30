using System.Text.RegularExpressions;

namespace Construo.NotificationAPI.Core.Helpers;

public class PhoneNumberUtils
{
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, "^\\+?(9[976][0-9]|8[987530][0-9]|6[987][0-9]|5[90][0-9]|42[0-9]|3[875][0-9]|2[98654321][0-9]|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1|0)[0-9]{0,14}$");
    }
    /// <summary>
    /// Makes all phone numbers conform to international phone number format i.e with country code
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public static string SanitizePhoneNumber(string phoneNumber)
    {
        string strPhone = phoneNumber.Replace("+", "");
        bool isValidPhone = IsValidPhoneNumber(strPhone);
        return isValidPhone ? "256" + strPhone.Substring(strPhone.Length - 9, 9) : "-1";
    }


    /// <summary>
    /// Makes all phone numbers conform to international phone number format i.e with country code
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="countryCode"></param>
    /// <returns></returns>
    public static string SanitizePhoneNumber(string phoneNumber, string countryCode)
    {
        var strPhone = phoneNumber.Replace("+", "");
        var isValidPhone = IsValidPhoneNumber(strPhone);
        var startsWithCode = strPhone.StartsWith(countryCode);
        if (isValidPhone && startsWithCode)
        {
            return strPhone;
        }

        if (isValidPhone)
        {
            return countryCode + strPhone.Substring(1, strPhone.Length - 1);
        }

        return "-1";
    }
}
