namespace DFramework.Infrastructure.Helpers
{
    using System.Text;

    public partial class String
    {
        public String()
        {
            this.Builder = new StringBuilder();
        }

        /// <summary>
        /// 字符串生成器
        /// </summary>
        public StringBuilder Builder { get; set; }

        public string Empty => string.Empty;

        public int Length => this.Builder.Length;

        /// <summary>
        /// 追加内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public String Append<T>(T value)
        {
            this.Builder.Append(value);
            return this;
        }

        /// <summary>
        /// 追加内容
        /// </summary>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public String Append(string value, params object[] args)
        {
            if (args == null) args = new object[] { string.Empty };

            if (args.Length == 0) this.Builder.Append(value);
            else this.Builder.AppendFormat(value, args);
            return this;
        }

        /// <summary>
        /// 追加换行
        /// </summary>
        /// <returns></returns>
        public String AppendLine()
        {
            this.Builder.AppendLine();
            return this;
        }

        /// <summary>
        /// 追加内容并换行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public String AppendLine<T>(T value)
        {
            this.Append(value);
            this.Builder.AppendLine();
            return this;
        }

        /// <summary>
        /// 追加内容并换行
        /// </summary>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public String AppendLine(string value, params object[] args)
        {
            this.Append(value, args);
            this.Builder.AppendLine();
            return this;
        }

        public String Clear()
        {
            this.Builder = this.Builder.Clear();
            return this;
        }
        /// <summary>
        /// 移除末尾字符串
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        public String RemoveEnd(string end)
        {
            var result = this.Builder.ToString();
            if (!result.EndsWith(end)) return this;

            this.Builder = new StringBuilder(result.TrimEnd(end.ToCharArray()));
            return this;
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public String Replace(string value)
        {
            this.Builder.Clear();
            this.Builder.Append(value);
            return this;
        }

        public override string ToString()
        {
            return this.Builder.ToString();
        }
    }
}