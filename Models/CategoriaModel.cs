using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MegaLivros.Models
{
    public class CategoriaModel
    {
        public CategoriaModel()
        {
            Produtos = new Collection<ProdutosModel>();
        
        }
        [Key]
        public int CategoriaId { get; set; }
        public string? NomeCategoria { get; set; }
        public int Valor { get; set; }

        [JsonIgnore]
        public ICollection<ProdutosModel>? Produtos { get; set; }
    }


}
