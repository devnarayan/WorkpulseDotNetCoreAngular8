using log4net.Core;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CORTNE.Extensions
{
    internal static class LoggingEventExtensions
    {
        private static string _apiVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// Store the Logs in XML Format(.xml)
        /// </summary>
        /// <param name="loggingEvent"></param>
        /// <param name="layout"></param>
        /// <returns></returns>
        internal static string GetXmlString(this LoggingEvent loggingEvent, ILayout layout = null)
        {
            string message = loggingEvent.RenderedMessage + Environment.NewLine + loggingEvent.GetExceptionString();
            if (layout != null)
            {
                using (var w = new StringWriter())
                {
                    layout.Format(w, loggingEvent);
                    message = w.ToString();
                }
            }

            var logXml = new XElement(
                "LogEntry",
                new XElement("UserName", loggingEvent.UserName),
                new XElement("TimeStamp",
                    loggingEvent.TimeStamp.ToString(CultureInfo.InvariantCulture)),
                new XElement("ThreadName", loggingEvent.ThreadName),
                new XElement("LoggerName", loggingEvent.LoggerName),
                new XElement("Level", loggingEvent.Level),
                new XElement("Identity", loggingEvent.Identity),
                new XElement("Domain", loggingEvent.Domain),
                new XElement("CreatedOn", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new XElement("RenderedMessage", message),
                new XElement("Location", loggingEvent.LocationInformation.FullInfo)
                );

            if (loggingEvent.Properties != null && loggingEvent.Properties.Count > 0)
            {
                var props = loggingEvent.Properties;
                if (props.Contains("AddPropertiesToXml"))
                {
                    foreach (var k in props.GetKeys())
                    {
                        var key = k.Replace(":", "_")
                                   .Replace("@", "_")
                                   .Replace(".", "_");
                        logXml.Add(new XElement(key, props[k].ToString()));
                    }
                }
            }

            if (loggingEvent.ExceptionObject != null)
            {
                logXml.Add(new XElement("Exception", loggingEvent.ExceptionObject.ToString()));
            }

            return logXml.ToString();
        }

        /// <summary>
        /// Store the Logs in Text file Format(.txt)
        /// </summary>
        /// <param name="loggingEvent"></param>
        /// <param name="layout"></param>
        /// <returns></returns>
        internal static string GetTextString(this LoggingEvent loggingEvent, ILayout layout = null)
        {
            string logMessage = "";
            string message = loggingEvent.RenderedMessage + Environment.NewLine + loggingEvent.GetExceptionString();
            if (layout != null)
            {
                using (var w = new StringWriter())
                {
                    layout.Format(w, loggingEvent);
                    message = w.ToString();
                }
            }

            logMessage = "TimeStamp :" + loggingEvent.TimeStamp.ToString(CultureInfo.InvariantCulture) + "\t" +
                       "AppVersion :" + _apiVersion + "\t" +
                       "UserName :" + loggingEvent.UserName + "\t" +
                       "ThreadName :" + loggingEvent.ThreadName + "\t" +
                       "LoggerName :" + loggingEvent.LoggerName + "\t" +
                       "Level :" + loggingEvent.Level + "\t" +
                       "Identity :" + loggingEvent.Identity + "\t" +
                       "Domain :" + loggingEvent.Domain + "\t" +
                       "CreatedOn :" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + "\t" +
                       "RenderedMessage :" + message + "\t" +
                       "Location :" + loggingEvent.LocationInformation.FullInfo;


            if (loggingEvent.ExceptionObject != null)
            {
                logMessage += "\t" + "Exception :" + loggingEvent.ExceptionObject.ToString();
            }

            return logMessage;
        }

    }
}
