using System;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace veracodeAPI
{
    public abstract class HmacAuthHeader
    {
        //private static readonly RNGCryptoServiceProvider RngRandom = new RNGCryptoServiceProvider();
        private static readonly RandomNumberGenerator RngRandom = RandomNumberGenerator.Create();

        public static readonly HmacAuthHeader HmacSha256 = new HmacSha256AuthHeader();

        private sealed class HmacSha256AuthHeader : HmacAuthHeader
        {
            protected override string GetHashAlgorithm() { return "HmacSHA256"; }

            protected override string GetAuthorizationScheme() { return "VERACODE-HMAC-SHA-256"; }

            protected override string GetRequestVersion() { return "vcode_request_version_1"; }

            protected override string GetTextEncoding() { return "UTF-8"; }

            protected override int GetNonceSize() { return 16; }

            internal HmacSha256AuthHeader() { }
        }

        protected abstract string GetHashAlgorithm();
        protected abstract string GetAuthorizationScheme();
        protected abstract string GetRequestVersion();
        protected abstract string GetTextEncoding();
        protected abstract int GetNonceSize();

        protected string CurrentDateStamp()
        {
            return ((long)((TimeSpan)(DateTime.UtcNow - new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();
        }

        protected byte[] NewNonce(int size)
        {
            byte[] nonceBytes = new byte[size];
            RngRandom.GetBytes(nonceBytes);

            return nonceBytes;
        }

        protected byte[] ComputeHash(byte[] data, byte[] key)
        {
            HMAC? mac = HMAC.Create(GetHashAlgorithm());
            if(mac != null)
            {
                mac.Key = key;
                return mac.ComputeHash(data);
            }
            else
            {
                Console.WriteLine("The ComputeHash function has some issue since mac is null");
                return data;
            }
        }

        protected byte[] CalculateDataSignature(byte[] apiKeyBytes, byte[] nonceBytes, string dateStamp, string data)
        {
            byte[] kNonce = ComputeHash(nonceBytes, apiKeyBytes);
            byte[] kDate = ComputeHash(Encoding.GetEncoding(GetTextEncoding()).GetBytes(dateStamp), kNonce);
            byte[] kSignature = ComputeHash(Encoding.GetEncoding(GetTextEncoding()).GetBytes(GetRequestVersion()), kDate);

            return ComputeHash(Encoding.GetEncoding(GetTextEncoding()).GetBytes(data), kSignature);
        }

        public string CalculateAuthorizationHeader(string apiId, string apiKey, string hostName, string uriString, string urlQueryParams, string httpMethod)
        {
            try
            {
                if (urlQueryParams != null)
                {
                    uriString += (urlQueryParams);
                }
                string data = $"id={apiId}&host={hostName}&url={uriString}&method={httpMethod}";
                string dateStamp = CurrentDateStamp();
                byte[] nonceBytes = NewNonce(GetNonceSize());
                byte[] dataSignature = CalculateDataSignature(FromHexBinary(apiKey), nonceBytes, dateStamp, data);
                string authorizationParam = $"id={apiId},ts={dateStamp},nonce={ToHexBinary(nonceBytes)},sig={ToHexBinary(dataSignature)}";

                return GetAuthorizationScheme() + " " + authorizationParam;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public static string ToHexBinary(byte[] bytes)
        {
            return Convert.ToHexString(bytes);
        }

        public static byte[] FromHexBinary(string hexBinaryString)
        {
            return Convert.FromHexString(hexBinaryString);
        }

        public static bool IsValidHexBinary(string hexBinaryString)
        {
            if (hexBinaryString != null)
            {
                try
                {
                    byte[] bytes = FromHexBinary(hexBinaryString);
                    return bytes != null;
                }
                catch (Exception) { }
            }

            return false;
        }

        public static bool IsValidAuthHeaderToken(string authHeaderToken)
        {
            if (authHeaderToken != null)
            {
                // For valid Authorization header token syntax see https://www.ietf.org/rfc/rfc2617.txt, https://www.ietf.org/rfc/rfc2068.txt
                bool isMatch = Regex.IsMatch(authHeaderToken, "^[\\x21\\x23-\\x27\\x2A-\\x2B\\x2D-\\x2E\\x30-\\x39\\x41-\\x5A\\x5E-\\x7A\\x7C\\x7E]+$");

                return isMatch;
            }

            return false;
        }

        private HmacAuthHeader() { }
    }
}     