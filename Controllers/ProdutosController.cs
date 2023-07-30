using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // É readonly para não ser alterada depois que for atribuida
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnityOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("menorPreco")]
        public IEnumerable<ProdutoDto> GetProdutosPorPreco()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos); 
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
        public ActionResult<IEnumerable<ProdutoDto>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            if(produtos is null) //Verifica se é null
            {
                return NotFound("Produtos não encontrados..."); //retorna 404
            }
            var produtosDto = _mapper.Map<List<ProdutoDto>>(produtos);
            return produtosDto;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDto> Get(int id) 
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return _mapper.Map<ProdutoDto>(produto);
        }

        //PRODUTOS
        [HttpPost]
        public IActionResult post(ProdutoDto produtoDto)
        {
            if(produtoDto is null)
            {
                return BadRequest();
            }
            var produto = _mapper.Map<Produto>(produtoDto);
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDto);
        }

        [HttpPut("{id:int}")]
        public IActionResult put(int id, ProdutoDto produtoDto) 
        {
            if(id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }
            var produto = _mapper.Map<Produto>(produtoDto);
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

            return Ok(_mapper.Map<ProdutoDto>(produto));
        }
    }
}
