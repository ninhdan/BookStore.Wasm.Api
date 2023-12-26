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
    public class LayoutController : ControllerBase
    {
        private readonly ILayoutRepository _layoutRepository;
        private readonly IMapper _mapper;

        public LayoutController(ILayoutRepository layoutRepository, IMapper mapper)
        {
            _layoutRepository = layoutRepository;
            _mapper = mapper;
        }

        // GET: api/Layouts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllLayout()
        {
            var layouts = _layoutRepository.GetAllLayout();
            var layoutsDto = new List<LayoutDto>();
            foreach (var layout in layouts)
            {
                layoutsDto.Add(_mapper.Map<LayoutDto>(layout));
            }
            return Ok(layoutsDto);
        }

        // GET: api/Layouts/5
        [HttpGet("{layoutId:Guid}", Name = "GetLayout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetLayout(Guid layoutId)
        {

            if (!_layoutRepository.LayoutExists(layoutId))
            {
                return NotFound();
            }

            var layoutDto = _mapper.Map<LayoutDto>(_layoutRepository.GetLayout(layoutId));
            return Ok(layoutDto);
        }

        // PUT: api/Layouts/5
        [HttpPut("{layoutId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateLayout(Guid layoutId, [FromBody] LayoutDto layoutDto)
        {
            if (layoutDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_layoutRepository.LayoutNameExists(layoutDto.LayoutName))
            {
                ModelState.AddModelError("", "Layout Name Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var layout = _mapper.Map<Layout>(layoutDto);

            if (!_layoutRepository.UpdateLayout(layout))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {layout.LayoutName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // POST: api/Layouts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateLayout([FromBody] LayoutDto layoutDto)
        {
            if (layoutDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_layoutRepository.LayoutNameExists(layoutDto.LayoutName))
            {
                ModelState.AddModelError("", "Layout Name Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var layout = _mapper.Map<Layout>(layoutDto);

            if (!_layoutRepository.CreateLayout(layout))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {layout.LayoutName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetLayout", new { layoutId = layout.LayoutId }, layout);
        }
        [HttpDelete("{layoutId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteLayout(Guid layoutId)
        {
            if (!_layoutRepository.LayoutExists(layoutId))
            {
                return NotFound();
            }

            var layout = _layoutRepository.GetLayout(layoutId);

            if (!_layoutRepository.DeleteLayout(layout))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {layout.LayoutName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
       
    }
}
