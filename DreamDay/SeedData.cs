using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Enums;
using Microsoft.AspNetCore.Identity;

namespace DreamDay
{
    public static class SeedData
    {
        public static void Initialize(ApplicaitonDbContext context)
        {
            if (context.Users.Any()) return; // Seed only if DB is empty

            var hasher = new PasswordHasher<User>();

            // Seed Users
            var admin = new User { Id = Guid.NewGuid(), Name = "Admin", Email = "admin@dreamday.com", Role = Role.ADMIN };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

            var planners = new List<User>();
            for (int i = 1; i <= 3; i++)
            {
                var planner = new User { Id = Guid.NewGuid(), Name = $"Planner {i}", Email = $"planner{i}@dreamday.com", Role = Role.PLANNER };
                planner.PasswordHash = hasher.HashPassword(planner, "Planner@123");
                planners.Add(planner);
            }

            var couples = new List<User>();
            for (int i = 1; i <= 4; i++)
            {
                var couple = new User { Id = Guid.NewGuid(), Name = $"Couple {i}", Email = $"couple{i}@dreamday.com", Role = Role.COUPLE };
                couple.PasswordHash = hasher.HashPassword(couple, "Couple@123");
                couples.Add(couple);
            }

            context.Users.AddRange(admin);
            context.Users.AddRange(planners);
            context.Users.AddRange(couples);

            // Seed Vendors
            var categories = new[] { "Food", "Drinks", "Photography", "Venue", "Florist", "Music", "Decoration", "Catering", "Lighting", "Bakery" };
            var vendors = categories.Select((cat, i) => new Vendor
            {
                Id = Guid.NewGuid(),
                Name = $"{cat} Vendor",
                Category = cat,
                Description = $"Professional {cat.ToLower()} services for weddings.",
                ContactInfo = $"contact@{cat.ToLower()}.com",
                PriceEstimate = 1000 + i * 250
            }).ToList();
            context.Vendors.AddRange(vendors);

            // Seed Weddings
            var weddings = couples.Select((couple, i) => new Wedding
            {
                Id = Guid.NewGuid(),
                UserId = couple.Id,
                PartnerOneName = $"Partner{i + 1}A",
                PartnerTwoName = $"Partner{i + 1}B",
                WeddingDate = DateTime.Today.AddDays(i * 15),
                TotalBudget = 15000 + (i * 2500),
                PlannerId = planners[i % planners.Count].Id
            }).ToList();
            context.Weddings.AddRange(weddings);

            // Seed Guests, Checklist, Budget, Timeline, Notes
            foreach (var wedding in weddings)
            {
                context.Guests.AddRange(new[]
                {
                new Guest { Id = Guid.NewGuid(), Name = "John Doe", Email = "guest1@mail.com", IsAttending = true, MealPreference = MealPreference.VEGETARIAN, WeddingId = wedding.Id },
                new Guest { Id = Guid.NewGuid(), Name = "Jane Smith", Email = "guest2@mail.com", IsAttending = false, MealPreference = MealPreference.NON_VEGETARIAN, WeddingId = wedding.Id }
            });

                context.CheckListItems.AddRange(new[]
                {
                new CheckListItem { Id = Guid.NewGuid(), Name = "Book Venue", DueDate = wedding.WeddingDate.AddDays(-30), WeddingId = wedding.Id, IsCompleted = true },
                new CheckListItem { Id = Guid.NewGuid(), Name = "Send Invitations", DueDate = wedding.WeddingDate.AddDays(-15), WeddingId = wedding.Id, IsCompleted = false }
            });

                context.BudgetItems.AddRange(new[]
                {
                new BudgetItem { Id = Guid.NewGuid(), Category = "Venue", AllocatedAmount = 5000, SpentAmount = 4500, Description = "Main hall", WeddingId = wedding.Id },
                new BudgetItem { Id = Guid.NewGuid(), Category = "Catering", AllocatedAmount = 4000, SpentAmount = 4200, Description = "Dinner buffet", WeddingId = wedding.Id }
            });

                context.WeddingTimelineItems.AddRange(new[]
                {
                new WeddingTimelineItem { Id = Guid.NewGuid(), WeddingId = wedding.Id, Title = "Ceremony Starts", Time = wedding.WeddingDate.AddHours(10), Description = "Start of ceremony" },
                new WeddingTimelineItem { Id = Guid.NewGuid(), WeddingId = wedding.Id, Title = "Dinner", Time = wedding.WeddingDate.AddHours(18), Description = "Buffet opens" }
            });

                context.WeddingNotes.Add(new WeddingNote
                {
                    Id = Guid.NewGuid(),
                    WeddingId = wedding.Id,
                    Message = "Ensure floral arrangements are delivered on time.",
                    SenderRole = "PLANNER",
                    Timestamp = DateTime.Now
                });

                // Assign 2 random vendors
                context.WeddingVendors.AddRange(vendors.Take(2).Select(v => new WeddingVendor
                {
                    Id = Guid.NewGuid(),
                    VendorId = v.Id,
                    WeddingId = wedding.Id
                }));
            }

            context.SaveChanges();
        }
    }
}
