using BorsaApi.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BorsaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DovizController : ControllerBase
    {
        private readonly IMongoCollection<usd> usdCollection;
        private readonly IMongoCollection<eur> eurCollection;
        private readonly IMongoCollection<che> cheCollection;
        private readonly IMongoCollection<jpy> jpyCollection;
        private readonly IMongoCollection<gbp> gbpCollection;

        public DovizController(IMongoClient client)
        {
            var database = client.GetDatabase("borsadb");
            usdCollection = database.GetCollection<usd>("usd");
            eurCollection = database.GetCollection<eur>("eur");
            cheCollection = database.GetCollection<che>("che");
            jpyCollection = database.GetCollection<jpy>("jpy");
            gbpCollection = database.GetCollection<gbp>("gbp");
        }

        [HttpGet("usd")]
        public IActionResult GetUsd()
        {
            var documents = usdCollection.AsQueryable().ToList();
            return Ok(documents);
        }

        [HttpGet("eur")]
        public IActionResult GetEur()
        {
            var documents = eurCollection.AsQueryable().ToList();
            return Ok(documents);
        }

        [HttpGet("che")]
        public IActionResult GetChe()
        {
            var documents = cheCollection.AsQueryable().ToList();
            return Ok(documents);
        }

        [HttpGet("gbp")]
        public IActionResult GetGbp()
        {
            var documents = gbpCollection.AsQueryable().ToList();
            return Ok(documents);
        }

        [HttpGet("jpy")]
        public IActionResult GetJpy()
        {
            var documents = jpyCollection.AsQueryable().ToList();
            return Ok(documents);
        }
    }
}
