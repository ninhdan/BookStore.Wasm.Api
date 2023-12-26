using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        // GET: api/suppliers
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllSupplier()
        {
            var suppliers = _supplierRepository.GetAllSupplier();
            var suppliersDto = new List<SupplierDto>();
            foreach (var supplier in suppliers)
            {
                suppliersDto.Add(_mapper.Map<SupplierDto>(supplier));
            }
            return Ok(suppliersDto);
        }

        // GET: api/Suppliers/5
        [HttpGet("{supplierId:Guid}", Name = "GetSupplier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetSupplier(Guid supplierId)
        {

            if (!_supplierRepository.SupplierExists(supplierId))
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(_supplierRepository.GetSupplier(supplierId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
        [ProducesResponseType(StatusCodes.Status409Conflict)] // Conflict
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Internal Server Error
        public IActionResult CreateSupplier([FromBody] SupplierDto supllierDto)
        {
            if (supllierDto == null || !TryValidateModel(supllierDto))
            {
                return BadRequest(ModelState);
            }

            var supplierMap = _mapper.Map<Supplier>(supllierDto);

            if (!_supplierRepository.CreateSupplier(supplierMap))
            {
                ModelState.AddModelError("", $"Failed to save the record {supplierMap.SupplierName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetSupplier", new { supplierId = supplierMap.SupplierId }, supplierMap);
        }

        // PUT: api/Suppliers/5
        [HttpPut("{supplierId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
        [ProducesResponseType(StatusCodes.Status409Conflict)] // Conflict
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Internal Server Error
        public IActionResult UpdateSupplier(Guid supplierId, [FromBody] SupplierDto supplierDto)
        {
            if (supplierDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSupplier = _supplierRepository.GetSupplier(supplierId);

            // Update the existing category with the new data
            _mapper.Map(supplierDto, existingSupplier);

            if (!_supplierRepository.UpdateSupplier(existingSupplier))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {existingSupplier.SupplierName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{supplierId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Internal Server Error
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found
        public IActionResult DeleteSupplier(Guid supplierId)
        {
            if (!_supplierRepository.SupplierExists(supplierId))
            {
                return NotFound();
            }

            var supplierToDelete = _supplierRepository.GetSupplier(supplierId);

            if (!_supplierRepository.DeleteSupplier(supplierToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {supplierToDelete.SupplierName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
