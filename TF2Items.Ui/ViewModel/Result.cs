namespace TF2Items.Ui.ViewModel
{
    public class Result
    {
        public static Result Successfull()
        {
            return new Result {Success = true};
        }

        public static Result Failed(string reason)
        {
            return new Result
                   {
                       Reason = reason,
                   };
        }

        public static Result UserAborted()
        {
            return new Result
                   {
                       UserAbort = true,
                   };
        }

        public bool Success { get; set; }
        public bool UserAbort { get; set; }
        public string Reason { get; set; }
    }
}