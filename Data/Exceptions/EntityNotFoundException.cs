namespace GwanjaLoveProto.Data.Exceptions
{
    [Serializable]
    internal class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string? message) : base(message)
        {
        }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public override string ToString()
        {
            return InnerException == null ? Message : InnerException.ToString();
        }
    }
}