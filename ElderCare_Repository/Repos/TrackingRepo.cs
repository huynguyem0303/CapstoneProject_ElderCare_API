using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;

namespace ElderCare_Repository.Repos
{
    public class TrackingRepo : GenericRepo<Tracking>, ITrackingRepo
    {
        public TrackingRepo(ElderCareContext context) : base(context)
        {
        }
    }
}
