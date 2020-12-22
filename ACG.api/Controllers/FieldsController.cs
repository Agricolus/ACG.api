using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;

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
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var boundaries = geometryFactory.CreateMultiPolygon(field.Boundaries.Select(b => geometryFactory.CreatePolygon(geometryFactory.CreateLinearRing(b.Select(pp => new Coordinate(pp[0], pp[1])).ToArray()))).ToArray());
            var unpassableBoundaries = geometryFactory.CreateMultiPolygon(field.UnpassableBoundaries.Select(b => geometryFactory.CreatePolygon(geometryFactory.CreateLinearRing(b.Select(pp => new Coordinate(pp[0], pp[1])).ToArray()))).ToArray());

            var f = new Model.Field()
            {
                Id = field.Id.Value,
                Name = field.Name,
                ProducerCode = field.ProducerCode,
                ClientId = field.ClientId,
                UserId = field.UserId,
                ExternalId = field.ExternalId,
                ModificationTime = field.ModificationTime,
                Boundaries = boundaries,
                UnpassableBoundaries = unpassableBoundaries,
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

