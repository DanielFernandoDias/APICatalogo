using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // É readonly para não ser alterada depois que for atribuida
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        //PRODUTOS
        //[HttpGet("/Primeiro")]
        //[HttpGet("Primeiro")] 
        [HttpGet("{valor:alpha:length(5)}")]
        public ActionResult<Produto> GetPrimeiro()
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault();
            if (produto is null) //Verifica se é null
            {
                return NotFound("Produtos não encontrados..."); //retorna 404
            }
            return produto;
        }

        //api/PRODUTOS
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
            if(produtos is null) //Verifica se é null
            {
                return NotFound("Produtos não encontrados..."); //retorna 404
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id) 
        {

            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return produto;
        }
        //PRODUTOS
        [HttpPost]
        public IActionResult post(Produto produto)
        {
            if(produto is null)
            {
                return BadRequest();
            }
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public IActionResult put(int id, Produto produto) 
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            _context.Produtos.Remove(produto); 
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
