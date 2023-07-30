using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly IConfiguration _configuration; // Acesso aos modelos de configurações
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnityOfWork uof, IConfiguration configuration, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _uof = uof;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Autor")]
        public String GetAutor()
        {
            var autor = _configuration["autor"];
            return $"Autor = {autor}";
        }

        [HttpGet("Conexao")]
        public string GetConexao()
        {
            var StringConnection = _configuration["ConnectionStrings:DefaultConnection"];
            return $"StringConection = {StringConnection}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<String> GetSaudacao([FromServices] IMeuServico meuServico, string nome) 
        {
            return meuServico.Saudacao(nome);
        }



        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDto>> GetCategoriasProdutos()
        {
            _logger.LogInformation("=============== GET api/Categorias/Produtos ===================");
            var categorias = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
            return _mapper.Map<List<CategoriaDto>>(categorias);
        } 

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDto>> Get()
        {
            try
            {
                _logger.LogInformation("=============== GET api/Categorias/ ===================");
                var categorias = _uof.CategoriaRepository.Get().ToList();
                return _mapper.Map<List<CategoriaDto>>(categorias);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "OCorreu  um problema ao tratar a sua solicitação"); // Lanço um status code na excessão, caso ocorra um erro, assim evito que exiba a pilha de chamado para o usuario
            }
            
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDto> get(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                _logger.LogInformation($"=============== GET api/Categorias/id = {id} ===================");
                if (categoria is null)
                {
                    _logger.LogInformation($"=============== GET api/Categorias/id = {id} NOT FOUND ===================");
                    return NotFound("Categoria não encontrada...");
                }
                return _mapper.Map<CategoriaDto>(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "OCorreu  um problema ao tratar a sua solicitação");
            }
        } 

        [HttpPost]
        public ActionResult post(CategoriaDto categoriaDto)
        {
            if (categoriaDto is null)
            {
                return BadRequest();
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult put(int id, Categoria categoriaDto)
        {
            if(id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok(categoriaDto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrado...");
            }
            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            return Ok(_mapper.Map<CategoriaDto>(categoria));
        }
    }
}
