using System;

namespace DAL.Entites
{
    public class Notification
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
