namespace SuperHeroAPI.Core.Entities
{
    public class SuperHero
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Place { get; set; }

        public bool isValid()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(Place);
        }

    }
}
