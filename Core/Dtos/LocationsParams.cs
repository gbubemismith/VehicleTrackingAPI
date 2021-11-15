using System;

namespace Core.Dtos
{
    public class LocationsParams
    {
        private const int MaxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
    }
}