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
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IMapper _mapper;

        public LanguageController(ILanguageRepository languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        // GET: api/Languages
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetLanguages()
        {
            var languages = _languageRepository.GetLanguages();
            var languagesDto = new List<LanguageDto>();
            foreach (var language in languages)
            {
                languagesDto.Add(_mapper.Map<LanguageDto>(language));
            }
            return Ok(languagesDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateLanguage([FromBody] LanguageDto languageDto)
        {
            if (languageDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_languageRepository.LanguageExists(languageDto.LanguageCode) || _languageRepository.LanguageNameExists(languageDto.LanguageName))
            {
                ModelState.AddModelError( "" ,"Language Exists!");
                return StatusCode(404, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var language = _mapper.Map<Language>(languageDto);
            if (!_languageRepository.CreateLanguage(language))
            {
                ModelState.AddModelError("", $"Something went wrong saving the language " +
                                                               $"{language.LanguageName}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetLanguage", new { languageId = language.LanguageId }, language);
        }

        [HttpGet("{languageId:Guid}", Name = "GetLanguage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetLanguage(Guid languageId)
        {
            if (!_languageRepository.LanguageExists(languageId))
            {
                return NotFound();
            }

            var language = _languageRepository.GetLanguage(languageId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var languageDto = _mapper.Map<LanguageDto>(language);

            return Ok(languageDto);
        }

        [HttpPut("{languageId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateLanguage(Guid languageId, [FromBody] LanguageDto languageDto)
        {
            if (languageDto == null || languageId != languageDto.LanguageId)
            {
                return BadRequest(ModelState);
            }

            var language = _mapper.Map<Language>(languageDto);

            if (!_languageRepository.UpdateLanguage(language))
            {
                ModelState.AddModelError("", $"Something went wrong updating the language " +
                                                                                  $"{language.LanguageName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{languageId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteLanguage(Guid languageId)
        {
            if (!_languageRepository.LanguageExists(languageId))
            {
                return NotFound();
            }

            var language = _languageRepository.GetLanguage(languageId);

            if (!_languageRepository.DeleteLanguage(language))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the language " +
                                                                                                     $"{language.LanguageName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateLanguages( Guid languageId,  [FromBody] LanguageDto languagesDto)
        {
            if (languagesDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLanguages = _languageRepository.GetLanguage(languageId);
            if(_languageRepository.LanguageNameExists(languagesDto.LanguageName) || _languageRepository.LanguageExists(languagesDto.LanguageCode))
            {
                ModelState.AddModelError("", "Language Exists!");
                return Conflict(ModelState);
            }

            _mapper.Map(languagesDto, existingLanguages);

            if(!_languageRepository.UpdateLanguage(existingLanguages))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {existingLanguages.LanguageName}");
                return StatusCode(500, ModelState);
            }

          
            return NoContent();
        }

    }
}
