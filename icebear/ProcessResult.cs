namespace icebear
{
    public class ProcessResult
    {
        public int ExitCode { get; set; }
        public string StandardOutput { get; set; }
        public string ErrorOutput { get; set; }
    }
}