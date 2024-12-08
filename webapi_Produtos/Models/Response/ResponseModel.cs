namespace webapi_Produtos.Models.Response
{
    public class ResponseModel<T>
    {
        public T? Itens { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public bool Sucesso { get; set; } = true;
    }
}
