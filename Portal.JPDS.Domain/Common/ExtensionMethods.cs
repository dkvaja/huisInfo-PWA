using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Portal.JPDS.Domain.Common
{
    public static class ExtensionMethods
    {
        public static bool IsImage(this string filePath)
        {
            var imageExtensions = new string[]{
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".svg",
                ".jfif",
                ".tiff",
                ".tif"
            };
            var extension = Path.GetExtension(filePath).ToLower();

            return imageExtensions.Contains(extension);
        }
        public static bool IsValidEmail(this string email)
        {
            try
            {
                if (email == null)
                    return false;

                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string ToUpperString(this Guid guid)
        {
            return guid.ToString().ToUpperInvariant();
        }

        public static string TrimToMaxLength(this string str, int length)
        {
            if (str != null)
            {
                return str.Length > length ? str.Substring(0, length) : str;
            }
            return string.Empty;
        }

        public static Guid? ToGuid(this string guidString)
        {
            Guid guid;
            if (Guid.TryParse(guidString, out guid))
            {
                return guid;
            }
            return null;
        }

        public static DateTime TruncateMilliSeconds(this DateTime dateTime)
        {
            return new DateTime(
                dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond),
                dateTime.Kind
            );
        }
    }
}
