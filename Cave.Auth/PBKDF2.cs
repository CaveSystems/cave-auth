using System;
using System.Security.Cryptography;
using System.Text;
using Cave.IO;
#if !NET20 && !NET35
using System.Numerics;
#endif

namespace Cave.Auth
{
    /// <summary>
    /// Implements password-based key derivation functionality, PBKDF2, by using a pseudo-random number generator based on any HMAC algorithm.
    /// </summary>
    /// <seealso cref="DeriveBytes" />
    public class PBKDF2 : DeriveBytes, IDisposable
    {
        /// <summary>
        /// Guesses the complexity (bit variation strength) of a specified salt or password.
        /// </summary>
        /// <param name="data">The password or salt.</param>
        /// <returns>Returns the estimated strength</returns>
        public static int GuessComplexity(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var result = 1;
            for (var i = 1; i < data.Length; i++)
            {
                var diff = Math.Abs(data[0] - data[1]);
                while (diff > 0)
                {
                    diff >>= 1;
                    result++;
                }
            }
            return result;
        }

        /// <summary>Creates a new instance with the specified HMAC.</summary>
        /// <returns></returns>
        public static PBKDF2 Create(HMAC algorithm) => new(algorithm);

#if !NET20 && !NET35
        /// <summary>Creates a new instance using the specified private bigint as key and salt (last 16 bytes are used as salt).</summary>
        /// <remarks>In this function the number of iterations are set to 2 for performance reasons. This should not impact security if the private 
        /// is chosen and protected well.</remarks>
        /// <param name="i">The BigInt.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">BigInt has less than 32 bytes (256 bits)!</exception>
        public static PBKDF2 FromPrivate(BigInteger i)
        {
            var data = i.ToByteArray();
            if (data.Length < 64)
            {
                throw new ArgumentException("BigInt has less than 64 bytes (512 bits)!");
            }

            var l_Splitter = data.Length / 2;
            var l_Password = ArrayExtension.GetRange(data, 0, l_Splitter);
            var salt = ArrayExtension.GetRange(data, l_Splitter);
            return new PBKDF2(l_Password, salt, 2);
        }
#endif

        int iterations = 1000;
        int hashNumber;
        byte[] salt;
        HMAC algorithm;
        FifoBuffer buffer;

        PBKDF2(HMAC algorithm) => this.algorithm = algorithm;

        /// <summary>Initializes a new instance of the <see cref="PBKDF2"/> class using the default <see cref="HMACSHA512"/> algorithm.</summary>
        public PBKDF2() : this(new HMACSHA512()) { }

        /// <summary>Initializes a new instance of the <see cref="PBKDF2" /> class using the default <see cref="HMACSHA512" /> algorithm.</summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        public PBKDF2(string password, byte[] salt) : this(new HMACSHA512())
        {
            SetPassword(password);
            SetSalt(salt);
        }

        /// <summary>Initializes a new instance of the <see cref="PBKDF2" /> class using the default <see cref="HMACSHA512" /> algorithm.</summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        public PBKDF2(byte[] password, byte[] salt) : this(new HMACSHA512())
        {
            SetPassword(password);
            SetSalt(salt);
        }

        /// <summary>Initializes a new instance of the <see cref="PBKDF2" /> class using the default <see cref="HMACSHA512" /> algorithm.</summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The iterations. This value is not checked and allows invalid values!</param>
        public PBKDF2(byte[] password, byte[] salt, int iterations) : this(new HMACSHA512())
        {
            this.iterations = iterations;
            SetPassword(password);
            SetSalt(salt);
        }

        void FillBuffer()
        {
            var i = ++hashNumber;
            var s = new byte[salt.Length + 4];
            Buffer.BlockCopy(salt, 0, s, 0, salt.Length);
            s[s.Length - 4] = (byte)(i >> 24);
            s[s.Length - 3] = (byte)(i >> 16);
            s[s.Length - 2] = (byte)(i >> 8);
            s[s.Length - 1] = (byte)i;
            // this is like j=0
            var u1 = algorithm.ComputeHash(s);
            var data = u1;
            // so we start at j=1
            for (var j = 1; j < iterations; j++)
            {
                var un = algorithm.ComputeHash(data);
                // xor
                for (var k = 0; k < u1.Length; k++)
                {
                    u1[k] = (byte)(u1[k] ^ un[k]);
                }

                data = un;
            }
            buffer.Enqueue(u1, true);
        }

        /// <summary>Sets the password.</summary>
        /// <param name="password">The password.</param>
        public void SetPassword(string password) => SetPassword(Encoding.UTF8.GetBytes(password));

        /// <summary>Gets or sets the iteration count.</summary>
        /// <value>The iteration count.</value>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentOutOfRangeException">IterationCount &lt; 1000</exception>
        public int IterationCount
        {
            get => iterations;
            set
            {
                if (buffer != null)
                {
                    throw new InvalidOperationException(string.Format("Cannot change the {0} after calling GetBytes() the first time!", "IterationCount"));
                }

                if (value < 1000)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "IterationCount < 1000");
                }

                iterations = value;
            }
        }

        /// <summary>Gets or sets the salt.</summary>
        /// <value>The salt.</value>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException">Salt</exception>
        /// <exception cref="ArgumentException">Salt &lt; 8 bytes</exception>
        public byte[] Salt => (byte[])salt.Clone();

        /// <summary>Sets the salt.</summary>
        /// <param name="salt">The salt.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="System.ArgumentException">Salt &lt; 8 bytes;value</exception>
        public void SetSalt(byte[] salt)
        {
            if (buffer != null)
            {
                throw new InvalidOperationException(string.Format("Cannot change parameter {0} after calling GetBytes() the first time!", nameof(salt)));
            }

            if (salt == null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            if (salt.Length < 8)
            {
                throw new ArgumentException("Salt < 8 bytes", nameof(salt));
            }

            this.salt = (byte[])salt.Clone();
        }

        /// <summary>Gets or sets the password.</summary>
        /// <value>The password.</value>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException">Password</exception>
        /// <exception cref="ArgumentException">Password &lt; 8 bytes</exception>
        public byte[] Password => (byte[])algorithm.Key.Clone();

        /// <summary>Sets the password.</summary>
        /// <param name="password">The password.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="System.ArgumentException">Password &lt; 8 bytes;value</exception>
        public void SetPassword(byte[] password)
        {
            if (buffer != null)
            {
                throw new InvalidOperationException(string.Format("Cannot change parameter {0} after calling GetBytes() the first time!", nameof(password)));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            algorithm.Key = (byte[])password.Clone();
        }

        /// <summary>Returns the next pseudo-random one time pad with the specified number of bytes.</summary>
        /// <param name="cb">Length of the byte buffer to retrieve.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Algorithm</exception>
        /// <exception cref="ArgumentException">
        /// Iterations &lt; 1000
        /// or
        /// Salt &lt; 8 bytes
        /// or
        /// Password &lt; 8 bytes
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Length</exception>
        public override byte[] GetBytes(int cb)
        {
            if (algorithm == null)
            {
                throw new InvalidOperationException("Algorithm unset!");
            }

            if (iterations < 1)
            {
                throw new InvalidOperationException("Iterations < 1");
            }

            if (salt == null | salt.Length < 8)
            {
                throw new InvalidOperationException("Salt < 8 bytes");
            }

            if (cb < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(cb));
            }

            buffer ??= new FifoBuffer();
            //enough data present ?
            while (buffer.Length < cb)
            {
                //fill buffer
                FillBuffer();
            }
            return buffer.Dequeue(cb);
        }

        /// <summary>Resets the state of the operation.</summary>
        public override void Reset()
        {
            buffer = null;
            hashNumber = 0;
        }

        /// <summary>Releases the unmanaged resources used by this instance and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
#if NET40_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                (algorithm as IDisposable).Dispose(); algorithm = null;
            }
        }
#elif NET35 || NET20
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                (algorithm as IDisposable).Dispose(); algorithm = null;
            }
        }

        /// <summary>
        /// Releases all resources used by the this instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#else
#error No code defined for the current framework or NETXX version define missing!
#endif
    }
}
