using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // É readonly para não ser alterada depois que for atribuida
        private readonly IUnityOfWork _uof;

        public ProdutosController(IUnityOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("menorPreco")]
        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        ////PRODUTOS
        ////[HttpGet("/Primeiro")]
        ////[HttpGet("Primeiro")] 
        //[HttpGet("{valor:alpha:length(5)}")]
        //public ActionResult<Produto> GetPrimeiro()
        //{
        //    var produto = _uof.Produtos.AsNoTracking().FirstOrDefault();
        //    if (produto is null) //Verifica se é null
        //    {
        //        return NotFound("Produtos não encontrados..."); //retorna 404
        //    }
        //    return produto;
        //}

        //api/PRODUTOS
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            if(produtos is null) //Verifica se é null
            {
                return NotFound("Produtos não encontrados..."); //retorna 404
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> GetAsync(int id) 
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
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
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public IActionResult put(int id, Produto produto) 
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }
            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            _uof.ProdutoRepository.Delete(produto); 
            _uof.Commit();

            return Ok(produto);
        }
    }
}
