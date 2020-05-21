using System;
using System.Collections.Generic;

namespace Lab2.Models
{
    public class Task
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public DateTime Deadline { get; set; }
        public Importance Importance { get; set; }
        public State State { get; set; }
        public DateTime? ClosedAt { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
