using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElderCare_Repository.Repos
{
    public class CarerRepository : GenericRepo<Carer>, ICarerRepository
    {
        public CarerRepository(ElderCareContext context) : base(context)
        {

        }

        public async Task<List<Carer?>> searchCarer(SearchCarerDto dto)
        {
            var list = GetAll().ToList();
            List<Carer> carer = new List<Carer>();
            List<Carer> carershift = new List<Carer>();
            List<Carer> carercate = new List<Carer>();
            List<CarerShilft> shift = await _context.Set<CarerShilft>().ToListAsync();
            List<CarerShilft> CarerShilft = new List<CarerShilft>();
            List<CarerCategory> cate = await _context.Set<CarerCategory>().ToListAsync();
            List<CarerCategory> CarerCategory = new List<CarerCategory>();
            string separator = " ";
            string genderlist = String.Join(separator, dto.Gender);
            string timelist = String.Join(separator, dto.TimeShift);
            string agelist = String.Join(separator, dto.Age);
            string catelist = String.Join(separator, dto.Cate);

            if (!timelist.IsNullOrEmpty())
            {
                for (int i = 0; i < shift.Count; i++)
                {
                    if (timelist.Contains(shift[i].Shilf.Name))
                        CarerShilft.Add(shift[i]);
                }
                for (int i = 0; i < CarerShilft.Count; i++)
                {
                    carershift = await _context.Set<Carer>().Where(x => (x.CarerId == CarerShilft[i].CarerId)).ToListAsync(); ;
                }

            }
            if (!catelist.IsNullOrEmpty())
            {
                for (int i = 0; i < cate.Count; i++)
                {
                    if (catelist.Contains(cate[i].Cate.Description))
                        CarerCategory.Add(cate[i]);
                }
                for (int i = 0; i < CarerCategory.Count; i++)
                {
                    carercate = await _context.Set<Carer>().Where(x => (x.CarerId == CarerCategory[i].CarerId)).ToListAsync(); ;
                }

            }


            if (!agelist.IsNullOrEmpty() && !genderlist.IsNullOrEmpty())
                for (int i = 0; i < list.Count; i++)
                {
                    if (genderlist.Contains(list[i].Gender) && agelist.Contains(list[i].Age))
                        carer.Add(list[i]);
                }
            if (genderlist.IsNullOrEmpty() && !genderlist.IsNullOrEmpty())
                for (int i = 0; i < list.Count; i++)
                {
                    if (agelist.Contains(list[i].Age))
                        carer.Add(list[i]);
                }
            if (!agelist.IsNullOrEmpty() && genderlist.IsNullOrEmpty())
                for (int i = 0; i < list.Count; i++)
                {
                    if (agelist.Contains(list[i].Age))
                        carer.Add(list[i]);
                }

            var combine = carer.Union(carershift).ToList();
            var result = combine.Union(carercate).ToList();
            return result;
        }
    }
}
