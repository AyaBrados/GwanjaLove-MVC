namespace GwanjaLoveProto.Data.Exceptions
{
    [Serializable]
    internal class EntityDuplicationException : Exception
    {
        public EntityDuplicationException()
        {
        }

        public EntityDuplicationException(string? message) : base(message)
        {
        }

        public EntityDuplicationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public override string ToString()
        {
            return InnerException == null ? Message : InnerException.ToString();
        }
    }
}