#region CopyRight 2018
/*
    Copyright (c) 2003-2018 Andreas Rohleder (andreas@rohleder.cc)
    All rights reserved
*/
#endregion
#region License LGPL-3
/*
    This program/library/sourcecode is free software; you can redistribute it
    and/or modify it under the terms of the GNU Lesser General Public License
    version 3 as published by the Free Software Foundation subsequent called
    the License.

    You may not use this program/library/sourcecode except in compliance
    with the License. The License is included in the LICENSE file
    found at the installation directory or the distribution package.

    The above copyright notice and this permission notice shall be included
    in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion License
#region Authors & Contributors
/*
   Author:
     Andreas Rohleder <andreas@rohleder.cc>

   Contributors:
 */
#endregion Authors & Contributors

using Cave.Collections.Generic;
using Cave.Net;
using Cave.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Cave.Auth
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc6238
    /// </summary>
    public class TimeBasedOTP
    {
        static long Ticks1970 { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        static SynchronizedDictionary<string, long> used = new SynchronizedDictionary<string, long>();

        #region private class

        /// <summary>Gets the challenge value for the current utc time stamp.</summary>
        /// <returns></returns>
        static long GetChallenge()
        {
            long seconds = (DateTime.UtcNow.Ticks - Ticks1970) / TimeSpan.TicksPerSecond;
            return seconds / Interval;
        }

        /// <summary>Cleans the (expired) used codes.</summary>
        static void CleanUsed()
        {
            long start = GetChallenge() - DriftPast;
            foreach (var item in used.ToArray())
            {
                //throw away old used codes before drift
                if (item.Value < start) if (!used.Remove(item.Key)) throw new KeyNotFoundException();
            }
        }

        /// <summary>Gets the code for the specified challenge.</summary>
        /// <param name="secret">The users secret.</param>
        /// <param name="challenge">The challenge.</param>
        /// <returns></returns>
        static string GetCode(string secret, long challenge)
        {
            byte[] challengeData = new byte[8];
            for (int j = 7; j >= 0; j--)
            {
                challengeData[j] = (byte)((int)challenge & 0xff);
                challenge >>= 8;
            }

            //truncate secret
            var key = Base32.OTP.Decode(secret.ToLower());
            for (int i = secret.Length; i < key.Length; i++)
            {
                key[i] = 0;
            }

            HMACSHA1 mac = new HMACSHA1(key);
            var hash = mac.ComputeHash(challengeData);

            int offset = hash[hash.Length - 1] & 0xf;

            int truncatedHash = 0;
            for (int j = 0; j < 4; j++)
            {
                truncatedHash <<= 8;
                truncatedHash |= hash[offset + j];
            }

            truncatedHash &= 0x7FFFFFFF;
            truncatedHash %= 1000000;

            string code = truncatedHash.ToString();
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
            long challenge = GetChallenge();
            return GetCode(secret, challenge);
        }

        /// <summary>Checks the code.</summary>
        /// <param name="secret">The secret.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static bool CheckCode(string secret, string code)
        {
            CleanUsed();
            if (used.ContainsKey(code)) return false;

            long challenge = GetChallenge();
            for (long i = challenge - DriftPast; i <= challenge + DriftFuture; i++)
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
            if (company == null) company = AssemblyVersionInfo.Program.Company;
            if (product == null) product = AssemblyVersionInfo.Program.Product;
            return $"https://chart.googleapis.com/chart?cht=qr&chs=500x500&chld=H&chl=otpauth://totp/{product}?secret={secret}%26issuer={company}";
        }

        /// <summary>Downloads the google QR image.</summary>
        /// <param name="secret">The secret.</param>
        /// <param name="company">The company.</param>
        /// <param name="product">The product.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static byte[] DownloadGoogleQR(string secret, string company = null, string product = null, int size = 250)
        {
            return HttpConnection.Get(GetGoogleQRLink(secret, company, product, size));
        }
    }
}
