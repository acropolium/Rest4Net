namespace Rest4Net.BitLy.Responses
{
    public interface IShorten
    {
        /// <summary>
        /// The actual link that should be used, and is a unique value for the
        /// given bit.ly account.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// A bit.ly identifier for long_url which is unique to the given account.
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// A bit.ly identifier for long_url which can be used to track aggregate
        /// stats across all matching bit.ly links.
        /// </summary>
        string GlobalHash { get; }

        /// <summary>
        /// An echo back of the longUrl request parameter. This may not always be
        /// equal to the URL requested. That's because some URL normalization may
        /// occur (e.g., due to encoding differences, or case differences in the
        /// domain). This long_url will always be functionally identical the the
        /// request parameter.
        /// </summary>
        string LongUrl { get; }
        
        /// <summary>
        /// Designates if this is the first time this long_url was shortened.
        /// The return value will equal 1 the first time a long_url is shortened.
        /// It will also then be added to the user history
        /// </summary>
        string NewHash { get; }
    }
}
