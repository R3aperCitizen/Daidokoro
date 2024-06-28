namespace Daidokoro.View.Controls;

public interface ITemplate<T> : IContentView
{
    public T Model { get; set; }
}