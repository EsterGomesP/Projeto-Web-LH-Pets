using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization; // Adicionado para formatação de moeda correta

namespace Projeto_Web_Lh_Pets_Alunos
{
    public class Banco
    {

        private List<Clientes> lista = new List<Clientes>();
        public List<Clientes> GetLista()
        {
            return lista;
        }

        public Banco()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();


                builder.DataSource = ".\\SQLEXPRESS";

                builder.InitialCatalog = "vendas";

                builder.IntegratedSecurity = true;

                using (SqlConnection conexao = new SqlConnection(builder.ConnectionString))
                {
                    string sql = "SELECT * FROM tblclientes";
                    using (SqlCommand comando = new SqlCommand(sql, conexao))
                    {
                        // Linha onde a conexão falhava:
                        conexao.Open();

                        using (SqlDataReader tabela = comando.ExecuteReader())
                        {
                            while (tabela.Read())
                            {
                                lista.Add(new Clientes()
                                {

                                    cpf_cnpj = tabela["cpf_cnpj"]?.ToString()!,
                                    nome = tabela["nome"]?.ToString()!,
                                    endereco = tabela["endereco"]?.ToString()!,
                                    rg_ie = tabela["rg_ie"]?.ToString()!,
                                    tipo = tabela["tipo"]?.ToString()!,

                                    // O 'as float' não existe no C#, convertendo de forma segura:
                                    valor = Convert.ToSingle(tabela["valor"]),
                                    valor_imposto = Convert.ToSingle(tabela["valor_imposto"]),
                                    total = Convert.ToSingle(tabela["total"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Este bloco irá exibir o erro de conexão se ele persistir, como o Error 233
                Console.WriteLine(e.ToString());
            }
        }


        public String GetListaString()
        {
            string enviar = "<!DOCTYPE html>\n<html>\n<head>\n<meta charset='utf-8' />\n" +
                            "<title>Cadastro de Clientes</title>\n</head>\n<body>";
            enviar = enviar + "<b>   CPF / CNPJ      -     Nome    -   Endereço    -   RG / IE   -   Tipo  -   Valor   - Valor Imposto -   Total   </b>";

            int i = 0;
            string corfundo = "", cortexto = "";

            CultureInfo culturaBrasil = new CultureInfo("pt-BR");

            foreach (Clientes cli in GetLista())
            {

                if (i % 2 == 0)
                { corfundo = "#6f47ff"; cortexto = "white"; }
                else
                { corfundo = "#ffffff"; cortexto = "#6f47ff"; }
                i++;


                enviar = enviar +
               $"\n<br><div style='background-color:{corfundo};color:{cortexto};'>" +
                cli.cpf_cnpj + " - " +
                cli.nome + " - " + cli.endereco + " - " + cli.rg_ie + " - " +
                cli.tipo + " - " + cli.valor.ToString("C", culturaBrasil) + " - " +
                cli.valor_imposto.ToString("C", culturaBrasil) + " - " + cli.total.ToString("C", culturaBrasil) + "<br>" +
                "</div>";
            }
            return enviar;
        }

        public void imprimirListaConsole()
        {

            CultureInfo culturaBrasil = new CultureInfo("pt-BR");

            Console.WriteLine("   CPF / CNPJ   " + " - " + "    Nome   " +
                " - " + "   Endereço   " + " - " + "  RG / IE  " + " - " +
                "   Tipo " + " - " + "  Valor  " + " - " + "Valor Imposto" +
                " - " + "  Total  ");

            foreach (Clientes cli in GetLista())
            {
                Console.WriteLine(cli.cpf_cnpj + " - " +
                cli.nome + " - " + cli.endereco + " - " + cli.rg_ie + " - " +
                cli.tipo + " - " + cli.valor.ToString("C", culturaBrasil) + " - " +
                cli.valor_imposto.ToString("C", culturaBrasil) + " - " + cli.total.ToString("C", culturaBrasil));
            }
        }
    }
}