namespace Rest4Net.IronCache.Responses.Implementation
{
    internal class CacheImpl : ICache
    {
#pragma warning disable 649
        private string project_id;
        private string name;
#pragma warning restore 649

        public string ProjectID
        {
            get { return project_id; }
            set { project_id = value; }
        }

        public string Name
        {
            get { return name; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", ProjectID ?? "null", Name);
        }
    }
}
