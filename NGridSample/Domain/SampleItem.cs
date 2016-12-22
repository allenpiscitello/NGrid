namespace NGridSample.Domain
{
    public class SampleItem
    {
        public int Id { get; set; }
        public string Column1 { get; set; }
        public int Column2 { get; set; }
        public bool Column3 { get; set; }

        public int ChildId { get; set; }

        public virtual ChildEntity ChildEntity { get; set; }
    }
}
