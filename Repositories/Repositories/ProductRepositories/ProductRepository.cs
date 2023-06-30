using AutoMapper;
using BusinessObject.Enums;
using BusinessObject.Models;
using DataAccess.DAOs;
using Repositories.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ProductRepositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;

        public ProductRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public List<Product> GetAllProducts()
        {
            return ProductDAO.GetAllProducts();
        }
        public Product GetProductById(int id)
        {
            return ProductDAO.GetProductById(id);
        }
        public bool AddProduct(CreateProductDTO createProduct)
        {
            if(createProduct == null) return false;
            var product = _mapper.Map<Product>(createProduct);
            product.ProductCode = GenerateCodeDAO.GenerateProductCode("Product");
            product.ProductStatus = (int)ProductStatus.Available;
            ProductDAO.Create(product);
            return true;
        }
        public bool UpdateProduct(ProductDTO updateProduct)
        { 
            var check = ProductDAO.GetProductById(updateProduct.ProductId);
            if(check == null) return false;
            var product = _mapper.Map(updateProduct, check);
            ProductDAO.UpdateProduct(product);
            return true;
        }

        public bool DeleteProduct(int id)
        {
            var check = ProductDAO.GetProductById(id);
            if (check == null) return false;
            ProductDAO.Delete(id);
            return true;
        }

        public bool ChangeProductStatus(int id)
        {   
            ProductDAO.ChangeProductStatus(id);
            return true;
        }
    }
}
