using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Projeto_Web_Lh_Pets_Alunos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.WebHost.UseUrls("http://localhost:5000");
            var app = builder.Build();

            app.UseStaticFiles();

            app.MapGet("/index", (HttpContext contexto) =>
            {
                contexto.Response.Redirect("index.html", false);
                return Task.CompletedTask;
            });

            Banco dba = new Banco();
            app.MapGet("/listaClientes", (HttpContext contexto) =>
            {
                return contexto.Response.WriteAsync(dba.GetListaString());
            });

            app.Run();
        }
    }
}
