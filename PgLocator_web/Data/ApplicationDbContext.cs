using Microsoft.EntityFrameworkCore;
using PgLocator_web.Models;

namespace PgLocator_web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<Pg> Pg { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Room> Room { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Owner>().HasData(
                new Owner
                {
                    Oid = 1,
                    Lid = 1,
                    Ownername = "John",
                    Email = "johndoe@example.com",
                    Dob = "1990-04-15",
                    Phone = "+1234567890",
                    Whatsapp = "+1234567890",
                    Chatlink = "https://chat.whatsapp.com/examplelink1",
                    Adress = "456 Elm Street, Springfield, IL, USA",
                    Password = "John"
                },
                new Owner
                {
                    Oid = 2,
                    Lid = 102,
                    Ownername = "Jane",
                    Email = "janesmith@example.com",
                    Dob = "1985-09-20",  
                    Phone = "+1987654321",
                    Whatsapp = "+1987654321",
                    Chatlink = "https://chat.whatsapp.com/examplelink2",
                    Adress = "789 Maple Avenue, Springfield, IL, USA",
                    Password = "Jane"
                },
                new Owner
                {
                    Oid = 3,
                    Lid = 103,
                    Ownername = "Alice",
                    Email = "alicejohnson@example.com",
                    Dob = "1992-11-30", 
                    Phone = "+1123456789",
                    Whatsapp = "+1123456789",
                    Chatlink = "https://chat.whatsapp.com/examplelink3",
                    Adress = "123 Oak Drive, Springfield, IL, USA",
                    Password = "Alice"
                },
                new Owner
                {
                    Oid = 4,
                    Lid = 104,
                    Ownername = "Bob",
                    Email = "bobbrown@example.com",
                    Dob = "1988-05-12",  
                    Phone = "+1456789012",
                    Whatsapp = "+1456789012",
                    Chatlink = "https://chat.whatsapp.com/examplelink4",
                    Adress = "321 Pine Street, Springfield, IL, USA",
                    Password = "Bob"
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Uid = 1,
                    Lid = 1,
                    Name = "Alice Johnson",
                    Email = "alice.johnson@example.com",
                    Phone = "123-456-7890",
                    Gender = "Female",
                    Dob = "1990-05-15", 
                    Status = "Active",
                    Password = "Alice"
                },
    new User
    {
        Uid = 2,
        Lid = 2,
        Name = "Bob Smith",
        Email = "bob.smith@example.com",
        Phone = "987-654-3210",
        Gender = "Male",
        Dob = "1988-09-22",
        Status = "Inactive",
        Password = "Bob"
    },
    new User
    {
        Uid = 3,
        Lid = 3,
        Name = "Charlie Brown",
        Email = "charlie.brown@example.com",
        Phone = "555-123-4567",
        Gender = "Male",
        Dob = "1995-12-03",
        Status = "Active",
        Password = "Charlie"
    },
    new User
    {
        Uid = 4,
        Lid = 4,
        Name = "Diana Prince",
        Email = "diana.prince@example.com",
        Phone = "444-987-6543",
        Gender = "Female",
        Dob = "1992-03-08",
        Status = "Active",
        Password = "Diana"
    }
            );
        }
    }
}