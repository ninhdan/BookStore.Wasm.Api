using AutoMapper;
using BookStoreApi.Models;
using BookStoreApi.Repositories.Interfaces;
using BookStoreView.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceRangeController : ControllerBase
    {
        private readonly IPriceRangeRepository _priceRangeRepository;
        private readonly IMapper _mapper;

        public PriceRangeController(IPriceRangeRepository priceRangeRepository, IMapper mapper)
        {
            _priceRangeRepository = priceRangeRepository;
            _mapper = mapper;
        }

        // GET: api/PriceRanges
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPriceRanges()
        {
            var priceRanges = _priceRangeRepository.GetAllPriceRange();
            var priceRangesDto = new List<PriceRangeDto>();
            foreach (var priceRange in priceRanges)
            {
                priceRangesDto.Add(_mapper.Map<PriceRangeDto>(priceRange));
            }
            return Ok(priceRangesDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreatePriceRange([FromBody] PriceRangeDto priceRangeDto)
        {
            if (priceRangeDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_priceRangeRepository.PriceRangeNameExists(priceRangeDto.PriceRangeName))
            {
                ModelState.AddModelError( "" ,"PriceRange Exists!");
                return StatusCode(404, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var priceRange = _mapper.Map<PriceRange>(priceRangeDto);
            if (!_priceRangeRepository.CreatePriceRange(priceRange))
            {
                ModelState.AddModelError("", $"Something went wrong saving the priceRange " +
                                                                                  $"{priceRange.RangeName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPriceRange", new { priceRangeId = priceRange.RangeId }, priceRange);
        }

        // GET: api/PriceRanges/5
        [HttpGet("{priceRangeId}", Name = "GetPriceRange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPriceRange(Guid priceRangeId)
        {
            if (!_priceRangeRepository.PriceRangeExists(priceRangeId))
            {
                return NotFound();
            }
            var priceRange = _priceRangeRepository.GetPriceRange(priceRangeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var priceRangeDto = _mapper.Map<PriceRangeDto>(priceRange);
            return Ok(priceRangeDto);
        }

        // PUT: api/PriceRanges/5
        [HttpPut("{priceRangeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePriceRange(Guid priceRangeId, [FromBody] PriceRangeDto priceRangeDto)
        {
            if (priceRangeDto == null)
            {
                return BadRequest(ModelState);
            }
            if (!_priceRangeRepository.PriceRangeExists(priceRangeId))
            {
                ModelState.AddModelError("", "PriceRange doesn't Exists!");
            }
            if (_priceRangeRepository.PriceRangeNameExists(priceRangeDto.PriceRangeName))
            {
                ModelState.AddModelError("", "PriceRange Exists!");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var priceRange = _mapper.Map<PriceRange>(priceRangeDto);
            if (!_priceRangeRepository.UpdatePriceRange(priceRange))
            {
                ModelState.AddModelError("", $"Something went wrong updating the priceRange " +
                                                                                                     $"{priceRange.RangeName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        // DELETE: api/PriceRanges/5
        [HttpDelete("{priceRangeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePriceRange(Guid priceRangeId)
        {
            if (!_priceRangeRepository.PriceRangeExists(priceRangeId))
            {
                return NotFound();
            }
            var priceRange = _priceRangeRepository.GetPriceRange(priceRangeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_priceRangeRepository.DeletePriceRange(priceRange))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the priceRange " +
                                                                                                                        $"{priceRange.RangeName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
