namespace IntoItIf.Base.Domain.Options
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;

   public static class OptionHelper
   {
      #region Public Methods and Operators

      public static Option<(T1 Item1, T2 Item2)> Combine<T1, T2>(
         this Option<T1> item1,
         Option<T2> item2,
         bool isOptional = false,
         T2 defaultWhenNone = default)
      {
         try
         {
            if (!(item1 is Some<T1> some1))
               return Fail<(T1 Item1, T2 Item2)>.Throw(new ArgumentNullException(nameof(item1)));
            if (!(item2 is Some<T2>) && !isOptional)
               return Fail<(T1 Item1, T2 Item2)>.Throw(new ArgumentNullException(nameof(item2)));
            return (some1.Content, item2.Reduce(defaultWhenNone));
         }
         catch (Exception ex)
         {
            return Fail<(T1 Item1, T2 Item2)>.Throw(ex);
         }
      }

      public static Option<(T1 Item1, T2 Item2)> Combine<T1, T2>(this Option<T1> item1, Func<T1, Option<T2>> item2Map)
      {
         return item1.Combine(item2Map, false, default);
      }

      public static Option<(T1 Item1, T2 Item2)> Combine<T1, T2>(
         this Option<T1> item1,
         Func<T1, Option<T2>> item2Map,
         bool isOptional,
         T2 defaultWhenNone)
      {
         try
         {
            if (!(item1 is Some<T1> some1))
               return Fail<(T1 Item1, T2 Item2)>.Throw(new ArgumentNullException(nameof(item1)));
            var item2 = item2Map(item1.ReduceOrDefault());
            if (!(item2 is Some<T2>) && !isOptional)
               return Fail<(T1 Item1, T2 Item2)>.Throw(new ArgumentNullException(nameof(item2)));
            return (some1.Content, item2.Reduce(defaultWhenNone));
         }
         catch (Exception ex)
         {
            return Fail<(T1 Item1, T2 Item2)>.Throw(ex);
         }
      }

      public static Option<bool> Contains<T>(this IEnumerable<Option<T>> list, T value)
      {
         return list.Select(x => x.ReduceOrDefault()).Contains(value);
      }

      public static Option<bool> Contains<T>(this IEnumerable<Option<T>> list, Option<T> option)
      {
         return list.Select(x => x.ReduceOrDefault()).Contains(option.ReduceOrDefault());
      }

      public static Bool<T, TResult> ElseIfMap<T, TIfInput, TResult>(
         this Bool<T, TResult> boolOption,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, TResult> trueMap)
      {
         try
         {
            return !boolOption.Value && boolOption.Input.IsSome() && inputPredicate(ifInput)
               ? Bool<T, TResult>.True(boolOption.Input, boolOption.Input.Map(trueMap))
               : Bool<T, TResult>.False(boolOption.Input);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> ElseIfMap<T, TResult>(
         this Bool<T, TResult> boolOption,
         Func<T, bool> predicate,
         Func<T, TResult> trueMap)
      {
         try
         {
            return boolOption.ElseIfMap(boolOption.Input.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> ElseIfMapAsync<T, TIfInput, TResult>(
         this Task<Bool<T, TResult>> boolOption,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, Task<TResult>> trueMap)
      {
         try
         {
            var realOption = await boolOption;
            return !realOption.Value && realOption.Input.IsSome() && inputPredicate(ifInput)
               ? Bool<T, TResult>.True(realOption.Input, await realOption.Input.MapAsync(trueMap))
               : Bool<T, TResult>.False(realOption.Input);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> ElseIfMapAsync<T, TResult>(
         this Task<Bool<T, TResult>> boolOption,
         Func<T, bool> predicate,
         Func<T, Task<TResult>> trueMap)
      {
         try
         {
            var realBoolOption = await boolOption;
            return await boolOption.ElseIfMapAsync(realBoolOption.Input.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> ElseIfMapFlatten<T, TIfInput, TResult>(
         this Bool<T, TResult> boolOption,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, Option<TResult>> trueMap)
      {
         try
         {
            return !boolOption.Value && boolOption.Input.IsSome() && inputPredicate(ifInput)
               ? Bool<T, TResult>.True(boolOption.Input, boolOption.Input.MapFlatten(trueMap))
               : Bool<T, TResult>.False(boolOption.Input);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> ElseIfMapFlatten<T, TResult>(
         this Bool<T, TResult> boolOption,
         Func<T, bool> predicate,
         Func<T, Option<TResult>> trueMap)
      {
         try
         {
            return boolOption.ElseIfMapFlatten(boolOption.Input.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> ElseIfMapFlattenAsync<T, TIfInput, TResult>(
         this Bool<T, TResult> boolOption,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, Task<Option<TResult>>> trueMap)
      {
         try
         {
            return !boolOption.Value && boolOption.Input.IsSome() && inputPredicate(ifInput)
               ? Bool<T, TResult>.True(boolOption.Input, await boolOption.Input.MapFlattenAsync(trueMap))
               : Bool<T, TResult>.False(boolOption.Input);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> ElseIfMapFlattenAsync<T, TResult>(
         this Bool<T, TResult> boolOption,
         Func<T, bool> predicate,
         Func<T, Task<Option<TResult>>> trueMap)
      {
         try
         {
            return await boolOption.ElseIfMapFlattenAsync(boolOption.Input.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> ElseMap<T, TResult>(this Bool<T, TResult> boolOption, Func<T, TResult> map)
      {
         try
         {
            Bool<T, TResult> result;
            if (!boolOption.Value && boolOption.Input.IsSome())
            {
               result = Bool<T, TResult>.True(boolOption.Input, boolOption.Input.Map(map));
            }

            else
            {
               result = boolOption;
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> ElseMapAsync<T, TResult>(
         this Task<Bool<T, TResult>> boolOption,
         Func<T, Task<TResult>> map)
      {
         try
         {
            var realBoolOption = await boolOption;
            if (!realBoolOption.Value && realBoolOption.Input.IsSome())
            {
               return Bool<T, TResult>.True(realBoolOption.Input, await realBoolOption.Input.MapAsync(map));
            }

            return realBoolOption;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> ElseMapFlatten<T, TResult>(
         this Bool<T, TResult> boolOption,
         Func<T, Option<TResult>> map)
      {
         try
         {
            Bool<T, TResult> result;
            if (!boolOption.Value && boolOption.Input.IsSome())
            {
               result = Bool<T, TResult>.True(boolOption.Input, boolOption.Input.MapFlatten(map));
            }
            else
            {
               result = boolOption;
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> ElseMapFlattenAsync<T, TResult>(
         this Bool<T, TResult> boolOption,
         Func<T, Task<Option<TResult>>> map)
      {
         try
         {
            Bool<T, TResult> result;
            if (!boolOption.Value && boolOption.Input.IsSome())
            {
               result = Bool<T, TResult>.True(boolOption.Input, await boolOption.Input.MapFlattenAsync(map));
            }
            else
            {
               result = boolOption;
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Option<bool> Execute<T>(this Option<T> option, Action<T> action)
      {
         try
         {
            if (!(option is Some<T> some)) return Fail<bool>.Throw(new ArgumentException());
            action(some);
            return true;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      public static async Task<Option<bool>> ExecuteAsync<T>(this Option<T> option, Func<T, Task> actionAsync)
      {
         try
         {
            if (!(option is Some<T> some)) return Fail<bool>.Throw(new ArgumentException());
            await actionAsync(some);
            return true;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      public static Task<Option<T>> GetOptionTask<T>(this T source)
      {
         return Task.FromResult(source.ToOption());
      }

      public static Task<Option<T>> GetOptionTask<T>(this Option<T> source)
      {
         return Task.FromResult(source);
      }

      public static Option<bool> IfExecute<T>(this Option<T> option, Func<T, bool> predicate, Action<T> action)
      {
         try
         {
            return predicate(option.ReduceOrDefault()) ? Execute(option, action) : false;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      public static async Task<Option<bool>> IfExecuteAsync<T>(
         this Option<T> option,
         Func<T, bool> predicate,
         Func<T, Task> actionAsync)
      {
         try
         {
            return predicate(option.ReduceOrDefault()) ? await option.ExecuteAsync(actionAsync) : false;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      public static Bool<T, TResult> IfMap<T, TIfInput, TResult>(
         this Option<T> option,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, TResult> trueMap)
      {
         try
         {
            Bool<T, TResult> result;
            if (option.IsSome() && inputPredicate(ifInput))
            {
               var trueMapResult = trueMap(option.ReduceOrDefault());
               result = Bool<T, TResult>.True(option, trueMapResult);
            }
            else
            {
               result = Bool<T, TResult>.False(option);
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> IfMap<T, TResult>(
         this Option<T> option,
         Func<T, bool> predicate,
         Func<T, TResult> trueMap)
      {
         try
         {
            var result = option.IfMap(option.ReduceOrDefault(), predicate, trueMap);
            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> IfMapAsync<T, TIfInput, TResult>(
         this Task<Option<T>> option,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, Task<TResult>> trueMap)
      {
         try
         {
            var realOption = await option;
            Bool<T, TResult> result;
            if (realOption.IsSome() && inputPredicate(ifInput))
            {
               var trueMapResult = trueMap(realOption.ReduceOrDefault());
               result = Bool<T, TResult>.True(realOption, await trueMapResult);
            }
            else
            {
               result = Bool<T, TResult>.False(realOption);
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> IfMapAsync<T, TResult>(
         this Task<Option<T>> option,
         Func<T, bool> predicate,
         Func<T, Task<TResult>> trueMap)
      {
         try
         {
            var realOption = await option;
            return await option.IfMapAsync(realOption.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> IfMapAsync<T, TIfInput, TResult>(
         this Option<T> option,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, Task<TResult>> trueMap)
      {
         try
         {
            Bool<T, TResult> result;
            if (option.IsSome() && inputPredicate(ifInput))
            {
               var trueMapResult = trueMap(option.ReduceOrDefault());
               result = Bool<T, TResult>.True(option, await trueMapResult);
            }
            else
            {
               result = Bool<T, TResult>.False(option);
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> IfMapAsync<T, TResult>(
         this Option<T> option,
         Func<T, bool> predicate,
         Func<T, Task<TResult>> trueMap)
      {
         try
         {
            return await option.IfMapAsync(option.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> IfMapFlatten<T, TIfInput, TResult>(
         this Option<T> option,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, Option<TResult>> trueMap)
      {
         try
         {
            Bool<T, TResult> result;
            if (option.IsSome() && inputPredicate(ifInput))
            {
               var trueMapResult = trueMap(option.ReduceOrDefault());
               result = Bool<T, TResult>.True(option, trueMapResult);
            }
            else
            {
               result = Bool<T, TResult>.False(option);
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Bool<T, TResult> IfMapFlatten<T, TResult>(
         this Option<T> option,
         Func<T, bool> predicate,
         Func<T, Option<TResult>> trueMap)
      {
         try
         {
            return option.IfMapFlatten(option.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> IfMapFlattenAsync<T, TIfInput, TResult>(
         this Option<T> option,
         TIfInput ifInput,
         Func<TIfInput, bool> inputPredicate,
         Func<T, Task<Option<TResult>>> trueMap)
      {
         try
         {
            Bool<T, TResult> result;
            if (option.IsSome() && inputPredicate(ifInput))
            {
               var trueMapResult = trueMap(option.ReduceOrDefault());
               result = Bool<T, TResult>.True(option, await trueMapResult);
            }
            else
            {
               result = Bool<T, TResult>.False(option);
            }

            return result;
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static async Task<Bool<T, TResult>> IfMapFlattenAsync<T, TResult>(
         this Option<T> option,
         Func<T, bool> predicate,
         Func<T, Task<Option<TResult>>> trueMap)
      {
         try
         {
            return await option.IfMapFlattenAsync(option.ReduceOrDefault(), predicate, trueMap);
         }
         catch (Exception ex)
         {
            return Bool<T, TResult>.False(Fail<T>.Throw(ex));
         }
      }

      public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> map)
      {
         try
         {
            return option is Some<T> some ? (Option<TResult>)map(some) : Fail<TResult>.Throw(new ArgumentException());
         }
         catch (Exception ex)
         {
            return Fail<TResult>.Throw(ex);
         }
      }

      public static async Task<Option<TResult>> MapAsync<T, TResult>(
         this Task<Option<T>> option,
         Func<T, Task<TResult>> mapAsync)
      {
         try
         {
            var realOption = await option;
            return realOption is Some<T> some
               ? (Option<TResult>)await mapAsync(some)
               : await Task.FromResult(Fail<TResult>.Throw(new ArgumentException()));
         }
         catch (Exception ex)
         {
            return Fail<TResult>.Throw(ex);
         }
      }

      public static async Task<Option<TResult>> MapAsync<T, TResult>(
         this Option<T> option,
         Func<T, Task<TResult>> mapAsync)
      {
         try
         {
            return option is Some<T> some
               ? (Option<TResult>)await mapAsync(some)
               : await Task.FromResult(Fail<TResult>.Throw(new ArgumentException()));
         }
         catch (Exception ex)
         {
            return Fail<TResult>.Throw(ex);
         }
      }

      public static Option<TResult> MapFlatten<T, TResult>(this Option<T> option, Func<T, Option<TResult>> map)
      {
         try
         {
            return option is Some<T> some
               ? map(some)
               : Fail<TResult>.Throw(new ArgumentException());
         }
         catch (Exception ex)
         {
            return Fail<TResult>.Throw(ex);
         }
      }

      public static async Task<Option<TResult>> MapFlattenAsync<T, TResult>(
         this Task<Option<T>> option,
         Func<T, Task<Option<TResult>>> mapAsync)
      {
         try
         {
            var realOption = await option;
            return realOption is Some<T> some
               ? await mapAsync(some)
               : Fail<TResult>.Throw(new ArgumentException());
         }
         catch (Exception ex)
         {
            return Fail<TResult>.Throw(ex);
         }
      }

      public static async Task<Option<TResult>> MapFlattenAsync<T, TResult>(
         this Option<T> option,
         Func<T, Task<Option<TResult>>> mapAsync)
      {
         try
         {
            return option is Some<T> some
               ? await mapAsync(some)
               : Fail<TResult>.Throw(new ArgumentException());
         }
         catch (Exception ex)
         {
            return Fail<TResult>.Throw(ex);
         }
      }

      public static T Reduce<T>(this Option<T> option, T whenNone)
      {
         return option is Some<T> some ? (T)some : whenNone;
      }

      public static T Reduce<T>(this Option<T> option, Func<T> whenNone)
      {
         return option is Some<T> some ? (T)some : whenNone();
      }

      public static T ReduceOrDefault<T>(this Option<T> option)
      {
         return option.Reduce(() => default);
      }

      public static Option<T>[] ToArrayOfOptions<T>(this IEnumerable<T> source)
      {
         return ToEnumerableofOptions(source).ToArray();
      }

      public static IEnumerable<Option<T>> ToEnumerableofOptions<T>(this IEnumerable<T> source)
      {
         return source.Select(ToOption);
      }

      public static Option<T> ToOption<T>(this T value)
      {
         return value;
      }

      public static Option<T[]> ToOptionOfArray<T>(this IEnumerable<Option<T>> options)
      {
         return options.Select(ReduceOrDefault).ToArray().ToOption();
      }

      public static Option<IEnumerable<T>> ToOptionOfEnumerable<T>(this IEnumerable<Option<T>> options)
      {
         return options.Select(ReduceOrDefault).ToOption();
      }

      public static Option<List<T>> ToOptionOfList<T>(this IEnumerable<Option<T>> options)
      {
         return options.Select(ReduceOrDefault).ToList().ToOption();
      }

      #endregion
   }
}