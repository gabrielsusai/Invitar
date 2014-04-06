using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Invitar
{
    [Authorize]
    [SessionExpire]
    [HandleError]
    public class BaseController : Controller
    {

    }
}