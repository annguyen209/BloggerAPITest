using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloggerAPI.Helper
{
    public interface IAPISupporter
    {
        object Authorize(HttpRequest Request, string accountName);
    }
}