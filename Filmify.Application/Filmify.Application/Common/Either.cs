namespace Filmify.Application.Common;

public class Either<L, R>
{
    public L? Left { get; }
    public R? Right { get; }
    public bool IsRight { get; }

    private Either(L left)
    {
        Left = left;
        IsRight = false;
    }

    private Either(R right)
    {
        Right = right;
        IsRight = true;
    }

    public static Either<L, R> Success(R right) => new Either<L, R>(right);
    public static Either<L, R> Fail(L left) => new Either<L, R>(left);

    // Optional helpers
    public T Match<T>(Func<L, T> leftFunc, Func<R, T> rightFunc)
    {
        if (IsRight && Right != null)
            return rightFunc(Right);
        else if (!IsRight && Left != null)
            return leftFunc(Left);
        else
            throw new InvalidOperationException("Either has no value.");
    }

}
