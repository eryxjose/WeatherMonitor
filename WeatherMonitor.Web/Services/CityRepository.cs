using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherMonitor.Web.Contracts;
using WeatherMonitor.Web.Data;
using WeatherMonitor.Web.Models;

namespace WeatherMonitor.Web.Services
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _db;

        public CityRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(City entity)
        {
            await _db.Cities.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(City entity)
        {
            _db.Cities.Remove(entity);
            return await Save();
        }

        public async Task<IList<City>> FindAll()
        {
            var recs = await _db.Cities.ToListAsync();
            return recs;
        }

        public async Task<City> FindById(int id)
        {
            var rec = await _db.Cities.FindAsync(id);
            return rec;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(City entity)
        {
            _db.Cities.Update(entity);
            return await Save();
        }
    }
}
