using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using RefactorName.WebApp.Models;

namespace RefactorName.WebApp.Controllers
{
    public class UploadController : Controller
    {
        // DICTIONARY OF ALL IMAGE FILE HEADER
        Dictionary<string, byte[]> ImageHeader;
        public UploadController()
        {
            //fill all known magic numbers
            ImageHeader = new Dictionary<string, byte[]>();
            ImageHeader.Add("DOCX", new byte[] { 0x50, 0x4B, 0x03, 0x04 });
            ImageHeader.Add("XLSX", new byte[] { 0x50, 0x4B, 0x03, 0x04 });
            ImageHeader.Add("DOC", new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 });
            ImageHeader.Add("XLS", new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 });
            ImageHeader.Add("PPT", new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 });
            ImageHeader.Add("PDF", new byte[] { 0x25, 0x50, 0x44, 0x46 });
            ImageHeader.Add("JPG", new byte[] { 0xFF, 0xD8, 0xFF });
            ImageHeader.Add("JPEG", new byte[] { 0xFF, 0xD8, 0xFF });
            ImageHeader.Add("JPE", new byte[] { 0xFF, 0xD8, 0xFF });
            ImageHeader.Add("JIF", new byte[] { 0xFF, 0xD8, 0xFF });
            ImageHeader.Add("JFIF", new byte[] { 0xFF, 0xD8, 0xFF });
            ImageHeader.Add("JFI", new byte[] { 0xFF, 0xD8, 0xFF });
            ImageHeader.Add("GIF", new byte[] { 0x47, 0x49, 0x46, 0x38 });
            ImageHeader.Add("PNG", new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a });
            ImageHeader.Add("BMP", new byte[] { 0x42, 0x4D });
        }

        public ActionResult Upload(HttpPostedFileBase qqfile, string name)
        {
            //validate parameters 
            List<FileUploadModel> fileUploadModels = Session["FileUploadModels"] as List<FileUploadModel>;

            if (string.IsNullOrEmpty(name) || fileUploadModels == null || !fileUploadModels.Any(x => x.Name == name))
                throw new InvalidOperationException("لا يمكن إجراء هذه العملية!");

            var fileUploadModel = fileUploadModels.Single(x => x.Name == name);

            string newfilename = "";
            var jss = new JavaScriptSerializer();
            try
            {
                string UploadDir = "~/Temp";
                bool IsExists = System.IO.Directory.Exists(Server.MapPath(UploadDir));
                if (!IsExists) System.IO.Directory.CreateDirectory(Server.MapPath(UploadDir));

                if (qqfile != null)
                {
                    // this works for IE
                    var filename = Path.Combine(Server.MapPath(UploadDir), Path.GetFileName(qqfile.FileName));
                    newfilename = Session.SessionID + "__" + Guid.NewGuid().ToString() + Path.GetExtension(filename);
                    // this works for IE
                    //check file size
                    if (qqfile.ContentLength > (fileUploadModel.FileMaxSize * 1048))
                        return Json(new { success = false, fileName = newfilename, error = "حجم الملف يتجاوز الحجم المسموح به.!" }, "text/html");

                    //check file type
                    if (!IsAllowedFile(qqfile, fileUploadModel.AllowedExtensions))
                        return Json(new { success = false, fileName = newfilename, error = "نوع الملف غير مسموح به.!" }, "text/html");

                    var path = Path.Combine(Server.MapPath(UploadDir), newfilename);
                    qqfile.SaveAs(path);
                    return Json(new { success = true, fileName = (Url.Content(UploadDir) + "/" + newfilename), message = "تم التحميل!" }, "text/html");
                }
                else
                {
                    // this works for Firefox, Chrome
                    var filename = Request["qqfile"];
                    if (!string.IsNullOrEmpty(filename))
                    {
                        filename = Path.Combine(Server.MapPath(UploadDir), Path.GetFileName(filename));

                        if (!IsAllowedFile(qqfile, fileUploadModel.AllowedExtensions))
                            return Json(new { success = false, fileName = newfilename, error = "نوع الملف غير مسموح به.!" }, "text/html");

                        if (Request.InputStream.Length > (fileUploadModel.FileMaxSize * 1048))
                            return Json(new { success = false, fileName = newfilename, error = "حجم الملف يتجاوز الحجم المسموح به.!" }, "text/html");

                        newfilename = Session.SessionID + "__" + Guid.NewGuid().ToString() + Path.GetExtension(filename);
                        var path = Path.Combine(Server.MapPath(UploadDir), newfilename);
                        using (var output = System.IO.File.Create(filename))
                        {
                            Request.InputStream.CopyTo(output);
                        }
                        return Json(new { success = true, fileName = (Url.Content(UploadDir) + "/" + newfilename), message = "تم التحميل!" }, "text/html");
                    }
                }
                return Json(new { success = false, fileName = newfilename, error = "فشل التحميل!" }, "text/html");
            }
            catch
            {
                return Json(new { success = false, fileName = newfilename, error = "فشل التحميل!" }, "text/html");
            }
        }

        [HttpGet]
        public ActionResult deleteFile(string fileName)
        {
            try
            {
                string completFileName = Server.MapPath(fileName);
                System.IO.File.Delete(completFileName);
                return Json(new { success = true }, "text/html", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = true }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Validate file according to allowed extensions and the magic numbers of the file for known types
        /// </summary>
        /// <param name="qqfile">HttpPostedFileBase object for uploaded file</param>
        /// <param name="allowedExtensions">file uploader array for allowed extensions</param>
        /// <returns>True for allowed file</returns>
        private bool IsAllowedFile(HttpPostedFileBase qqfile, string[] allowedExtensions)
        {
            try
            {
                //convert array to upper case
                allowedExtensions =Array.ConvertAll(allowedExtensions, s=>s.ToUpper());

                if (qqfile != null)
                {
                    // this works for IE
                    var filename = Path.GetFileName(qqfile.FileName);
                    string ext = Path.GetExtension(filename).TrimStart('.').ToUpper();
                    if (!allowedExtensions.Contains(ext))
                        return false;
                    else
                    {
                        //check magic numbers
                        if (ImageHeader.ContainsKey(ext))
                        {
                            //reade bytes
                            byte[] magicNumbers = new byte[ImageHeader[ext].Length];
                            qqfile.InputStream.Read(magicNumbers, 0, ImageHeader[ext].Length);
                            if (!CompareArray(magicNumbers, ImageHeader[ext]))
                                return false;
                        }
                    }
                }
                else
                {
                    var filename = Request["qqfile"];
                    string ext = Path.GetExtension(filename).TrimStart('.').ToUpper();
                    if (!allowedExtensions.Contains(ext))
                        return false;
                    else
                    {
                        //check magic numbers
                        //reade bytes
                        byte[] magicNumbers = new byte[ImageHeader[ext].Length];
                        Request.InputStream.Read(magicNumbers, 0, ImageHeader[ext].Length);
                        if (!CompareArray(magicNumbers, ImageHeader[ext]))
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private bool CompareArray(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                    return false;
            }

            return true;
        }

    }
}
