using webapi_Produtos.Models.Produto;
using webapi_Produtos.Models.Response;

namespace webapi_Produtos.Services
{
    public interface IProduto
    {
        Task<ResponseModel<List<ProdutoModel>>> GetAll();
        Task<ResponseModel<List<ProdutoModel>>> GetProdutoById(int id);
        Task<ResponseModel<List<ProdutoModel>>> GetProdutoByName(string query);
        Task<ResponseModel<List<ProdutoModel>>> GetProdutosDisponiveisEmEstoque();
        Task<ResponseModel<List<ProdutoModel>>> AdicionarNovoProduto(ProdutoRequest novoProduto);
        Task<ResponseModel<List<ProdutoModel>>> RemoverProdutoPorId(int id);
        Task<ResponseModel<List<ProdutoModel>>> AtualizarEstoqueProdutoById(int id, ProdutoEstoqueRequest request);
    }
}
