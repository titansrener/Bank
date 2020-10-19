using BANCO.Core;
using BANCO.Data;
using BANCO.Data.Implementation;
using BANCO.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BANCO.Tests
{
    public class ContaTests
    {
        private readonly HttpClient _client;
        private readonly Contexto context;
        private readonly ContaRepository repo;

        public ContaTests()
        {
            context = new Contexto(new DbContextOptionsBuilder<Contexto>()
            .UseSqlServer("Data Source=DESKTOP-M7GD3GG\\SQLEXPRESS;Initial Catalog=API_BD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").Options);
            repo = new ContaRepository(context);

            var server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile($"appsettings.Development.json", optional: false, reloadOnChange: true);
            })
            .UseStartup<Startup>())
            {
                AllowSynchronousIO = true
            };
            _client = server.CreateClient();
        }

        private Conta CriarContaPost
        {
            get
            {
                var tipoResponsabilidade = new Conta
                {
                    NumeroConta = "0004",
                    CpfCliente = "04952487511",
                    NomeBanco = "BANCO A",
                    NomeCliente = "Cliente TESTE01",
                    RendaMensal = 6000,
                    Saldo = 100
                };

                return tipoResponsabilidade;
            }
        }

        [Theory]
        [InlineData("GET", "0004")]
        public async Task GetContaOkAsync(string method, string numero)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/api/Contas/");

            string json = JsonConvert.SerializeObject(CriarContaPost);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                // Arrange
                var request2 = new HttpRequestMessage(new HttpMethod(method), $"/api/Contas/{numero}");
                // Act
                var response2 = await _client.SendAsync(request2);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            }
        }

        [Theory]
        [InlineData("GET", "suiahsuhashuahsu")]
        public async Task GetContaNotFoundAsync(string method, string numero)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/Contas/{numero}");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "003", "A", "10")]
        public async Task GetSacarOkAsync(string method, string numeroConta, string nomeBanco, decimal valorSaque)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/Contas/Sacar/{numeroConta}{nomeBanco}{valorSaque}");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
