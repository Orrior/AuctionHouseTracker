namespace WebApplication1.Interfaces;

public abstract class BasicAuction
{
    public interface IAuctionInfo
    {
        //TODO!!! Maybe long, not int?
        public long Id { get; set; }
        public string Name { get; set; }
        public string ItemClass { get; set; }
        public string ItemSubClass { get; set; }
    }

    public interface IAuctionSlot
    {
        //TODO!!! Maybe long, not int?
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public string TimeLeft { get; set; }
        //TODO! Creation condition if initialized first time.
        public DateTime TimeStamp { get; set; }
    }
}