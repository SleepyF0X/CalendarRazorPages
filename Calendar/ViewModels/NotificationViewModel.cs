using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.ViewModels
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string Description { get; set; }
    }
}
