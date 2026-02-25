using System;
using Cysharp.Threading.Tasks;

namespace Tutan.Functional
{
    public static partial class F
    {
        public static async UniTask<Result<T>> TryAsync<T>(Func<UniTask<T>> f)
        {
            try { return Success(await f()); }
            catch (Exception ex) { return new Error(ex.Message); }
        }

        public static async UniTask<Result<Unit>> TryAsync(Func<UniTask> action)
        {
            try { await action(); return Success(Unit()); }
            catch (Exception ex) { return new Error(ex.Message); }
        }
    }
}
