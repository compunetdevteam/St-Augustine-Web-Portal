﻿using SwiftSkool.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HopeAcademySMS.Controllers
{
    public class FileDownloadController : Controller
    {
        // GET: /FileProcess/  

        DownloadFiles obj;
        public FileDownloadController()
        {
            obj = new DownloadFiles();
        }

        public ActionResult Index()
        {
            var filesCollection = obj.GetFiles();
            return View(filesCollection);
        }

        public FileResult Download(string FileId)
        {
            int CurrentFileID = Convert.ToInt32(FileId);
            var filesCol = obj.GetFiles();
            string CurrentFileName = (from fls in filesCol
                                      where fls.FileId == CurrentFileID
                                      select fls.FilePath).First();

            string contentType = string.Empty;

            if (CurrentFileName.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }

            else if (CurrentFileName.Contains(".docx"))
            {
                contentType = "application/docx";
            }
            return File(CurrentFileName, contentType, CurrentFileName);
        }
    }
}
