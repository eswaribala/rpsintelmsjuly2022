using CQRSCartAPI.Events;
using CQRSCartAPI.Models;
using CQRSCartAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSCartAPI.Commands
{
    public class CartCommandHandler : ICommandHandler<Command>
    {
        private CartSqliteRepository _repository;
        private AMQPEventPublisher _eventPublisher;
        public CartCommandHandler(AMQPEventPublisher eventPublisher, CartSqliteRepository repository)
        {
            _eventPublisher = eventPublisher;
            _repository = repository;
        }
        public void Execute(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command is null");
            }
            if (command is CreateCartCommand createCommand)
            {
                Cart created = _repository.Create(createCommand.ToCartRecord());
                _eventPublisher.PublishEvent(createCommand.ToCartEvent(created.CartId));
            }
            else if (command is UpdateCartCommand updateCommand)
            {
                Cart record = _repository.GetById(updateCommand.Id);
                _repository.Update(updateCommand.ToCartRecord(record));
                _eventPublisher.PublishEvent(updateCommand.ToCartEvent());
            }
            else if (command is DeleteCartCommand deleteCommand)
            {
                _repository.Remove(deleteCommand.Id);
                _eventPublisher.PublishEvent(deleteCommand.ToCartEvent());
            }
        }
    }
}
