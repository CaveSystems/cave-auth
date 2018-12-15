namespace Cave.Auth
{
    /// <summary>
    /// Provides gender settings
    /// </summary>
    public enum Gender : int
    {
        /// <summary>undefined</summary>
        Undefined = 0,

        /// <summary>female</summary>
        Female = 'x',

        /// <summary>male</summary>
        Male = 'y',

        /// <summary>Anything not <see cref="Female"/> or <see cref="Male"/></summary>
        Other = '*',
    }
}
