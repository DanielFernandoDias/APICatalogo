using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
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

        public CategoriasController(IUnityOfWork uof, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("Autor")]
        public String GetAutor()
        {
            var autor = _configuration["autor"];
            return $"Autor = {autor}";
        }

        [HttpGet("Conexao")]
        public String GetConexao()
        {
            var StringConnection = _configuration["ConnectionStrings:DefaultConnection"];
            return $"StringConection = {StringConnection}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<String> GetSaudacao([FromServices] IMeuServico meuServico, String nome) 
        {
            return meuServico.Saudacao(nome);
        }



        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("=============== GET api/Categorias/Produtos ===================");
            var categorias = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
            return categorias;
        } 

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                _logger.LogInformation("=============== GET api/Categorias/ ===================");
                var categorias = _uof.CategoriaRepository.Get().ToList();
                return categorias;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "OCorreu  um problema ao tratar a sua solicitação"); // Lanço um status code na excessão, caso ocorra um erro, assim evito que exiba a pilha de chamado para o usuario
            }
            
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> get(int id)
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
                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "OCorreu  um problema ao tratar a sua solicitação");
            }
        } 

        [HttpPost]
        public ActionResult post(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest();
            }
            _uof.CategoriaRepository.Add(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest();
            }
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok(categoria);

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

            return Ok(categoria);
        }



    }
}
