using System.Text.Json.Serialization;

namespace webapi_Produtos.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CategoriaEnum
    {
        Casa,
        Escritorio,
        Jardinagem
    }
}
