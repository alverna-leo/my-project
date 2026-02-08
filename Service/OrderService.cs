//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Domain.Interfaces;
//using Domain.Models;

//namespace Domain.Service
//{
//    public class OrderService : IOrderService
//    {
//        private readonly IOrderRepository _repository;

//        public OrderService(IOrderRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<List<Order>> GetAllOrdersWithDetailsAsync()
//        {
//            return await _repository.GetAllWithDetailsAsync();
//        }
//    }
//}
