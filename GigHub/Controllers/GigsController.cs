using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    [Authorize]
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendances.Where(_ => _.AttendeeId == userId)
                .Select(_=>_.Gig)
                .Include(_=>_.Artist)
                .Include(_=>_.Genre)
                .ToList();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs=gigs,
                ShowActions=User.Identity.IsAuthenticated
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            var following = _context.Followings.Where(_ => _.FollowerId == userId)
                .Select(_=>_.Followee)
                .ToList();

            var viewModel = new FollowingViewModel
            {
                Followees = following
            };

            return View(viewModel);
        }

        // GET: Gigs
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();

                return View("Create", viewModel);
            }
            var userId = User.Identity.GetUserId();

            var gig = new Gig
            {
                ArtistId = userId,
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Vanue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}