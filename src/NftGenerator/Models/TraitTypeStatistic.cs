namespace NftGenerator.Models
{
    public class TraitTypeStatistic
    {
        public TraitTypeStatistic(int id, object value, string type, double rarity)
        {
            Id = id;
            Value = value;
            Type = type;
            Rarity = rarity;
        }

        public int Id { get; private set; }
        public object Value { get; private set; }
        public string Type { get; private set; }
        public double Rarity { get; private set; }
    }
}
