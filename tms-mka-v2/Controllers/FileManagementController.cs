using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using tms_mka_v2.Infrastructure;

namespace tms_mka_v2.Controllers
{
    public class FileManagementController : Controller
    {
        private const string FILE_DIRECTORY = "~/Uploads/";

        public string Save(IEnumerable<HttpPostedFileBase> files)
        {
            //kamus lokal
            string fullname = "", fileName = "", savedFilename = "", physicalPath = "", imagePath = "", friendlyFilename = "";

            //algoritma
            if (files != null)
            {
                foreach (var file in files)
                {
                    var identifier = Guid.NewGuid();
                    bool fileDirectory = Directory.Exists(Server.MapPath(FILE_DIRECTORY));
                    if (fileDirectory == false)
                    {
                        Directory.CreateDirectory(Server.MapPath(FILE_DIRECTORY));
                    }
                    fullname = file.FileName;
                    fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    friendlyFilename = new DisplayFormatHelper().URLFriendly(fileName) + Path.GetExtension(file.FileName);
                    savedFilename = identifier + "_" + friendlyFilename;

                    //save file
                    physicalPath = Path.Combine(Server.MapPath(FILE_DIRECTORY), savedFilename);
                    file.SaveAs(physicalPath);
                }
            }

            imagePath = FILE_DIRECTORY + savedFilename;
            return new JavaScriptSerializer().Serialize(new { fname = fileName, filepath = imagePath, imagelink = physicalPath, fullname = fullname });
        }
        /**
 * remove file using kendo file uploader
 */
        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    //var physicalPath = Path.Combine(Server.MapPath(FILE_DIRECTORY), fileName);
                    var physicalPath = FILE_DIRECTORY + fileName;
                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public string Upload(IEnumerable<HttpPostedFileBase> files, string Dir)
        {
            string imagePath = "", fileName = "", physicalPath = "";
            List<string> listFile = new List<string>();
            //algoritma
            if (files != null)
            {
                foreach (var file in files)
                {
                    var identifier = Guid.NewGuid();
                    bool fileDirectory = Directory.Exists(Server.MapPath(Dir));
                    if (fileDirectory == false)
                    {
                        Directory.CreateDirectory(Server.MapPath(Dir));
                    }

                    //Some browsers send file names with full path. This needs to be stripped.
                    fileName = identifier + Path.GetFileName(file.FileName);
                    physicalPath = Path.Combine(Server.MapPath(Dir), fileName);

                    //The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                    imagePath = Dir.Replace("~","") + "/" + fileName; 
                }
            }

            return new JavaScriptSerializer().Serialize(new { filepath = imagePath, imagelink = imagePath, fileName = fileName });
        }
        public string Delete(string[] fileNames, string Dir, bool temp = false)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (!temp)
            {
                if (fileNames != null)
                {
                    foreach (var fullName in fileNames)
                    {
                        if (fullName != null)
                        {
                            var fileName = Path.GetFileName(fullName);
                            var physicalPath = "";
                            if (Server != null)
                                physicalPath = Path.Combine(Server.MapPath(Dir), fileName);
                            else
                                physicalPath = Path.Combine(Dir, fileName);


                            if (System.IO.File.Exists(physicalPath))
                            {
                                // The files are not actually removed in this demo
                                System.IO.File.Delete(physicalPath);
                            }
                        }
                    }
                }
            }

            // Return an empty string to signify success
            return new JavaScriptSerializer().Serialize(new { fileName = fileNames });
        }
    }
}
