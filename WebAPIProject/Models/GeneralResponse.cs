namespace WebAPIProject.Models
{
    public class GeneralResponse
    {
        public bool  IsPass { get; set; }
        public int StatusCode { get; set; }
        public dynamic Data { get; set; }
        //public T Data { get; set; }
    }
}
