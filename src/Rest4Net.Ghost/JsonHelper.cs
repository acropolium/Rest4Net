using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Rest4Net.Ghost.Exceptions;

namespace Rest4Net.Ghost
{
    internal class JsonHelper
    {
        private static int CountEnum(IEnumerable<JProperty> properties)
        {
            var r = 0;
            foreach (var p in properties)
                r++;
            return r;
        }

        public static JToken CheckResponseForError(JToken token)
        {
            try
            {
                var o = token as JObject;
                if (o != null && CountEnum(o.Properties()) == 1 && o["status"].Value<int>() == 403)
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
