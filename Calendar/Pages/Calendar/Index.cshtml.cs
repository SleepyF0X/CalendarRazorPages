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
        private readonly ApplicationDbContext _context;

        public int Year{ get; private set; }
        public int Month{ get; private set; }
        public IEnumerable<DateTime> Calendar { get; private set; }
        public IQueryable<Notification> Notifications { get; private set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(string year, string month)
        {
            ParseRouteParams(year,month);
            if (year == null || month == null)
            {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
            }
            else if (Month > 12||Month<1)
            {
                return NotFound();
            }
            Calendar = GetCalendar();
            Notifications = _context.Notifications.Where(n => n.Date.Year.Equals(Year) && n.Date.Month.Equals(Month));
            return Page();
        }

        #region suply methods

        private IEnumerable<DateTime> GetCalendar()
        {
            var days = new List<DateTime>();
            var startMonth = new DateTime(Year, Month, 1);
            var startDayOfWeek = (int)startMonth.DayOfWeek;
            if (startDayOfWeek == 0) { startDayOfWeek = 7; }
            var calendarDays = startMonth.AddDays(1 - startDayOfWeek);
            for (var i=0; i < 42; i++)
            {
                days.Add(calendarDays.AddDays(i));
            }
            return days;
        }
        public string GetMonthName()
        {
            return CultureInfo.GetCultureInfo("en").DateTimeFormat.GetMonthName(Month);
        }
        private void ParseRouteParams(string year, string month)
        {
            var intMonth = Convert.ToInt32(month);
            var intYear = Convert.ToInt32(year);
            Year = intYear;
            Month = intMonth;
        }

        #endregion
    }
}
