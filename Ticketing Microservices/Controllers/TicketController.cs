using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModel.Ticket;
using System;
using System.Threading.Tasks;

namespace Ticketing_Microservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly IBus _bus;
        public TicketController(IBus bus)
        {
            _bus = bus;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            try
            {
                if(ticket != null)
                {
                    ticket.BookedOn = DateTime.Now;
                    Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                    var endPoint = await _bus.GetSendEndpoint(uri);
                    await endPoint.Send(ticket);
                    return Ok();
                }
                return BadRequest();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}
