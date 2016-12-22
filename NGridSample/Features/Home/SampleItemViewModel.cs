﻿namespace NGridSample.Features.Home
{
    using NGrid.Core;
    public class SampleItemViewModel
    {

        [GridAttributes.Hidden]
        public int Id { get; set; }
        public string Column1 { get; set; }
        public int Column2 { get; set; }
        public string Column3 { get; set; }

        public string ChildEntityName { get; set; }
    }
}