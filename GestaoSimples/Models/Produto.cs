namespace GestaoSimples.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double Preco { get; set; }
        public int Estoque { get; set; }
        public string? CaminhoImagem { get; set; }
    }
}
