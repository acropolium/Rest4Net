namespace Rest4Net.IronCache.Responses.Implementation
{
    internal class CacheImpl : ICache
    {
        private string project_id;
        private string name;

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
