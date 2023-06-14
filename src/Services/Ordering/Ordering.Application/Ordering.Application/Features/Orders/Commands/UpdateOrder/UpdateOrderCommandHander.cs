using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHander : IRequestHandler<UpdateOrderCommand>
    {
        readonly private IOrderRepository _orderRepository;
        readonly private IMapper _mapper;
        readonly private ILogger<UpdateOrderCommandHander> _logger;

        public UpdateOrderCommandHander(IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<UpdateOrderCommandHander> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if (orderEntity == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }
            _mapper.Map(request, orderEntity, typeof(UpdateOrderCommand), typeof(Order));
            await _orderRepository.UpdateAsync(orderEntity);
            _logger.LogInformation($"Order {request.Id} is successfully updated.");
        }
    }
}
