using System.ComponentModel;

namespace spoty_clon_backend.Models.Dtos.FilterDtos
{
    public class GenericFilterDto
    {
        public GenericFilterDto()
        {
            SorteablesFields = new List<string>();
        }

        public int Page { get; set; }

        public string? SearchString { get; set; }

        public string? OrderField { get; set; }

      
        
        public IEnumerable<string> SorteablesFields { get; set; }

        public IEnumerable<string> FilterablesFields { get; set; }
    }
}
