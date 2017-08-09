using Hmm.Utility.Dal.DataEntity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hmm.Dal.Tests
{
    public static class TestHelperExtensions
    {
        public static void AddEntity<T>(this List<T> list, T entity) where T : new()
        {
            var newEntity = new T();
            ShallowCopy(newEntity, entity);
            list.Add(newEntity);
        }

        private static void ShallowCopy(object dest, object src)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var destFields = dest.GetType().GetFields(flags);
            var srcFields = src.GetType().GetFields(flags);

            foreach (var srcField in srcFields)
            {
                var destField = destFields.FirstOrDefault(field => field.Name == srcField.Name);

                if (destField == null || destField.IsLiteral)
                {
                    continue;
                }

                if (srcField.FieldType == destField.FieldType)
                {
                    destField.SetValue(dest, srcField.GetValue(src));
                }
            }

            var destBase = dest.GetType().GetTypeInfo().BaseType;
            var srcBase = src.GetType().GetTypeInfo().BaseType;
            while (destBase != null && srcBase != null)
            {
                destFields = destBase.GetFields(flags);
                srcFields = srcBase.GetFields(flags);

                foreach (var srcField in srcFields)
                {
                    var destField = destFields.FirstOrDefault(field => field.Name == srcField.Name);

                    if (destField != null && !destField.IsLiteral)
                    {
                        if (srcField.FieldType == destField.FieldType)
                            destField.SetValue(dest, srcField.GetValue(src));
                    }
                }

                destBase = destBase.GetTypeInfo().BaseType;
                srcBase = srcBase.GetTypeInfo().BaseType;
            }
        }

        public static int GetNextId<T>(this IList<T> list) where T : Entity
        {
            var curid = list.Count > 0 ? list.Max(n => n.Id) : 0;
            return curid + 1;
        }
    }
}