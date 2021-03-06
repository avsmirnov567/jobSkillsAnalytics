﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JobSkillsDb.Entities;
using System.Threading.Tasks;
using System.Data.Entity;
using JobSkillsDb.Migrations;

namespace Portal.Controllers
{
    public class MarkedZonesController : ApiController
    {
        // GET: api/MarkedZones
        public async Task<IEnumerable<MarkedZone>> Get()
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                return await db.MarkedZones.Include(z => z.Vacancy).Include(z=>z.Skill).ToListAsync();
            }
        }

        // GET: api/MarkedZones/5
        public async Task<MarkedZone> Get(int id)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                return await db.MarkedZones.Include(z => z.Vacancy).Include(z => z.Skill).SingleOrDefaultAsync(z=>z.Id == id);
            }
        }

        public async Task<IEnumerable<MarkedZone>> ZonesForVacancy(int vacancyId)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                return await db.MarkedZones.Include(z => z.Skill).Where(z => z.Vacancy.Id == vacancyId).ToListAsync();
            }
        }

        // POST: api/MarkedZones
        public void Post([FromBody]MarkedZone markedZone)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                markedZone.Vacancy = null;
                markedZone.Skill = null;
                db.MarkedZones.Add(markedZone);
                db.SaveChanges();
                MarkedZone textZone =
                    db.MarkedZones.Include(z => z.Skill)
                        .Include(z => z.Vacancy)
                        .SingleOrDefault(z => z.Id == markedZone.Id);
            }
        }

        // PUT: api/MarkedZones/5
        public void Put(int id, [FromBody]MarkedZone markedZone)
        {
        }

        // DELETE: api/MarkedZones/5
        public void Delete(int id)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                MarkedZone zone = new MarkedZone() {Id = id};
                db.MarkedZones.Attach(zone);
                db.MarkedZones.Remove(zone);
                db.SaveChanges();
            }
        }
    }
}
