namespace fast_api.Contracts.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public ItemDTO[] CategoryItems { get; set; }

        public int Buy { get; set; }
        public int Sell { get; set; }
    }
}