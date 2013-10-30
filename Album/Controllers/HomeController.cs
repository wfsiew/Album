using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Album.Models;

namespace Album.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            string path = Server.MapPath("~/images");
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fi = di.GetFiles();

            List<int> l = new List<int>();
            List<Photo> lp = new List<Photo>();

            for (int i = 0; i < fi.Length; i++)
            {
                l.Add(i);
                FileInfo file = fi[i];
                Photo o = new Photo
                {
                    Url = Url.Content("~/images/") + file.Name,
                    Title = file.Name,
                    Size = file.Length,
                    Active = i == 0
                };

                lp.Add(o);
            }

            PhotoContainer model = new PhotoContainer
            {
                Indicators = l,
                Photos = lp
            };

            return View(model);
        }

        public ActionResult Download(string f)
        {
            string path = Server.MapPath("~/images");
            string filepath = Path.Combine(path, f);

            if (!System.IO.File.Exists(filepath))
                return HttpNotFound();

            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + f + "\"");
            return new FilePathResult(filepath, "application/octet-stream");
        }

        public ActionResult Gallery()
        {
            string path = Server.MapPath("~/images");
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fi = di.GetFiles();

            List<Photo> lp = new List<Photo>();

            for (int i = 0; i < fi.Length; i++)
            {
                FileInfo file = fi[i];
                Photo o = new Photo
                {
                    Url = Url.Content("~/images/") + file.Name,
                    Title = file.Name,
                    Size = file.Length,
                    Active = i == 0
                };

                lp.Add(o);
            }

            return View(lp);
        }

        public ActionResult Thumbnail(string f)
        {
            if (string.IsNullOrEmpty(f))
                return HttpNotFound();

            string path = Server.MapPath("~/images");
            string filepath = Path.Combine(path, f);

            Image image = Image.FromFile(filepath);
            Size thumbnailSize = GetThumbnailSize(image);
            Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);

            byte[] b = null;
            using (MemoryStream ms = new MemoryStream())
            {
                thumbnail.Save(ms, ImageFormat.Png);
                b = ms.ToArray();
            }

            return new FileContentResult(b, "image/png");
        }

        private Size GetThumbnailSize(Image original)
        {
            // Maximum size of any dimension.
            const int maxPixels = 240;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }

            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
	}
}