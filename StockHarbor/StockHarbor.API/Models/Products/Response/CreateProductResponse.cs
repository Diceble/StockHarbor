﻿using StockHarbor.Domain.Enums;

namespace StockHarbor.API.Models.Products.Response;

public record CreateProductResponse(int Id, string Name, string Description, string Sku, ProductStatus Status, ProductType ProductType);