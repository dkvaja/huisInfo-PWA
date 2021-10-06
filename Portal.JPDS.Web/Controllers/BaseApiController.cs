using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Portal.JPDS.AppCore.Common;
using Portal.JPDS.Domain.Common;
using Svg;

namespace Portal.JPDS.Web.Controllers
{
    [Authorize(PolicyConstants.ViewOnlyAccess)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IMimeMappingService _mimeMappingService;
        private readonly IRepoSupervisor _repoSupervisor;
        public BaseApiController(IHostEnvironment hostEnvironment, IRepoSupervisor repoSupervisor, IMimeMappingService mimeMappingService)
        {
            _mimeMappingService = mimeMappingService;
            _repoSupervisor = repoSupervisor;
            _hostEnvironment = hostEnvironment;
        }

        internal async Task<IActionResult> GetFileStream(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && System.IO.File.Exists(filePath))
            {
                var filename = System.IO.Path.GetFileName(filePath);
                var contentType = _mimeMappingService.Map(filePath);

                var byteArray = await System.IO.File.ReadAllBytesAsync(filePath).ConfigureAwait(false);

                return File(byteArray, contentType, filename);
            }
            return NotFound();
        }

        internal bool ThumbnailAbortCallBack()
        {
            return false;
        }

        internal async Task<IActionResult> GetFileThumbnail(string attachmentId, int size = 200)
        {
            var filePath = _repoSupervisor.Attachments.GetAttachmentLocation(attachmentId);
            if (!string.IsNullOrWhiteSpace(filePath) && System.IO.File.Exists(filePath))
            {
                //cache valid image thumbnail response
                Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = new TimeSpan(8, 0, 0, 0)
                };
                string projectDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var defaultImage = projectDir + "\\Content\\Images\\Thumbnail\\";
                var byteArray = await System.IO.File.ReadAllBytesAsync(filePath).ConfigureAwait(false);
                if (byteArray.Length > 0)
                {
                    size = size <= 0 || size > 200 ? 200 : size;
                    var fileExtension = Path.GetExtension(filePath);
                    var rootPath = _hostEnvironment.ContentRootPath + "\\Content\\Thumbnails";
                    var thumbnailPath = rootPath + "\\" + attachmentId + "_" + size + ".png";
                    if (filePath.IsImage() || fileExtension == ".pdf")
                    {
                        if (!Directory.Exists(rootPath))
                        {
                            Directory.CreateDirectory(rootPath);
                        }
                    }
                    if (fileExtension == ".pdf")
                    {
                        if (System.IO.File.Exists(thumbnailPath))
                        {
                            return await GetExistingThumbnail(thumbnailPath).ConfigureAwait(false);
                        }
                        else
                        {
                            var bytePdfArray = GetPdfThumbnail(filePath, 0);
                            return GetBitmapStream(bytePdfArray, "image/png", size, thumbnailPath);
                        }
                    }
                    else if (filePath.IsImage())
                    {
                        if (System.IO.File.Exists(thumbnailPath))
                        {
                            return await GetExistingThumbnail(thumbnailPath).ConfigureAwait(false);
                        }
                        else
                        {
                            if (fileExtension == ".svg")
                            {
                                var svgDocument = SvgDocument.Open(filePath);
                                using (var bitmap = svgDocument.Draw())
                                {
                                    var newstream = new MemoryStream();
                                    if (bitmap.Height > size || bitmap.Width > size)
                                    {
                                        Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailAbortCallBack);
                                        Image myThumbnail = null;
                                        myThumbnail = bitmap.GetThumbnailImage(size, size, myCallback, IntPtr.Zero);
                                        myThumbnail.Save(newstream, System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    else
                                    {
                                        bitmap.Save(newstream, System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    newstream.Position = 0;
                                    FileStream file = new FileStream(thumbnailPath, FileMode.Create, FileAccess.Write);
                                    newstream.WriteTo(file);
                                    file.Close();
                                    return new FileStreamResult(newstream, "image/png");
                                }
                            }
                            else
                            {
                                var contentType = _mimeMappingService.Map(filePath);
                                return GetBitmapStream(byteArray, contentType, size, thumbnailPath);
                            }
                        }
                    }
                    else if (fileExtension == ".doc" || fileExtension == ".docx" || fileExtension == ".rtf")
                    {
                        var imagepath = defaultImage + "Word.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                    else if (fileExtension == ".txt" || fileExtension == ".csv")
                    {
                        var imagepath = defaultImage + "Text.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                    else if (fileExtension == ".csv")
                    {
                        var imagepath = defaultImage + "Csv.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                    else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        var imagepath = defaultImage + "Excel.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                    else if (fileExtension == ".ppt" || fileExtension == ".pptx")
                    {
                        var imagepath = defaultImage + "Powerpoint.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                    else if (fileExtension == ".odc")
                    {
                        var imagepath = defaultImage + "Odc.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                    else if (fileExtension == ".dwg")
                    {
                        var imagepath = defaultImage + "Dwg.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                    else
                    {
                        var imagepath = defaultImage + "Other.png";
                        byteArray = await System.IO.File.ReadAllBytesAsync(imagepath).ConfigureAwait(false);
                        var contentType = _mimeMappingService.Map(imagepath);
                        return GetBitmapStream(byteArray, contentType, size);
                    }
                }
            }
            return NotFound();
        }

        internal byte[] GetPdfThumbnail(string filePath, int pageNumber)
        {
            string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string gsDllPath = Path.Combine(binPath, "GhostScript");
            MagickNET.SetGhostscriptDirectory(gsDllPath);
            MagickNET.SetTempDirectory(gsDllPath);
            var settings = new MagickReadSettings
            {
                // Settings the density to 300 dpi will create an image with a better quality
                Density = new Density(300),
                FrameIndex = 0, // First page
                FrameCount = pageNumber  // Number of pages
            };

            using (var images = new MagickImageCollection())
            {
                // Read only the first page of the pdf file based on setting
                images.Read(filePath, settings);
                // Write page to file that contains the page number
                using (var newstream = new MemoryStream())
                {
                    images[pageNumber].Write(newstream, MagickFormat.Png);
                    return newstream.ToArray();
                }
            }
        }

        internal FileStreamResult GetBitmapStream(byte[] byteArray, string contentType, int size, string thumbnailPath = null)
        {
            MemoryStream stream = new(byteArray);
            using (Bitmap image = new(stream))
            {
                if (image.Height > size || image.Width > size)
                {
                    Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailAbortCallBack);
                    Image myThumbnail = null;
                    stream = new MemoryStream();
                    myThumbnail = image.GetThumbnailImage(size, size, myCallback, IntPtr.Zero);
                    myThumbnail.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
                stream.Position = 0;
                if (!string.IsNullOrWhiteSpace(thumbnailPath))
                {
                    FileStream file = new FileStream(thumbnailPath, FileMode.Create, FileAccess.Write);
                    stream.WriteTo(file);
                    file.Close();
                }
                return new FileStreamResult(stream, contentType);
            }
        }

        internal async Task<FileStreamResult> GetExistingThumbnail(string thumbnailPath)
        {
            var byteArray = await System.IO.File.ReadAllBytesAsync(thumbnailPath).ConfigureAwait(false);
            MemoryStream stream = new(byteArray);
            stream.Position = 0;
            return new FileStreamResult(stream, "image/png");
        }
    }
}