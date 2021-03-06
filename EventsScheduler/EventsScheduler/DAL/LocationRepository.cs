﻿using EventsScheduler.DAL.Entities;
using EventsScheduler.DAL.Interfaces;
using System.Linq;

namespace EventsScheduler.DAL
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext context)
            : base(context)
        {
        }

        public AppDbContext AppDbContext
        {
            get { return Context as AppDbContext; }
        }

        public Location GetLocationByAddress(string address)
        {
            var result = AppDbContext.Locations.Where(l => l.Address == address).ToArray();
            return result.Length > 0 ? result[0] : null;
        }
    }
}
