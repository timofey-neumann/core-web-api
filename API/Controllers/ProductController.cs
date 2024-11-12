using API.Helpers;
using Core.Common;
using Asp.Versioning;
using Core.Entities.Business;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;
    private readonly IMemoryCache _memoryCache;

    public ProductController(ILogger<ProductController> logger, IProductService productService, IMemoryCache memoryCache)
    {
        _logger = logger;
        _productService = productService;
        _memoryCache = memoryCache;
    }

    [HttpGet("paginated-data")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int? pageNumber, int? pageSize, string? search, string? sortBy, string? sortOrder, CancellationToken cancellationToken)
    {
        try
        {
            int pageSizeValue = pageSize ?? 10;
            int pageNumberValue = pageNumber ?? 1;
            sortBy ??= "Id";
            sortOrder ??= "desc";

            var filters = new List<ExpressionFilter>();
            if (!string.IsNullOrWhiteSpace(search) && search != null)
            {
                filters.AddRange(new[]
                {
                        new ExpressionFilter
                        {
                            PropertyName = "Code",
                            Value = search,
                            Comparison = Comparison.Contains
                        },
                        new ExpressionFilter
                        {
                            PropertyName = "Name",
                            Value = search,
                            Comparison = Comparison.Contains
                        },
                        new ExpressionFilter
                        {
                            PropertyName = "Description",
                            Value = search,
                            Comparison = Comparison.Contains
                        }
                    });

                if (double.TryParse(search, out double price))
                {
                    filters.Add(new ExpressionFilter
                    {
                        PropertyName = "Price",
                        Value = price,
                        Comparison = Comparison.Equal
                    });
                }
            }

            var products = await _productService.GetPaginatedData(pageNumberValue, pageSizeValue, filters, sortBy, sortOrder, cancellationToken);

            var response = new ResponseViewModel<PaginatedDataViewModel<ProductViewModel>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = products
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving products");

            var errorResponse = new ResponseViewModel<IEnumerable<ProductViewModel>>
            {
                Success = false,
                Message = "Error retrieving products",
                Error = new ErrorViewModel
                {
                    Code = "ERROR_CODE",
                    Message = ex.Message
                }
            };

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productService.GetAll(cancellationToken);

            var response = new ResponseViewModel<IEnumerable<ProductViewModel>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = products
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving products");

            var errorResponse = new ResponseViewModel<IEnumerable<ProductViewModel>>
            {
                Success = false,
                Message = "Error retrieving products",
                Error = new ErrorViewModel
                {
                    Code = "ERROR_CODE",
                    Message = ex.Message
                }
            };

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        try
        {
            var product = new ProductViewModel();

            if (_memoryCache.TryGetValue($"Product_{id}", out ProductViewModel cachedProduct))
            {
                product = cachedProduct;
            }
            else
            {
                product = await _productService.GetById(id, cancellationToken);

                if (product != null)
                {
                    _memoryCache.Set($"Product_{id}", product, TimeSpan.FromMinutes(10));
                }
            }

            var response = new ResponseViewModel<ProductViewModel>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = product
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            if (ex.Message == "No data found")
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel<ProductViewModel>
                {
                    Success = false,
                    Message = "Product not found",
                    Error = new ErrorViewModel
                    {
                        Code = "NOT_FOUND",
                        Message = "Product not found"
                    }
                });
            }

            _logger.LogError(ex, $"An error occurred while retrieving the product");

            var errorResponse = new ResponseViewModel<ProductViewModel>
            {
                Success = false,
                Message = "Error retrieving product",
                Error = new ErrorViewModel
                {
                    Code = "ERROR_CODE",
                    Message = ex.Message
                }
            };

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            string message = "";
            if (await _productService.IsExists("Name", model.Name, cancellationToken))
            {
                message = $"The product name- '{model.Name}' already exists";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "DUPLICATE_NAME",
                        Message = message
                    }
                });
            }

            if (await _productService.IsExists("Code", model.Code, cancellationToken))
            {
                message = $"The product code- '{model.Code}' already exists";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "DUPLICATE_CODE",
                        Message = message
                    }
                });
            }

            try
            {
                var data = await _productService.Create(model, cancellationToken);

                var response = new ResponseViewModel<ProductViewModel>
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding the product");
                message = $"An error occurred while adding the product- " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel<ProductViewModel>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "ADD_ROLE_ERROR",
                        Message = message
                    }
                });
            }
        }

        return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel<ProductViewModel>
        {
            Success = false,
            Message = "Invalid input",
            Error = new ErrorViewModel
            {
                Code = "INPUT_VALIDATION_ERROR",
                Message = ModelStateHelper.GetErrors(ModelState)
            }
        });
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Edit(ProductUpdateViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            string message = "";
            if (await _productService.IsExistsForUpdate(model.Id, "Name", model.Name, cancellationToken))
            {
                message = $"The product name- '{model.Name}' already exists";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "DUPLICATE_NAME",
                        Message = message
                    }
                });
            }

            if (await _productService.IsExistsForUpdate(model.Id, "Code", model.Code, cancellationToken))
            {
                message = $"The product code- '{model.Code}' already exists";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "DUPLICATE_CODE",
                        Message = message
                    }
                });
            }

            try
            {
                await _productService.Update(model, cancellationToken);

                _memoryCache.Remove($"Product_{model.Id}");

                var response = new ResponseViewModel
                {
                    Success = true,
                    Message = "Product updated successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the product");
                message = $"An error occurred while updating the product- " + ex.Message;

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorViewModel
                    {
                        Code = "UPDATE_ROLE_ERROR",
                        Message = message
                    }
                });
            }
        }

        return StatusCode(StatusCodes.Status400BadRequest, new ResponseViewModel
        {
            Success = false,
            Message = "Invalid input",
            Error = new ErrorViewModel
            {
                Code = "INPUT_VALIDATION_ERROR",
                Message = ModelStateHelper.GetErrors(ModelState)
            }
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _productService.Delete(id, cancellationToken);

            _memoryCache.Remove($"Product_{id}");

            var response = new ResponseViewModel
            {
                Success = true,
                Message = "Product deleted successfully"
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            if (ex.Message == "No data found")
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseViewModel
                {
                    Success = false,
                    Message = "Product not found",
                    Error = new ErrorViewModel
                    {
                        Code = "NOT_FOUND",
                        Message = "Product not found"
                    }
                });
            }

            _logger.LogError(ex, "An error occurred while deleting the product");

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel
            {
                Success = false,
                Message = "Error deleting the product",
                Error = new ErrorViewModel
                {
                    Code = "DELETE_ROLE_ERROR",
                    Message = ex.Message
                }
            });
        }
    }
}