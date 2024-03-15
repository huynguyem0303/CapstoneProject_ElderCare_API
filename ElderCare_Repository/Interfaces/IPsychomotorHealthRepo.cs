using DataAccess.Interfaces;
using ElderCare_Domain.Models;

namespace ElderCare_Repository.Interfaces
{
    public interface IPsychomotorHealthRepo : IGenericRepo<PsychomotorHealth>
    {
        public Task<PsychomotorHealth?> GetByIdsAsync(int HealthDetailId, int PsychomotorHealthId);
    }
}
