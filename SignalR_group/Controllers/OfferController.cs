using Microsoft.AspNetCore.Mvc;
using SignalR_group.Helpers;

namespace SignalR_group.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        // GET: api/<OfferController>
        [HttpGet]
        public double Get()
        {
            return FileHelper.Read();
        }

        [HttpGet("Room")]
        public double Get(string room)
        {
            return FileHelper.Read(room);
        }

        [HttpGet("IncreaseRoom")]
        public void IncreaseRoom(string room, double data)
        {
            var result = FileHelper.Read(room) + data;
            FileHelper.Write(room, result);
        }

        [HttpGet("Increase")]
        public void Increase(double data)
        {
            var result = FileHelper.Read() + data;
            FileHelper.Write(result);
        }


    }
}
