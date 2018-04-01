using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysAdmin.Models;
using SysAdmin.Repositories;

namespace SysAdmin.Controllers
{
    public class FilesController : Controller
    {
        private readonly SocialFilesContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly BlobsRepository _blobReposiory = new BlobsRepository();

        public FilesController(SocialFilesContext context, IHostingEnvironment e)
        {
            _context = context;
            _hostingEnvironment = e;
        }

        // GET: Files
        public async Task<IActionResult> Index()
        {
            return View(await _context.Files.ToListAsync());
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            FilesCommentsViewModel filesCommentsViewModel = new FilesCommentsViewModel();

            if (id == null)
            {
                return NotFound();
            }

            var files = await _context.Files
                .SingleOrDefaultAsync(m => m.FileId == id);
            if (files == null)
            {
                return NotFound();
            }

            var comments = await (from c in _context.Comments where c.FileId == id select c).ToListAsync();

            filesCommentsViewModel.Files = files;
            filesCommentsViewModel.Comments = comments;
            filesCommentsViewModel.Comment = new Comments();

            return View(filesCommentsViewModel);
        }

        // GET: Files/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileId,UserEmail,FileName,FileDescription,FileVerified")] Files files, IFormFile FileUploaded)
        {
            if (ModelState.IsValid)
            {

                if (FileUploaded != null)
                {

                    var fileExtension = "." + FileUploaded.ContentType.Substring(FileUploaded.ContentType.LastIndexOf("/") + 1);
                    MailAddress emailAddress = new MailAddress(User.Identity.Name);
                    string Username = emailAddress.User;

                    if (!await _blobReposiory.DoesBlobExistAsync(Username + FileUploaded.FileName))
                    {

                        files.UserEmail = Username;
                        files.FileName = files.FileName + fileExtension;
                        _context.Add(files);
                        await _context.SaveChangesAsync();
                        await _blobReposiory.UploadBlobAsync(FileUploaded, Username + files.FileName);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("FileName", "Woops, this file already exists!");

                    }
                }

            }
            return View(files);
        }

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await _context.Files.SingleOrDefaultAsync(m => m.FileId == id);
            if (files == null)
            {
                return NotFound();
            }
            return View(files);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileId,UserEmail,FileName,FileDescription,FileVerified")] Files files)
        {
            if (id != files.FileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(files);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilesExists(files.FileId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(files);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await _context.Files
                .SingleOrDefaultAsync(m => m.FileId == id);
            if (files == null)
            {
                return NotFound();
            }

            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var files = await _context.Files.SingleOrDefaultAsync(m => m.FileId == id);
            _context.Files.Remove(files);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilesExists(int id)
        {
            return _context.Files.Any(e => e.FileId == id);
        }
    }
}
