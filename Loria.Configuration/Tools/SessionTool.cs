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
        private const string ReceipeIdSessionKey = "ReceipeIdSession";
        private const string ReceipeNameSessionKey = "ReceipeNameSession";

        public static void ClearSession()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Remove(ReceipeInSessionKey);
                HttpContext.Current.Session.Remove(ReceipeOutSessionKey);
                HttpContext.Current.Session.Remove(ReceipeIdSessionKey);
                HttpContext.Current.Session.Remove(ReceipeNameSessionKey);
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

        public static int? GetReceipeIdSession()
        {
            int? receipeId = null;

            if (HttpContext.Current != null)
            {
                int? sessionReceipeId = (int?)HttpContext.Current.Session[ReceipeIdSessionKey];

                if (sessionReceipeId != null)
                {
                    receipeId = sessionReceipeId;
                }
            }

            return receipeId;
        }

        public static void SetReceipeIdSession(int? receipeId)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Add(ReceipeIdSessionKey, receipeId);
            }
        }

        public static string GetReceipeNameSession()
        {
            string receipeName = null;

            if (HttpContext.Current != null)
            {
                string sessionReceipeName = (string)HttpContext.Current.Session[ReceipeNameSessionKey];

                if (sessionReceipeName != null)
                {
                    receipeName = sessionReceipeName;
                }
            }

            return receipeName;
        }

        public static void SetReceipeNameSession(string receipeName)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Add(ReceipeNameSessionKey, receipeName);
            }
        }
    }
}