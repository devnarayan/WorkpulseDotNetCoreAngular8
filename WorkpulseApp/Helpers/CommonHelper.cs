using WorkpulseApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestGraphApi.Models;
using Microsoft.AspNetCore.Hosting;
using WorkpulseApp.ViewModel;
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.LGPLv2.Core;
using Document = iTextSharp.text.Document;
using Microsoft.AspNetCore.Hosting.Internal;
using iTextSharp.text.html.simpleparser;


namespace WorkpulseApp.Helpers
{
    public class CommonHelper
    {
        private readonly CORTNEDEVContext _context;
        private readonly IHostingEnvironment _env;

        public CommonHelper(CORTNEDEVContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #region Log Exceptions

        public  void LogException(ErrorViewModel errorViewModel)
        {
            try
            {
                var dbEx = _context.AppException;
                dbEx.Add(new AppException
                {
                    Method = errorViewModel.Action,

                    Controller = errorViewModel.Controller,
                    Stacktrace = errorViewModel.ErrorDetail,
                    ExceptionMessage = errorViewModel.ExceptionMessage,
                    ExceptionType = "Error",
                    ErrorDate = System.DateTime.Now

                });

                _context.SaveChanges();
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public  void LogInformation(ErrorViewModel errorModel)
        {
            try
            {
                var dbEx = _context.AppException;
                dbEx.Add(new AppException
                {
                    Method = errorModel.Action,

                    Controller = errorModel.Controller,
                    Stacktrace = errorModel.ErrorDetail,
                    ExceptionMessage = errorModel.ExceptionMessage,
                    ExceptionType = "Information",
                    ErrorDate = System.DateTime.Now

                });

                _context.SaveChanges();
            }
            catch (System.Exception exc)
            {
                throw exc;
                //_logger.Error("Exception Occured :", exc);
            }
        }
        public void AddException(System.Exception ex, string method)
        {
            try
            {
                var dbEx = _context.AppException;
                dbEx.Add(new AppException
                {
                    Method = method,
                    ExceptionMessage = ex.StackTrace,
                    Stacktrace = ex.ToString()
                });

                _context.SaveChanges();
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public ErrorViewModel GetCurrentContollerActionName(ControllerContext httpContext, System.Exception exception)
        {
            ErrorViewModel erroModel = new ErrorViewModel();
            var routeValues = httpContext.RouteData.Values;
            if (routeValues != null)
            {
                if (routeValues.ContainsKey("action"))
                {
                    erroModel.Action = routeValues["action"].ToString();
                }
                if (routeValues.ContainsKey("controller"))
                {
                    erroModel.Controller = routeValues["controller"].ToString();
                }
            }
            erroModel.ErrorDetail = GetExceptionMessageStackTrace(exception);
            erroModel.ExceptionMessage = GetExceptionMessage(exception);

            return erroModel;
        }

        public  string GetExceptionMessage(System.Exception Ex)
        {
            if (Ex.InnerException == null)
            {
                return Ex.Message.ToString();
            }
            else
            {
                return Ex.Message.ToString() + Environment.NewLine + Environment.NewLine + GetExceptionMessage(Ex.InnerException);
            }
        }

        public  string GetExceptionStackTrace(System.Exception Ex)
        {
            if (Ex.InnerException == null)
            {
                return Ex.StackTrace.ToString();
            }
            else
            {
                return Ex.StackTrace.ToString() + Environment.NewLine + Environment.NewLine + GetExceptionStackTrace(Ex.InnerException);
            }
        }
        public  string GetExceptionMessageStackTrace(System.Exception Ex)
        {
            if (Ex.InnerException == null)
            {
                return $"Error# {Ex.Message} {Environment.NewLine} StackTrace# { Ex.StackTrace}";
            }
            else
            {
                return $"Error# {Ex.Message} {Environment.NewLine} StackTrace# { Ex.StackTrace}  {Environment.NewLine} {GetExceptionMessageStackTrace(Ex.InnerException)}";
            }
        }

        #endregion

        #region Print

        public void ConvertAsposeDocToAsposePDF()
        {
            string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;

            string outputpath = contentRootPath + "\\Templates";
            //string fileNamee = contentRootPath + "\\Templates\\First_Notice_MQA_Template.doc";
            string dir = Path.Combine(contentRootPath, "Templates\\replacedDocument.docx");

            // load the file to be converted
           // var wrdf = new Aspose.Words.Document(dir);
            // save in different formats
            //wrdf.Save(dir + "output.docx", Aspose.Words.SaveFormat.Docx);
            //wrdf.Save(outputpath + "\\replacedDocument.pdf", Aspose.Words.SaveFormat.Pdf);
            //wrdf.Save(dir + "output.html", Aspose.Words.SaveFormat.Html);
            //wrdf = null;

        }

        public void ConvertAsposeALLDocToAsposeALLPDF()
        {
            string contentRootPath = _env.ContentRootPath;
            string webRootPath = _env.WebRootPath;

            string outputpath = contentRootPath + "\\Templates";
            //string fileNamee = contentRootPath + "\\Templates\\First_Notice_MQA_Template.doc";
            string dir = Path.Combine(contentRootPath, "Templates\\AllinOneFile.docx");

            // load the file to be converted
          //  var wrdf = new Aspose.Words.Document(dir);
            // save in different formats
            //wrdf.Save(dir + "output.docx", Aspose.Words.SaveFormat.Docx);
          //  wrdf.Save(outputpath + "\\AllinOneFile.pdf", Aspose.Words.SaveFormat.Pdf);
            //wrdf.Save(dir + "output.html", Aspose.Words.SaveFormat.Html);
          //  wrdf = null;

        }


        #endregion
    }
}
