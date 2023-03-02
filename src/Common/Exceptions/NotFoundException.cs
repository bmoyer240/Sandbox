namespace System;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object identifier)
    : base($"Instance '{name}' not found for id '{identifier}'")
    {
    }
}

public class NotFoundException<TEntity> : NotFoundException
{
    public NotFoundException(object identifier)
    : base(nameof(TEntity), identifier)
    {
    }
}