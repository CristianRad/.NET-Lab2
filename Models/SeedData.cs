using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Lab2.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TaskContext(serviceProvider.GetRequiredService<DbContextOptions<TaskContext>>()))
            {
                // Look for any tasks.
                if (context.Tasks.Any())
                {
                    return;   // DB table has been seeded
                }

                context.Tasks.AddRange(
                    new Task
                    {
                        Title = "Finish Lab 1",
                        Description = "Intro To ASP .NET",
                        Added = DateTime.Parse("2020-05-03"),
                        Deadline = DateTime.Parse("2020-05-10"),
                        Importance = Importance.MEDIUM,
                        State = State.CLOSED,
                        ClosedAt = DateTime.Parse("2020-05-09")
                    },

                    new Task
                    {
                        Title = "Finish Lab 2",
                        Description = "Use Database Within ASP .NET Project",
                        Added = DateTime.Parse("2020-05-10"),
                        Deadline = DateTime.Parse("2020-05-17"),
                        Importance = Importance.HIGH,
                        State = State.IN_PROGRESS
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
