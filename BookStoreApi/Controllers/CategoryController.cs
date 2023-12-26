using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;


namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllCategory()
        {
            var categories = _categoryRepository.GetAllCategory();
            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDto>(category));
            }
            return Ok(categoriesDto);
        }

        // GET: api/Categories/5
        [HttpGet("{categoryId:Guid}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCategory(Guid categoryId)
        {

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }


            var categoryDto = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

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
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || !TryValidateModel(categoryDto))
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepository.CategoryNameExists(categoryDto.CategoryName))
            {
                ModelState.AddModelError(nameof(categoryDto.CategoryName), "Category with the same name already exists.");
                return Conflict(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", $"Failed to save the record {categoryMap.CategoryName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = categoryMap.CategoryId }, categoryMap);
        }

        // PUT: api/Categories/5
        [HttpPut("{categoryId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
        [ProducesResponseType(StatusCodes.Status409Conflict)] // Conflict
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Internal Server Error
        public IActionResult UpdateCategory(Guid categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = _categoryRepository.GetCategory(categoryId);

         
            if (_categoryRepository.CategoryNameExists(categoryDto.CategoryName))
            {
                ModelState.AddModelError("CategoryName", $"Duplicated category name: {categoryDto.CategoryName}");
                return Conflict(ModelState);
            }

           
            // Update the existing category with the new data
            _mapper.Map(categoryDto, existingCategory);

            if (!_categoryRepository.UpdateCategory(existingCategory))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {existingCategory.CategoryName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Internal Server Error
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found
        public IActionResult DeleteCategory(Guid categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {categoryToDelete.CategoryName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
