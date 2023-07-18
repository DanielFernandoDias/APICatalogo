using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Categorias")] // Nem deveria pois já incluir no DB Context
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>(); // Boa prática inicializar uma coleção em uma entidade
        }
        [Key]
        public int CategoriaId { get; set; } // Usar o ID para o EF identificar que é um ChavePK
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }
        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public ICollection<Produto>? Produtos { get; set; } // Crio um-para-muitos e já é o suficiente
    }
}