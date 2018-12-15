using System;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Cave.IO;

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
                throw new ArgumentNullException("data");
            }

            int result = 1;
            for (int i = 1; i < data.Length; i++)
            {
                int diff = Math.Abs(data[0] - data[1]);
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
        public static PBKDF2 Create(HMAC algorithm)
        {
            return new PBKDF2(algorithm);
        }

        /// <summary>Creates a new instance using the specified private bigint as key and salt (last 16 bytes are used as salt).</summary>
        /// <remarks>In this function the number of iterations are set to 2 for performance reasons. This should not impact security if the private 
        /// is chosen and protected well.</remarks>
        /// <param name="i">The BigInt.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">BigInt has less than 32 bytes (256 bits)!</exception>
        public static PBKDF2 FromPrivate(BigInteger i)
        {
            byte[] data = i.ToByteArray();
            if (data.Length < 64)
            {
                throw new ArgumentException("BigInt has less than 64 bytes (512 bits)!");
            }

            int l_Splitter = data.Length / 2;
            byte[] l_Password = ArrayExtension.GetRange(data, 0, l_Splitter);
            byte[] salt = ArrayExtension.GetRange(data, l_Splitter);
            return new PBKDF2(l_Password, salt, 2);
        }

        int m_Iterations = 1000;
        int m_HashNumber;
        byte[] m_Salt;
        HMAC m_Algorithm;
        FifoBuffer m_Buffer;

        PBKDF2(HMAC algorithm)
        {
            m_Algorithm = algorithm;
        }

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
            m_Iterations = iterations;
            SetPassword(password);
            SetSalt(salt);
        }

        void FillBuffer()
        {
            int i = ++m_HashNumber;
            byte[] s = new byte[m_Salt.Length + 4];
            Buffer.BlockCopy(m_Salt, 0, s, 0, m_Salt.Length);
            s[s.Length - 4] = (byte)(i >> 24);
            s[s.Length - 3] = (byte)(i >> 16);
            s[s.Length - 2] = (byte)(i >> 8);
            s[s.Length - 1] = (byte)i;
            // this is like j=0
            byte[] u1 = m_Algorithm.ComputeHash(s);
            byte[] data = u1;
            // so we start at j=1
            for (int j = 1; j < m_Iterations; j++)
            {
                byte[] un = m_Algorithm.ComputeHash(data);
                // xor
                for (int k = 0; k < u1.Length; k++)
                {
                    u1[k] = (byte)(u1[k] ^ un[k]);
                }

                data = un;
            }
            m_Buffer.Enqueue(u1);
        }

        /// <summary>Sets the password.</summary>
        /// <param name="password">The password.</param>
        public void SetPassword(string password)
        {
            SetPassword(Encoding.UTF8.GetBytes(password));
        }

        /// <summary>Gets or sets the iteration count.</summary>
        /// <value>The iteration count.</value>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentOutOfRangeException">IterationCount &lt; 1000</exception>
        public int IterationCount
        {
            get => m_Iterations;
            set
            {
                if (m_Buffer != null)
                {
                    throw new InvalidOperationException(string.Format("Cannot change the {0} after calling GetBytes() the first time!", "IterationCount"));
                }

                if (value < 1000)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "IterationCount < 1000");
                }

                m_Iterations = value;
            }
        }

        /// <summary>Gets or sets the salt.</summary>
        /// <value>The salt.</value>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException">Salt</exception>
        /// <exception cref="ArgumentException">Salt &lt; 8 bytes</exception>
        public byte[] Salt => (byte[])m_Salt.Clone();

        /// <summary>Sets the salt.</summary>
        /// <param name="salt">The salt.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="System.ArgumentException">Salt &lt; 8 bytes;value</exception>
        public void SetSalt(byte[] salt)
        {
            if (m_Buffer != null)
            {
                throw new InvalidOperationException(string.Format("Cannot change the {0} after calling GetBytes() the first time!", "Salt"));
            }

            if (salt == null)
            {
                throw new ArgumentNullException("value");
            }

            if (salt.Length < 8)
            {
                throw new ArgumentException("Salt < 8 bytes", "value");
            }

            m_Salt = (byte[])salt.Clone();
        }

        /// <summary>Gets or sets the password.</summary>
        /// <value>The password.</value>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException">Password</exception>
        /// <exception cref="ArgumentException">Password &lt; 8 bytes</exception>
        public byte[] Password => (byte[])m_Algorithm.Key.Clone();

        /// <summary>Sets the password.</summary>
        /// <param name="password">The password.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentNullException">value</exception>
        /// <exception cref="System.ArgumentException">Password &lt; 8 bytes;value</exception>
        public void SetPassword(byte[] password)
        {
            if (m_Buffer != null)
            {
                throw new InvalidOperationException(string.Format("Cannot change the {0} after calling GetBytes() the first time!", "Password"));
            }

            if (password == null)
            {
                throw new ArgumentNullException("value");
            }

            m_Algorithm.Key = (byte[])password.Clone();
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
            if (m_Algorithm == null)
            {
                throw new InvalidDataException("Algorithm unset!");
            }

            if (m_Iterations < 1)
            {
                throw new ArgumentException("Iterations < 1");
            }

            if (m_Salt == null | m_Salt.Length < 8)
            {
                throw new ArgumentException("Salt < 8 bytes");
            }

            if (cb < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(cb));
            }

            if (m_Buffer == null)
            {
                m_Buffer = new FifoBuffer();
            }
            //enough data present ?
            while (m_Buffer.Length < cb)
            {
                //fill buffer
                FillBuffer();
            }
            return m_Buffer.Dequeue(cb);
        }

        /// <summary>Resets the state of the operation.</summary>
        public override void Reset()
        {
            m_Buffer = null;
            m_HashNumber = 0;
        }

        /// <summary>Releases the unmanaged resources used by this instance and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
#if NET40 || NET45 || NET46 || NET47 || NETSTANDARD20 || NETCOREAPP20
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                (m_Algorithm as IDisposable).Dispose(); m_Algorithm = null;
            }
        }
#elif NET35 || NET20
		protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                (m_Algorithm as IDisposable).Dispose(); m_Algorithm = null;
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
