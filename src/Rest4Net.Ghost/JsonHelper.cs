using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Rest4Net.Ghost.Exceptions;

namespace Rest4Net.Ghost
{
    internal static class JsonHelper
    {
        public static JToken CheckResponseForError(this JToken token)
        {
            try
            {
                var o = token as JObject;
                if (o != null && o.Properties().Count() == 1 && o["status"].Value<int>() == 403)
                    throw new GhostPleaseSignInException();
            }
            catch (Exception e)
            {
                if (e is GhostPleaseSignInException)
                    throw;
            }
            if (token["error"] == null)
                return token;
            var errorMessage = token["error"].Value<string>();
            if (errorMessage == "Please sign in")
                throw new GhostPleaseSignInException();
            if (errorMessage == "Post not found")
                throw new GhostPostNotFound();
            throw new GhostException(errorMessage);
        }
    }
}
