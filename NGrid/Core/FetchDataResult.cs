namespace NGrid.Core
{
    public class FetchDataResult<T>
    {
        public GridColumn[] Columns { get; set; }
        public T[] Data { get; set; }
    }
}
