namespace NGridSample
{
    using Domain;
    using Infrastructure;

    public static class SampleData
    {
        public static void AddTestData(ApiContext context)
        {
            var child1 = AddChildData(context, "Type1");
            var child2 = AddChildData(context, "A Type2");


            AddItem(context, "DEF", 123, true, child1);
            AddItem(context, "ABC", 456, false, child2);
            AddItem(context, "ABC", 78, true, child1);
            AddItem(context, "JKL", 123, false, child2);
            context.SaveChanges();
        }

        private static void AddItem(ApiContext context, string column1, int column2, bool column3, ChildEntity child)
        {
            var testItem = new SampleItem
            {
                Column1 = column1,
                Column2 = column2,
                Column3 = column3,
                ChildId = child.Id,
                ChildEntity = child
            };

            context.Set<SampleItem>().Add(testItem);
        }

        private static ChildEntity AddChildData(ApiContext context, string name)
        {
            var data = new ChildEntity
            {
                Name = name
            };
            context.Set<ChildEntity>().Add(data);
            return data;
        }

    }
}
