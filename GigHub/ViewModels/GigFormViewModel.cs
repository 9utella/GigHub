using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public int Genre { get; set; }

        public DateTime GetDateTime()
        {
            var dateTime = string.Format("{0} {1}", Date, Time);
            return DateTime.ParseExact(dateTime, "dd MM yyyy HH:mm", null);
        }
        public IEnumerable<Genre> Genres { get; set; }
    }
}