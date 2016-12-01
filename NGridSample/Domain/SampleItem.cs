﻿namespace NGridSample.Domain
{
    using Infrastructure;

    public class SampleItem
    {
        [GridAttributes.Hidden]
        public int Id { get; set; }
        public string Column1 { get; set; }
        public int Column2 { get; set; }
    }
}
