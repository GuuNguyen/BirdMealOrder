using BusinessObject.Models;
using Repositories.DTOs.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ProductRepositories
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        Product GetProductByCode(string code);
        List<Product> GetProductsByMealId(int id);
        List<Product> GetProductsByIds(List<int> productIds);
        bool AddProduct(CreateProductDTO product);
        bool UpdateProduct(ProductDTO product);
        bool DeleteProduct(int id);
        bool ChangeProductStatus(int id);
    }
}
