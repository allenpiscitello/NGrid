namespace NGridSample
{
    using Domain;
    using Infrastructure;

    public static class SampleData
    {
        public static void AddTestData(ApiContext context)
        {
            AddItem(context, "DEF", 123, true);
            AddItem(context, "ABC", 456, false);
            AddItem(context, "ABC", 78, true);
            AddItem(context, "JKL", 123, false);
            context.SaveChanges();
        }

        private static void AddItem(ApiContext context, string column1, int column2, bool column3)
        {
            var testItem = new SampleItem
            {
                Column1 = column1,
                Column2 = column2,
                Column3 = column3
            };

            context.Set<SampleItem>().Add(testItem);
        }
    }
}
