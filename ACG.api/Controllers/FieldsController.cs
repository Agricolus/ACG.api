using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using FIWARE;
using FIWARE.ContextBroker.Enums;
using FIWARE.ContextBroker.Dto;
using System;
using FIWARE.ContextBroker.Helpers;
using ACG.api.Model;
using NetTopologySuite.Geometries;

namespace ACG.api.Controllers
{
    [ApiController]
    [Route("/fields")]
    public class FieldsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ACGContext db;

        public FieldsController(IConfiguration configuration, ACGContext db)
        {
            this.configuration = configuration;
            this.db = db;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFields(string userId)
        {
            var fields = await (from f in db.Fields
                                where f.UserId == userId
                                select new Dto.Field()
                                {
                                    Id = f.Id,
                                    Area = f.Area,
                                    ClientId = f.ClientId,
                                    ExternalId = f.ExternalId,
                                    Name = f.Name,
                                    ModificationTime = f.ModificationTime,
                                    ProducerCode = f.ProducerCode,
                                    IsRegistered = true,
                                    UserId = f.UserId,
                                    Boundaries = f.Boundaries.Geometries.Select(g => g.Coordinates.Select(c => new double[] { c.X, c.Y }).ToArray()).ToArray(),
                                    UnpassableBoundaries = f.UnpassableBoundaries.Geometries.Select(g => g.Coordinates.Select(c => new double[] { c.X, c.Y }).ToArray()).ToArray(),
                                }).ToListAsync();

            return Ok(fields);
        }


        [HttpPost("import/producer")]
        public async Task<IActionResult> RegisterFields([FromBody] Dto.Field field)
        {
            var f = new Model.Field()
            {
                Id = field.Id.Value,
                Name = field.Name,
                ProducerCode = field.ProducerCode,
                ClientId = field.ClientId,
                UserId = field.UserId,
                ExternalId = field.ExternalId,
                ModificationTime = field.ModificationTime,
                Boundaries = new MultiPolygon(field.Boundaries.Select(b => new Polygon(new LinearRing(b.Select(polypoint => new Coordinate(polypoint[0], polypoint[1])).ToArray()))).ToArray()),
                UnpassableBoundaries = new MultiPolygon(field.UnpassableBoundaries.Select(b => new Polygon(new LinearRing(b.Select(polypoint => new Coordinate(polypoint[0], polypoint[1])).ToArray()))).ToArray()),
                IsRegistered = true
            };
            db.Fields.Add(f);
            await db.SaveChangesAsync();
            var outField = new Dto.Field()
            {
                Id = f.Id,
                Area = f.Area,
                ClientId = f.ClientId,
                ExternalId = f.ExternalId,
                Name = f.Name,
                ModificationTime = f.ModificationTime,
                ProducerCode = f.ProducerCode,
                IsRegistered = true,
                UserId = f.UserId,
                Boundaries = f.Boundaries.Geometries.Select(g => g.Coordinates.Select(c => new double[] { c.X, c.Y }).ToArray()).ToArray(),
                UnpassableBoundaries = f.UnpassableBoundaries.Geometries.Select(g => g.Coordinates.Select(c => new double[] { c.X, c.Y }).ToArray()).ToArray(),
            };
            return Ok(outField);
        }

    }
}

