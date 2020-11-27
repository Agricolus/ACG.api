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
        // [HttpGet("import/{user}/producer")]
        // public async Task<IActionResult> ImportMachines(string producer, string user)
        // {

        //     return NotFound();
        // }


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
                OtherData = JsonConvert.SerializeObject(machine.OtherData)
            };
            db.Machines.Add(m);
            await db.SaveChangesAsync();
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
                OtherData = m.OtherData
            };
            var cbConfig = configuration.GetSection("contextBroker");
            var cbUrl = cbConfig.GetValue<string>("cbUrl");
            var contextBrokerNotificationEndpoint = cbConfig.GetValue<string>("machineNoditiciationCB");

            try
            {
                var cbClient = new ContextBrokerClient("agri_contractor_gateway", "/", cbUrl);
                var cbEntity = await cbClient.CreateEntity<Dto.Machine>(outMachine, AttributesFormatEnum.keyValues);
                var sub = new Subscription()
                {
                    Subject = new Subject()
                    {
                        Entities = new Entity[] {
                            new Entity {
                                Id = outMachine.Id.Value.ToString(),
                                Type = outMachine.Type
                            }
                        }
                    },
                    Notification = new Notification()
                    {
                        Http = new Http()
                        {
                            Url = new Uri(string.Format(contextBrokerNotificationEndpoint, outMachine.Id.Value.ToString()))
                        }
                    }
                };
                await cbClient.CreateSubscription(sub);
            }
            catch (Exception e)
            {
                db.Machines.Remove(m);
                await db.SaveChangesAsync();
                throw new Exception($"main api context broker error: {e.Message}", e);
            }
            return Ok(outMachine);
        }

        // [HttpGet("{machineId}")]
        // public async Task<IActionResult> GetMachine(string machineId)
        // {

        //     return BadRequest();
        // }

    }
}

