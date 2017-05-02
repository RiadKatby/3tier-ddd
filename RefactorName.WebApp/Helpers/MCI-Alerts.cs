using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp
{
    public enum MCIMessageType
    {
        Info,
        Danger,
        Success,
        Warning
    }

    internal class MCIMessage
    {
        public MCIMessageType Type { get; set; }
        public string Message { get; set; }
        public int Timeout { get; set; } //0 mean forever

    }

    public static class MCIAlert
    {
        public static void AddMCIMessage(Controller controller, string message, MCIMessageType type = MCIMessageType.Info, int timeout = 5)
        {
            int time = timeout * 1000;
            var alerts = controller.TempData["MCIMessages"] != null ? controller.TempData["MCIMessages"] as List<MCIMessage> : new List<MCIMessage>();

            alerts.Add(new MCIMessage()
            {
                Message = message,
                Type = type,
                Timeout = time
            });
            controller.TempData["MCIMessages"] = alerts;
        }

        public static MvcHtmlString RenderMCIMessagesArea(this HtmlHelper helper, bool isFluid = false, string cssClass = "")
        {
            string html = "";
            string container = isFluid ? "container-fluid" : "container";
            try
            {
                html += "<div class='message-box-container " + container + "'>";
                html += "<div class='row'>";
                html += "<div class='message-box ";
                html += string.IsNullOrWhiteSpace(cssClass) ? "col-xs-12" : cssClass;
                html += "'>";
                if (helper.ViewContext.TempData["MCIMessages"] != null)
                {
                    var alerts = helper.ViewContext.TempData["MCIMessages"] as List<MCIMessage>;
                    if (alerts.Count > 0)
                    {
                        foreach (var item in alerts)
                            html += string.Format("<div class='center-block alert alert-dismissible alert-{1}'  data-mcimessage-timeout='{2}'>{0}</div>", item.Message, Enum.GetName(typeof(MCIMessageType), item.Type).ToLower(), item.Timeout);
                    }
                }
                html += "</div></div></div>";
            }
            catch { }
            return new MvcHtmlString(html);
        }

    }
}