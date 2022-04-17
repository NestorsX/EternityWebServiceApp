using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class AttractionRepository : IRepository<Attraction>
    {
        private readonly EternityDBContext _context;

        public AttractionRepository(EternityDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Attraction> Get()
        {
            return _context.Attractions.ToList().OrderBy(x => x.AttractionId);
        }

        public Attraction Get(int id)
        {
            return _context.Attractions.FirstOrDefault(x => x.AttractionId == id);
        }

        public void Create(Attraction attraction)
        {
            _context.Attractions.Add(attraction);
            _context.SaveChanges();
        }

        public void Update(Attraction attraction)
        {
            _context.Attractions.Update(attraction);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Attractions.Remove(Get(id));
            _context.SaveChanges();
        }
    }
}
