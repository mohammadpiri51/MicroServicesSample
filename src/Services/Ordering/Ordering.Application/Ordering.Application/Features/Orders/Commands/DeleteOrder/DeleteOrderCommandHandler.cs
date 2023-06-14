using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        readonly private IOrderRepository _orderRepository;
        readonly private IMapper _mapper;
        readonly private ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if (orderEntity == null)
            {
                throw new NotFoundException(nameof(Order), request.Id); 
            }
            await _orderRepository.DeleteAsync(orderEntity);
            _logger.LogInformation($"Order {request.Id} is successfully deleted.");
        }
    }
}
