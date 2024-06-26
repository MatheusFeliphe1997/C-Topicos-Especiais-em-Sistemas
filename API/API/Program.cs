using API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();

Produto produto = new Produto();
// produto.setNome("Bolacha");
produto.Nome = "Bolacha";
// Console.WriteLine(produto.getNome());
Console.WriteLine(produto.Nome);

List<Produto> produtos = new List<Produto>();
produtos.Add(new Produto("Celular", "IOS", 4000));
produtos.Add(new Produto("Celular", "Android", 2500));
produtos.Add(new Produto("Televisão", "LG", 2000));
produtos.Add(new Produto("Notebook", "Avell", 5000));

//EndPoints - Funcionalidades
//GET: http://localhost:5155
app.MapGet("/", () => "Minha primeira API em C# com watch");

//GET: http://localhost:5155/api/p roduto/listar
app.MapGet("/api/produto/listar", () => 
    produtos);

//GET: http://localhost:5155/api/produto/buscar/id_do_produto
app.MapGet("/api/produto/buscar/{id}", (string id) => 
{
    foreach (Produto produtoCadastrado in produtos)
    {
        if(produtoCadastrado.Id == id)
        {
            return Results.Ok(produtoCadastrado);
        }
    }
    // Produto não encontrado
    return Results.NotFound("Produto não encontrado!");
});

//POST: http://localhost:5155/api/produto/cadastrar
app.MapPost("/api/produto/cadastrar", ([FromBody] Produto produto, [FromServices] AppDataContext ctx) =>
{
   
    ctx.Produtos.Add(produto);
    ctx.SaveChanges();

    return Results.Created("", produto);
});

// DELETE: http://localhost:5155/api/produto/remover/
app.MapDelete("/api/produto/remover/{id}", (string id) =>
{
    foreach (Produto produtoRemover in produtos)
    {
        if(produtoRemover.Id == id)
        {
            produtos.Remove(produtoRemover);
            return Results.Ok(produtoRemover);
        }
    }
    // Produto não encontrado
    return Results.NotFound("Produto não encontrado!");
});

// PATCH: http://localhost:5155/api/produto/atualizar/{id}
app.MapPut("/api/produto/atualizar/{id}", (string id, Produto produto) =>
{

    foreach (Produto produtoCadastrado in produtos)
    {
        if (produtoCadastrado.Id == id)
        {
            produtoCadastrado.Nome = produto.Nome;
            produtoCadastrado.Descricao = produto.Descricao;
            produtoCadastrado.Valor = produto.Valor;
         
            return Results.Ok(produtoCadastrado);
        }
    }

 
        return Results.NotFound("Produto não encontrado!");

});


app.Run();