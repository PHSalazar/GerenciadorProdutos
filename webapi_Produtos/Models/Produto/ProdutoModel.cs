using System.ComponentModel.DataAnnotations;
using webapi_Produtos.Enums;

namespace webapi_Produtos.Models.Produto
{
    public class ProdutoModel
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public bool StatusEstoque { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
