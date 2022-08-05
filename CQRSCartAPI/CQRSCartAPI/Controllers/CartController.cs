using CQRSCartAPI.Commands;
using CQRSCartAPI.Events;
using CQRSCartAPI.Models;
using CQRSCartAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRSCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICommandHandler<Command> _commandHandler;
        private readonly CartMongoRepository _mongoRepository;
        private readonly CartSqliteRepository _sqliteRepository;
        private readonly CartMessageListener _listener;

        public CartController(ICommandHandler<Command> commandHandler,
            CartSqliteRepository sqliteRepository,
            CartMongoRepository repository, CartMessageListener listener)
        {
            _commandHandler = commandHandler;
            _sqliteRepository = sqliteRepository;
            _mongoRepository = repository;
            _listener = listener;

            //if (_mongoRepository.GetCustomers().Count == 0)
            //{
            //    var customerCmd = new CreateCustomerCommand
            //    {
            //        Name = "Ajay",
            //        Email = "ajay@email.com",
            //        Age = 23,
            //        Phones = new List<CreatePhoneCommand>
            //        {
            //            new CreatePhoneCommand { Type = PhoneType.CELLPHONE, AreaCode = 123, Number = 7543010 }
            //        }
            //    };
            //    _commandHandler.Execute(customerCmd);

            //}
        }
        [HttpGet]
        public List<CartEntity> Get()
        {

            return _mongoRepository.GetCarts();
        }
        [HttpGet("{id}", Name = "GetCart")]
        public IActionResult GetById(long id)
        {
            var product = _mongoRepository.GetCart(id);
            if (product == null)
            {
                return NotFound();
            }
            return new ObjectResult(product);
        }
        [HttpGet("{email}")]
        public IActionResult GetByName(string name)
        {
            var product = _mongoRepository.GetCartByCartName(name);
            if (product == null)
            {
                return NotFound();
            }
            return new ObjectResult(product);
        }
        [HttpPost]
        public IActionResult Post([FromBody] CreateCartCommand cart)
        {
            _commandHandler.Execute(cart);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                _listener.Start();
            }).Start();

            return CreatedAtRoute("GetCart", new { id = cart.Id }, cart);
        }
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] UpdateCartCommand cart)
        {
            var record = _sqliteRepository.GetById(id);
            if (record == null)
            {
                return NotFound();
            }
            cart.Id = id;
            _commandHandler.Execute(cart);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var record = _sqliteRepository.GetById(id);
            if (record == null)
            {
                return NotFound();
            }
            _commandHandler.Execute(new DeleteCartCommand()
            {
                Id = id
            });
            return NoContent();
        }
    }
}
