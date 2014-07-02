using System;
using System.Collections.Generic;
using Rest4Net.Ghost.Responses;
using Rest4Net.Ghost.Responses.Implementation;

namespace Rest4Net.Ghost
{
    //The reference was taken from: https://github.com/TryGhost/Ghost/wiki/%5BWIP%5D-API-Documentation
    public class GhostProvider : RestApiProvider
    {
        public GhostProvider(string domain,
            string login,
            string password,
            bool useHttps = false,
            string urlPath = null,
            int port = -1)
            : base(
                (new GhostProtocol(domain, port, useHttps) {Login = login, Password = password}).BuildVariables(
                    urlPath, useHttps, domain, port))
        {
        }

        public GhostProvider(string domain,
            string urlPath,
            string login,
            string password,
            bool useHttps = false,
            int port = -1)
            : this(domain, login, password, useHttps, urlPath, port)
        { }

        protected override Command Cmd(string path, RequestType requestType = RequestType.Get)
        {
            return base.Cmd("/ghost/api/v0.1"+path, requestType);
        }

        public IPosts GetPosts(uint page = 1, uint limit = 15, ContentStatus status = ContentStatus.All, bool withStaticPages = false)
        {
            return Cmd("/posts/")
                .WithParameter("page", page)
                .WithParameter("limit", limit)
                .WithParameter("status", ContentStatusHelper.ToGhostString(status))
                .WithParameter("staticPages", withStaticPages.ToString().ToLower())
                .Execute().To<Posts>(JsonHelper.CheckResponseForError);
        }

        public IPost GetPost(int id)
        {
            return Cmd("/posts/" + id + "/")
                .WithParameter("status", ContentStatusHelper.ToGhostString(ContentStatus.All))
                .Execute().To<Post>(JsonHelper.CheckResponseForError);
        }

        public IPost DeletePost(int id)
        {
            return Cmd("/posts/" + id + "/", RequestType.Delete)
                .Execute().To<Post>(JsonHelper.CheckResponseForError);
        }

        public string GenerateSlug(string title)
        {
            return Cmd("/posts/getSlug/" + title).Execute().ToJson().ToString();
        }

        public IPost CreatePost(string title, string textMarkdown, DateTime? publishDate = null)
        {
            var p = new Post {Title = title, Markdown = textMarkdown, CreatedAt = DateTime.UtcNow};
            if (publishDate != null)
                p.PublishedAt = publishDate.Value;
            return p;
        }

        public IPost SavePost(IPost post)
        {
            post.UpdatedAt = DateTime.UtcNow;
            return
                ((post.Id > 0) ? Cmd("/posts/" + post.Id + "/", RequestType.Put) : Cmd("/posts/", RequestType.Post))
                    .WithBody(post.ToJson().ToString()).Execute().To<Post>(JsonHelper.CheckResponseForError);
        }

        public IEnumerable<ITag> GetTags()
        {
            foreach (var tag in Cmd("/tags/").Execute().To<List<Tag>>(JsonHelper.CheckResponseForError))
                yield return tag;
        }

        public string DbExport()
        {
            return Cmd("/db/").Execute().ToJson().ToString();
        }

        public void DbImport(string data)
        {
            Cmd("/db/", RequestType.Post).WithBody(data).Execute();
        }

        public void DbClean()
        {
            Cmd("/db/", RequestType.Delete).Execute();
        }
    }
}
