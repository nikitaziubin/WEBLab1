using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Controllers;
using WebApplication3.Models;


namespace TestProject1
{
    public class DatabaseFixture : IDisposable
    {
        public HotelContext Db { get; private set; }
        public DatabaseFixture()
        {
            var constr = "Server=LAPTOP-J5R1H9E6;Database=HotelDB;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True ";
            var optionsBuilder = new DbContextOptionsBuilder<HotelContext>();
            optionsBuilder.UseSqlServer(constr);

            Db = new HotelContext(optionsBuilder.Options);

            // initialize data in the test database
        }

        public void Dispose()
        {
            // clean up test data from the database
        }
    }
    public class FullSumsControllerTests : IClassFixture<DatabaseFixture>
    {
        private readonly HotelContext _context;
        DatabaseFixture dbFixture;

        public FullSumsControllerTests(DatabaseFixture fixture)
        {
            this.dbFixture = fixture;
        }

        [Fact]
        public void GetRoomNotNull()
        {
            
            // Arrange
            RoomsController controller = new RoomsController(dbFixture.Db);

            // Act
            Task<ActionResult<Room>> roomtest = controller.GetRoom(1012);
            // Assert
            Room room = roomtest.Result.Value;
            Assert.NotNull(room);
        }

        [Fact]
        public void GetRoomNull()
        {

            // Arrange
            RoomsController controller = new RoomsController(dbFixture.Db);

            // Act
            Task<ActionResult<Room>> roomtest = controller.GetRoom(1);
            // Assert
            Room room = roomtest.Result.Value;
            Assert.Null(room);
        }
        [Fact]
        public void GetRoomNotNull4()
        {
            RoomsController controller = new RoomsController(dbFixture.Db);
            for (int i = 1012; i < 1015; i++)
            {
                Task<ActionResult<Room>> roomtest = controller.GetRoom(i);
                Room room = roomtest.Result.Value;
                Assert.NotNull(room);
            }   
        }
        [Fact]
        public void GetBookingNull4()
        {
            BookingsController controller = new BookingsController(dbFixture.Db);
            Task<ActionResult<Booking>> booktests = controller.GetBooking(1);
            Booking book = booktests.Result.Value;
            Assert.Null(book);
        }
        [Fact]
        public void GetBookingContainsTere()
        {
            BookingsController controller = new BookingsController(dbFixture.Db);
            Task<ActionResult<Booking>> booktests = controller.GetBooking(1);
            Booking book = booktests.Result.Value;
            Assert.IsNotType<Booking>(book);
        }
    }
}
