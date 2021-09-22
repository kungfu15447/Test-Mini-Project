using System;
using HotelBooking.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelBooking.IntegrationTests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public readonly HotelBookingContext Context;
        private readonly SqliteConnection connection;

        public DatabaseFixture()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Initialize test database
            var options = new DbContextOptionsBuilder<HotelBookingContext>()
                .UseSqlite(connection).Options;
            Context = new HotelBookingContext(options);
            IDbInitializer dbInitializer = new DbInitializer();
            dbInitializer.Initialize(Context);
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}