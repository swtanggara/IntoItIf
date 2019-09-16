namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;

   public sealed class PropertyHelper
   {
      #region Static Fields

      private static readonly MethodInfo CallPropertyGetterByReferenceOpenGenericMethod =
         typeof(PropertyHelper).GetMethod(
            "CallPropertyGetterByReference",
            BindingFlags.Static | BindingFlags.NonPublic);

      private static readonly MethodInfo CallPropertyGetterOpenGenericMethod =
         typeof(PropertyHelper).GetMethod("CallPropertyGetter", BindingFlags.Static | BindingFlags.NonPublic);

      private static readonly MethodInfo CallPropertySetterOpenGenericMethod =
         typeof(PropertyHelper).GetMethod("CallPropertySetter", BindingFlags.Static | BindingFlags.NonPublic);

      private static readonly ConcurrentDictionary<Type, PropertyHelper[]> ReflectionCache =
         new ConcurrentDictionary<Type, PropertyHelper[]>();

      #endregion

      #region Fields

      private readonly Func<object, object> _valueGetter;

      #endregion

      #region Constructors and Destructors

      private PropertyHelper(PropertyInfo property)
      {
         Name = property.Name;
         _valueGetter = MakeFastPropertyGetter(property);
      }

      #endregion

      #region Delegates

      // ReSharper disable once TypeParameterCanBeVariant
      private delegate TValue ByRefFunc<TDeclaringType, TValue>(ref TDeclaringType arg);

      #endregion

      #region Public Properties

      public string Name { get; }

      #endregion

      #region Public Methods and Operators

      public static PropertyHelper[] GetProperties(object instance)
      {
         return GetProperties(instance, CreateInstance, ReflectionCache);
      }

      public object GetValue(object instance)
      {
         return _valueGetter(instance);
      }

      #endregion

      #region Methods

      internal static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo)
      {
         var getMethod = propertyInfo.GetGetMethod();
         var reflectedType = getMethod.ReflectedType;
         var returnType = getMethod.ReturnType;
         Delegate @delegate;
         if (reflectedType != null && reflectedType.IsValueType)
            @delegate = Delegate.CreateDelegate(
               typeof(Func<object, object>),
               getMethod.CreateDelegate(typeof(ByRefFunc<,>).MakeGenericType(reflectedType, returnType)),
               CallPropertyGetterByReferenceOpenGenericMethod.MakeGenericMethod(reflectedType, returnType));
         else
            @delegate = Delegate.CreateDelegate(
               typeof(Func<object, object>),
               getMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(reflectedType, returnType)),
               CallPropertyGetterOpenGenericMethod.MakeGenericMethod(reflectedType, returnType));
         return (Func<object, object>)@delegate;
      }

      internal static Action<TDeclaringType, object> MakeFastPropertySetter<TDeclaringType>(PropertyInfo propertyInfo)
         where TDeclaringType : class
      {
         var setMethod = propertyInfo.GetSetMethod();
         var reflectedType = propertyInfo.ReflectedType;
         var parameterType = setMethod.GetParameters()[0]
            .ParameterType;
         return
            (Action<TDeclaringType, object>)
            Delegate.CreateDelegate(
               typeof(Action<TDeclaringType, object>),
               setMethod.CreateDelegate(typeof(Action<,>).MakeGenericType(reflectedType, parameterType)),
               CallPropertySetterOpenGenericMethod.MakeGenericMethod(reflectedType, parameterType));
      }

      // ReSharper disable once UnusedMember.Local
      private static object CallPropertyGetter<TDeclaringType, TValue>(
         Func<TDeclaringType, TValue> getter,
         object @this)
      {
         return getter((TDeclaringType)@this);
      }

      // ReSharper disable once UnusedMember.Local
      private static object CallPropertyGetterByReference<TDeclaringType, TValue>(
         ByRefFunc<TDeclaringType, TValue> getter,
         object @this)
      {
         var declaringType = (TDeclaringType)@this;
         return getter(ref declaringType);
      }

      // ReSharper disable once UnusedMember.Local
      private static void CallPropertySetter<TDeclaringType, TValue>(
         Action<TDeclaringType, TValue> setter,
         object @this,
         object value)
      {
         setter((TDeclaringType)@this, (TValue)value);
      }

      private static PropertyHelper CreateInstance(PropertyInfo property)
      {
         return new PropertyHelper(property);
      }

      private static PropertyHelper[] GetProperties(
         object instance,
         Func<PropertyInfo, PropertyHelper> createPropertyHelper,
         ConcurrentDictionary<Type, PropertyHelper[]> cache)
      {
         var type = instance.GetType();
         if (!cache.TryGetValue(type, out var propertyHelperArray))
         {
            var enumerable = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Where(
                  prop =>
                  {
                     if (prop.GetIndexParameters()
                            .Length ==
                         0)
                        return prop.GetMethod != null as MethodInfo;
                     return false;
                  });
            var list = new List<PropertyHelper>();
            foreach (var propertyInfo in enumerable)
            {
               var propertyHelper = createPropertyHelper(propertyInfo);
               list.Add(propertyHelper);
            }

            propertyHelperArray = list.ToArray();
            cache.TryAdd(type, propertyHelperArray);
         }

         return propertyHelperArray;
      }

      #endregion
   }
}