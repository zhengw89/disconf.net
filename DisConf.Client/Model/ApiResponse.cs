namespace DisConf.Client.Model
{
    internal class ApiResponse<T>
    {
        public T Data { get; set; }

        public ApiResponseError Error { get; set; }

        public bool HasError { get { return this.Error != null; } }
    }
}
