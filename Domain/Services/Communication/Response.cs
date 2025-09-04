namespace DealManagementSystem.Domain.Services.Communication;

public class Response<T>
{
  public bool Success { get; protected set; }
  public string Message { get; protected set; }
  public T? Item { get; private set; }
    private Response(bool success, string message, T? item)
    {
      Success = success;
      Message = message;
      Item = item;
    }

    public Response(T item) : this(true, string.Empty, item)
    {
    }

    public Response(string message) : this(false, message, default)
    {
    }

}

