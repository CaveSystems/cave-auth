using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using Cave.Collections.Generic;
using Cave.Net;

namespace Cave.Auth
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc6238
    /// </summary>
    public class TimeBasedOTP
    {
        static long Ticks1970 { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        static readonly SynchronizedDictionary<string, long> used = new();

        #region private class

        /// <summary>Gets the challenge value for the current utc time stamp.</summary>
        /// <returns></returns>
        static long GetChallenge()
        {
            var seconds = (DateTime.UtcNow.Ticks - Ticks1970) / TimeSpan.TicksPerSecond;
            return seconds / Interval;
        }

        /// <summary>Cleans the (expired) used codes.</summary>
        static void CleanUsed()
        {
            var start = GetChallenge() - DriftPast;
            foreach (var item in used.ToArray())
            {
                //throw away old used codes before drift
                if (item.Value < start)
                {
                    if (!used.Remove(item.Key))
                    {
                        throw new KeyNotFoundException();
                    }
                }
            }
        }

        /// <summary>Gets the code for the specified challenge.</summary>
        /// <param name="secret">The users secret.</param>
        /// <param name="challenge">The challenge.</param>
        /// <returns></returns>
        [SuppressMessage("Security", "CA5350: HMACSHA1 required.")]
        static string GetCode(string secret, long challenge)
        {
            var challengeData = new byte[8];
            for (var j = 7; j >= 0; j--)
            {
                challengeData[j] = (byte)((int)challenge & 0xff);
                challenge >>= 8;
            }

            //truncate secret
            var key = Base32.OTP.Decode(secret.ToLower());
            for (var i = secret.Length; i < key.Length; i++)
            {
                key[i] = 0;
            }

            var mac = new HMACSHA1(key);
            var hash = mac.ComputeHash(challengeData);

            var offset = hash[hash.Length - 1] & 0xf;

            var truncatedHash = 0;
            for (var j = 0; j < 4; j++)
            {
                truncatedHash <<= 8;
                truncatedHash |= hash[offset + j];
            }

            truncatedHash &= 0x7FFFFFFF;
            truncatedHash %= 1000000;

            var code = truncatedHash.ToString();
            return code.PadLeft(6, '0');
        }

        #endregion

        /// <summary>Gets or sets the interval in seconds.</summary>
        /// <value>The interval in seconds.</value>
        public static int Interval { get; set; } = 30;

        /// <summary>Gets or sets the allowed drift into the past in steps.</summary>
        /// <value>The allowed drift into the past in steps.</value>
        public static int DriftPast { get; set; }

        /// <summary>Gets or sets the allowed drift into future in steps.</summary>
        /// <value>The allowed drift into the future in steps.</value>
        public static int DriftFuture { get; set; }

        /// <summary>Initializes a new instance of the <see cref="TimeBasedOTP"/> class.</summary>
        private TimeBasedOTP() { }

        /// <summary>Gets the current code.</summary>
        /// <param name="secret">The secret.</param>
        /// <returns>The current code. </returns>
        public static string GetCurrentCode(string secret)
        {
            var challenge = GetChallenge();
            return GetCode(secret, challenge);
        }

        /// <summary>Checks the code.</summary>
        /// <param name="secret">The secret.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static bool CheckCode(string secret, string code)
        {
            CleanUsed();
            if (used.ContainsKey(code))
            {
                return false;
            }

            var challenge = GetChallenge();
            for (var i = challenge - DriftPast; i <= challenge + DriftFuture; i++)
            {
                if (code == GetCode(secret, i))
                {
                    used.Add(code, i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>Gets the google qr link.</summary>
        /// <param name="secret">The secret.</param>
        /// <param name="company">The company.</param>
        /// <param name="product">The product.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string GetGoogleQRLink(string secret, string company = null, string product = null, int size = 250)
        {
            company ??= AssemblyVersionInfo.Program.Company;
            product ??= AssemblyVersionInfo.Program.Product;
            return $"https://chart.googleapis.com/chart?cht=qr&chs={size}x{size}&chld=H&chl=otpauth://totp/{product}?secret={secret}%26issuer={company}";
        }

        /// <summary>Downloads the google QR image.</summary>
        /// <param name="secret">The secret.</param>
        /// <param name="company">The company.</param>
        /// <param name="product">The product.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static byte[] DownloadGoogleQR(string secret, string company = null, string product = null, int size = 250) => HttpConnection.Get(GetGoogleQRLink(secret, company, product, size));
    }
}
