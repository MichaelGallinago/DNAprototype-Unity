using System.Text;

namespace UxmlViewBinding
{
    public readonly struct IncrementalBuilder
    {
        public readonly StringBuilder Builder;

        public IncrementalBuilder(StringBuilder stringBuilder) => Builder = stringBuilder;
    
        public static IncrementalBuilder operator +(IncrementalBuilder sb, string value)
        {
            sb.Builder.Append(value);
            return sb;
        }
        
        public static IncrementalBuilder operator +(IncrementalBuilder sb, int value)
        {
            sb.Builder.Append(value);
            return sb;
        }
        
        public static IncrementalBuilder operator +(IncrementalBuilder sb, float value)
        {
            sb.Builder.Append(value);
            return sb;
        }
        
        public static IncrementalBuilder operator +(IncrementalBuilder sb, long value)
        {
            sb.Builder.Append(value);
            return sb;
        }
        
        public static IncrementalBuilder operator +(IncrementalBuilder sb, double value)
        {
            sb.Builder.Append(value);
            return sb;
        }
    
        public static IncrementalBuilder operator +(IncrementalBuilder sb, char value)
        {
            sb.Builder.Append(value);
            return sb;
        }

        public static IncrementalBuilder operator +(IncrementalBuilder sb1, IncrementalBuilder sb2) => sb2;

        public static bool operator ==(IncrementalBuilder sb1, IncrementalBuilder sb2) => sb1.Builder == sb2.Builder;
        public static bool operator !=(IncrementalBuilder sb1, IncrementalBuilder sb2) => sb1.Builder != sb2.Builder;

        public static IncrementalBuilder operator -(IncrementalBuilder sb, string text)
        {
            sb.Builder.AppendLine().Append(text);
            return sb;
        }

        public static implicit operator string(IncrementalBuilder sb) => sb.ToString();
        public static implicit operator IncrementalBuilder(StringBuilder sb) => new(sb);
        public static implicit operator StringBuilder(IncrementalBuilder sb) => sb.Builder;
    
        public IncrementalBuilder ToPascalCase(string text)
        {
            return this + char.ToUpper(text[0]) + text.Substring(1);
        }
        
        public IncrementalBuilder Clear()
        {
            Builder.Clear();
            return this;
        }

        public override string ToString() => Builder.ToString();
    }
}