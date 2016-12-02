namespace NGrid.Core
{
    using MediatR;

    public class FetchDataQuery<T> : IAsyncRequest<FetchDataResult<T>> where T : class
    {
        public SortOption[] SortColumns { get; set; }
    }

}
