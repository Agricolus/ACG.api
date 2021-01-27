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
using NetTopologySuite;
using System.Collections.Generic;
using ACG.api.Dto;

namespace ACG.api.Controllers
{
    [ApiController]
    [Route("/machines")]
    public class MachinesController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ACGContext db;

        public MachinesController(IConfiguration configuration, ACGContext db)
        {
            this.configuration = configuration;
            this.db = db;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMachine(string userId)
        {
            var machines = await (from m in db.Machines
                                  where m.UserId == userId
                                  select new Dto.Machine()
                                  {
                                      Id = m.Id,
                                      Code = m.Code,
                                      Description = m.Description,
                                      Lat = m.Lat,
                                      Lng = m.Lng,
                                      Model = m.Model,
                                      Name = m.Name,
                                      ProducerCode = m.ProducerCode,
                                      ProducerCommercialName = m.ProducerCommercialName,
                                      Type = m.Type,
                                      UserId = m.UserId,
                                      PTime = m.PTime
                                  }).ToListAsync();

            return Ok(machines);
        }

        [HttpGet("{userId}/{machineId}")]
        public async Task<IActionResult> GetMachineLocations(string userId, Guid machineId, DateTime? start = null, DateTime? end = null)
        {
            var points = await (from ml in db.MachinesHistory
                                where ml.MachineId == machineId &&
                                    (start == null || ml.PTime >= start) &&
                                    (end == null || ml.PTime <= end)
                                orderby ml.PTime ascending
                                select ml).ToListAsync();


            var allPoints = points.Select(v => new { operation = v.Operation, time = v.PTime, point = new double[] { v.Lat, v.Lng } }).GroupBy(x => x.time.Date).Select(g => g.Select(x => x));
            return Ok(allPoints);           
        }

        [HttpPost("import/producer")]
        public async Task<IActionResult> RegisterMachines([FromBody] Dto.Machine machine)
        {
            var m = new Model.Machine()
            {
                Code = machine.Code,
                Description = machine.Description,
                Model = machine.Model,
                Name = machine.Name,
                ProducerCode = machine.ProducerCode,
                ProducerCommercialName = machine.ProducerCommercialName,
                Type = machine.Type,
                UserId = machine.UserId,
                Lat = machine.Lat,
                Lng = machine.Lng,
                PTime = machine.PTime,
                ExternalId = machine.ExternalId,
                Id = machine.Id.Value,
                OtherData = machine.OtherData != null ? machine.OtherData.ToString() : null
            };
            db.Machines.Add(m);
            try
            {
                await db.SaveChangesAsync();
                var cbMachineEntity = new Dto.ContextBroker.Machine()
                {
                    Id = m.Id.ToString(),
                    Type = m.Type,
                    Code = m.Code,
                    Description = m.Description,
                    Model = m.Model,
                    Name = m.Name,
                    ProducerCode = m.ProducerCode,
                    ProducerCommercialName = m.ProducerCommercialName,
                    UserId = m.UserId,
                    Position = new Dto.ContextBroker.GeoJsonPoint()
                    {
                        Coordinates = new double[] { m.Lat.Value, m.Lng.Value }
                    },
                    PTime = m.PTime.Value,
                    ExternalId = m.ExternalId
                };
                var cbConfig = configuration.GetSection("contextBroker");
                var cbUrl = cbConfig.GetValue<string>("cbUrl");
                var cbService = cbConfig.GetValue<string>("cbService");
                var cbServicePath = cbConfig.GetValue<string>("cbServicePath");
                var contextBrokerNotificationEndpoint = cbConfig.GetValue<string>("machineNoditiciationCB");

                var cbClient = new ContextBrokerClient(cbService, cbServicePath, cbUrl);
                var cbEntity = await cbClient.CreateEntity<Dto.ContextBroker.Machine>(cbMachineEntity, AttributesFormatEnum.keyValues);
                var sub = new Subscription()
                {
                    Subject = new Subject()
                    {
                        Entities = new Entity[] {
                            new Entity {
                                Id = cbMachineEntity.Id,
                                Type = cbMachineEntity.Type
                            }
                        }
                    },
                    Notification = new Notification()
                    {
                        Http = new Http()
                        {
                            Url = new Uri(string.Format(contextBrokerNotificationEndpoint, cbMachineEntity.Id))
                        }
                    }
                };
                var cbSubsrcitpionId = await cbClient.CreateSubscription(sub);
                m.CBSubscriptionId = cbSubsrcitpionId;
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                db.Machines.Remove(m);
                await db.SaveChangesAsync();
                throw new Exception($"main api context broker error: {e.Message}", e);
            }
            var outMachine = new Dto.Machine()
            {
                Code = m.Code,
                Description = m.Description,
                Model = m.Model,
                Name = m.Name,
                ProducerCode = m.ProducerCode,
                ProducerCommercialName = m.ProducerCommercialName,
                Type = m.Type,
                UserId = m.UserId,
                Lat = m.Lat,
                Lng = m.Lng,
                PTime = m.PTime,
                ExternalId = m.ExternalId,
                Id = m.Id,
                OtherData = JsonConvert.DeserializeObject<object>(m.OtherData)
            };
            return Ok(outMachine);
        }


        [HttpPost("notification")]

        public async Task<IActionResult> ReceiveNotification([FromBody] Notification<Dto.ContextBroker.Machine> machineNotification)
        {
            var machineData = machineNotification.DataTyped.First();

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var machinePoint = geometryFactory.CreatePoint(new Coordinate(machineData.Position.Coordinates[0], machineData.Position.Coordinates[1]));

            var machineHistory = new MachineHistory()
            {
                MachineId = Guid.Parse(machineData.Id),
                PTime = machineData.PTime,
                Lat = machineData.Position.Coordinates[0],
                Lng = machineData.Position.Coordinates[1],
                Position = machinePoint
            };
            var machine = await db.Machines.Where(m => m.Id.ToString() == machineData.Id).FirstOrDefaultAsync();
            machine.Lat = machineData.Position.Coordinates[0];
            machine.Lng = machineData.Position.Coordinates[1];
            db.MachinesHistory.Add(machineHistory);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{userId}/{machineId}/operationpoints")]

        public async Task<IActionResult> ReceiveMachineOperationPoints([FromBody] List<OperationPoint> operationPoints, string userId, Guid machineId)
        {
            foreach (var point in operationPoints)
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var machinePoint = geometryFactory.CreatePoint(new Coordinate(point.X, point.Y));
                var machineHistory = new MachineHistory()
                {
                    MachineId = machineId,
                    PTime = point.Timestamp,
                    Lat = point.Y,
                    Lng = point.X,
                    Position = machinePoint,
                    Operation = point.Operation
                };
                db.MachinesHistory.Add(machineHistory);
            }
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}

