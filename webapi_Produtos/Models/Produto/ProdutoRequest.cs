using webapi_Produtos.Enums;

namespace webapi_Produtos.Models.Produto
{
    public class ProdutoRequest
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
