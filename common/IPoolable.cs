namespace brickpong2D.common;
public interface IPoolable
{
    void OnAcquire();
    void OnRelease();
}