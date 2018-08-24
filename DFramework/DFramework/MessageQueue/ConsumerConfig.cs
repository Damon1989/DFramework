namespace DFramework.MessageQueue
{
    public class ConsumerConfig
    {
        public static readonly ConsumerConfig DefaultConfig = new ConsumerConfig();
        public int BackOffIncrement { get; set; } = 30;

        public int FullLoadThreshold { get; set; } = 1000;
        public int WaitInterval { get; set; } = 1000;
        public int MailboxProcessBatchCount { get; set; } = 100;
        public string AutoOffsetReset { get; set; } = MessageQueue.AutoOffsetReset.Largest;
    }
}