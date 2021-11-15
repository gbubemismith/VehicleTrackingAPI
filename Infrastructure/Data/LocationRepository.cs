using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(VehicleDbContext context) : base(context)
        {

        }
    }
}