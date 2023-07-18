using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; // Acesso aos modelos de configurações
        private readonly ILogger _logger; 

        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _context = context;
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
            var categorias = _context.Categorias.AsNoTracking().Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
            return categorias;
        } 

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                _logger.LogInformation("=============== GET api/Categorias/ ===================");
                var categorias = _context.Categorias.AsNoTracking().ToList();
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
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);
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
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest();
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);

        }

        [HttpDelete("{id:int}")]
        public ActionResult delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrado...");
            }
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }



    }
}
