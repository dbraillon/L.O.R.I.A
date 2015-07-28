using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loria.Configuration.Tools
{
    public class ReceipeInSessionModel
    {
        public int TriggerItemId { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }

    public class ReceipeOutSessionModel
    {
        public int ActionItemId { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }

    public class SessionTool
    {
        private const string ReceipeInSessionKey = "ReceipeInSession";
        private const string ReceipeOutSessionKey = "ReceipeOutSession";

        public static void ClearReceipeInOutSession()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Remove(ReceipeInSessionKey);
            }
        }

        public static ReceipeInSessionModel GetReceipeInSessionModel()
        {
            ReceipeInSessionModel receipeInModel = null;

            if (HttpContext.Current != null)
            {
                ReceipeInSessionModel sessionModel = (ReceipeInSessionModel)HttpContext.Current.Session[ReceipeInSessionKey];

                if (sessionModel != null)
                {
                    receipeInModel = sessionModel;
                }
            }

            return receipeInModel;
        }

        public static void SetReceipeInSessionModel(ReceipeInSessionModel model)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Add(ReceipeInSessionKey, model);
            }
        }

        public static ReceipeOutSessionModel GetReceipeOutSessionModel()
        {
            ReceipeOutSessionModel receipeOutModel = null;

            if (HttpContext.Current != null)
            {
                ReceipeOutSessionModel sessionModel = (ReceipeOutSessionModel)HttpContext.Current.Session[ReceipeOutSessionKey];

                if (sessionModel != null)
                {
                    receipeOutModel = sessionModel;
                }
            }

            return receipeOutModel;
        }

        public static void SetReceipeOutSessionModel(ReceipeOutSessionModel model)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Add(ReceipeOutSessionKey, model);
            }
        }
    }
}