
# API - Gerenciador de Produtos  
  
Esta WebAqpi faz parte do desafio propoto por um bootcamp que participei, onde o objetivo era:  
  
Desenvolver um sistema Gerenciador de Produtos, que deveria armazenar as informações do produto como:  
- ID  
- Nome, descrição, categoria, preço, disponibilidade no estoque e outros.

## Funcionalidades  
A API deveria ter as seguintes características:  
  
- Qualquer pessoa pode Consultar todos os Produtos cadastrados.  
- Qualquer pessoa pode Consultar os Produtos em Estoque.  
- Somente o Gerente do sistema pode Excluir um Produto  
- O Gerente e o Funcionário podem Atualizar estoque de um produto

## Criação do BD
[Script de Criação de BD e Tabela](https://github.com/PHSalazar/GerenciadorProdutos/blob/main/webapi_Produtos/CriacaoTabelaSQLServer.sql)

    CREATE DATABASE BootcampSquadra2024_PedroSalazar
    GO
    
    USE BootcampSquadra2024_PedroSalazar
    GO
    
    CREATE TABLE [dbo].[Produtos]
    (
    	[Id] INT NOT NULL IDENTITY, 
        [Nome] VARCHAR(50) NOT NULL, 
        [Descricao] VARCHAR(50) NOT NULL, 
        [Categoria] INT NOT NULL, 
        [Preco] DECIMAL(18, 2) NOT NULL,
        [StatusEstoque] BIT NOT NULL, 
        [QuantidadeEstoque] INT NOT NULL
    )
    
   ## Como Usar 

 
  



Para usar a API, pode utilizar o próprio Swagger ou o Postman, por exemplo.  
Deixo abaixo a lista de rotas disponíveis para consumo da API:

Alguns endpoints precisam de login, como Excluir e Atualizar Estoque:

Para o login, foi criado um controller para autenticação (/Auth) usando login e senha. Como exemplo, estão disponíveis dois usuários locais: "ger" que dá acesso completo a aplicação e **func** que dá acesso a algumas rotas de consumo da API.  
  
- `/Auth [Método POST]`: Endpoint para Login com os dados:  
- username: **ger** password: **ger** (acesso total)  
- username: **func** password: **func**
Body da solicitação:
```json
{
  "username": "usuario",
  "password": "senha"
}
```

| Endpoint | Método | Descrição | OBS |
|--|--|--|--|
| `/Produto` | **GET** |Obtém a lista de todos os produtos cadastrados.|
|``/Produto/{id}``|**GET**|Obtém produto por ID (FromRoute).|--|
|`/Produto/buscaNome/{nome}`|**GET** | Obtém produto por nome (completo ou parte do nome) (FromRoute). |
| `/Produto/EmEstoque`      | **GET** | Obtém uma lista dos produtos com estoque. |
| `/Produto` | **POST** |Cria um novo produto.| ```{ "nome": "string",  "descricao": "string",  "categoria": [Categorias: Casa = 1, Escritorio = 2, Jardinagem =3],  "preco": 0,  "quantidadeEstoque": 0}```
| `/Produto/atualizarEstoque/{id}` | **PUT** | Atualiza estoque de um produto por ID (FromRoute) com a quantidade atualizada (FromBody). | ```{"quantidade": 10}``` |
| `/Produto/{id}`         | **DELETE** | Remove um produto pelo ID (FromRoute). |



## Uso no Swagger
Após realizar o login pelo endpoint `Auth`, será devolvido um token que deve ser usado para autenticar nas rotas de Produtos.
Para ser autenticado no Swagger, só clicar em **Authorize** e digitar *'Bearer [token]'*.

![Autenticacao Swagger](https://github.com/user-attachments/assets/ba679910-d8ab-420e-9102-8b89801e5153)

## Uso no Postman
Após realizar o login pelo endpoint `Auth`, será devolvido um token que deve ser usado para autenticar nas rotas de Produtos.
Para ser autenticado no Postman, só clicar em **Authorization**, escolher '*Bearer [token]*' como o tipo de autenticação e enviar o token gerado.

![Autenticacao Postman](https://github.com/user-attachments/assets/e0915ae5-13dd-4540-a81e-1d9ff56aa1d2)

**Depois desses passos, os endpoints estarão devidamente autorizados.**

## Notas

**Categoria**
Foi criado uma entidade enum Categorias, permitindo somente cadastro de produtos em categorias validadas:

    Enum:  
    [  Casa, Escritorio, Jardinagem  ]
    [ 0, 1, 2 ]

**SQL Server**
Foi usado o SQL Server para persistencia de dados.
**Banco de Dados de exemplo**: BootcampSquadra2024

**Script para criação do Banco de Dados e da tabela no BD**:
```json
CREATE DATABASE BootcampSquadra2024_PedroSalazar
GO

USE BootcampSquadra2024_PedroSalazar
GO

CREATE TABLE [dbo].[Produtos]
(
	[Id] INT NOT NULL IDENTITY, 
    [Nome] VARCHAR(50) NOT NULL, 
    [Descricao] VARCHAR(50) NOT NULL, 
    [Categoria] INT NOT NULL, 
    [Preco] DECIMAL(18, 2) NOT NULL,
    [StatusEstoque] BIT NOT NULL, 
    [QuantidadeEstoque] INT NOT NULL
)
```

**Scripts de exemplo para adicionar um novo produto**:
```json
{
  "nome": "Cadeira",
  "descricao": "Sem descrição",
  "categoria": 1,
  "preco": 189.99,
  "quantidadeEstoque": 70
}
```

```json
{
  "nome": "Mesa de jantar",
  "descricao": "Sem descrição",
  "categoria": "Casa",
  "preco": 1999.99,
  "quantidadeEstoque": 150
}
```

*Lembrando que se a *quantidadeEstoque* for igual a 0, o status em estoque automaticamente será indisponível.*
