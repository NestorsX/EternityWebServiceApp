using EternityWebServiceApp.Interfaces;
using EternityWebServiceApp.Models;
using EternityWebServiceApp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace EternityWebServiceApp.Services
{
    public class DataReferenceRepository : IRepository<DataReferenceViewModel>
    {
        private readonly EternityDBContext _context;

        public DataReferenceRepository(EternityDBContext context)
        {
            _context = context;
        }

        public IEnumerable<DataReferenceViewModel> Get()
        {
            IEnumerable<DataReferenceViewModel> result = new List<DataReferenceViewModel>();
            IEnumerable<DataReference> dataReferences = _context.DataReferences.ToList();
            foreach (var item in dataReferences)
            {
                result = result.Append(new DataReferenceViewModel
                {
                    DataReferenceId = item.DataReferenceId,
                    CityId = item.CityId,
                    AttractionId = item.AttractionId,
                    CityName = _context.Cities.First(x => x.CityId == item.CityId).Title,
                    AttractionName  =_context.Attractions.First(x => x.AttractionId == item.AttractionId).Title
                });
            }

            return result;
        }

        public DataReferenceViewModel Get(int id)
        {
            DataReference dataReference = _context.DataReferences.FirstOrDefault(x => x.DataReferenceId == id);
            return new DataReferenceViewModel
            {
                DataReferenceId = dataReference.DataReferenceId,
                CityId = dataReference.CityId,
                AttractionId = dataReference.AttractionId,
                CityName = _context.Cities.First(x => x.CityId == dataReference.CityId).Title,
                AttractionName = _context.Attractions.First(x => x.AttractionId == dataReference.AttractionId).Title
            };
        }

        public void Create(DataReferenceViewModel dataReference)
        {

            _context.DataReferences.Add(new DataReference
            {
                DataReferenceId = null,
                CityId = dataReference.CityId,
                AttractionId = dataReference.AttractionId
            });

            _context.SaveChanges();
        }

        public void Update(DataReferenceViewModel dataReference)
        {
            _context.DataReferences.Update(new DataReference
            {
                DataReferenceId = dataReference.DataReferenceId,
                CityId = dataReference.CityId,
                AttractionId = dataReference.AttractionId
            });

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.DataReferences.Remove(_context.DataReferences.FirstOrDefault(x => x.DataReferenceId == id));
            _context.SaveChanges();
        }
    }
}
