﻿using Microsoft.EntityFrameworkCore;
using NSE.Catalog.API.Models;
using NSE.Core.Data;

namespace NSE.Catalog.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _catalogContext;

        public ProductRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public IUnitOfWork UnitOfWork => _catalogContext;

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _catalogContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _catalogContext.Products.FindAsync(id);
        }

        public void Add(Product product)
        {
            _catalogContext.Products.Add(product);
        }

        public void Update(Product product)
        {
            _catalogContext.Products.Update(product);
        }

        public void Dispose()
        {
            _catalogContext?.Dispose();
        }

    }
}
