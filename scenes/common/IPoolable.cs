namespace brickpong2D.scenes.common;

public interface IPoolable
{
    void OnAcquire();
    void OnRelease();
}