using GolobeTravelApi.Data;
using GolobeTravelApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GolobeTravelApi
{
    public static class SeedData
    {
        public static void MigrateAndSeed(IServiceProvider serviceProvider)
        {
            Console.WriteLine("🔄 Running SeedData...");
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            if (!context.TblHotelEntities.Any())
            {
                var hotelEntities = new List<HotelEntity>
                {
                    new HotelEntity
                    {
                        EntityId = "44292080",
                        EntityName = "Mpumalanga",
                        EntityLocation = "-25.8638909153, 30.207138411",
                        EntityHierarchy = "Mpumalanga, South Africa"
                    },
                    new HotelEntity
                    {
                        EntityId = "27544958",
                        EntityName = "Malé",
                        EntityLocation = "4.20039967541815, 73.52786843708289",
                        EntityHierarchy = "Malé Atoll, Maldives"
                    },
                    new HotelEntity
                    {
                        EntityId = "27539908",
                        EntityName = "Cape Town",
                        EntityLocation = "-33.8929939226769, 18.59380272261024",
                        EntityHierarchy = "Cape Town, South Africa"
                    },
                    new HotelEntity
                    {
                        EntityId = "27539733",
                        EntityName = "Paris",
                        EntityLocation = "48.85662237511698, 2.3428759930239886",
                        EntityHierarchy = "Paris, France"
                    },
                    new HotelEntity
                    {
                        EntityId = "27537542",
                        EntityName = "New York",
                        EntityLocation = "40.6940959901, -73.9282670243",
                        EntityHierarchy = "New York, United States"
                    },
                    new HotelEntity
                    {
                        EntityId = "27542089",
                        EntityName = "Tokyo",
                        EntityLocation = "29.992220724554866, 140.76724260851924",
                        EntityHierarchy = "Tokyo, Japan"
                    },
                    new HotelEntity
                    {
                        EntityId = "27538528",
                        EntityName = "Baku",
                        EntityLocation = "40.24663835625875, 50.01205565445854",
                        EntityHierarchy = "Baku, Azerbaijan"
                    },
                    new HotelEntity
                    {
                        EntityId = "27540839",
                        EntityName = "Dubai",
                        EntityLocation = "25.13678353682617, 55.30052976035437",
                        EntityHierarchy = "Dubai, United Arab Emirates"
                    },
                    new HotelEntity
                    {
                        EntityId = "95673323",
                        EntityName = "Istanbul",
                        EntityLocation = "41.2599083, 28.7427717",
                        EntityHierarchy = "Istanbul Province, Türkiye (Turkey)"
                    }
                };

                context.TblHotelEntities.AddRange(hotelEntities);
                context.SaveChanges();
            }

       

            if (!context.TblUser.Any())
            {
                var users = new List<User>
                {
                new User { CognitoId = "f6929264-b071-704b-d4ed-a27b9a1d15e5", UserName = "JLedoza", FirstName = "John", LastName = "Ledoza", Email = "jledoza@email.com" },
                new User { CognitoId = "d13c6238-2081-7056-a235-d94145f8f405", UserName = "ZSteve", FirstName = "Steve", LastName = "Zakri", Email = "zsteve@email.com" }
                };

                context.TblUser.AddRange(users);
                context.SaveChanges();
            }

            if (!context.TblUserFavorites.Any())
            {
                var users = context.TblUser.OrderBy(u => u.Id).ToList();
                var hotels = context.TblHotelData.Take(3).ToList();

                if (users.Count >= 2 && hotels.Count >= 2)
                {
                    var john = users[0];
                    var steve = users[1];

                    context.TblUserFavorites.Add(new UserFavorites
                    {
                        UserId = john.Id,
                        HotelId = 204044750
                    });

                    context.TblUserFavorites.AddRange(new List<UserFavorites>
                    {
                        new UserFavorites { UserId = steve.Id, HotelId = 114419684 },
                        new UserFavorites { UserId = steve.Id, HotelId = 46950878 }
                    });

                    context.SaveChanges();
                }
            }

            if (!context.TblHotelBookings.Any())
            {
                var user = context.TblUser.FirstOrDefault(u => u.CognitoId == "d13c6238-2081-7056-a235-d94145f8f405");
                var hotel = context.TblHotelData.FirstOrDefault();

                if (user != null && hotel != null)
                {
                    var checkinDate = DateTime.UtcNow.AddDays(7);
                    var checkoutDate = checkinDate.AddDays(3);

                    var booking = new HotelBooking
                    {
                        UserId = user.Id,
                        HotelId = hotel.HotelId,
                        CheckinDate = checkinDate,
                        CheckoutDate = checkoutDate,
                        Adults = 2,
                        Children = 1,
                        Rooms = 1,
                        BasePrice = 1500,
                        ServiceFee = 150,
                        Taxes = 225,
                        Nights = 3
                    };

                    context.TblHotelBookings.Add(booking);
                    context.SaveChanges();
                }
            }

            if (!context.TblHotelData.Any())
            {
                var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "InitialSeedData", "cleaned_hotel_data_seed.sql");
                if (File.Exists(sqlFilePath))
                {
                    Console.WriteLine("🚀 Seeding HotelData from SQL file...");
                    var sql = File.ReadAllText(sqlFilePath);
                    var statements = sql
                        .Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .Where(s => s.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    using var conn = (NpgsqlConnection)context.Database.GetDbConnection();
                    conn.Open();

                    foreach (var stmt in statements)
                    {
                        try
                        {
                            using var cmd = new NpgsqlCommand(stmt, conn);
                            cmd.ExecuteNonQuery();
                            //context.Database.ExecuteSqlRaw(stmt);
                            Console.WriteLine($"✅ Inserted: {stmt[..Math.Min(80, stmt.Length)]}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Failed on: {stmt[..Math.Min(80, stmt.Length)]}");
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }

                    conn.Close();
                }
                else
                {
                    Console.WriteLine("⚠️ SQL seed file not found.");
                }
            }
        }
    }
}
