using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Calendar.ViewModels;
using DAL;
using DAL.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calendar.Pages.Calendar
{
    public class NotificationModel : PageModel
    {
        private ApplicationDbContext _context;
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public IQueryable<Notification> Notifications;
        public IQueryable<Notification> MonthNotifications;
        public NotificationModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(string year, string month, string day)
        {
            ParseRouteParams(year, month,day);
            if (year == null || month == null || day == null||Month > 12||Month<1||!GetCalendar().Select(c => c.Day).Contains(Day)||Day<1)
            {
                return NotFound();
            }
            Notifications = _context.Notifications.Where(n => n.Date.Year == Year && n.Date.Month == Month && n.Date.Day == Day);
            MonthNotifications = _context.Notifications.Where(n => n.Date.Year == Year && n.Date.Month == Month);
            return Page();
        }
        [BindProperty] 
        public NotificationViewModel NotificationViewModel { get; set; }
        public async Task<IActionResult> OnPostAsync(string year, string month, string day)
        {
            ParseRouteParams(year, month,day);
            if (!ModelState.IsValid)
            {
                return OnGet(Year.ToString(), Month.ToString(), Day.ToString());
            }
            var notification = new Notification
            {
                Date = new DateTime(Year, Month, Day, NotificationViewModel.Hours, NotificationViewModel.Minutes,0),
                Description = NotificationViewModel.Description
            };
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            return OnGet(Year.ToString(), Month.ToString(), Day.ToString());
        }
        public async Task<IActionResult> OnPostDeleteAsync(Guid notificationId, string year, string month, string day)
        {
            if (Guid.Empty.Equals(notificationId))
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }

            return OnGet(year, month, day);
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

        private void ParseRouteParams(string year, string month, string day)
        {
            var intMonth = Convert.ToInt32(month);
            var intYear = Convert.ToInt32(year);
            var intDay = Convert.ToInt32(day);
            Year = intYear;
            Month = intMonth;
            Day = intDay;
        }
    }
}
