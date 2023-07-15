namespace health_check.Models
{
    public class StatusBadRequest
    {
        public short Status { get; set; } = StatusCodes.Status400BadRequest;
    }
}
