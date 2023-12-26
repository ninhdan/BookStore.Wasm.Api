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
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;

        public SubCategoryController(ISubCategoryRepository subCategoryRepository, IMapper mapper)
        {
            _subCategoryRepository = subCategoryRepository;
            _mapper = mapper;
        }

        // GET: api/SubCategories
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllSubCategory()
        {
            try
            {
                var subCategories = _subCategoryRepository.GetSubCategoriesWithCategory();
                var subCategoriesDto = _mapper.Map<List<SubCategoryDto>>(subCategories);
                return Ok(subCategoriesDto);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/SubCategories/5
        [HttpGet("{subCategoryId:Guid}", Name = "GetSubCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetSubCategory(Guid subCategoryId)
        {

            if (!_subCategoryRepository.SubCategoryExists(subCategoryId))
            {
                return NotFound();
            }

            var SubCategoryDto = _mapper.Map<SubCategoryDto>(_subCategoryRepository.GetSubCategory(subCategoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(SubCategoryDto);

        }

        // POST: api/SubCategories
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateSubCategory([FromBody] SubCategoryDto subCategoryDto)
        {
            if (subCategoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_subCategoryRepository.SubCategoryExists(subCategoryDto.SubCategoryId))
            {
                ModelState.AddModelError("", "SubCategory Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            
            var subCategory = _mapper.Map<SubCategory>(subCategoryDto);

            if (!_subCategoryRepository.CreateSubCategory(subCategory))
            {
                ModelState.AddModelError("", $"Something went wrong saving the subCategory " +
                                                               $"{subCategory.SubcategoryName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetSubCategory", new { subCategoryId = subCategory.SubcategoryId }, subCategory);
        }

        // PUT: api/SubCategories/5
        [HttpPut("{subCategoryId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateSubCategory(Guid subCategoryId, [FromBody] SubCategoryDto subCategoryDto)
        {
            if (subCategoryDto == null || subCategoryId != subCategoryDto.SubCategoryId)
            {
                return BadRequest(ModelState);
            }

            var subCategory = _mapper.Map<SubCategory>(subCategoryDto);

            if (!_subCategoryRepository.UpdateSubCategory(subCategory))
            {
                ModelState.AddModelError("", $"Something went wrong updating the subCategory " +
                                                                                  $"{subCategory.SubcategoryName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // DELETE: api/SubCategories/5
        [HttpDelete("{subCategoryId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSubCategory(Guid subCategoryId)
        {
            if (!_subCategoryRepository.SubCategoryExists(subCategoryId))
            {
                return NotFound();
            }

            var subCategory = _subCategoryRepository.GetSubCategory(subCategoryId);

            if (!_subCategoryRepository.DeleteSubCategory(subCategory))
            {
                ModelState.AddModelError("", $"Something went wrong deleting subCategory " +
                                                                                                     $"{subCategory.SubcategoryName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
