using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAcessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace OrdersMicroservice.API.ApiControllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        //GET: /api/orders
        [HttpGet]
        public async Task<IEnumerable<OrderResponse?>> Get()
        {
            List<OrderResponse?> orders = await _ordersService.GetOrders();
            return orders;
        }

        //GET: /api/orders/search/order-id/{id}
        [HttpGet("search/order-id/{id}")]
        public async Task<OrderResponse?> GetOrderByOrderID(Guid id)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(
                temp => temp.OrderID,
                id
                );
            OrderResponse? order = await _ordersService.GetOrderByCondition(filter);
            return order;
        }

        //GET: /api/orders/search/product-id/{id}
        [HttpGet("search/product-id/{id}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductID(Guid id)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(
                temp => temp.OrderItems,
                Builders<OrderItem>.Filter.Eq(
                    tempProduct => tempProduct.ProductID,
                    id)
                );

            List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
            return orders;
        }

        //GET: /api/orders/search/order-date/{date}
        [HttpGet("search/order-date/{date}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime date)
        {
            var startDate = date.Date.ToUniversalTime();
            var endDate = startDate.AddDays(1);
            
            FilterDefinition<Order> filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Gte(x => x.OrderDate, startDate),
                Builders<Order>.Filter.Lt(x => x.OrderDate, endDate)
            );

            List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
            return orders;
        }

        //GET: /api/orders/search/user-id/{id}
        [HttpGet("search/user-id/{id}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByUserID(Guid id)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(
                temp => temp.UserID,
                id
                );

            List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
            return orders;
        }

        //POST: /api/orders
        [HttpPost]
        public async Task<IActionResult> Post(OrderAddRequest orderAddRequest)
        {
            if (orderAddRequest == null)
                return BadRequest("Invalid order data");

            OrderResponse? orderResponse = await _ordersService.AddOrder(orderAddRequest);
            if (orderResponse == null)
                return Problem("Error in adding product");

            return Created($"api/orders/search/order-id/{orderResponse?.OrderID}", orderResponse);
        }

        //PUT: /api/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            Guid id,
            OrderUpdateRequest orderUpdateRequest)
        {
            if (orderUpdateRequest == null)
                return BadRequest("Invalid order data");

            if (id != orderUpdateRequest.OrderID)
                return BadRequest("OrderID in the URL doesn't match with the OrderID in the Request body");

            OrderResponse? orderResponse = await _ordersService.UpdateOrder(orderUpdateRequest);
            if (orderResponse == null)
                return Problem("Error in updating product");

            return Ok(orderResponse);
        }

        //DELETE: /api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid OrderID");

            bool isDeleted = await _ordersService.DeleteOrder(id);
            if (!isDeleted)
                return NotFound("Error in deleting product");

            return Ok(isDeleted);
        }
    }
}
