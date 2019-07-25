//-------------------------------------------------------
//	
//-------------------------------------------------------

public interface IWrappedRef<T>
{
    string NestedPrefabName { get; }
    T Reference { get; set; }
}
