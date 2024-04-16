using DataAccess.Repositories;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class TimetableRepo : GenericRepo<Timetable>, ITimetableRepo
    {
        public TimetableRepo(ElderCareContext context) : base(context)
        {
        }
        public new async Task AddAsync(Timetable entity)
        {
            await ValidatingTimetable(entity);
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task ValidatingTimetable(Timetable entity)
        {
            var contract = await _context.Contracts.Include(e => e.ContractServices).FirstOrDefaultAsync(e => e.ContractId == entity.ContractId) ?? throw new Exception("Incorrect contract Id");
            List<string> errors = new();
            List<Tracking> trackings = entity.Trackings.ToList();
            if (!entity.Trackings.IsNullOrEmpty())
            {
                for (int i = 0; i < trackings.Count; i++)
                {
                    if (contract.ContractType is (int?)ContractType.ServiceContract)
                    {
                        if (trackings[i].ContractServicesId == null)
                        {
                            errors.Add("This contract is a service contract, please enter ContractServicesId");
                        }
                        else if (!contract.ContractServices.Any(e => e.ContractServicesId == trackings[i].ContractServicesId))
                        {
                            errors.Add($"This contract doesn't have this ContractServicesId: {trackings[i].ContractServicesId}");
                        }
                    }
                    else if (contract.ContractType is (int?)ContractType.PackageContract)
                    {
                        if (trackings[i].PackageServicesId == null)
                        {
                            errors.Add("This contract is a package contract, please enter PackageServicesId");
                        }
                        else
                        {
                            var packageServices = _context.PackageServices.Where(e => e.PackageId == contract.PackageId) ?? throw new Exception("This contract's package have no services!");
                            if (!await packageServices.AnyAsync(e => e.PackageServicesId == trackings[i].PackageServicesId))
                            {
                                errors.Add($"This contract doesn't have this PackageServicesId: {trackings[i].PackageServicesId}");
                            }
                        }
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new Exception(String.Join(",\n", errors));
            }
            entity.CarerId ??= contract.CarerId;
            if (await _context.Carers.FindAsync(entity.CarerId) is null)
            {
                throw new Exception("Invalid carerId");
            }
        }
    }
}
