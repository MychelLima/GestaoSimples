using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace GestaoSimples.Data
{
    public static class Database
    {
        private static readonly string DbPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gestao.db");

        public static string ConnectionString => $"Data Source={DbPath}";

        public static void Inicializar()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NomeUsuario TEXT NOT NULL UNIQUE,
                    Senha TEXT NOT NULL,
                    Perfil TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Clientes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    Email TEXT,
                    Telefone TEXT
                );

                CREATE TABLE IF NOT EXISTS Produtos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    Preco REAL NOT NULL,
                    Estoque INTEGER NOT NULL,
                    CaminhoImagem TEXT
                );

                CREATE TABLE IF NOT EXISTS Pedidos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClienteId INTEGER NOT NULL,
                    ProdutoId INTEGER NOT NULL,
                    Quantidade INTEGER NOT NULL,
                    Total REAL NOT NULL,
                    Data TEXT NOT NULL,
                    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
                    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id)
                );
            ";
            cmd.ExecuteNonQuery();

            // Cria um usuário administrador padrão, se ainda não existir nenhum
            var cmdSeed = conn.CreateCommand();
            cmdSeed.CommandText = @"
                INSERT OR IGNORE INTO Usuarios (NomeUsuario, Senha, Perfil)
                VALUES ('admin', '123456', 'Administrador');
            ";
            cmdSeed.ExecuteNonQuery();
        }
    }
}