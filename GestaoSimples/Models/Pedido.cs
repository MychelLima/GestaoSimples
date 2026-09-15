using System;

namespace GestaoSimples.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public double Total { get; set; }
        public DateTime Data { get; set; }
    }
}
