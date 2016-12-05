namespace NGrid.Core
{
    using MediatR;

    public class FetchDataQuery<T, U> : IAsyncRequest<FetchDataResult<U>> where T : class
    {
        public SortOption[] SortColumns { get; set; }
    }

}
