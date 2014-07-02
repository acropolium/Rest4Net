namespace Rest4Net.IronMq.Responses.Implementation
{
    internal class QueueImpl : IQueue
    {
#pragma warning disable 649
        private string id;
        private string project_id;
        private string name;
        private int size;
#pragma warning restore 649

        public string ID
        {
            get { return id; }
        }

        public string ProjectID
        {
            get { return project_id; }
            set { project_id = value; }
        }

        public string Name
        {
            get { return name; }
        }

        public int Size
        {
            get { return size; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} ({2})", ID ?? "null", Name, Size);
        }
    }
}
