using System;
using System.Linq.Expressions;
using System.Reflection;
using RefactorName.GraphDiff.Internal.Graph;

namespace RefactorName.GraphDiff.Internal.GraphBuilders
{
    /// <summary>
    /// Reads an IUpdateConfiguration mapping and produces an GraphNode graph.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ConfigurationGraphBuilder : ExpressionVisitor
    {
        private GraphNode _currentMember;
        private string _currentMethod = "";

        public GraphNode BuildGraph<T>(Expression<Func<IUpdateConfiguration<T>, object>> expression)
        {
            var initialNode = new GraphNode();
            _currentMember = initialNode;
            Visit(expression);
            return initialNode;
        }

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            var accessor = GetMemberAccessor(memberExpression);
            var newMember = CreateNewMember(accessor);

            _currentMember.Members.Push(newMember);
            _currentMember = newMember;

            return base.VisitMember(memberExpression);
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            _currentMethod = expression.Method.Name;

            // go left to right in the subtree (ignore first argument for now)
            for (int i = 1; i < expression.Arguments.Count; i++)
            {
                Visit(expression.Arguments[i]);
            }

            // go back up the tree and continue
            _currentMember = _currentMember.Parent;
            return Visit(expression.Arguments[0]);
        }

        private GraphNode CreateNewMember(PropertyInfo accessor)
        {
            GraphNode newMember;
            switch (_currentMethod)
            {
                case "OwnedEntity":
                    newMember = GraphNodeFactory.Create(_currentMember, accessor, false, true);
                    break;
                case "AssociatedEntity":
                    newMember = GraphNodeFactory.Create(_currentMember, accessor, false, false);
                    break;
                case "OwnedCollection":
                    newMember = GraphNodeFactory.Create(_currentMember, accessor, true, true);
                    break;
                case "AssociatedCollection":
                    newMember = GraphNodeFactory.Create(_currentMember, accessor, true, false);
                    break;
                default:
                    throw new NotSupportedException("The method used in the update mapping is not supported");
            }
            return newMember;
        }

        private static PropertyInfo GetMemberAccessor(MemberExpression memberExpression)
        {
            PropertyInfo accessor = null;
            var expression = memberExpression.Expression;
            var constantExpression = expression as ConstantExpression;

            if (constantExpression != null)
            {
                var container = constantExpression.Value;
                var member = memberExpression.Member;

                var fieldInfo = member as FieldInfo;
                if (fieldInfo != null)
                {
                    dynamic value = fieldInfo.GetValue(container);
                    accessor = (PropertyInfo)value.Body.Member;
                }

                var info = member as PropertyInfo;
                if (info != null)
                {
                    dynamic value = info.GetValue(container, null);
                    accessor = (PropertyInfo)value.Body.Member;
                }
            }
            else
            {
                accessor = (PropertyInfo)memberExpression.Member;
            }

            if (accessor == null)
                throw new NotSupportedException("Unknown accessor type found!");

            return accessor;
        }
    }
}