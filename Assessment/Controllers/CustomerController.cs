using Assessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("CustomerAppCon"));

            var dbList = dbClient.GetDatabase("Assessment").GetCollection<Customer>("Customer").AsQueryable();

            return new JsonResult(dbList);

        }

        //Add customer records
        [HttpPost]

        public JsonResult Post(Customer cus)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("CustomerAppCon"));

            int LastCustomerId = dbClient.GetDatabase("Assessment").GetCollection<Customer>("Customer").AsQueryable().Count();
            cus.CustomerID = LastCustomerId + 1;

            dbClient.GetDatabase("Assessment").GetCollection<Customer>("Customer").InsertOne(cus);

            return new JsonResult("Customer Records Added Successfully");

        }


        //Update customer records
        [HttpPut]

        public JsonResult Put(Customer cus)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("CustomerAppCon"));

            var filter = Builders<Customer>.Filter.Eq("CustomerId", cus.CustomerID);
            var update = Builders<Customer>.Update.Set("CustomerName", cus.CustomerName)
                                                  .Set("CustomerEmail", cus.CustomerEmail)
                                                   .Set("CustomerDOB", cus.CustomerDOB);

            dbClient.GetDatabase("Assessment").GetCollection<Customer>("Customer").UpdateOne(filter, update);

            return new JsonResult("Customer Records Update Successfully");

        }

        //delete customer records

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("CustomerAppCon"));

            var filter = Builders<Customer>.Filter.Eq("CustomerId", id);
            

            dbClient.GetDatabase("Assessment").GetCollection<Customer>("Customer").DeleteOne(filter);

            return new JsonResult("Customer Record Delete Successfully");

        }



    }
}
