namespace GwanjaLoveProto.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            return value.FirstOrDefault().ToString().ToUpper() + value.Substring(1);
        }
    }
}
