using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calendar.Pages.Calendar
{
    public class IndexModel : PageModel
    {
        public int Year{ get; set; }
        public int Month{ get; set; }
        private ApplicationDbContext _context;
        public IQueryable<Notification> Notifications { get; set; }
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(string year, string? month)
        {
            var intMonth = Convert.ToInt32(month);
            var intYear = Convert.ToInt32(year);
            if (year == null || month == null)
            {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
            }
            else if (intMonth > 12||intMonth<1)
            {
                return NotFound();
            }
            else
            {
                Year = intYear;
                Month = intMonth;
            }
            Notifications = _context.Notifications.Where(n => n.Date.Year == Year && n.Date.Month == Month);
            return Page();
        }

        public IEnumerable<DateTime> GetCalendar()
        {
            var days = new List<DateTime>();
            var startMonth = new DateTime(Year, Month, 1);
            var startDayOfWeek = (int)startMonth.DayOfWeek;
            if (startDayOfWeek == 0) { startDayOfWeek = 7; }
            var CalendarDays = startMonth.AddDays(1 - startDayOfWeek);
            for (var i=0; i < 42; i++)
            {
                days.Add(CalendarDays.AddDays(i));
            }
            return days;
        }
        public string GetMonthName()
        {
            return CultureInfo.GetCultureInfo("en").DateTimeFormat.GetMonthName(Month);
        }
    }
}
