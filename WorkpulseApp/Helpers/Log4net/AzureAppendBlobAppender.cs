using System;
using System.IO;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using log4net.Core;
using Microsoft.Azure;
using System.Xml;
using log4net.Appender;
using WorkpulseApp.Extensions;
using Microsoft.IdentityModel.Protocols;

namespace WorkpulseApp.Helpers.Log4net
{
    public class AzureAppendBlobAppender: BufferingAppenderSkeleton
    {


        private CloudStorageAccount _account;
        private CloudBlobClient _client;
        private CloudBlobContainer _cloudBlobContainer;


        public string ConnectionStringName { get; set; }
        private string _lineFeed = "";
        private string LoggerBlobConnectionString = "LoggerBlobConnectionString";
        private static int sendBufferSize = 1;
        LoggingEvent[] _logevents = new LoggingEvent[sendBufferSize];
        int i = 0;

        private string _containerName;

        public string ContainerName
        {
            get
            {
                if (String.IsNullOrEmpty(_containerName))
                    throw new ApplicationException("Container not found");
                return _containerName;
            }
            set
            {
                _containerName = value;
            }
        }

        private string _directoryName;

        public string DirectoryName
        {
            get
            {
                if (String.IsNullOrEmpty(_directoryName))
                    throw new ApplicationException("DirectoryNameNotSpecified");
                return _directoryName;
            }
            set
            {
                _directoryName = value;
            }
        }

        /// <summary>
        /// Sends the events.
        /// </summary>
        /// <param name="events">The events that need to be send.</param>
        /// <remarks>
        /// <para>
        /// The subclass must override this method to process the buffered events.
        /// </para>
        /// </remarks>

        protected override void SendBuffer(LoggingEvent[] events)
        {
            //CloudAppendBlob appendBlob = _cloudBlobContainer.GetAppendBlobReference(Filename(_directoryName));
            //if (!appendBlob.Exists()) appendBlob.CreateOrReplace();
            //else _lineFeed = Environment.NewLine;

            //Parallel.ForEach(events, ProcessEvent);
        }
        protected override void Append(LoggingEvent loggingEvent)
        {
            CloudAppendBlob appendBlob = _cloudBlobContainer.GetAppendBlobReference(Filename(_directoryName));
           // if (!appendBlob.ExistsAsync())
                appendBlob.CreateOrReplaceAsync();
           // else
                _lineFeed = Environment.NewLine;

            _logevents.SetValue(loggingEvent, i);
            i = i + 1;
            if (i == sendBufferSize)
            {
                i = 0;
                ProcessEvent(_logevents);
                //Parallel.ForEach(_logevents, ProcessEvent);
            }

        }
        private void ProcessEvent(LoggingEvent[] loggingEvent)
        {
            string logMessage = string.Empty;
            CloudAppendBlob appendBlob = _cloudBlobContainer.GetAppendBlobReference(Filename(_directoryName));
            foreach (var log in loggingEvent)
            {                 
                logMessage += _lineFeed + log.GetTextString(Layout);
            }

            //var xml = _lineFeed + loggingEvent.GetXmlString(Layout);
            //XmlDocument xmldoc = new XmlDocument();
            //xmldoc.LoadXml(xml);
            //string str = GetXMLAsString(xmldoc);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(logMessage)))
            {
                appendBlob.AppendBlockAsync(ms);
            }
        }
        //public string GetXMLAsString(XmlDocument myxml)
        //{
        //    return myxml.OuterXml;
        //}


        private static string Filename(string directoryName)
        {
            return string.Format("{0}/{1}_log.txt",
                                 directoryName,
                                 DateTime.Today.ToString("yyyy_MM_dd",
                                                                 DateTimeFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// Initialize the appender based on the options set
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is part of the <see cref="T:log4net.Core.IOptionHandler"/> delayed object
        ///             activation scheme. The <see cref="M:log4net.Appender.BufferingAppenderSkeleton.ActivateOptions"/> method must 
        ///             be called on this object after the configuration properties have
        ///             been set. Until <see cref="M:log4net.Appender.BufferingAppenderSkeleton.ActivateOptions"/> is called this
        ///             object is in an undefined state and must not be used. 
        /// </para>
        /// <para>
        /// If any of the configuration properties are modified then 
        ///             <see cref="M:log4net.Appender.BufferingAppenderSkeleton.ActivateOptions"/> must be called again.
        /// </para>
        /// </remarks>


        public override void ActivateOptions()
        {
            base.ActivateOptions();

            _account = CloudStorageAccount.Parse(LoggerBlobConnectionString);
            _client = _account.CreateCloudBlobClient();
            _cloudBlobContainer = _client.GetContainerReference(ContainerName.ToLower());
            _cloudBlobContainer.CreateIfNotExistsAsync();
        }



    }
}
