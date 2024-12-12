﻿using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.ServiceContracts;
using DataAcessLayer.Entities;
using DataAcessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;

namespace BusinessLogicLayer.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<OrderAddRequest> _orderAddRequestValidator;
    private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
    private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator;
    private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator;
    private readonly UsersMicroserviceClient _usersMicroserviceClient;
    private readonly ProductsMicroserviceClient _productsMicroserviceClient;

    public OrdersService(
        IOrdersRepository ordersRepository,
        IMapper mapper,
        IValidator<OrderAddRequest> orderAddRequestValidator,
        IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
        IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
        IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator,
        UsersMicroserviceClient usersMicroserviceClient,
        ProductsMicroserviceClient productsMicroserviceClient)
    {
        _ordersRepository = ordersRepository;
        _mapper = mapper;
        _orderAddRequestValidator = orderAddRequestValidator;
        _orderItemAddRequestValidator = orderItemAddRequestValidator;
        _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;
        _orderUpdateRequestValidator = orderUpdateRequestValidator;
        _usersMicroserviceClient = usersMicroserviceClient;
        _productsMicroserviceClient = productsMicroserviceClient;
    }

    public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
    {
        // Check for null parameter
        if (orderAddRequest == null)
            throw new ArgumentNullException(nameof(orderAddRequest));

        // Validate OrderAddRequest using FluentValidations
        ValidationResult orderAddRequestValidationResult = await _orderAddRequestValidator.ValidateAsync(orderAddRequest);
        if (!orderAddRequestValidationResult.IsValid)
        {
            string errors = string.Join(", ",
                orderAddRequestValidationResult.Errors
                    .Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        List<ProductDTO?> products = new List<ProductDTO?>();

        // Validate OrderItemAddRequest using FluentValidations
        foreach (OrderItemAddRequest orderItemAddRequest in orderAddRequest.OrderItems)
        {
            ValidationResult orderItemAddRequestValidationResult = await _orderItemAddRequestValidator.ValidateAsync(orderItemAddRequest);

            if (!orderItemAddRequestValidationResult.IsValid)
            {
                string erros = string.Join(", ",
                    orderItemAddRequestValidationResult.Errors
                        .Select(temp => temp.ErrorMessage));
                throw new ArgumentException(erros);
            }

            // TO DO: Add logic for checking if ProductID exists in ProductsMicroservice
            ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItemAddRequest.ProductID);
            if (product == null)
                throw new ArgumentException("Invalid Product ID");

            products.Add(product);
        }

        // TO DO: Add logic for checking if UserID exists in UsersMicroservice
        UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderAddRequest.UserID);
        if (user == null)
            throw new ArgumentException("Invalid User ID");

        // Convert data from OrderAddRequest to Order
        Order orderInput = _mapper.Map<Order>(orderAddRequest); // Map OrderAddRequest to 'Order' type
                                                                // (it invokes OrderAddRequestToOrderMappingProfile class)
        // Generate values
        foreach (OrderItem orderItem in orderInput.OrderItems)
        {
            orderItem.TotalPrice = orderItem.Quantity + orderItem.UnitPrice;
        }
        orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);

        // Invoke repository
        Order? addedOrder = await _ordersRepository.AddOrder(orderInput);
        if (addedOrder == null)
            return null;

        OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(addedOrder); // Map addedOrder ('Order' type) into 'OrderResponse' type
                                                                                   // (it invokes OrderToOrderResponseMappingProfile)
        // TO DO: Load ProductName and Category in each OrderItem
        if (addedOrderResponse != null)
        {
            foreach (OrderItemResponse orderItemResponse in addedOrderResponse.OrderItems)
            {
                ProductDTO? productDto = products.Where(temp => 
                        temp.ProductID == orderItemResponse.ProductID)
                        .FirstOrDefault();

                if (productDto == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDto, orderItemResponse);
            }
            
            // TO DO: Load UserPersonName and Email from UsersMicroservice
            if (user != null)
                _mapper.Map<UserDTO, OrderResponse>(user, addedOrderResponse);
        }
        return addedOrderResponse;
    }

    public async Task<bool> DeleteOrder(Guid orderID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);

        Order? existingOrder = await _ordersRepository.GetOrderByCondition(filter);
        if (existingOrder == null)
            return false;

        bool isDeleted = await _ordersRepository.DeleteOrder(orderID);
        return isDeleted;
    }

    public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        Order? order = await _ordersRepository.GetOrderByCondition(filter);
        if (order == null)
            return null;

        OrderResponse orderResponse = _mapper.Map<OrderResponse>(order);

        // TO DO: Load ProductName and Category in each OrderItem
        if (orderResponse != null)
        {
            foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
            {
                ProductDTO? productDto = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

                if (productDto == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDto, orderItemResponse);
            }
            
            // TO DO: Load UserPersonName and Email from UsersMicroservice
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
            if (user != null)
                _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
        }
        return orderResponse;
    }

    public async Task<List<OrderResponse?>> GetOrders()
    {
        IEnumerable<Order?> orders = await _ordersRepository.GetOrders();
        IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);

        // TO DO: Load ProductName and Category in each OrderItem
        foreach (OrderResponse? orderResponse in orderResponses)
        {
            if (orderResponse == null)
                continue;

            foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
            {
                ProductDTO? productDto = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

                if (productDto == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDto, orderItemResponse);
            }
            
            // TO DO: Load UserPersonName and Email from UsersMicroservice
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
            if (user != null)
                _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
        }
        return orderResponses.ToList();
    }

    public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        IEnumerable<Order?> orders = await _ordersRepository.GetOrdersByCondition(filter);
        IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);

        // TO DO: Load ProductName and Category in each OrderItem
        foreach (OrderResponse? orderResponse in orderResponses)
        {
            if (orderResponse == null)
                continue;

            foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
            {
                ProductDTO? productDto = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);

                if (productDto == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDto, orderItemResponse);
            }
            
            // TO DO: Load UserPersonName and Email from UsersMicroservice
            UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderResponse.UserID);
            if (user != null)
                _mapper.Map<UserDTO, OrderResponse>(user, orderResponse);
        }
        return orderResponses.ToList();
    }

    public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
    {
        // Check for null parameter
        if (orderUpdateRequest == null)
            throw new ArgumentNullException(nameof(orderUpdateRequest));

        // Validate OrderUpdateRequest using FluentValidations
        ValidationResult orderUpdateRequestValidationResult = await _orderUpdateRequestValidator.ValidateAsync(orderUpdateRequest);
        if (!orderUpdateRequestValidationResult.IsValid)
        {
            string errors = string.Join(", ",
                orderUpdateRequestValidationResult.Errors
                    .Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        List<ProductDTO?> products = new List<ProductDTO?>();
        
        // Validate OrderItemUpdateRequest using FluentValidations
        foreach (OrderItemUpdateRequest orderItemUpdateRequest in orderUpdateRequest.OrderItems)
        {
            ValidationResult orderItemUpdateRequestValidationResult = await _orderItemUpdateRequestValidator.ValidateAsync(orderItemUpdateRequest);

            if (!orderItemUpdateRequestValidationResult.IsValid)
            {
                string erros = string.Join(", ",
                    orderItemUpdateRequestValidationResult.Errors
                        .Select(temp => temp.ErrorMessage));
                throw new ArgumentException(erros);
            }

            // TO DO: Add logic for checking if ProductID exists in ProductsMicroservice
            ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItemUpdateRequest.ProductID);
            if (product == null)
                throw new ArgumentException("Invalid Product ID");
            
            products.Add(product);
        }

        // TO DO: Add logic for checking if UserID exists in UsersMicroservice
        UserDTO? user = await _usersMicroserviceClient.GetUserByUserID(orderUpdateRequest.UserID);
        if (user == null)
            throw new ArgumentException("Invalid User ID");

        // Convert data from OrderUpdateRequest to Order
        Order orderInput = _mapper.Map<Order>(orderUpdateRequest); // Map OrderUpdateRequest to 'Order' type
                                                                   // (it invokes OrderUpdateRequestToOrderMappingProfile class)
        // Generate values
        foreach (OrderItem orderItem in orderInput.OrderItems)
        {
            orderItem.TotalPrice = orderItem.Quantity + orderItem.UnitPrice;
        }
        orderInput.TotalBill = orderInput.OrderItems.Sum(temp => temp.TotalPrice);

        // Invoke repository
        Order? updatedOrder = await _ordersRepository.UpdateOrder(orderInput);
        if (updatedOrder == null)
            return null;

        OrderResponse updatedOrderResponse = _mapper.Map<OrderResponse>(updatedOrder); // Map updatedOrder ('Order' type) into 'OrderResponse' type
                                                                                       // (it invokes OrderToOrderResponseMappingProfile)
        // TO DO: Load ProductName and Category in each OrderItem
        if (updatedOrderResponse != null)
        {
            foreach (OrderItemResponse orderItemResponse in updatedOrderResponse.OrderItems)
            {
                ProductDTO? productDto = products.Where(temp => 
                        temp.ProductID == orderItemResponse.ProductID)
                        .FirstOrDefault();

                if (productDto == null)
                    continue;

                _mapper.Map<ProductDTO, OrderItemResponse>(productDto, orderItemResponse);
            }
            
            // TO DO: Load UserPersonName and Email from UsersMicroservice
            if (user != null)
                _mapper.Map<UserDTO, OrderResponse>(user, updatedOrderResponse);
        }
        return updatedOrderResponse;
    }
}