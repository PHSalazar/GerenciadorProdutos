using Azure;
using Microsoft.AspNetCore.Mvc;
using webapi_Produtos.Data;
using webapi_Produtos.Models.Produto;
using webapi_Produtos.Models.Response;

namespace webapi_Produtos.Services
{
    public class ProdutoService : IProduto
    {
        private AppDbContext _context;
        private ResponseModel<List<ProdutoModel>> _response;
        public ProdutoService(AppDbContext context)
        {
            _context = context;
            _response = new ResponseModel<List<ProdutoModel>>();
        }
        public async Task<ResponseModel<List<ProdutoModel>>> AdicionarNovoProduto(ProdutoRequest novoProduto)
        {
            if (string.IsNullOrEmpty(novoProduto.Nome))
            {
                _response.Mensagem = "Nome do produto não pode ser vazio.";
                _response.Sucesso = false;
                return _response;
            }

            if (novoProduto.Categoria != Enums.CategoriaEnum.Casa 
                || novoProduto.Categoria != Enums.CategoriaEnum.Escritorio 
                || novoProduto.Categoria != Enums.CategoriaEnum.Jardinagem)
            {
                _response.Mensagem = "Categoria do produto inválida.";
                _response.Sucesso = false;
                return _response;
            }

            if (novoProduto.Preco <= 0)
            {
                _response.Mensagem = "O preço do produto não pode menor ou igual a 0.";
                _response.Sucesso = false;
                return _response;
            }

            ProdutoModel novoProdutoModel = new ProdutoModel()
            { 
                Nome = novoProduto.Nome,
                Descricao = novoProduto.Descricao,
                Categoria = novoProduto.Categoria,
                StatusEstoque = novoProduto.QuantidadeEstoque > 0 ? true : false,
                Preco = novoProduto.Preco,
                QuantidadeEstoque = novoProduto.QuantidadeEstoque
            };

            _context.Produtos.Add(novoProdutoModel);
            await _context.SaveChangesAsync();

            _response = await GetAll();
            _response.Mensagem = "Item cadastrado com sucesso.";
            return _response;
        }

        public async Task<ResponseModel<List<ProdutoModel>>> AtualizarEstoqueProdutoById(int id, ProdutoEstoqueRequest request)
        {
            var produtoDB = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produtoDB == null)
            {
                _response.Mensagem = $"Não foi encontrado nenhum produto com ID {id}";
                _response.Sucesso = false;
                return _response;
            }

            if (request.Quantidade < 0)
            {
                _response.Mensagem = $"Quantidade do produto não pode ser menor do que zero.";
                _response.Sucesso = false;
                return _response;
            }

            produtoDB.QuantidadeEstoque = request.Quantidade;
            produtoDB.StatusEstoque = produtoDB.QuantidadeEstoque > 0 ? true : false;
            
            await _context.SaveChangesAsync();

            _response.Itens = [produtoDB];
            _response.Mensagem = "Quantidade atualizada com sucesso.";
            return _response;
        }

        public async Task<ResponseModel<List<ProdutoModel>>> GetAll()
        {
            var produtosDB = _context.Produtos.ToList();

            if(produtosDB.Count() == 0)
            {
                _response.Mensagem = "Nenhum item cadastrado no banco de dados.";
                _response.Sucesso = false;
                return _response;
            }

            _response.Itens = produtosDB;
            _response.Mensagem = "Itens cadastrados no banco de dados listados com sucesso.";
            return _response;
        }

        public async Task<ResponseModel<List<ProdutoModel>>> GetProdutoById(int id)
        {
            var produtosDB = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produtosDB == null)
            {
                _response.Mensagem = $"Não foi encontrado nenhum produto com ID {id}";
                _response.Sucesso = false;
                return _response;
            }

            _response.Itens = [produtosDB];
            _response.Mensagem = $"Produto com ID {id} listado com sucesso ";
            return _response;
        }

        public async Task<ResponseModel<List<ProdutoModel>>> GetProdutoByName(string query)
        {
            var produtosDB = _context.Produtos.Where(p => p.Nome.ToLower().Contains(query.ToLower())).ToList();

            if (produtosDB.Count() == 0)
            {
                _response.Mensagem = $"Não foi encontrado nenhum produto que contenha o texto '{query}' no nome.";
                _response.Sucesso = false;
                return _response;
            }

            _response.Itens = produtosDB;
            _response.Mensagem = $"Produto(s) com o texto '{query}' listado(s) com sucesso ";
            return _response;
        }

        public async Task<ResponseModel<List<ProdutoModel>>> GetProdutosDisponiveisEmEstoque()
        {
            var produtosBD = _context.Produtos.Where(p => p.StatusEstoque == true);

            if (produtosBD == null)
            {
                _response.Mensagem = $"Nenhum produto com o estoque disponível.";
                _response.Sucesso = false;
                return _response;
            }


            _response.Itens = produtosBD.ToList();
            _response.Mensagem = $"Produto(s) com o estoque disponível listado(s) com sucesso.";

            return _response;
        }

        public async Task<ResponseModel<List<ProdutoModel>>> RemoverProdutoPorId(int id = 0)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
            {
                _response.Mensagem = $"Nenhum produto com o ID {id}.";
                _response.Sucesso = false;
                return _response;
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            _response = await GetAll();
            _response.Mensagem = $" {produto.Nome} removido do BD com sucesso do BD.";

            return _response;

        }

    }
}
