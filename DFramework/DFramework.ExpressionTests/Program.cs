using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DFramework.ExpressionTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ExpressionTest1();

            WriteLine(Query<Person>(p => p.Age >= 19));
            ReadLine();
        }

        private static void ExpressionTest1()
        {   //创建表达式树
            Expression<Func<int, bool>> expTree = num => num >= 5;

            //获取输入参数
            ParameterExpression param = expTree.Parameters[0];
            //获取lambda表达式主题部分
            BinaryExpression body = (BinaryExpression)expTree.Body;
            //获取num>=5的右半部分
            ConstantExpression right = (ConstantExpression)body.Right;
            //获取num>=5的左半部分
            ParameterExpression left = (ParameterExpression)body.Left;
            //获取比较运算符
            ExpressionType type = body.NodeType;

            WriteLine($"解析后：{param} {body} {left} {type} {right}");
        }

        private static string Query<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            var param = expression.Parameters[0];
            var body = (BinaryExpression)expression.Body;

            var left = body.Left;
            var name = (left as MemberExpression).Member.Name;

            var right = (ConstantExpression)body.Right;
            var nodeType = body.NodeType;

            var sb = new StringBuilder();

            var type = typeof(T);
            var properties = type.GetProperties();
            sb.Append("select ");
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                sb.Append(property.Name);

                if (i != properties.Length - 1)
                {
                    sb.Append(" ,");
                }
            }

            sb.Append(" from ");
            sb.Append(type.Name);
            sb.Append(" where ");
            sb.Append(name);

            if (nodeType.Equals(ExpressionType.GreaterThanOrEqual))
            {
                sb.Append(">=");
            }

            sb.Append(right);
            return sb.ToString();
        }
    }

    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}