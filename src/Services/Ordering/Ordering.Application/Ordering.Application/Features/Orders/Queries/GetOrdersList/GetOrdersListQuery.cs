﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrderVm>>
    {
        public GetOrdersListQuery(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; } = string.Empty;
    }
}
