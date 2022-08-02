namespace Ni.Models;

public class NiDataProtocol<T> : NiProtocol
{
    public T? Data { get; set; }
}