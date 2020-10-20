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
    public class WeatherEntryRepository : IWeatherEntryRepository
    {
        private readonly ApplicationDbContext _db;

        public WeatherEntryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(WeatherEntry entity)
        {
            await _db.WeatherEntries.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(WeatherEntry entity)
        {
            _db.WeatherEntries.Remove(entity);
            return await Save();
        }

        public async Task<IList<WeatherEntry>> FindAll()
        {
            var recs = await _db.WeatherEntries.ToListAsync();
            return recs;
        }

        public async Task<IList<WeatherEntry>> FindByCityAndPeriod(string city, DateTime start, DateTime end)
        {
            var result = await (from w in _db.WeatherEntries.Include(o => o.City)
                                where w.City.Name == city && w.CreatedDate > start && w.CreatedDate < end
                                select w).ToListAsync();
            
            return result;
        }

        public async Task<WeatherEntry> FindById(int id)
        {
            var rec = await _db.WeatherEntries.FindAsync(id);
            return rec;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(WeatherEntry entity)
        {
            _db.WeatherEntries.Update(entity);
            return await Save();
        }
    }
}
