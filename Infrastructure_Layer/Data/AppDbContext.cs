using Domain_Layer.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelPicture> HotelPictures { get; set; }
        public DbSet<HotelPhoneNumber> HotelPhoneNumbers { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<HotelRoomPicture> HotelRoomPictures { get; set; }
        public DbSet<HotelAmenity> HotelAmenities { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> payments { get; set; }

    }
}
