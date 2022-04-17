using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class CityRepository : IRepository<City>
    {
        private readonly EternityDBContext _context;

        public CityRepository(EternityDBContext context)
        {
            _context = context;
        }

        public IEnumerable<City> Get()
        {
            return _context.Cities.ToList().OrderBy(x => x.CityId);
        }

        public City Get(int id)
        {
            return _context.Cities.FirstOrDefault(x => x.CityId == id);
        }

        public void Create(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
        }

        public void Update(City city)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Cities.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
